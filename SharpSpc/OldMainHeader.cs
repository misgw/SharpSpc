using System.Runtime.InteropServices;
using System.Text;

namespace SharpSpc;
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 512)]
public struct OldMainHeader
{
    // Token: 0x17000023 RID: 35
    // (get) Token: 0x06000076 RID: 118 RVA: 0x00003A04 File Offset: 0x00001C04
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

    // Token: 0x17000024 RID: 36
    // (get) Token: 0x06000077 RID: 119 RVA: 0x00003A30 File Offset: 0x00001C30
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

    // Token: 0x17000025 RID: 37
    // (get) Token: 0x06000078 RID: 120 RVA: 0x00003A5C File Offset: 0x00001C5C
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

    // Token: 0x17000026 RID: 38
    // (get) Token: 0x06000079 RID: 121 RVA: 0x00003A8C File Offset: 0x00001C8C
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

    // Token: 0x17000027 RID: 39
    // (get) Token: 0x0600007A RID: 122 RVA: 0x00003AB8 File Offset: 0x00001CB8
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

    // Token: 0x17000028 RID: 40
    // (get) Token: 0x0600007B RID: 123 RVA: 0x00003AE4 File Offset: 0x00001CE4
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

    // Token: 0x0600007C RID: 124 RVA: 0x00003B14 File Offset: 0x00001D14
    private static unsafe string ReadAsciiString(byte* data, int length)
    {
        int end = 0;
        while (end < length && data[end] > 0)
        {
            end++;
        }

        return Encoding.ASCII.GetString(data, end);
    }

    // Token: 0x04000084 RID: 132
    public FileType Ftflags;

    // Token: 0x04000085 RID: 133
    public byte Fversn;

    // Token: 0x04000086 RID: 134
    public byte ExperimentType;

    // Token: 0x04000087 RID: 135
    public sbyte YExponent;

    // Token: 0x04000088 RID: 136
    public int PointCount;

    // Token: 0x04000089 RID: 137
    public double FirstX;

    // Token: 0x0400008A RID: 138
    public double LastX;

    // Token: 0x0400008B RID: 139
    public int SubfileCount;

    // Token: 0x0400008C RID: 140
    public byte XUnits;

    // Token: 0x0400008D RID: 141
    public byte YUnits;

    // Token: 0x0400008E RID: 142
    public byte ZUnits;

    // Token: 0x0400008F RID: 143
    public byte PostingDisposition;

    // Token: 0x04000090 RID: 144
    public int Date;

    // Token: 0x04000091 RID: 145
    private unsafe fixed byte _Resolution[9];

    // Token: 0x04000092 RID: 146
    private unsafe fixed byte _Instrument[9];

    // Token: 0x04000093 RID: 147
    public short PeakPoint;

    // Token: 0x04000094 RID: 148
    private unsafe fixed float _Spare[8];

    // Token: 0x04000095 RID: 149
    private unsafe fixed byte _Memo[130];

    // Token: 0x04000096 RID: 150
    private unsafe fixed byte _AxisLabels[30];

    // Token: 0x04000097 RID: 151
    public int LogOffset;

    // Token: 0x04000098 RID: 152
    public int FileFlags;

    // Token: 0x04000099 RID: 153
    public byte ProcessingCode;

    // Token: 0x0400009A RID: 154
    public byte CalibrationLevel;

    // Token: 0x0400009B RID: 155
    public short SampleNumber;

    // Token: 0x0400009C RID: 156
    public float Concentration;

    // Token: 0x0400009D RID: 157
    private unsafe fixed byte _MethodFile[48];

    // Token: 0x0400009E RID: 158
    public float ZIncrement;

    // Token: 0x0400009F RID: 159
    public int WPlaneCount;

    // Token: 0x040000A0 RID: 160
    public float WIncrement;

    // Token: 0x040000A1 RID: 161
    public byte WUnits;

    // Token: 0x040000A2 RID: 162
    private unsafe fixed byte _Reserved[187];
}