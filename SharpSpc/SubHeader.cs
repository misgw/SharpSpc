using System.Runtime.InteropServices;
using System.Text;

namespace SharpSpc;
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 32)]
public struct SubHeader
{
    // Token: 0x17000029 RID: 41
    // (get) Token: 0x0600007D RID: 125 RVA: 0x00003B50 File Offset: 0x00001D50
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

    // Token: 0x0600007E RID: 126 RVA: 0x00003B7C File Offset: 0x00001D7C
    private static unsafe string ReadAsciiString(byte* data, int length)
    {
        int end = 0;
        while (end < length && data[end] > 0)
        {
            end++;
        }

        return Encoding.ASCII.GetString(data, end);
    }

    // Token: 0x040000A9 RID: 169
    public SubfileType SubFlags;

    // Token: 0x040000AA RID: 170
    public sbyte YExponent;

    // Token: 0x040000AB RID: 171
    public short SubfileIndex;

    // Token: 0x040000AC RID: 172
    public float ZStart;

    // Token: 0x040000AD RID: 173
    public float ZEnd;

    // Token: 0x040000AE RID: 174
    public float Noise;

    // Token: 0x040000AF RID: 175
    public int XYXYPointCount;

    // Token: 0x040000B0 RID: 176
    public int ScanCount;

    // Token: 0x040000B1 RID: 177
    public float WValue;

    // Token: 0x040000B2 RID: 178
    public unsafe fixed byte _Reserved[4];
}
