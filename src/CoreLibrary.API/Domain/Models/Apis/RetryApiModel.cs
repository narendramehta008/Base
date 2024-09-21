using CoreLibrary.API.Domain.Constants;
using Polly.Contrib.WaitAndRetry;
using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.API.Domain.Models.Apis;

[ExcludeFromCodeCoverage]
public class RetryApiModel
{
    public RetryApiModel(RetryCycleModel? retryCycleModel = null, IEnumerable<TimeSpan>? healtCheckTimes = null, IEnumerable<TimeSpan>? timeSpans = null)
    {
        RetryCycleModel = retryCycleModel;
        HealthCheckTimes = healtCheckTimes ?? Backoff.ExponentialBackoff(TimeSpan.FromSeconds(GlobalConstants.HEALTHCHECKINITIALDELAY), GlobalConstants.THRESHOLDLIMIT, fastFirst: true);
        TimeSpans = timeSpans ?? Backoff.ExponentialBackoff(TimeSpan.FromSeconds(GlobalConstants.INITIALDELAY), GlobalConstants.THRESHOLDLIMIT);
    }

    public IEnumerable<TimeSpan>? HealthCheckTimes { get; }
    public RetryCycleModel? RetryCycleModel { get; }
    public IEnumerable<TimeSpan>? TimeSpans { get; }
}

[ExcludeFromCodeCoverage]
public class BaseApiModel
{
    public BaseApiModel(string url, string? version = null)
    {
        Url = url ?? throw new ArgumentNullException(url);
        if (!Url.EndsWith('/')) Url += "/";
        Version = version ?? GlobalConstants.DEFAULTAPIVERSION;
    }

    public string Url { get; set; }
    public string Version { get; }
}