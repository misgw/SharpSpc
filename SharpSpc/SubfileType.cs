using System;

namespace SharpSpc;
[Flags]
public enum SubfileType : byte
{
    // Token: 0x040000C1 RID: 193
    Normal = 0,
    // Token: 0x040000C2 RID: 194
    Changed = 1,
    // Token: 0x040000C3 RID: 195
    PeakTableDisabled = 8,
    // Token: 0x040000C4 RID: 196
    ArithmeticModified = 128
}