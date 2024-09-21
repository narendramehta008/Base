using CoreLibrary.API.Domain.Interfaces.Services;
using CoreLibrary.API.Domain.Models.Services;
namespace CoreLibrary.API.Strategy;

public class TestStrategy(ILogger<TestStrategy> _logger) : IOperationStrategy
{
    public async Task<bool> Execute(OperationStrategyModel strategyModel)
    {
        _logger.LogInformation("Started Received {operation} step running at: {time}", GetType().Name, DateTimeOffset.UtcNow);
        return true;
    }
}