using System.Runtime.InteropServices;
using System.Text;

namespace SharpSpc;
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 512)]
public struct MainHeader
{
    public readonly unsafe string Resolution
    {
        get
        {
            fixed (byte* ptr = &_Resolution[0])
            {
                return ReadAsciiString(ptr, 9);
            }
        }
    }

    public readonly unsafe string Instrument
    {
        get
        {
            fixed (byte* ptr = &_Instrument[0])
            {
                return ReadAsciiString(ptr, 9);
            }
        }
    }

    public readonly unsafe string Memo
    {
        get
        {
            fixed (byte* ptr = &_Memo[0])
            {
                return ReadAsciiString(ptr, 130);
            }
        }
    }

    public readonly unsafe string AxisLabels
    {
        get
        {
            fixed (byte* ptr = &_AxisLabels[0])
            {
                return ReadAsciiString(ptr, 30);
            }
        }
    }

    public readonly unsafe string MethodFile
    {
        get
        {
            fixed (byte* ptr = &_MethodFile[0])
            {
                return ReadAsciiString(ptr, 48);
            }
        }
    }

    public readonly unsafe string Reserved
    {
        get
        {
            fixed (byte* ptr = &_Reserved[0])
            {
                return ReadAsciiString(ptr, 187);
            }
        }
    }

    private static unsafe string ReadAsciiString(byte* data, int length)
    {
        int end = 0;
        while (end < length && data[end] > 0)
        {
            end++;
        }

        return Encoding.ASCII.GetString(data, end);
    }

    public FileType Ftflags;

    public byte Fversn;

    public byte ExperimentType;

    public sbyte YExponent;

    public int PointCount;

    public double FirstX;

    public double LastX;

    public int SubfileCount;

    public byte XUnits;

    public byte YUnits;

    public byte ZUnits;

    public byte PostingDisposition;

    public int Date;

    public unsafe fixed byte _Resolution[9];

    public unsafe fixed byte _Instrument[9];

    public short PeakPoint;

    public unsafe fixed float _Spare[8];

    public unsafe fixed byte _Memo[130];

    public unsafe fixed byte _AxisLabels[30];

    public int LogOffset;

    public int FileFlags;

    public byte ProcessingCode;

    public byte CalibrationLevel;

    public short SampleNumber;

    public float Concentration;

    public unsafe fixed byte _MethodFile[48];

    public float ZIncrement;

    public int WPlaneCount;

    public float WIncrement;

    public byte WUnits;

    public unsafe fixed byte _Reserved[187];
}
