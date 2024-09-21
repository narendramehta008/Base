using CoreLibrary.API.Domain.Services;
using JetBrains.Annotations;
using System.Diagnostics;

namespace CoreLibrary.API.Domain.Services.TimeGuid;

[PublicAPI]
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class PreciseTimestampGenerator
{
    public const long TicksPerMicrosecond = 10L;

    private static readonly double stopwatchTickFrequency = 10000000.0 / Stopwatch.Frequency;

    [NotNull]
    public static readonly PreciseTimestampGenerator Instance = new PreciseTimestampGenerator(TimeSpan.FromSeconds(1.0), TimeSpan.FromMilliseconds(100.0));

    private readonly long syncPeriodTicks;

    private readonly long maxAllowedDivergenceTicks;

    private long baseTimestampTicks;

    private long lastTimestampTicks;

    private long stopwatchStartTimestamp;

    public PreciseTimestampGenerator(TimeSpan syncPeriod, TimeSpan maxAllowedDivergence)
    {
        if (!Stopwatch.IsHighResolution)
        {
            throw new InvalidOperationException("Stopwatch is not based on a high-resolution timer");
        }

        syncPeriodTicks = syncPeriod.Ticks;
        maxAllowedDivergenceTicks = maxAllowedDivergence.Ticks;
        baseTimestampTicks = DateTime.UtcNow.Ticks;
        lastTimestampTicks = baseTimestampTicks;
        stopwatchStartTimestamp = Stopwatch.GetTimestamp();
    }

    public long NowTicks()
    {
        long num = Volatile.Read(ref lastTimestampTicks);
        long num2;
        while (true)
        {
            num2 = GenerateNextTimestamp(num);
            long num3 = Interlocked.CompareExchange(ref lastTimestampTicks, num2, num);
            if (num3 == num)
            {
                break;
            }

            num = num3;
        }

        return num2;
    }

    private long GenerateNextTimestamp(long localLastTimestampTicks)
    {
        long ticks = DateTime.UtcNow.Ticks;
        long num = Volatile.Read(ref baseTimestampTicks);
        long num2 = GetDateTimeTicks(Stopwatch.GetTimestamp() - stopwatchStartTimestamp);
        if (num2 > syncPeriodTicks)
        {
            lock (this)
            {
                num = baseTimestampTicks = ticks;
                stopwatchStartTimestamp = Stopwatch.GetTimestamp();
                num2 = 0L;
            }
        }

        long num3 = Math.Max(num + num2, localLastTimestampTicks + 10);
        if (num2 < 0 || Math.Abs(num3 - ticks) > maxAllowedDivergenceTicks)
        {
            return Math.Max(ticks, localLastTimestampTicks + 10);
        }

        return num3;
    }

    public static long GetDateTimeTicks(long stopwatchTicks)
    {
        double num = stopwatchTicks;
        num *= stopwatchTickFrequency;
        return (long)num;
    }
}