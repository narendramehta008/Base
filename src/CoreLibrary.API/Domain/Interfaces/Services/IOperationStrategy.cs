using CoreLibrary.API.Domain.Models.Services;

namespace CoreLibrary.API.Domain.Interfaces.Services;

public interface IOperationStrategy
{
    Task<bool> Execute(OperationStrategyModel strategyModel);
}
