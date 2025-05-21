using System.Runtime.InteropServices;
using System.Text;

namespace SharpSpc;
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 32)]
public struct SubHeader
{
    public readonly unsafe string Reserved
    {
        get
        {
            fixed (byte* ptr = &_Reserved[0])
            {
                return ReadAsciiString(ptr, 4);
            }
        }
    }

    private static unsafe string ReadAsciiString(byte* data, int length)
    {
        int end = 0;
        while (end < length && data[end] > 0)
        {
            end++;
        }

        return Encoding.ASCII.GetString(data, end);
    }

    public SubfileType SubFlags;

    public sbyte YExponent;

    public short SubfileIndex;

    public float ZStart;

    public float ZEnd;

    public float Noise;

    public int XYXYPointCount;

    public int ScanCount;

    public float WValue;

    public unsafe fixed byte _Reserved[4];
}
