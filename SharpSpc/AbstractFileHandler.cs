using System;
using System.IO;

namespace SharpSpc;
internal abstract class AbstractFileHandler(Stream stream, ReadOptions options) : ISpcReader
{

    // Token: 0x1700000C RID: 12
    // (get) Token: 0x06000019 RID: 25 RVA: 0x00002157 File Offset: 0x00000357
    // (set) Token: 0x0600001A RID: 26 RVA: 0x0000215F File Offset: 0x0000035F
    protected MainHeader MainHeader { get; set; }

    // Token: 0x1700000D RID: 13
    // (get) Token: 0x0600001B RID: 27 RVA: 0x00002168 File Offset: 0x00000368
    // (set) Token: 0x0600001C RID: 28 RVA: 0x00002170 File Offset: 0x00000370
    protected Subfile[] Subfiles { get; set; }

    // Token: 0x1700000E RID: 14
    // (get) Token: 0x0600001D RID: 29 RVA: 0x00002179 File Offset: 0x00000379
    // (set) Token: 0x0600001E RID: 30 RVA: 0x00002181 File Offset: 0x00000381
    protected LogHeader? LogHeader { get; set; }

    // Token: 0x1700000F RID: 15
    // (get) Token: 0x0600001F RID: 31 RVA: 0x0000218A File Offset: 0x0000038A
    // (set) Token: 0x06000020 RID: 32 RVA: 0x00002192 File Offset: 0x00000392
    protected SubDirectory? SubDirectory { get; set; }

    // Token: 0x17000010 RID: 16
    // (get) Token: 0x06000021 RID: 33 RVA: 0x0000219B File Offset: 0x0000039B
    // (set) Token: 0x06000022 RID: 34 RVA: 0x000021A3 File Offset: 0x000003A3
    public string LogText { get; set; }

    // Token: 0x17000011 RID: 17
    // (get) Token: 0x06000023 RID: 35 RVA: 0x000021AC File Offset: 0x000003AC
    // (set) Token: 0x06000024 RID: 36 RVA: 0x000021B4 File Offset: 0x000003B4
    public byte[] LogBinData { get; set; }

    // Token: 0x17000012 RID: 18
    // (get) Token: 0x06000025 RID: 37 RVA: 0x000021BD File Offset: 0x000003BD
    protected EndianReader Reader => new(stream, options);

    // Token: 0x06000026 RID: 38 RVA: 0x000021D0 File Offset: 0x000003D0
    public void Read()
    {
        ReadMainHeader();
        ReadSubfiles();
        ReadLog();
    }

    // Token: 0x06000027 RID: 39 RVA: 0x000021E8 File Offset: 0x000003E8
    public unsafe void ReadMainHeader()
    {
        MainHeader mainHeader = new() {
            Ftflags = options.Ftflags,
            Fversn = options.Fversn
        };
        bool unsafeRead = options.UnsafeRead;
        if (unsafeRead)
        {
            byte* p = &mainHeader.ExperimentType;
            ReadOnce(p, 510);
        }
        else
        {
            ReadMainHeaderStep(ref mainHeader);
        }
        bool flag = !options.IsOldFormat;
        if (flag)
        {
            Subfiles = new Subfile[mainHeader.SubfileCount];
        }
        MainHeader = mainHeader;
    }

    // Token: 0x06000028 RID: 40 RVA: 0x00002288 File Offset: 0x00000488
    private unsafe void ReadMainHeaderStep(ref MainHeader header)
    {
        header.ExperimentType = Reader.ReadByte();
        header.YExponent = Reader.ReadSByte();
        header.PointCount = Reader.ReadInt32();
        header.FirstX = Reader.ReadDouble();
        header.LastX = Reader.ReadDouble();
        header.SubfileCount = Reader.ReadInt32();
        header.XUnits = Reader.ReadByte();
        header.YUnits = Reader.ReadByte();
        header.ZUnits = Reader.ReadByte();
        header.PostingDisposition = Reader.ReadByte();
        header.Date = Reader.ReadInt32();
        fixed (byte* ptr8 = &header._Resolution.FixedElementField)
        {
            byte* ptr = ptr8;
            Reader.ReadBytes(ptr, 9);
        }
        fixed (byte* ptr9 = &header._Instrument.FixedElementField)
        {
            byte* ptr2 = ptr9;
            Reader.ReadBytes(ptr2, 9);
        }
        header.PeakPoint = Reader.ReadInt16();
        fixed (float* ptr10 = &header._Spare.FixedElementField)
        {
            float* ptr3 = ptr10;
            for (int i = 0; i < 8; i++)
            {
                ptr3[i] = Reader.ReadSingle();
            }
        }
        fixed (byte* ptr11 = &header._Memo.FixedElementField)
        {
            byte* ptr4 = ptr11;
            Reader.ReadBytes(ptr4, 130);
        }
        fixed (byte* ptr12 = &header._AxisLabels.FixedElementField)
        {
            byte* ptr5 = ptr12;
            Reader.ReadBytes(ptr5, 30);
        }
        header.LogOffset = Reader.ReadInt32();
        header.FileFlags = Reader.ReadInt32();
        header.ProcessingCode = Reader.ReadByte();
        header.CalibrationLevel = Reader.ReadByte();
        header.SampleNumber = Reader.ReadInt16();
        header.Concentration = Reader.ReadSingle();
        fixed (byte* ptr13 = &header._MethodFile.FixedElementField)
        {
            byte* ptr6 = ptr13;
            Reader.ReadBytes(ptr6, 48);
        }
        header.ZIncrement = Reader.ReadSingle();
        header.WPlaneCount = Reader.ReadInt32();
        header.WIncrement = Reader.ReadSingle();
        header.WUnits = Reader.ReadByte();
        fixed (byte* ptr14 = &header._Reserved.FixedElementField)
        {
            byte* ptr7 = ptr14;
            Reader.ReadBytes(ptr7, 187);
        }
    }

    // Token: 0x06000029 RID: 41 RVA: 0x0000253C File Offset: 0x0000073C
    public virtual unsafe SubHeader ReadSubHeader()
    {
        SubHeader subHeader = default;
        bool unsafeRead = options.UnsafeRead;
        if (unsafeRead)
        {
            byte* p = (byte*)&subHeader.SubFlags;
            ReadOnce(p, 32);
        }
        else
        {
            ReadSubHeaderStep(ref subHeader);
        }
        return subHeader;
    }

    // Token: 0x0600002A RID: 42 RVA: 0x00002588 File Offset: 0x00000788
    private unsafe void ReadSubHeaderStep(ref SubHeader subHeader)
    {
        subHeader.SubFlags = (SubfileType)Reader.ReadByte();
        subHeader.YExponent = Reader.ReadSByte();
        subHeader.SubfileIndex = Reader.ReadInt16();
        subHeader.ZStart = Reader.ReadSingle();
        subHeader.ZEnd = Reader.ReadSingle();
        subHeader.Noise = Reader.ReadSingle();
        subHeader.XYXYPointCount = Reader.ReadInt32();
        subHeader.ScanCount = Reader.ReadInt32();
        subHeader.WValue = Reader.ReadSingle();
        fixed (byte* ptr2 = &subHeader._Reserved.FixedElementField)
        {
            byte* ptr = ptr2;
            Reader.ReadBytes(ptr, 4);
        }
    }

    // Token: 0x0600002B RID: 43
    public abstract void ReadSubfiles();

    // Token: 0x0600002C RID: 44 RVA: 0x00002654 File Offset: 0x00000854
    public unsafe SubDirectory ReadDirectory()
    {
        SubDirectory subDirectory = default;
        bool unsafeRead = options.UnsafeRead;
        if (unsafeRead)
        {
            byte* p = (byte*)&subDirectory.SubfileOffset;
            ReadOnce(p, 12);
        }
        else
        {
            ReadDirectoryStep(ref subDirectory);
        }
        return subDirectory;
    }

    // Token: 0x0600002D RID: 45 RVA: 0x000026A0 File Offset: 0x000008A0
    private void ReadDirectoryStep(ref SubDirectory subDirectory)
    {
        subDirectory.SubfileOffset = Reader.ReadInt32();
        subDirectory.SubfileSize = Reader.ReadInt32();
        subDirectory.ZStart = Reader.ReadSingle();
    }

    // Token: 0x0600002E RID: 46 RVA: 0x000026D8 File Offset: 0x000008D8
    public unsafe void ReadLog()
    {
        bool flag = MainHeader.LogOffset <= 0;
        if (!flag)
        {
            LogHeader logHeader = default;
            bool unsafeRead = options.UnsafeRead;
            if (unsafeRead)
            {
                byte* p = (byte*)&logHeader.DiskSize;
                ReadOnce(p, 64);
            }
            else
            {
                ReadLogStep(ref logHeader);
            }
            bool flag2 = logHeader.BinaryAreaSize > 0;
            if (flag2)
            {
                bool loadLogBinData = options.LoadLogBinData;
                if (loadLogBinData)
                {
                    LogBinData = Reader.ReadBytes(logHeader.BinaryAreaSize);
                }
                else
                {
                    Reader.SeekAtCurrent(logHeader.BinaryAreaSize);
                }
            }
            bool flag3 = logHeader.LogTextSize > 0;
            if (flag3)
            {
                bool loadLogText = options.LoadLogText;
                if (loadLogText)
                {
                    byte[] strBuffer = Reader.ReadBytes(logHeader.LogTextSize);
                    LogText = options.Encoding.GetString(strBuffer);
                }
                else
                {
                    Reader.SeekAtCurrent(logHeader.LogTextSize);
                }
            }
            LogHeader = new LogHeader?(logHeader);
        }
    }

    // Token: 0x0600002F RID: 47 RVA: 0x00002804 File Offset: 0x00000A04
    private unsafe void ReadLogStep(ref LogHeader logHeader)
    {
        logHeader.DiskSize = Reader.ReadInt32();
        logHeader.MemorySize = Reader.ReadInt32();
        logHeader.TextOffset = Reader.ReadInt32();
        logHeader.BinaryAreaSize = Reader.ReadInt32();
        logHeader.DiskAreaSize = Reader.ReadInt32();
        fixed (byte* ptr2 = &logHeader._Spare.FixedElementField)
        {
            byte* ptr = ptr2;
            Reader.ReadBytes(ptr, 44);
        }
    }

    // Token: 0x06000030 RID: 48 RVA: 0x0000288C File Offset: 0x00000A8C
    public virtual float[] ReadSubXData(int fnpts)
    {
        return Reader.ReadData<float>(fnpts);
    }

    // Token: 0x06000031 RID: 49 RVA: 0x000028AC File Offset: 0x00000AAC
    public virtual float[] ReadSubYData(int fnpts, sbyte fexp)
    {
        bool flag = MainHeader.YExponent == sbyte.MinValue;
        float[] result;
        if (flag)
        {
            result = Reader.ReadData<float>(MainHeader.PointCount);
        }
        else
        {
            bool is16BitY = options.Is16BitY;
            if (is16BitY)
            {
                short[] buffer16 = Reader.ReadData<short>(fnpts);
                result = Convert(buffer16, fexp);
            }
            else
            {
                int[] buffer17 = Reader.ReadData<int>(fnpts);
                result = Convert(buffer17, fexp);
            }
        }
        return result;
    }

    // Token: 0x06000032 RID: 50 RVA: 0x00002928 File Offset: 0x00000B28
    private unsafe void ReadOnce(byte* destination, int size)
    {
        Span<byte> span = new(stackalloc byte[(UIntPtr)size], size);
        Span<byte> buffer = span;
        bool flag = Reader.Read(buffer) != size;
        if (flag)
        {
            throw new InvalidDataException();
        }
        Buffer.MemoryCopy(Unsafe.AsPointer<byte>(Unsafe.AsRef<byte>(buffer[0])), (void*)destination, (long)size, (long)size);
    }

    // Token: 0x06000033 RID: 51 RVA: 0x00002980 File Offset: 0x00000B80
    protected static float[] Convert(int[] yData, sbyte exponent)
    {
        float[] scaled = new float[yData.Length];
        for (int i = 0; i < yData.Length; i++)
        {
            double y = Math.Pow(2.0, exponent) * yData[i] / Math.Pow(2.0, 32.0);
            scaled[i] = (float)y;
        }
        return scaled;
    }

    // Token: 0x06000034 RID: 52 RVA: 0x000029E8 File Offset: 0x00000BE8
    protected static float[] Convert(short[] yData, sbyte exponent)
    {
        float[] scaled = new float[yData.Length];
        for (int i = 0; i < yData.Length; i++)
        {
            double y = Math.Pow(2.0, exponent) * yData[i] / Math.Pow(2.0, 32.0);
            scaled[i] = (float)y;
        }
        return scaled;
    }

    // Token: 0x06000035 RID: 53 RVA: 0x00002A50 File Offset: 0x00000C50
    protected static double[] GetXData(int length, double ffirst, double flast)
    {
        bool flag = length < 1;
        if (flag)
        {
            throw new InvalidDataException();
        }
        double[] xData = new double[length];
        bool flag2 = flast == ffirst;
        double[] result;
        if (flag2)
        {
            xData.AsSpan().Fill(flast);
            result = xData;
        }
        else
        {
            double diff = (flast - ffirst) / (length - 1);
            for (int i = 0; i < length; i++)
            {
                xData[i] = ffirst + (diff * i);
            }
            result = xData;
        }
        return result;
    }
}