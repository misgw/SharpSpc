namespace SharpSpc;
[Flags]
public enum SubfileType : byte
{
    Normal = 0,
    Changed = 1,
    PeakTableDisabled = 8,
    ArithmeticModified = 128
}