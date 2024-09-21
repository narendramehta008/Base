using CoreLibrary.API.Domain.Constants;
using CoreLibrary.API.Domain.Enums;
using CoreLibrary.API.Domain.Extensions;
using CoreLibrary.API.Domain.Handlers;
using CoreLibrary.API.Domain.Interfaces.Repositories;
using CoreLibrary.API.Domain.Models.Authorization;
using CoreLibrary.API.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CoreLibrary.API.Domain.Models.Apis;

namespace CoreLibrary.API.Domain.Services.Repositories;

public class RestApiRepository : IRestApiRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<RestApiRepository> _logger;
    private readonly IMemoryCache _memoryCache;
    private RetryApiModel? _retryApiModel = new();

    [ExcludeFromCodeCoverage]
    public RestApiRepository(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache, ILogger<RestApiRepository> logger)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Dictionary<string, string> AcceptJsonHeader() =>
            "Accept".Header(GlobalConstants.MEDIATYPEJSON);

    public async Task<ApiResponse> ApiResponseAsync(string url)
    {
        return await ApiResponseOptAsync(url);
    }

    /// <summary>
    /// In case we passing entire HttpRequestMessage? RequestMessage
    /// </summary>
    /// <param name="apiRequest"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<ApiResponse> ApiResponseAsync(ApiRequest apiRequest)
    {
        if (apiRequest.RequestMessage?.RequestUri is null)
            throw new ArgumentNullException(GlobalConstants.INVALIDURL);
        await ServiceHealthCheckOptAsync(apiRequest.RequestMessage.RequestUri.AbsolutePath, apiRequest);
        apiRequest.Headers = await SetIdpConfig(apiRequest.Headers, apiRequest.IdpConfig);
        return await _httpClientFactory.CreateClient(apiRequest).ApiResponseAsync(apiRequest.RequestMessage, apiRequest.IsContentStream);
    }

    public async Task<ApiResponse> ApiResponseAsync(string url, ApiRequest apiRequest) =>
        await ApiResponseOptAsync(url, apiRequest);

    public Dictionary<string, string> AuthorizationHeader(Authentications authentication, string token) =>
            authentication.AuthorizationHeader(token);

    public async Task<string> GenerateAccessTokenAsync(IdpConfigOptions idpConfig)
    {
        var headers = AcceptJsonHeader().AddThis("x-sungard-idp-api-key", idpConfig.ApiKey);

        var parameters = new Dictionary<string, string>()
    {
        {"grant_type", "client_credentials" },
        {"client_id", idpConfig.ClientId },
        {"client_secret", idpConfig.ClientSecret },
    };

        var key = idpConfig.UniqueId;
        if (_memoryCache.TryGetValue(key, out string? token))
            return token!;

        var response = await ApiResponseAsync(idpConfig.Url, idpConfig.ApiRequest ?? new ApiRequest() { Method = HttpMethod.Post, Headers = headers, Content = new FormUrlEncodedContent(parameters) });

        if (response is not null && response.HttpStatusCode == HttpStatusCode.OK)
        {
            IdpAuthResponse? authResponse = idpConfig.TokenDeserializer != null ? idpConfig.TokenDeserializer(JObject.Parse(response.Response)) :
                response.Response.Deserialize<IdpAuthResponse>();
            if (authResponse is not null)
            {
                _memoryCache.Set(key, authResponse.AccessToken, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(authResponse.ExpiresIn),
                });

                return authResponse.AccessToken;
            }
        }

        if (response is not null && response.HttpStatusCode != HttpStatusCode.OK)
            throw new Exception($"Error while generating access token, Status Code: {response.HttpStatusCode}, Error Message: {response.Response}");

        return null ?? string.Empty;
    }

    public async Task<ApiResponse> HealthCheckAsync(string baseUrl, string path) =>
        await HealthCheckOptAsync(baseUrl, path);

    public async Task<ApiResponse> HealthCheckAsync(string baseUrl) =>
        await HealthCheckOptAsync(baseUrl);

    public async Task<ApiResponse> RetryApiResponseAsync(string url) =>
        await RetryApiResponseOptAsync(url);

    public async Task<ApiResponse> RetryApiResponseAsync(string url, ApiRequest apiRequest) =>
      await RetryApiResponseOptAsync(url, apiRequest);

    /// <summary>
    /// for once check without multiple retries
    /// </summary>
    /// <param name="url"></param>
    /// <param name="healthCheckPath"></param>
    /// <returns></returns>
    public async Task<ApiResponse> RetryApiResponseAsync(string url, string healthCheckPath) =>
        await RetryApiResponseOptAsync(url, new ApiRequest() { HealthCheckPath = healthCheckPath, HealthCheck = HealthCheck.Once });

    public async Task<ApiResponse> RetryHealthCheckAsync(string baseUrl, ApiRequest apiRequest) =>
        await RetryHealthCheckOptAsync(baseUrl, apiRequest);

    public async Task ServiceHealthCheckAsync(string url) =>
         await ServiceHealthCheckOptAsync(url);

    public async Task ServiceHealthCheckAsync(string url, ApiRequest apiRequest) =>
        await ServiceHealthCheckOptAsync(url, apiRequest);

    /// <summary>
    /// Set retryApi to run the retries
    /// to perform same time of retry operations in multiple api request
    /// </summary>
    /// <param name="retryApiModel"></param>
    /// <returns></returns>
    public RestApiRepository SetApiModel(RetryApiModel? retryApiModel)
    {
        _retryApiModel = retryApiModel;
        return this;
    }

    private async Task<ApiResponse> ApiResponseOptAsync(string url, ApiRequest? apiRequest = null)
    {
        apiRequest ??= new();
        await ServiceHealthCheckOptAsync(url, apiRequest);
        apiRequest.Headers = await SetIdpConfig(apiRequest.Headers, apiRequest.IdpConfig);
        if (apiRequest.Headers is not null && apiRequest.AcceptJsonHeader)
            apiRequest.Headers.AddThis(GlobalConstants.Accept, GlobalConstants.MEDIATYPEJSON);
        return await _httpClientFactory.CreateClient(apiRequest).ApiResponseAsync(url, apiRequest);
    }

    private async Task<ApiResponse> HealthCheckOptAsync(string baseUrl, string? path = null)
    {
        path ??= GlobalConstants.HEALTHCHECKPATH;
        if (Uri.TryCreate(baseUrl, UriKind.Absolute, out Uri? uri) && uri != null)
            return await ApiResponseAsync(uri.UrlAppendPath(path));
        return ApiResponse.BadRequest(GlobalConstants.INVALIDURL);
    }

    private async Task<ApiResponse> RetryApiResponseOptAsync(string url, ApiRequest? apiRequest = null)
    {
        if (_retryApiModel?.TimeSpans == null)
            throw new ArgumentNullException(GlobalConstants.RETRYAPIMODELINITIAILZE);

        apiRequest ??= new ApiRequest();

        await ServiceHealthCheckOptAsync(url, apiRequest);
        apiRequest.Headers = await SetIdpConfig(apiRequest.Headers, apiRequest.IdpConfig);

        var response = ApiResponse.BadRequest();

        await url.HttpRetryPolicy((response, count) =>
        {
            apiRequest.Action?.Invoke(response, count);
        }, apiRequest, _retryApiModel.TimeSpans).ExecuteAsync(async () =>
        {
            response = await ApiResponseAsync(url, apiRequest);

            response.OffsetResponseSuccessStatusForRetry = apiRequest.OffsetResponseSuccessStatusForRetry;

            return response;
        });
        return response;
    }

    private async Task<ApiResponse> RetryHealthCheckOptAsync(string baseUrl, ApiRequest? apiRequest = null)
    {
        if (_retryApiModel?.TimeSpans == null)
            throw new ArgumentNullException(GlobalConstants.RETRYAPIMODELINITIAILZE);

        apiRequest ??= new ApiRequest();
        var response = ApiResponse.BadRequest();

        await baseUrl.HttpRetryPolicy((response, count) =>
        {
            apiRequest.HealthCheckAction?.Invoke(response, count);
            _logger.LogWarning("HealthCheck url: {url} {response}", response.Request, response);
        }, apiRequest, _retryApiModel.HealthCheckTimes).ExecuteAsync(async () =>
        {
            response = await HealthCheckOptAsync(baseUrl, apiRequest.HealthCheckPath);
            return response;
        });
        return response;
    }

    private async Task ServiceHealthCheckOptAsync(string url, ApiRequest? apiRequest = null)
    {
        apiRequest ??= new();
        // if None then true, Once and then check health, Retry with config else default
        var flag = apiRequest.HealthCheck == HealthCheck.None || (apiRequest.HealthCheck == HealthCheck.Once && (await HealthCheckOptAsync(url, apiRequest.HealthCheckPath)).IsSuccess)
            || (apiRequest.HealthCheck == HealthCheck.Retry && (await RetryHealthCheckOptAsync(url, apiRequest)).IsSuccess);

        if (!flag)
            throw new Exception($"There was a problem when connecting with internal services {url} please try again after some time");
    }

    private async Task<ICollection<KeyValuePair<string, string>>?> SetIdpConfig(ICollection<KeyValuePair<string, string>>? headers, IdpConfigOptions? idpConfig)
    {
        if (idpConfig != null && idpConfig.IsAuthEnabled)
        {
            var token = await GenerateAccessTokenAsync(idpConfig);
            headers ??= new Dictionary<string, string>();
            headers.AddThese(AuthorizationHeader(idpConfig.Authentication, token));
        }
        return headers;
    }
}
