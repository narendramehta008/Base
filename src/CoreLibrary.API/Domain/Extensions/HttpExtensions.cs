using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net;
using System.Web;
using CoreLibrary.API.Domain.Models;
using GraphQL.Client.Http;
using GraphQL;
using GraphQL.Client.Serializer.Newtonsoft;
using CoreLibrary.API.Domain.Enums;

namespace CoreLibrary.API.Domain.Extensions;
[ExcludeFromCodeCoverage]
public static class HttpExtensions
{
    public static HttpRequestHeaders AddRange(this HttpRequestHeaders headers, IEnumerable<KeyValuePair<string, string>>? newHeaders)
    {
        newHeaders.ForEach(header =>
        {
            headers.Add(header.Key, header.Value);
        });
        return headers;
    }

    public static HttpRequestHeaders AddRange(this HttpRequestHeaders headers, IEnumerable<KeyValuePair<string, IEnumerable<string>>>? newHeaders)
    {
        newHeaders.ForEach(header =>
        {
            headers.Add(header.Key, header.Value);
        });
        return headers;
    }

    public static string ParamBuilder(this string url, IEnumerable<KeyValuePair<string, string>>? parameters)
    {
        if (parameters is null || !parameters.Any())
            return url;

        var builder = new UriBuilder(url);
        var query = HttpUtility.ParseQueryString(builder.Query);
        parameters.ForEach(parameter =>
        {
            query[parameter.Key] = parameter.Value;
        });
        builder.Query = query.ToString();
        return builder.ToString();
    }

    public static async Task<ApiResponse> ApiResponseAsync(this HttpClient httpClient, HttpRequestMessage requestMessage, bool isStream)
    {
        try
        {
            var httpResponse = await httpClient.SendAsync(requestMessage);

            if (isStream)
                return new ApiResponse(httpResponse.StatusCode, httpResponse.RequestMessage?.RequestUri?.ToString() ?? requestMessage.RequestUri?.ToString()!, httpResponse.Content.ReadAsStream(), httpResponse.Content.Headers.ContentType?.MediaType);
            var response = await httpResponse.Content.ReadAsStringAsync();
            return new ApiResponse(httpResponse.StatusCode, httpResponse.RequestMessage?.RequestUri?.ToString() ?? requestMessage.RequestUri?.ToString()!, response, httpResponse.Content.Headers.ContentType?.MediaType);
        }
        catch (Exception ex)
        {
            return new ApiResponse(HttpStatusCode.InternalServerError, requestMessage?.RequestUri?.ToString() ?? "", ex.Message, "*/*", ex);
        }
    }

    public static async Task<ApiResponse> ApiResponseAsync(this HttpClient httpClient, string url, bool isStream, HttpMethod? method = null, IEnumerable<KeyValuePair<string, string>>? headers = null, IEnumerable<KeyValuePair<string, string>>? parameters = null, HttpContent? content = null)
    {
        var message = new HttpRequestMessage()
        {
            Method = method ?? HttpMethod.Get,
            RequestUri = new Uri(url.ParamBuilder(parameters)),
            Content = content
        };
        message.Headers.AddRange(headers);
        return await httpClient.ApiResponseAsync(message, isStream);
    }
    public static async Task<ApiResponse> ApiResponseAsync(this HttpClient httpClient, string url, string query, IEnumerable<KeyValuePair<string, string>>? headers = null)
    {
        try
        {
            var graphQLHttpClientOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(url)
            };

            httpClient.DefaultRequestHeaders.AddRange(headers);

            var graphQLClient = new GraphQLHttpClient(graphQLHttpClientOptions, new NewtonsoftJsonSerializer(), httpClient);
            var request = new GraphQLRequest
            {
                Query = query
            };
            var graphQLResponse = await graphQLClient.SendQueryAsync<object>(request);
            return new ApiResponse(graphQLResponse.Data?.ToString() ?? "");
        }
        catch (Exception ex)
        {
            return new ApiResponse(HttpStatusCode.InternalServerError, url, ex.Message, "*/*");
        }


    }

    public static async Task<ApiResponse> ApiResponseAsync(this HttpClient httpClient, string url, ApiRequest apiRequest)
    {
        if (apiRequest.Timeout.HasValue)
            httpClient.Timeout = apiRequest.Timeout.Value;
        if (!string.IsNullOrWhiteSpace(apiRequest.Query))
            return await httpClient.ApiResponseAsync(url, apiRequest.Query, apiRequest.Headers);
        return await httpClient.ApiResponseAsync(url, apiRequest.IsContentStream, apiRequest.Method, apiRequest.Headers, apiRequest.Parameters, apiRequest.Content);
    }

    public static HttpClient CreateClient(this IHttpClientFactory httpClientFactory, ApiRequest apiRequest)
    {
        if (apiRequest.HttpClientManager?.HttpMessageHandler != null)
            return new HttpClient(apiRequest.HttpClientManager.HttpMessageHandler, apiRequest.HttpClientManager.DisposeHandler);
        return httpClientFactory.CreateClient();
    }

    public static Dictionary<string, string> AuthorizationHeader(this Authentications authentication, string token)
    {
        return new Dictionary<string, string>()
        {
            {"Authorization",$"{authentication} {token}" }
        };
    }

    public static Dictionary<string, string> Header(this string header, string value)
    {
        return new Dictionary<string, string>()
        {
            {header,value }
        };
    }
    public static HttpClientManager WebProxySetting(this Uri uri)
    {
        var proxy = new WebProxy
        {
            Address = uri,
            BypassProxyOnLocal = false,
            UseDefaultCredentials = false,
        };
        var httpClientHandler = new HttpClientHandler
        {
            Proxy = proxy,
        };
        return new(httpClientHandler);
    }
}
