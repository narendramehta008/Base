using CoreLibrary.API.Domain.Constants;
using CoreLibrary.API.Domain.Enums;
using CoreLibrary.API.Domain.Models;
using CoreLibrary.API.Domain.Models.Authorization;
using CoreLibrary.API.Domain.Services.Repositories;
using CoreLibrary.Tests.Helper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using CoreLibrary.API.Domain.Extensions;
using Polly.Contrib.WaitAndRetry;
using Moq.Protected;
using CoreLibrary.API.Domain.Models.Apis;

namespace CoreLibrary.Tests.Repositories;

public class RestApiRepositoryTests
{
    private const string url = "https://api.sampleapis.com/codingresources/codingResources";
    private static readonly string content = File.ReadAllText(@"SampleResponse/token.json");

    [Theory, AutoMockData]
    public async void ApiResponseAsync_Returns_Success(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
        Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        MessageHandler(httpFactory, messageHandler, content);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);
        var result = await apiRepo.ApiResponseAsync(url);

        //Assert
        Assert.NotNull(result?.Response);
        Assert.Equal(HttpStatusCode.OK, result?.HttpStatusCode);
        Assert.IsType<ApiResponse>(result);
    }

    [Theory, AutoMockData]
    public async void ApiResponseAsync_MessageRequest_Returns_Success(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
        Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        MessageHandler(httpFactory, messageHandler, content);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);
        var result = await apiRepo.ApiResponseAsync(new ApiRequest()
        {
            RequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            },
            IdpConfig = new IdpConfigOptions()
            {
                ApiKey = "apiKey",
                ClientId = "clientId",
                ClientSecret = "secret",
                Url = "https://tokenurl.com/accesstoken"
            }
        });

        //Assert
        Assert.NotNull(result?.Response);
        Assert.Equal(HttpStatusCode.OK, result?.HttpStatusCode);
        Assert.IsType<ApiResponse>(result);
    }

    [Theory, AutoMockData]
    public async void ApiResponseAsync_MessageRequest_Returns_InvalidUrl(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
        Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        MessageHandler(httpFactory, messageHandler, content);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => apiRepo.ApiResponseAsync(new ApiRequest()
        {
            RequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get
            }
        }));

        //Assert
        Assert.Equal(GlobalConstants.INVALIDURL, ex.ParamName);
    }

    [Theory, AutoMockData]
    public async void ApiResponseAsync_Returns_NotFound(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
        Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var content = "Not found.";
        MessageHandler(httpFactory, messageHandler, content, HttpStatusCode.NotFound);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);
        var result = await apiRepo.ApiResponseAsync(url);

        //Assert
        Assert.NotNull(result?.Response);
        Assert.Equal(HttpStatusCode.NotFound, result?.HttpStatusCode);
        Assert.IsType<ApiResponse>(result);
    }

    /// <summary>
    /// Test retry and result predicate
    /// </summary>
    /// <param name="httpFactory"></param>
    /// <param name="memoryCache"></param>
    /// <param name="logger"></param>
    /// <param name="messageHandler"></param>
    [Theory, AutoMockData]
    public async void ApiResponseWithApiRequestAsync_Returns_NotFound(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
        Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var content = "Not found.";
        var status = HttpStatusCode.NotFound;
        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);
        var result = await apiRepo.ApiResponseAsync(url, new ApiRequest()
        {
            ResultPredicate = (resp) => resp.HttpStatusCode == HttpStatusCode.NotFound,
        });

        //Assert
        Assert.NotNull(result?.Response);
        Assert.Equal(status, result?.HttpStatusCode);
        Assert.IsType<ApiResponse>(result);
    }

    /// <summary>
    ///             //
    /// </summary>
    /// <param name="httpFactory"></param>
    /// <param name="memoryCache"></param>
    /// <param name="logger"></param>
    /// <param name="messageHandler"></param>
    [Theory, AutoMockData]
    public async void ApiResponseWithHealthPathAsync_Returns_Ok(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
        Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var content = "Services are running normally.";
        var status = HttpStatusCode.OK;
        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);
        var result = await apiRepo.ApiResponseAsync(url, new ApiRequest()
        {
            HealthCheck = HealthCheck.Once,
            HealthCheckPath = "cws/2.2/status",
            Headers = apiRepo.AcceptJsonHeader().AddThese(Authentications.Basic.AuthorizationHeader("Basic KeyAbc")),

        });

        //Assert
        Assert.NotNull(result?.Response);
        Assert.Equal(status, result?.HttpStatusCode);
        Assert.IsType<ApiResponse>(result);
    }

    [Theory, AutoMockData]
    public async void GenerateAccessTokenAsync_Returns_Ok(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
        Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var status = HttpStatusCode.OK;
        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);
        var result = await apiRepo.GenerateAccessTokenAsync(new IdpConfigOptions()
        {
            ApiKey = "apiKey",
            ClientId = "clientId",
            ClientSecret = "secret",
            Url = "https://tokenurl.com/accesstoken"
        });

        //Assert
        Assert.NotNull(result);
        Assert.Equal("eyJ0eXAiOiAiSldUIiwgImFQ", result);
    }
    [Theory, AutoMockData]
    public async void GenerateAccess_WithDeserializer_TokenAsync_Returns_Ok(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
        Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var status = HttpStatusCode.OK;
        var tokenResponse = "{\r\n    \"success\": true,\r\n    \"token\": \"a49805dc8ea2f6caf729b97e0e65df84f67aeaab\",\r\n    \"remaining_time\": \"899.998291\"\r\n}";
        MessageHandler(httpFactory, messageHandler, tokenResponse, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);
        var parameters = new Dictionary<string, string>()
            {
                {"username", "abc" },
                {"password", "WP2024" }
            };
        var config = new IdpConfigOptions()
        {
            Url = "https://uat.com/obtain-auth-token/",
            ApiRequest = new()
            {
                Method = HttpMethod.Post,
                Content = new FormUrlEncodedContent(parameters)
            },
            TokenDeserializer = (token) =>
            {
                return new IdpAuthResponse()
                {
                    AccessToken = token.SelectTokenValue<string>("token")!,
                    ExpiresIn = Convert.ToDouble(token.SelectTokenValue<string>("remaining_time")),
                };
            }
        };
        var result = await apiRepo.GenerateAccessTokenAsync(config);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("a49805dc8ea2f6caf729b97e0e65df84f67aeaab", result);
    }

    public static IMemoryCache GetMemoryCache(object expectedValue)
    {
        var mockMemoryCache = new Mock<IMemoryCache>();
        mockMemoryCache
            .Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
            .Returns(true);
        return mockMemoryCache.Object;
    }

    delegate void OutDelegate<TIn, TOut>(TIn input, out TOut output);
    [Theory, AutoMockData]
    public async void GenerateAccessTokenAsync_InMemory_Returns_Ok(Mock<IHttpClientFactory> httpFactory, /*Mock<IMemoryCache> memoryCache,*/
        Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var idp = new IdpConfigOptions()
        {
            ApiKey = "apiKey",
            ClientId = "clientId",
            ClientSecret = "secret",
            Url = "https://tokenurl.com/accesstoken"
        };


        var status = HttpStatusCode.OK;
        string token = "eyJ0eXAiOiAiSldUIiwgImFQ";

        var memoryCache = GetMemoryCache(token);

        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache, logger.Object);
        var result = await apiRepo.GenerateAccessTokenAsync(idp);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(token, result);
    }

    [Theory, AutoMockData]
    public void GenerateAccessTokenAsync_Throws_Exception(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
       Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var status = HttpStatusCode.Unauthorized;
        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);

        //Assert
        var ex = Assert.ThrowsAsync<Exception>(() => apiRepo.GenerateAccessTokenAsync(new IdpConfigOptions()
        {
            ApiKey = "apiKey",
            ClientId = "clientId",
            ClientSecret = "secret",
            Url = "https://tokenurl.com/accesstoken"
        }));
    }

    [Theory, AutoMockData]
    public async void HealthCheck_Returns_Ok(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
      Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var status = HttpStatusCode.Unauthorized;
        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);

        //Assert
        var result = await apiRepo.HealthCheckAsync(url, "cws/2.2/status");
        Assert.Equal("https://api.sampleapis.com/cws/2.2/status", result.Request);
    }

    [Theory, AutoMockData]
    public async void HealthCheckDefaultPath_Returns_Ok(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
      Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var status = HttpStatusCode.OK;
        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);

        //Assert
        var result = await apiRepo.HealthCheckAsync(url);
        Assert.Equal("https://api.sampleapis.com/" + GlobalConstants.HEALTHCHECKPATH, result.Request);
    }
    [Theory, AutoMockData]
    public async void HealthCheckEmptyUrl_Returns_BadRequest(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
      Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var status = HttpStatusCode.BadRequest;
        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);

        //Assert
        var result = await apiRepo.HealthCheckAsync(string.Empty);
        Assert.Equal(result.HttpStatusCode, status);
    }

    [Theory, AutoMockData]
    public async void RetryApiResponseAsync_Returns_Ok(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
      Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var status = HttpStatusCode.Unauthorized;
        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);

        //Assert
        var result = await apiRepo.RetryApiResponseAsync(url);

        Assert.Equal(status, result.HttpStatusCode);
    }
    [Theory, AutoMockData]
    public async void RetryApiResponseAsyncWithoutTimespans_Returns_Ok(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
      Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var status = HttpStatusCode.OK;
        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);
        apiRepo.SetApiModel(null);

        //Assert
        var result = await Assert.ThrowsAsync<ArgumentNullException>(() => apiRepo.RetryApiResponseAsync(url));

        Assert.Equal(GlobalConstants.RETRYAPIMODELINITIAILZE, result.ParamName);
    }

    [Theory, AutoMockData]
    public async void RetryApiResponseAsync_Returns_ServiceUnavailable(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
      Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var count = 0;
        var status = HttpStatusCode.ServiceUnavailable;
        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);

        //Assert
        var result = await apiRepo.RetryApiResponseAsync(url, new ApiRequest()
        {
            Action = async (res, co) =>
            {
                ++count;
                await Task.FromResult(count);
            }
        });
        Assert.Equal(3, count);
    }

    [Theory, AutoMockData]
    public async void RetryApiResponseAsync_OnceCheck_Returns_Ok(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
      Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var status = HttpStatusCode.OK;
        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);

        //Assert
        var result = await apiRepo.RetryApiResponseAsync(url, "/health-check");
        Assert.Equal(result.HttpStatusCode, status);
    }


    [Theory, AutoMockData]
    public async void RetryHealthCheckAsync_Custom_Returns_ServiceUnavailable(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
      Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var count = 4;
        var status = HttpStatusCode.ServiceUnavailable;
        MessageHandler(httpFactory, messageHandler, content, status);
        var retries = 0;

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);
        var healtCheckTimes = Backoff.LinearBackoff(TimeSpan.FromSeconds(1), count, fastFirst: true);
        apiRepo.SetApiModel(new RetryApiModel(healtCheckTimes: healtCheckTimes));
        //Assert
        var result = await apiRepo.RetryHealthCheckAsync(url, new ApiRequest()
        {
            HealthCheckPath = "health-check",
            HealthCheckAction = (res, co) =>
            {
                retries = co;
            }
        });
        Assert.Equal(healtCheckTimes.Count(), retries);
    }

    [Theory, AutoMockData]
    public async void RetryHealthCheckAsync_WithoutTimespans_Returns_Ok(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
      Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var status = HttpStatusCode.OK;
        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);
        apiRepo.SetApiModel(null);

        //Assert
        var result = await Assert.ThrowsAsync<ArgumentNullException>(() => apiRepo.RetryHealthCheckAsync(url, new ApiRequest()));

        Assert.Equal(GlobalConstants.RETRYAPIMODELINITIAILZE, result.ParamName);
    }

    [Theory, AutoMockData]
    public async void ServiceHealthCheckAsync_Returns_ServiceUnavailable(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
      Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var status = HttpStatusCode.ServiceUnavailable;
        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);
        apiRepo.SetApiModel(null);

        //Assert
        await apiRepo.ServiceHealthCheckAsync(url);
    }

    [Theory, AutoMockData]
    public async void ServiceHealthCheckAsyncWithApiRequest_Returns_ServiceUnavailable(Mock<IHttpClientFactory> httpFactory, Mock<IMemoryCache> memoryCache,
      Mock<ILogger<RestApiRepository>> logger, Mock<HttpMessageHandler> messageHandler)
    {
        //Arrange
        var status = HttpStatusCode.ServiceUnavailable;
        MessageHandler(httpFactory, messageHandler, content, status);

        //Act
        RestApiRepository apiRepo = new(httpFactory.Object, memoryCache.Object, logger.Object);
        apiRepo.SetApiModel(null);

        //Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => apiRepo.ServiceHealthCheckAsync(url, new ApiRequest()
        {
            HealthCheck = HealthCheck.Once,
            HealthCheckPath = "cws/2.2/status"
        }));
    }

    private static void MessageHandler(Mock<IHttpClientFactory> httpFactory, Mock<HttpMessageHandler> messageHandler, string content, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
    {
        messageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = httpStatusCode,
                Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json")
            });
        var httpclient = new HttpClient(messageHandler.Object);
        httpFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpclient);
    }
}
