using CoreLibrary.API.Domain.Models.Authorization;
using CoreLibrary.API.Domain.Models;
using CoreLibrary.API.Domain.Enums;
using CoreLibrary.API.Domain.Services.Repositories;
using CoreLibrary.API.Domain.Models.Apis;

namespace CoreLibrary.API.Domain.Interfaces.Repositories;

public interface IRestApiRepository
{
    Dictionary<string, string> AcceptJsonHeader();

    Task<ApiResponse> ApiResponseAsync(string url);

    Task<ApiResponse> ApiResponseAsync(ApiRequest apiRequest);

    Task<ApiResponse> ApiResponseAsync(string url, ApiRequest apiRequest);

    Dictionary<string, string> AuthorizationHeader(Authentications authentication, string token);

    Task<string> GenerateAccessTokenAsync(IdpConfigOptions idpConfig);

    Task<ApiResponse> HealthCheckAsync(string baseUrl);

    Task<ApiResponse> HealthCheckAsync(string baseUrl, string path);

    Task<ApiResponse> RetryApiResponseAsync(string url);

    Task<ApiResponse> RetryApiResponseAsync(string url, ApiRequest apiRequest);

    Task<ApiResponse> RetryApiResponseAsync(string url, string healthCheckPath);

    Task<ApiResponse> RetryHealthCheckAsync(string baseUrl, ApiRequest apiRequest);

    Task ServiceHealthCheckAsync(string url);

    Task ServiceHealthCheckAsync(string url, ApiRequest apiRequest);

    RestApiRepository SetApiModel(Models.Apis.RetryApiModel? apiModel);
}