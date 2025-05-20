using System.Runtime.InteropServices;

namespace SharpSpc
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 64)]
    public struct LogHeader
    {
        public int DiskSize;

        public int MemorySize;

        public int TextOffset;

        public int BinaryAreaSize;

        public int DiskAreaSize;

        public unsafe fixed byte _Spare[44];

        public readonly int LogTextSize => DiskSize - TextOffset;
    }
}
