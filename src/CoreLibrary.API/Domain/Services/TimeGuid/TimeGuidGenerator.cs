using JetBrains.Annotations;

namespace CoreLibrary.API.Domain.Services.TimeGuid;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
internal class TimeGuidGenerator
{
    private readonly PreciseTimestampGenerator preciseTimestampGenerator;

    public TimeGuidGenerator([NotNull] PreciseTimestampGenerator preciseTimestampGenerator)
    {
        this.preciseTimestampGenerator = preciseTimestampGenerator;
    }

    [NotNull]
    public byte[] NewGuid()
    {
        Timestamp timestamp = new Timestamp(preciseTimestampGenerator.NowTicks());
        return TimeGuidBitsLayout.Format(timestamp, GenerateRandomClockSequence(), GenerateRandomNode());
    }

    [NotNull]
    public byte[] NewGuid([NotNull] Timestamp timestamp)
    {
        return TimeGuidBitsLayout.Format(timestamp, GenerateRandomClockSequence(), GenerateRandomNode());
    }

    [NotNull]
    public byte[] NewGuid([NotNull] Timestamp timestamp, ushort clockSequence)
    {
        return TimeGuidBitsLayout.Format(timestamp, clockSequence, GenerateRandomNode());
    }

    [NotNull]
    public static byte[] GenerateRandomNode()
    {
        byte[] array = new byte[6];
        ThreadLocalRandom.Instance.NextBytes(array);
        return array;
    }

    public static ushort GenerateRandomClockSequence()
    {
        return (ushort)ThreadLocalRandom.Instance.Next(0, 16384);
    }
}

public static class ThreadLocalRandom
{
    private static readonly ThreadLocal<Random> threadLocalRandom = new ThreadLocal<Random>(delegate
    {
        lock (globalRandom)
        {
            return new Random(globalRandom.Next());
        }
    });

    private static readonly Random globalRandom = new Random(Environment.TickCount);

    [NotNull]
    public static Random Instance => threadLocalRandom.Value;
}