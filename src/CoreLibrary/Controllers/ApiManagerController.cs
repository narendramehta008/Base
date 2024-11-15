using CoreLibrary.API.Application.Common.Handler;
using CoreLibrary.API.Domain.Constants;
using CoreLibrary.API.Domain.Interfaces.Repositories;
using CoreLibrary.API.Domain.Models;
using CoreLibrary.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CoreLibrary.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApiManagerController : ControllerBase
{
    private readonly IDbRepository _dbRepository;
    private readonly IRestApiRepository _restApi;
    private readonly ILogger<ApiManagerController> _logger;
    private readonly HttpClient _httpClient;

    public ApiManagerController(IDbRepository dbRepository, IHttpClientFactory httpClientFactory, IRestApiRepository restApi, ILogger<ApiManagerController> logger)
    {
        _dbRepository = dbRepository;
        _restApi = restApi;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Get(GetModel postData)
    {
        ApiResponse apiResponse = ApiResponse.BadRequest();
        Action action = () =>
        {
            var uri = new Uri(postData.Url);
            var apiRequest = new ApiRequest()
            {
                Headers = postData.Headers
            };
            apiResponse = _restApi.ApiResponseAsync(postData.Url, apiRequest).Result;
        };
        action.Handle(_logger, ex => { apiResponse = new(System.Net.HttpStatusCode.BadRequest, ex.InnerException?.Message ?? ex.Message); });
        return StatusCode((int)apiResponse.HttpStatusCode, apiResponse.Response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Post(PostDataModel postData)
    {
        ApiResponse apiResponse = ApiResponse.BadRequest();
        Action action = () =>
        {
            var apiRequest = new ApiRequest()
            {
                Method = HttpMethod.Post,
                Headers = postData.Headers
            };
            if (!string.IsNullOrWhiteSpace(postData.Payload))
                apiRequest.Content = new StringContent(postData.Payload, Encoding.UTF8, GlobalConstants.MEDIATYPEJSON);
            apiResponse = _restApi.ApiResponseAsync(postData.Url, apiRequest).Result;
        };
        action.Handle(_logger, ex => { apiResponse = new(System.Net.HttpStatusCode.BadRequest, postData.Url); });
        return StatusCode((int)apiResponse.HttpStatusCode, apiResponse.Response);
    }
}

public record GetModel(string Url, ICollection<KeyValuePair<string, string>>? Headers);
public record PostDataModel(string Url, string? Payload, ICollection<KeyValuePair<string, string>>? Headers);