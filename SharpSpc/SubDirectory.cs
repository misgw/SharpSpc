using System.Runtime.InteropServices;

namespace SharpSpc;
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12)]
public struct SubDirectory
{
    // Token: 0x040000A3 RID: 163
    public int SubfileOffset;

    // Token: 0x040000A4 RID: 164
    public int SubfileSize;

    // Token: 0x040000A5 RID: 165
    public float ZStart;
}