using CoreLibrary.API.Domain.Services.TimeGuid.Enums;
using JetBrains.Annotations;

namespace CoreLibrary.API.Domain.Services.TimeGuid;

[PublicAPI]
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public sealed class TimeGuid : IEquatable<TimeGuid>, IComparable<TimeGuid>, IComparable
{
    [NotNull]
    public static readonly TimeGuid MinValue = new TimeGuid(TimeGuidBitsLayout.MinTimeGuid);

    [NotNull]
    public static readonly TimeGuid MaxValue = new TimeGuid(TimeGuidBitsLayout.MaxTimeGuid);

    private static readonly TimeGuidGenerator guidGen = new TimeGuidGenerator(PreciseTimestampGenerator.Instance);

    private byte[] bytes { get; }

    public TimeGuid([NotNull] Timestamp timestamp, ushort clockSequence, [NotNull] byte[] node)
        : this(TimeGuidBitsLayout.Format(timestamp, clockSequence, node))
    {
    }

    public TimeGuid([NotNull] byte[] bytes)
    {
        if (TimeGuidBitsLayout.GetVersion(bytes) != GuidVersion.TimeBased)
        {
            throw new InvalidOperationException("Invalid v1 guid: [" + string.Join(", ", bytes.Select((x) => x.ToString("x2"))) + "]");
        }

        this.bytes = bytes;
    }

    public TimeGuid(Guid guid)
    {
        byte[] array = ReorderGuidBytesInCassandraWay(guid.ToByteArray());
        if (TimeGuidBitsLayout.GetVersion(array) != GuidVersion.TimeBased)
        {
            throw new InvalidOperationException($"Invalid v1 guid: {guid}");
        }

        bytes = array;
    }

    public static bool IsTimeGuid(Guid guid)
    {
        byte[] array = ReorderGuidBytesInCassandraWay(guid.ToByteArray());
        return TimeGuidBitsLayout.GetVersion(array) == GuidVersion.TimeBased;
    }

    [NotNull]
    public static TimeGuid Parse([CanBeNull] string str)
    {
        if (!TryParse(str, out var result))
        {
            throw new InvalidOperationException("Cannot parse TimeGuid from: " + str);
        }

        return result;
    }

    public static bool TryParse([CanBeNull] string str, out TimeGuid result)
    {
        result = null;
        if (!Guid.TryParse(str, out var result2))
        {
            return false;
        }

        byte[] array = ReorderGuidBytesInCassandraWay(result2.ToByteArray());
        if (TimeGuidBitsLayout.GetVersion(array) != GuidVersion.TimeBased)
        {
            return false;
        }

        result = new TimeGuid(array);
        return true;
    }

    [NotNull]
    public Timestamp GetTimestamp()
    {
        return TimeGuidBitsLayout.GetTimestamp(bytes);
    }

    public ushort GetClockSequence()
    {
        return TimeGuidBitsLayout.GetClockSequence(bytes);
    }

    [NotNull]
    public byte[] GetNode()
    {
        return TimeGuidBitsLayout.GetNode(bytes);
    }

    [NotNull]
    public byte[] ToByteArray()
    {
        return bytes;
    }

    public Guid ToGuid()
    {
        return new Guid(ReorderGuidBytesInCassandraWay(bytes));
    }

    public override string ToString()
    {
        return $"Guid: {ToGuid()}, Timestamp: {GetTimestamp()}, ClockSequence: {GetClockSequence()}";
    }

    public bool Equals([CanBeNull] TimeGuid other)
    {
        if ((object)other == null)
        {
            return false;
        }

        if ((object)this == other)
        {
            return true;
        }

        return ByteArrayComparer.Instance.Equals(bytes, other.bytes);
    }

    public override bool Equals([CanBeNull] object other)
    {
        if (other == null)
        {
            return false;
        }

        if (this == other)
        {
            return true;
        }

        if (other is TimeGuid other2)
        {
            return Equals(other2);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return ByteArrayComparer.Instance.GetHashCode(bytes);
    }

    public int CompareTo([CanBeNull] TimeGuid other)
    {
        if (other == null)
        {
            return 1;
        }

        int num = GetTimestamp().CompareTo(other.GetTimestamp());
        if (num != 0)
        {
            return num;
        }

        num = GetClockSequence().CompareTo(other.GetClockSequence());
        if (num != 0)
        {
            return num;
        }

        byte[] node = GetNode();
        byte[] node2 = other.GetNode();
        if (node.Length != node2.Length)
        {
            throw new InvalidOperationException($"Node lengths are different for: {this} and {other}");
        }

        for (int i = 0; i < node.Length; i++)
        {
            num = node[i].CompareTo(node2[i]);
            if (num != 0)
            {
                return num;
            }
        }

        return 0;
    }

    public int CompareTo([CanBeNull] object other)
    {
        return CompareTo(other as TimeGuid);
    }

    public static bool operator ==(TimeGuid left, TimeGuid right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TimeGuid left, TimeGuid right)
    {
        return !Equals(left, right);
    }

    public static bool operator >([CanBeNull] TimeGuid left, [CanBeNull] TimeGuid right)
    {
        return Comparer<TimeGuid>.Default.Compare(left, right) > 0;
    }

    public static bool operator <(TimeGuid left, TimeGuid right)
    {
        return Comparer<TimeGuid>.Default.Compare(left, right) < 0;
    }

    public static bool operator >=([CanBeNull] TimeGuid left, [CanBeNull] TimeGuid right)
    {
        return Comparer<TimeGuid>.Default.Compare(left, right) >= 0;
    }

    public static bool operator <=(TimeGuid left, TimeGuid right)
    {
        return Comparer<TimeGuid>.Default.Compare(left, right) <= 0;
    }

    [NotNull]
    public static TimeGuid NowGuid()
    {
        return new TimeGuid(guidGen.NewGuid());
    }

    [NotNull]
    public static TimeGuid NewGuid([NotNull] Timestamp timestamp)
    {
        return new TimeGuid(guidGen.NewGuid(timestamp));
    }

    [NotNull]
    public static TimeGuid NewGuid([NotNull] Timestamp timestamp, ushort clockSequence)
    {
        return new TimeGuid(guidGen.NewGuid(timestamp, clockSequence));
    }

    [NotNull]
    public static TimeGuid MinForTimestamp([NotNull] Timestamp timestamp)
    {
        return new TimeGuid(TimeGuidBitsLayout.Format(timestamp, 0, TimeGuidBitsLayout.MinNode));
    }

    [NotNull]
    public static TimeGuid MaxForTimestamp([NotNull] Timestamp timestamp)
    {
        return new TimeGuid(TimeGuidBitsLayout.Format(timestamp, 16383, TimeGuidBitsLayout.MaxNode));
    }

    [NotNull]
    private static byte[] ReorderGuidBytesInCassandraWay([NotNull] byte[] b)
    {
        if (b.Length != 16)
        {
            throw new InvalidOperationException("b must be 16 bytes long");
        }

        return new byte[16]
        {
            b[3],
            b[2],
            b[1],
            b[0],
            b[5],
            b[4],
            b[7],
            b[6],
            b[8],
            b[9],
            b[10],
            b[11],
            b[12],
            b[13],
            b[14],
            b[15]
        };
    }

    [NotNull]
    public TimeGuid Before()
    {
        Timestamp timestamp = GetTimestamp();
        if (this == MinForTimestamp(timestamp))
        {
            return MaxForTimestamp(timestamp - TimeSpan.FromTicks(1L));
        }

        if (ByteArrayComparer.Instance.Equals(GetNode(), TimeGuidBitsLayout.MinNode))
        {
            return new TimeGuid(timestamp, (ushort)(GetClockSequence() - 1), TimeGuidBitsLayout.MaxNode);
        }

        return new TimeGuid(timestamp, GetClockSequence(), TimeGuidBitsLayout.DecrementNode(GetNode()));
    }
}