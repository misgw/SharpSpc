using System.Runtime.InteropServices;

namespace SharpSpc;
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12)]
public struct SubDirectory
{
    public int SubfileOffset;

    public int SubfileSize;

    public float ZStart;
}