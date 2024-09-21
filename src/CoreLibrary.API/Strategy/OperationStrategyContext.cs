using CoreLibrary.API.Domain.Interfaces.Services;
using CoreLibrary.API.Domain.Models.Services;

namespace CoreLibrary.API.Strategy;

public class OperationStrategyContext(IOperationStrategy _serviceStrategy)
{
    public void SetStrategy(IOperationStrategy ServiceStrategy)
    {
        _serviceStrategy = ServiceStrategy;
    }

    public async Task<bool> ExecuteStrategy(OperationStrategyModel strategyModel)
    {
        return await _serviceStrategy.Execute(strategyModel);
    }
}
