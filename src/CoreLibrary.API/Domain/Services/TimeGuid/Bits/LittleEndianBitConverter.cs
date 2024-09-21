namespace CoreLibrary.API.Domain.Services.TimeGuid.Bits;


[PublicAPI]
public sealed class LittleEndianBitConverter : EndianBitConverter
{
    public override Endianness Endianness => Endianness.LittleEndian;

    public override bool IsLittleEndian()
    {
        return true;
    }

    protected override void CopyBytesImpl(long value, int bytes, byte[] buffer, int index)
    {
        for (int i = 0; i < bytes; i++)
        {
            buffer[i + index] = (byte)(value & 0xFF);
            value >>= 8;
        }
    }

    protected override long FromBytes(byte[] buffer, int startIndex, int bytesToConvert)
    {
        long num = 0L;
        for (int i = 0; i < bytesToConvert; i++)
        {
            num = (num << 8) | buffer[startIndex + bytesToConvert - 1 - i];
        }

        return num;
    }
}

[PublicAPI]
public sealed class BigEndianBitConverter : EndianBitConverter
{
    public override Endianness Endianness => Endianness.BigEndian;

    public override bool IsLittleEndian()
    {
        return false;
    }

    protected override void CopyBytesImpl(long value, int bytes, byte[] buffer, int index)
    {
        int num = index + bytes - 1;
        for (int i = 0; i < bytes; i++)
        {
            buffer[num - i] = (byte)(value & 0xFF);
            value >>= 8;
        }
    }

    protected override long FromBytes(byte[] buffer, int startIndex, int bytesToConvert)
    {
        long num = 0L;
        for (int i = 0; i < bytesToConvert; i++)
        {
            num = (num << 8) | buffer[startIndex + i];
        }

        return num;
    }
}
[PublicAPI]
public enum Endianness
{
    LittleEndian,
    BigEndian
}