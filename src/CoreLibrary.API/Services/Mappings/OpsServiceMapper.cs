using CoreLibrary.API.Domain.Extensions;
using CoreLibrary.API.Domain.Interfaces.Services;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CoreLibrary.API.Services.Mappings;

[ExcludeFromCodeCoverage]
public static class OpsServiceMapper
{
    public static Dictionary<Predicate<int>, Type> Dict { get; set; } = new();

    public static void TrySet(IEnumerable<SubOperationType> subOps)
    {
        if (!subOps.Any() || Dict.Any())
            return;
        Set(subOps);
    }

    public static void Set(IEnumerable<SubOperationType> subOps, string suffix = "Strategy")
    {
        Dict = [];
        var serviceStrategies = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.GetInterfaces().Any(b => b == typeof(IOperationStrategy)));

        foreach (var operationType in subOps)
        {
            // To avoid adding callback subtype as it is updated by callback and logic can't implement in BG
            if (serviceStrategies.TryGetFirst(a => a.Name.Contains(operationType.Code + suffix, StringComparison.OrdinalIgnoreCase), out Type? type))
                Dict.Add((opType) => { return opType == operationType.Id; }, type!);
        }
    }

    public static Type GetService(int operationType)
    {
        return Dict.FirstOrDefault((a) => a.Key(operationType)).Value;
    }
}

public class SubOperationType
{
    public required string Code { get; set; }
    public int Id { get; internal set; }
}