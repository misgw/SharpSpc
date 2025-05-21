using System.Text;

namespace SharpSpc;
public class SpcFile
{
    public static object Read(string path, Encoding? encoding = null)
    {
        using FileStream fs = File.OpenRead(path);
        if (fs.Length < MAIN_HEADER_SIZE + SUB_HEADER_SIZE)
        {
            throw new InvalidDataException();
        }

        FileType ftflgs = (FileType)fs.ReadByte();
        byte fversn = (byte)fs.ReadByte();
        if (fversn is not VERSION_NEW_LSB_1ST and not VERSION_NEW_MSB_1ST and not VERSION_OLD)
        {
            throw new InvalidDataException();
        }

        ReadOptions options = new() {
            IsOldFormat = fversn == VERSION_OLD,
            IsBigEndian = fversn == VERSION_NEW_MSB_1ST,
            Is16BitY = ftflgs.HasFlag(FileType.TSPREC),
            Encoding = encoding ?? Encoding.ASCII,
            Ftflags = ftflgs,
            Fversn = fversn
        };
        FileType fileType = ftflgs & ~FileType.DERIVED_INDEPENDENT;
        AbstractFileHandler abstractFileHandler = fileType switch {
            FileType.TNULL => new SingleEvenlySpacedXFileHandler(fs, options),
            FileType.TXVALS => new SingleUnevenlySpacedXFileHandler(fs, options),
            FileType.TMULTI => new MultiEvenlySpacedXFileHandler(fs, options),
            FileType.MULTI_UNEVEN_SPACED_COMMON_X => new MultiUnevenlySpacedCommonXFileHandler(fs, options),
            FileType.MULTI_UNEVENL_SPACED_UNIQUE_X => new MultiUnevenlySpacedUniqueXFileHandler(fs, options),
            _ => throw new InvalidDataException(),
        };
        AbstractFileHandler fileHandler = abstractFileHandler;
        fileHandler.Read();
        return fileHandler;
    }
}