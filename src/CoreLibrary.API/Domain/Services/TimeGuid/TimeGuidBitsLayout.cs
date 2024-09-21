using CoreLibrary.API.Domain.Services.TimeGuid.Enums;
using JetBrains.Annotations;

namespace CoreLibrary.API.Domain.Services.TimeGuid;

[PublicAPI]
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public static class TimeGuidBitsLayout
{
    private const int signBitMask = 128;

    private const int versionOffset = 6;

    private const byte versionByteMask = 15;

    private const int versionByteShift = 4;

    private const int variantOffset = 8;

    private const byte variantByteMask = 63;

    private const byte variantBitsValue = 128;

    private const int clockSequenceHighByteOffset = 8;

    private const int clockSequenceLowByteOffset = 9;

    public const int TimeGuidSize = 16;

    public const int NodeSize = 6;

    private const int nodeOffset = 10;

    public const ushort MinClockSequence = 0;

    public const ushort MaxClockSequence = 16383;

    public static readonly byte[] MinNode = new byte[6];

    public static readonly byte[] MaxNode = new byte[6] { 255, 255, 255, 255, 255, 255 };

    public static readonly Timestamp GregorianCalendarStart = new Timestamp(new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc).Ticks);

    public static readonly Timestamp GregorianCalendarEnd = new Timestamp(new DateTime(1652084544606846975L, DateTimeKind.Utc).Ticks);

    public static readonly byte[] MinTimeGuid = Format(GregorianCalendarStart, 0, MinNode);

    public static readonly byte[] MaxTimeGuid = Format(GregorianCalendarEnd, 16383, MaxNode);

    [NotNull]
    public static byte[] Format([NotNull] Timestamp timestamp, ushort clockSequence, [NotNull] byte[] node)
    {
        if (node.Length != 6)
        {
            throw new InvalidOperationException($"node must be {6} bytes long");
        }

        if (timestamp < GregorianCalendarStart)
        {
            throw new InvalidOperationException($"timestamp must not be less than {GregorianCalendarStart}");
        }

        if (timestamp > GregorianCalendarEnd)
        {
            throw new InvalidOperationException($"timestamp must not be greater than {GregorianCalendarEnd}");
        }

        if (clockSequence > 16383)
        {
            throw new InvalidOperationException($"clockSequence must not be greater than {(ushort)16383}");
        }

        long ticks = (timestamp - GregorianCalendarStart).Ticks;
        byte[] bytes = EndianBitConverter.Little.GetBytes(ticks);
        byte[] bytes2 = EndianBitConverter.Big.GetBytes(clockSequence);
        byte[] array = new byte[16]
        {
            bytes[3],
            bytes[2],
            bytes[1],
            bytes[0],
            bytes[5],
            bytes[4],
            bytes[7],
            bytes[6],
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
        };
        int num = 8;
        for (int i = 0; i < 2; i++)
        {
            array[num++] = (byte)(bytes2[i] ^ 0x80u);
        }

        for (int j = 0; j < 6; j++)
        {
            array[num++] = (byte)(node[j] ^ 0x80u);
        }

        array[6] &= 15;
        array[6] |= 16;
        array[8] &= 63;
        array[8] |= 128;
        return array;
    }

    public static GuidVersion GetVersion([NotNull] byte[] bytes)
    {
        if (bytes.Length != 16)
        {
            throw new InvalidOperationException($"bytes must be {16} bytes long");
        }

        return (GuidVersion)(bytes[6] >> 4);
    }

    [NotNull]
    public static Timestamp GetTimestamp([NotNull] byte[] bytes)
    {
        if (bytes.Length != 16)
        {
            throw new InvalidOperationException($"bytes must be {16} bytes long");
        }

        byte[] array = new byte[8]
        {
            bytes[3],
            bytes[2],
            bytes[1],
            bytes[0],
            bytes[5],
            bytes[4],
            bytes[7],
            bytes[6]
        };
        array[^1] &= 15;
        long num = EndianBitConverter.Little.ToInt64(array, 0);
        return new Timestamp(num + GregorianCalendarStart.Ticks);
    }

    public static ushort GetClockSequence([NotNull] byte[] bytes)
    {
        if (bytes.Length != 16)
        {
            throw new InvalidOperationException($"bytes must be {16} bytes long");
        }

        byte b = (byte)(bytes[8] ^ 0x80u);
        byte b2 = (byte)(bytes[9] ^ 0x80u);
        return EndianBitConverter.Big.ToUInt16(new byte[2] { b, b2 }, 0);
    }

    [NotNull]
    public static byte[] GetNode([NotNull] byte[] bytes)
    {
        if (bytes.Length != 16)
        {
            throw new InvalidOperationException($"bytes must be {16} bytes long");
        }

        byte[] array = new byte[6];
        for (int i = 0; i < 6; i++)
        {
            array[i] = (byte)(bytes[10 + i] ^ 0x80u);
        }

        return array;
    }

    [NotNull]
    public static byte[] IncrementNode([NotNull] byte[] nodeBytes)
    {
        if (nodeBytes.Length != 6)
        {
            throw new InvalidOperationException($"nodeBytes must be {6} bytes long");
        }

        bool flag = true;
        byte[] array = new byte[6];
        for (int num = 5; num >= 0; num--)
        {
            int num2 = flag ? nodeBytes[num] + 1 : nodeBytes[num];
            if (num2 > 255)
            {
                array[num] = 0;
                flag = true;
            }
            else
            {
                array[num] = (byte)num2;
                flag = false;
            }
        }

        if (flag)
        {
            throw new InvalidOperationException("Cannot increment MaxNode");
        }

        return array;
    }

    [NotNull]
    public static byte[] DecrementNode([NotNull] byte[] nodeBytes)
    {
        if (nodeBytes.Length != 6)
        {
            throw new InvalidOperationException($"nodeBytes must be {6} bytes long");
        }

        bool flag = true;
        byte[] array = new byte[6];
        for (int num = 5; num >= 0; num--)
        {
            int num2 = flag ? nodeBytes[num] - 1 : nodeBytes[num];
            if (num2 < 0)
            {
                array[num] = byte.MaxValue;
                flag = true;
            }
            else
            {
                array[num] = (byte)num2;
                flag = false;
            }
        }

        if (flag)
        {
            throw new InvalidOperationException("Cannot decrement MinNode");
        }

        return array;
    }
}