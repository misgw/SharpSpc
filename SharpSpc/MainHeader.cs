using System.Runtime.InteropServices;
using System.Text;

namespace SharpSpc;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 512)]
public struct MainHeader
{
    // Token: 0x1700001D RID: 29
    // (get) Token: 0x0600006F RID: 111 RVA: 0x000038B8 File Offset: 0x00001AB8
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

    // Token: 0x1700001E RID: 30
    // (get) Token: 0x06000070 RID: 112 RVA: 0x000038E4 File Offset: 0x00001AE4
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

    // Token: 0x1700001F RID: 31
    // (get) Token: 0x06000071 RID: 113 RVA: 0x00003910 File Offset: 0x00001B10
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

    // Token: 0x17000020 RID: 32
    // (get) Token: 0x06000072 RID: 114 RVA: 0x00003940 File Offset: 0x00001B40
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

    // Token: 0x17000021 RID: 33
    // (get) Token: 0x06000073 RID: 115 RVA: 0x0000396C File Offset: 0x00001B6C
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

    // Token: 0x17000022 RID: 34
    // (get) Token: 0x06000074 RID: 116 RVA: 0x00003998 File Offset: 0x00001B98
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

    // Token: 0x06000075 RID: 117 RVA: 0x000039C8 File Offset: 0x00001BC8
    private static unsafe string ReadAsciiString(byte* data, int length)
    {
        int end = 0;
        while (end < length && data[end] > 0)
        {
            end++;
        }

        return Encoding.ASCII.GetString(data, end);
    }

    // Token: 0x04000065 RID: 101
    public FileType Ftflags;

    // Token: 0x04000066 RID: 102
    public byte Fversn;

    // Token: 0x04000067 RID: 103
    public byte ExperimentType;

    // Token: 0x04000068 RID: 104
    public sbyte YExponent;

    // Token: 0x04000069 RID: 105
    public int PointCount;

    // Token: 0x0400006A RID: 106
    public double FirstX;

    // Token: 0x0400006B RID: 107
    public double LastX;

    // Token: 0x0400006C RID: 108
    public int SubfileCount;

    // Token: 0x0400006D RID: 109
    public byte XUnits;

    // Token: 0x0400006E RID: 110
    public byte YUnits;

    // Token: 0x0400006F RID: 111
    public byte ZUnits;

    // Token: 0x04000070 RID: 112
    public byte PostingDisposition;

    // Token: 0x04000071 RID: 113
    public int Date;

    // Token: 0x04000072 RID: 114
    public unsafe fixed byte _Resolution[9];

    // Token: 0x04000073 RID: 115
    public unsafe fixed byte _Instrument[9];

    // Token: 0x04000074 RID: 116
    public short PeakPoint;

    // Token: 0x04000075 RID: 117
    public unsafe fixed float _Spare[8];

    // Token: 0x04000076 RID: 118
    public unsafe fixed byte _Memo[130];

    // Token: 0x04000077 RID: 119
    public unsafe fixed byte _AxisLabels[30];

    // Token: 0x04000078 RID: 120
    public int LogOffset;

    // Token: 0x04000079 RID: 121
    public int FileFlags;

    // Token: 0x0400007A RID: 122
    public byte ProcessingCode;

    // Token: 0x0400007B RID: 123
    public byte CalibrationLevel;

    // Token: 0x0400007C RID: 124
    public short SampleNumber;

    // Token: 0x0400007D RID: 125
    public float Concentration;

    // Token: 0x0400007E RID: 126
    public unsafe fixed byte _MethodFile[48];

    // Token: 0x0400007F RID: 127
    public float ZIncrement;

    // Token: 0x04000080 RID: 128
    public int WPlaneCount;

    // Token: 0x04000081 RID: 129
    public float WIncrement;

    // Token: 0x04000082 RID: 130
    public byte WUnits;

    // Token: 0x04000083 RID: 131
    public unsafe fixed byte _Reserved[187];
}
