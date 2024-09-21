using System.Runtime.CompilerServices;

namespace CoreLibrary.API.Domain.Services.TimeGuid;


[PublicAPI]
public class ByteArrayComparer : IEqualityComparer<byte[]>, IComparer<byte[]>
{
    public static readonly ByteArrayComparer Instance = new ByteArrayComparer();

    public bool Equals(byte[] x, byte[] y)
    {
        if ((x == null) ^ (y == null))
        {
            return false;
        }

        if (x == y)
        {
            return true;
        }

        if (x.Length != y.Length)
        {
            return false;
        }

        return Memcmp(x, y, x.Length) == 0;
    }

    public int Compare(byte[] x, byte[] y)
    {
        if (x == null)
        {
            if (y != null)
            {
                return -1;
            }

            return 0;
        }

        if (y == null)
        {
            return 1;
        }

        if (x == y)
        {
            return 0;
        }

        if (x.Length < y.Length)
        {
            int num = Memcmp(x, y, x.Length);
            if (num == 0)
            {
                return -1;
            }

            return num;
        }

        if (x.Length > y.Length)
        {
            int num2 = Memcmp(x, y, y.Length);
            if (num2 == 0)
            {
                return 1;
            }

            return num2;
        }

        return Memcmp(x, y, x.Length);
    }

    public unsafe int GetHashCode(byte[] bytes)
    {
        if (bytes == null)
        {
            return 0;
        }

        int num = bytes.Length;
        int num2 = num;
        fixed (byte* ptr = bytes)
        {
            int num3 = 0;
            int* ptr2 = (int*)ptr;
            int num4 = num - 4;
            while (num3 <= num4)
            {
                num2 = (num2 * 397) ^ *ptr2;
                num3 += 4;
                ptr2++;
            }

            byte* ptr3 = (byte*)ptr2;
            while (num3 < num)
            {
                num2 = (num2 * 397) ^ *ptr3;
                num3++;
                ptr3++;
            }

            return num2;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe static int Memcmp(byte[] x, byte[] y, long n)
    {
        if (n == 0L)
        {
            return 0;
        }

        fixed (byte* ptr = x)
        {
            fixed (byte* ptr3 = y)
            {
                byte* ptr2 = ptr;
                byte* ptr4 = ptr3;
                if (n > 4)
                {
                    long num = (long)ptr2 % 4L;
                    long num2 = (long)ptr4 % 4L;
                    if (num != 0L && num == num2)
                    {
                        for (long num3 = 4 - num; num3 > 0; num3--)
                        {
                            int num4 = *(ptr2++) - *(ptr4++);
                            if (num4 != 0)
                            {
                                return num4;
                            }

                            n--;
                        }
                    }

                    uint* ptr5 = (uint*)ptr2;
                    uint* ptr6 = (uint*)ptr4;
                    while (n > 4 && *ptr5 == *ptr6)
                    {
                        ptr5++;
                        ptr6++;
                        n -= 4;
                    }

                    ptr2 = (byte*)ptr5;
                    ptr4 = (byte*)ptr6;
                }

                while (n > 0)
                {
                    int num5 = *(ptr2++) - *(ptr4++);
                    if (num5 != 0)
                    {
                        return num5;
                    }

                    n--;
                }

                return 0;
            }
        }
    }
}
