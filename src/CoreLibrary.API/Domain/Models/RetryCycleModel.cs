using CoreLibrary.API.Domain.Constants;
using CoreLibrary.API.Domain.Handlers;
using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.API.Domain.Models;

[ExcludeFromCodeCoverage]
public class RetryCycleModel
{
    public static readonly IEnumerable<KeyValuePair<int, int>> DefaultRetryChecks = RetryHandler.LinearIncrement(GlobalConstants.CYCLEINITIALDELAY, GlobalConstants.RETRYCYCLES, fastFirst: true);

    public RetryCycleModel(DateTime dateModified, int numberOfRetry, IEnumerable<KeyValuePair<int, int>>? retryChecks = null)
    {
        DateModified = dateModified;
        NumberOfRetry = numberOfRetry;
        RetryChecks = retryChecks ?? DefaultRetryChecks;
    }

    public DateTime DateModified { get; }
    public int NumberOfRetry { get; }

    /// <summary>
    /// Key is Attempts and value is Min time difference check
    /// </summary>
    public IEnumerable<KeyValuePair<int, int>> RetryChecks { get; }
}
