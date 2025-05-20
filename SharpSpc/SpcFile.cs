using System.IO;
using System.Text;

namespace SharpSpc;
internal class SpcFile
{
    // Token: 0x0600006C RID: 108 RVA: 0x00003728 File Offset: 0x00001928
    public static AbstractFileHandler Read(string path, Encoding encoding = null)
    {
        AbstractFileHandler result;
        using (FileStream fs = File.OpenRead(path))
        {
            bool flag = fs.Length < 544L;
            if (flag)
            {
                throw new InvalidDataException();
            }
            FileType ftflgs = (FileType)fs.ReadByte();
            byte fversn = (byte)fs.ReadByte();
            bool flag2 = fversn != 75 && fversn != 76 && fversn != 77;
            if (flag2)
            {
                throw new InvalidDataException();
            }
            ReadOptions options = new ReadOptions {
                IsOldFormat = fversn == 77,
                IsBigEndian = fversn == 76,
                Is16BitY = ftflgs.HasFlag(FileType.TSPREC),
                Encoding = encoding ?? Encoding.ASCII,
                Ftflags = ftflgs,
                Fversn = fversn
            };
            FileType fileType = ftflgs & ~(FileType.TSPREC | FileType.TORDRD | FileType.TALABS);
            if (!true)
            {
            }
            AbstractFileHandler abstractFileHandler;
            if (fileType <= FileType.TMULTI)
            {
                if (fileType == FileType.TNULL)
                {
                    abstractFileHandler = new SingleEvenlySpacedXFileHandler(fs, options);
                    goto IL_129;
                }
                if (fileType == FileType.TMULTI)
                {
                    abstractFileHandler = new MultiEvenlySpacedXFileHandler(fs, options);
                    goto IL_129;
                }
            }
            else
            {
                if (fileType == FileType.TXVALS)
                {
                    abstractFileHandler = new SingleUnevenlySpacedXFileHandler(fs, options);
                    goto IL_129;
                }
                if (fileType == FileType.MULTI_UNEVEN_SPACED_COMMON_X)
                {
                    abstractFileHandler = new MultiUnevenlySpacedCommonXFileHandler(fs, options);
                    goto IL_129;
                }
                if (fileType == FileType.MULTI_UNEVENL_SPACED_UNIQUE_X)
                {
                    abstractFileHandler = new MultiUnevenlySpacedUniqueXFileHandler(fs, options);
                    goto IL_129;
                }
            }
            throw new InvalidDataException();
        IL_129:
            if (!true)
            {
            }
            AbstractFileHandler fileHandler = abstractFileHandler;
            fileHandler.Read();
            result = fileHandler;
        }
        return result;
    }
}