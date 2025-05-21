namespace SharpSpc;
[Flags]
public enum FileType : byte
{
    TNULL = 0,
    TSPREC = 1,
    [Obsolete("(not used) Enables fexper in older software.")]
    TCGRAM = 2,
    TMULTI = 4,
    [Obsolete("(not used) If TMULTI and TRANDM then Z values in SUBHDR structures are in random order.")]
    TRANDM = 8,
    TORDRD = 16,
    TALABS = 32,
    TXYXYS = 64,
    TXVALS = 128,
    MULTI_UNEVEN_SPACED_COMMON_X = 132,
    MULTI_UNEVENL_SPACED_UNIQUE_X = 196,
    DERIVED_INDEPENDENT = 49
}