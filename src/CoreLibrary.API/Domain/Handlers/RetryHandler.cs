using CoreLibrary.API.Domain.Constants;
using CoreLibrary.API.Domain.Models;
using Polly.Contrib.WaitAndRetry;
using Polly;
using System.Diagnostics.CodeAnalysis;
using Serilog;
using CoreLibrary.API.Domain.Extensions;

namespace CoreLibrary.API.Domain.Handlers;

[ExcludeFromCodeCoverage]
public static class RetryHandler
{
    public static readonly IEnumerable<TimeSpan> DefaultTimeSpans = Backoff.ExponentialBackoff(TimeSpan.FromSeconds(GlobalConstants.INITIALDELAY), GlobalConstants.THRESHOLDLIMIT);

    /// <summary>
    /// Synchronous Consecutive hits
    /// </summary>
    /// <param name="retryType"></param>
    /// <param name="action"></param>
    /// <param name="timeSpans"></param>
    /// <returns></returns>
    public static IAsyncPolicy<ApiResponse> HttpRetryPolicy(this string retryType, Action<ApiResponse, int> action, ApiRequest apiRequest, IEnumerable<TimeSpan>? timeSpans = null)
    {
        timeSpans ??= DefaultTimeSpans;
        return Policy<ApiResponse>
        .Handle<HttpRequestException>()
        .OrResult(x => (apiRequest.OffsetResponseSuccessStatusForRetry ? x.IsSuccess : !x.IsSuccess) && x.HttpStatusCode != System.Net.HttpStatusCode.BadRequest && (apiRequest.ResultPredicate == null || apiRequest.ResultPredicate(x)))
        .WaitAndRetryAsync(
            timeSpans, (exception, timeSpan) =>
            {
                var attemptNumber = timeSpans.IndexOf(timeSpan) + 1;
                Log.Error("Failed to request: {type}, attempt: {attemptNumber}, message: {message}", retryType, attemptNumber, exception.Result.Response ?? exception.Exception?.Message);
                action(exception.Result, attemptNumber);
            });
    }

    /// <summary>
    /// Synchronous Consecutive hits
    /// </summary>
    /// <param name="retryType"></param>
    /// <param name="action"></param>
    /// <param name="timeSpans"></param>
    /// <returns></returns>
    public static IAsyncPolicy<ApiResponse> HttpRetryPolicy(this string retryType, Action<ApiResponse, int> action, Action<ApiResponse, int> finalAction, IEnumerable<TimeSpan>? timeSpans = null)
    {
        timeSpans ??= DefaultTimeSpans;

        return Policy<ApiResponse>
      .Handle<HttpRequestException>()
       .OrResult(x => !x.IsSuccess && x.HttpStatusCode != System.Net.HttpStatusCode.BadRequest)
      .WaitAndRetryAsync(
          timeSpans, (exception, timeSpan) =>
          {
              var attemptNumber = timeSpans.IndexOf(timeSpan) + 1;
              Log.Error("Failed to request: {type}, attempt: {attemptNumber}, message: {message}", retryType, attemptNumber, exception.Result.Response ?? exception.Exception?.Message);

              if (attemptNumber == GlobalConstants.THRESHOLDLIMIT)
                  finalAction(exception.Result, attemptNumber);
              else action(exception.Result, attemptNumber);
          });
    }

    /// <summary>
    /// In cycles checking whether the modified time and retry time meets the conditions
    /// </summary>
    /// <param name="retryType"></param>
    /// <param name="action"></param>
    /// <param name="timeSpans"></param>
    /// <returns></returns>
    public static bool CheckNumberOfRetry(this RetryCycleModel retry, Action? dbAction = null)
    {
        var difference = DateTime.UtcNow.Subtract(retry.DateModified).TotalMinutes;

        var isRetry = retry.RetryChecks.Any(a => a.Key > retry.NumberOfRetry && a.Value <= difference);

        if (isRetry)
        {
            dbAction?.Invoke();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Generate the Linear Increment delays for Cycles check
    /// </summary>
    /// <param name="initialDelay"></param>
    /// <param name="retryCycles"></param>
    /// <param name="factor"></param>
    /// <param name="fastFirst"></param>
    /// <param name="retryIncrement"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<KeyValuePair<int, int>> LinearIncrement(int initialDelay, int retryCycles, double factor = 1.0, bool fastFirst = false, int retryIncrement = 3)
    {
        if (initialDelay <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(initialDelay)} , {initialDelay}, should be >= 0ms");
        }

        if (retryCycles < 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(retryCycles)}, {retryCycles}, should be >= 0");
        }
        if (retryIncrement <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(retryIncrement)}, {retryIncrement}, should be >= 0");
        }

        if (factor < 0.0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(factor)}, {factor}, should be >= 0");
        }

        if (retryCycles == 0)
        {
            return new Dictionary<int, int>();
        }

        return Enumerate(initialDelay, retryCycles, fastFirst, factor, retryIncrement);
        static IEnumerable<KeyValuePair<int, int>> Enumerate(int initial, int retry, bool fast, double f, int retryIncrement)
        {
            int i = 0;
            int r = retryIncrement;
            if (fast)
            {
                i++;
                r += retryIncrement;
                yield return new KeyValuePair<int, int>(retryIncrement, 0);
            }

            double ad = f * initial;
            int delay = initial;

            while (i < retry)
            {
                yield return new KeyValuePair<int, int>(r, delay);
                i++;
                r += retryIncrement;
                delay += (int)ad;
            }
        }
    }
    public static IEnumerable<KeyValuePair<int, int>> LinearIncrement(this Cycle cycle)
        => LinearIncrement(cycle.InitialDelay, cycle.TotalCycles, cycle.Factor, cycle.FastFirst, cycle.Increment);
    public static IEnumerable<TimeSpan> ExponentialBackoff(this Api api)
        => Backoff.ExponentialBackoff(TimeSpan.FromSeconds(api.InitialDelay), api.Attempts, api.Factor, api.FastFirst);
    public static (bool, double) CheckRetry(this RetryCycleModel retry, Action? dbAction = null)
    {
        var difference = DateTime.UtcNow.Subtract(retry.DateModified).TotalMinutes;

        KeyValuePair<int, int>? nextRetry = retry.RetryChecks.FirstOrDefault(a => a.Key > retry.NumberOfRetry);

        if (nextRetry is not null)
        {
            var timeLeft = nextRetry.Value.Value - difference;
            if (timeLeft > 0)
                return (false, timeLeft);
            return (true, timeLeft);
        }
        return (false, 0);
    }
}
