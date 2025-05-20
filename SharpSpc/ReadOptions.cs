using System;
using System.Text;

namespace SharpSpc;
internal class ReadOptions
{
    public bool IsOldFormat { get; set; }

    public bool IsBigEndian { get; set; }

    public bool UnsafeRead
    {
        get
        {
            return IsBigEndian ^ BitConverter.IsLittleEndian;
        }
    }

    public Encoding Encoding { get; set; }

    public FileType Ftflags { get; set; }

    public byte Fversn { get; set; }

    public bool Is16BitY { get; set; }

    public bool LoadLogBinData { get; set; } = false;

    public bool LoadLogText { get; set; } = true;
}
