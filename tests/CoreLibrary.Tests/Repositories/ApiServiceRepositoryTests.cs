using CoreLibrary.API.Domain.Interfaces.Repositories;
using CoreLibrary.API.Domain.Models;
using CoreLibrary.API.Domain.Models.Apis;
using CoreLibrary.API.Domain.Services.Repositories;
using CoreLibrary.API.Domain.Services.TimeGuid;
using CoreLibrary.Tests.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace CoreLibrary.Tests.Repositories;

public class ApiServiceRepositoryTests
{
    private static readonly string content = File.ReadAllText(@"SampleResponse/token.json");
    [Theory, AutoMockData]
    public async Task VisionNetService_ApiRequestAsync(Mock<ILogger<ApiServiceRepository>> logger,
           Mock<IRestApiRepository> restApiRepository)
    {
        //Arrange
        var config = new ConfigurationBuilder().AddJsonFile("SampleResponse/appsettings.test.json").Build();

        restApiRepository.Setup(x => x.ApiResponseAsync(It.IsAny<string>(), It.IsAny<ApiRequest>())).ReturnsAsync(new ApiResponse(HttpStatusCode.OK, content));
        restApiRepository.Setup(x => x.SetApiModel(It.IsAny<RetryApiModel>()));

        //Act
        ApiServiceRepository serviceManager = new(restApiRepository.Object, logger.Object, config);
        var result = await serviceManager.ApiRequestAsync("docId", "Auth", "Type1");

        //Assert
        Assert.True(result.IsSuccess);
    }

    [Theory, AutoMockData]
    public async Task VisionNetService_RetryApiRequestAsync(Mock<ILogger<ApiServiceRepository>> logger,
           Mock<IRestApiRepository> restApiRepository)
    {
        //Arrange
        var config = new ConfigurationBuilder().AddJsonFile("SampleResponse/appsettings.test.json").Build();

        restApiRepository.Setup(x => x.RetryApiResponseAsync(It.IsAny<string>(), It.IsAny<ApiRequest>())).ReturnsAsync(new ApiResponse(HttpStatusCode.OK, content));
        restApiRepository.Setup(x => x.SetApiModel(It.IsAny<RetryApiModel>()));

        //Act
        ApiServiceRepository serviceManager = new(restApiRepository.Object, logger.Object, config);
        var result = await serviceManager.RetryApiRequestAsync("docId", "Auth", "Type1", "Retry:Api");

        //Assert
        Assert.True(result.IsSuccess);
    }
    [Theory, AutoMockData]
    public async Task VisionNetService_RetryApiRequestAsync_WithoutRetryPath(Mock<ILogger<ApiServiceRepository>> logger,
           Mock<IRestApiRepository> restApiRepository)
    {
        //Arrange
        var config = new ConfigurationBuilder().AddJsonFile("SampleResponse/appsettings.test.json").Build();

        restApiRepository.Setup(x => x.RetryApiResponseAsync(It.IsAny<string>(), It.IsAny<ApiRequest>())).ReturnsAsync(new ApiResponse(HttpStatusCode.OK, content));
        restApiRepository.Setup(x => x.SetApiModel(It.IsAny<RetryApiModel>()));

        //Act
        ApiServiceRepository serviceManager = new(restApiRepository.Object, logger.Object, config);
        var result = await serviceManager.RetryApiRequestAsync("docId", "Auth", "Type1");

        //Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void UniqueTimeUUID_Test()
    {
        var range = 10000;
        var data = Enumerable.Range(1, range).Select(index =>
        {
            return Task.Factory.StartNew(() =>
            {
                var ts = Timestamp.Now;
                return new KeyValuePair<Timestamp, Guid>(ts, TimeGuid.NewGuid(ts).ToGuid());
            });

        }).ToList();

        var list = Task.WhenAll(data).Result;
        var ticks = TimeGuid.Parse(list[0].Value.ToString()).GetTimestamp().Ticks;
        Assert.Equal(range, list.DistinctBy((dt) => dt.Value).Count());
    }
}
