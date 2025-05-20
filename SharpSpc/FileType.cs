using System;

namespace SharpSpc;
[Flags]
public enum FileType : byte
{
    // Token: 0x040000B4 RID: 180
    TNULL = 0,
    // Token: 0x040000B5 RID: 181
    TSPREC = 1,
    // Token: 0x040000B6 RID: 182
    [Obsolete("(not used) Enables fexper in older software.")]
    TCGRAM = 2,
    // Token: 0x040000B7 RID: 183
    TMULTI = 4,
    // Token: 0x040000B8 RID: 184
    [Obsolete("(not used) If TMULTI and TRANDM then Z values in SUBHDR structures are in random order.")]
    TRANDM = 8,
    // Token: 0x040000B9 RID: 185
    TORDRD = 16,
    // Token: 0x040000BA RID: 186
    TALABS = 32,
    // Token: 0x040000BB RID: 187
    TXYXYS = 64,
    // Token: 0x040000BC RID: 188
    TXVALS = 128,
    // Token: 0x040000BD RID: 189
    MULTI_UNEVEN_SPACED_COMMON_X = 132,
    // Token: 0x040000BE RID: 190
    MULTI_UNEVENL_SPACED_UNIQUE_X = 196,
    // Token: 0x040000BF RID: 191
    DERIVED_INDEPENDENT = 49
}