using CoreLibrary.API.Domain.Handlers;
using CoreLibrary.API.Domain.Interfaces.Repositories;
using CoreLibrary.API.Domain.Models;
using CoreLibrary.API.Domain.Models.Apis;
using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.API.Domain.Services.Repositories;

public interface IApiServiceRepository
{
    Task<ApiResponse> ApiRequestAsync(string url, string category, string? type = null);
    Task<ApiResponse> RetryApiRequestAsync(string url, string category, string? type = null, string? apiRetryPath = null);
}

public class ApiServiceRepository : IApiServiceRepository
{
    private readonly IRestApiRepository _restApiRepository;
    private readonly ILogger<ApiServiceRepository> _logger;
    private readonly IConfiguration _configuration;

    [ExcludeFromCodeCoverage]
    public ApiServiceRepository(IRestApiRepository restApiRepository, ILogger<ApiServiceRepository> logger, IConfiguration configuration)
    {
        _restApiRepository = restApiRepository ?? throw new ArgumentNullException(nameof(restApiRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    /// <summary>
    /// Here url could be url or someid according to UrlFormat in ApiRequestConfig
    /// </summary>
    /// <param name="url"></param>
    /// <param name="category"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public async Task<ApiResponse> ApiRequestAsync(string url, string category, string? type = null)
    {
        var configs = _configuration.GetSection("ApiRequestConfig").Get<ApiRequestConfig[]>()!;
        ApiRequestConfig apiRequest = FetchApiRequest(category, type, configs);
        return await _restApiRepository.ApiResponseAsync(apiRequest.RequestUrl(url), apiRequest);
    }

    [ExcludeFromCodeCoverage]
    private static ApiRequestConfig FetchApiRequest(string category, string? type, ApiRequestConfig[] configs)
    {
        var apiCategory = configs.Where(a => a.Category == category);
        var apiRequest = string.IsNullOrWhiteSpace(type) ? apiCategory.First() :
            apiCategory.First(a => a.Types.Any(b => !string.IsNullOrWhiteSpace(b) && string.Equals(b, type, StringComparison.OrdinalIgnoreCase))).Initialize();
        return apiRequest;
    }

    /// <summary>
    /// Here url could be url or someid according to UrlFormat in ApiRequestConfig
    /// </summary>
    /// <param name="url"></param>
    /// <param name="category"></param>
    /// <param name="type"></param>
    /// <param name="apiRetryPath"></param>
    /// <returns></returns>
    public async Task<ApiResponse> RetryApiRequestAsync(string url, string category, string? type = null, string? apiRetryPath = null)
    {
        var configs = _configuration.GetSection("ApiRequestConfig").Get<ApiRequestConfig[]>()!;
        ApiRequestConfig apiRequest = FetchApiRequest(category, type, configs);

        Api api = string.IsNullOrWhiteSpace(apiRetryPath) ? new() : _configuration.GetSection(apiRetryPath ?? string.Empty).Get<Api>() ?? new();
        _restApiRepository.SetApiModel(new(timeSpans: api.ExponentialBackoff()));
        return await _restApiRepository.RetryApiResponseAsync(apiRequest.RequestUrl(url), apiRequest); ;
    }
}

