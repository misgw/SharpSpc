namespace SharpSpc;
internal abstract class AbstractFileHandler(Stream stream, ReadOptions options) : ISpcReader
{
    protected MainHeader MainHeader { get; set; }

    protected Subfile[]? Subfiles { get; set; }

    protected LogHeader? LogHeader { get; set; }

    protected SubDirectory? SubDirectory { get; set; }

    public string? LogText { get; set; }

    public byte[]? LogBinData { get; set; }

    protected EndianReader Reader => new(stream, options);

    public void Read()
    {
        ReadMainHeader();
        ReadSubfiles();
        ReadLog();
    }

    public unsafe void ReadMainHeader()
    {
        MainHeader mainHeader = new() {
            Ftflags = options.Ftflags,
            Fversn = options.Fversn
        };
        if (options.UnsafeRead)
        {
            byte* p = &mainHeader.ExperimentType;
            ReadOnce(p, 510);
        }
        else
        {
            ReadMainHeaderStep(ref mainHeader);
        }

        if (!options.IsOldFormat)
        {
            Subfiles = new Subfile[mainHeader.SubfileCount];
        }

        MainHeader = mainHeader;
    }

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
        fixed (byte* ptr = &header._Resolution[0])
        {
            Reader.ReadBytes(ptr, 9);
        }

        fixed (byte* ptr = &header._Instrument[0])
        {
            Reader.ReadBytes(ptr, 9);
        }

        header.PeakPoint = Reader.ReadInt16();
        fixed (float* ptr = &header._Spare[0])
        {
            for (int i = 0; i < 8; i++)
            {
                ptr[i] = Reader.ReadSingle();
            }
        }

        fixed (byte* ptr = &header._Memo[0])
        {
            Reader.ReadBytes(ptr, 130);
        }

        fixed (byte* ptr = &header._AxisLabels[0])
        {
            Reader.ReadBytes(ptr, 30);
        }

        header.LogOffset = Reader.ReadInt32();
        header.FileFlags = Reader.ReadInt32();
        header.ProcessingCode = Reader.ReadByte();
        header.CalibrationLevel = Reader.ReadByte();
        header.SampleNumber = Reader.ReadInt16();
        header.Concentration = Reader.ReadSingle();
        fixed (byte* ptr = &header._MethodFile[0])
        {
            Reader.ReadBytes(ptr, 48);
        }

        header.ZIncrement = Reader.ReadSingle();
        header.WPlaneCount = Reader.ReadInt32();
        header.WIncrement = Reader.ReadSingle();
        header.WUnits = Reader.ReadByte();
        fixed (byte* ptr = &header._Reserved[0])
        {
            Reader.ReadBytes(ptr, 187);
        }
    }

    public virtual unsafe SubHeader ReadSubHeader()
    {
        SubHeader subHeader = default;
        if (options.UnsafeRead)
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
        fixed (byte* ptr = &subHeader._Reserved[0])
        {
            Reader.ReadBytes(ptr, 4);
        }
    }

    public abstract void ReadSubfiles();

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

    private void ReadDirectoryStep(ref SubDirectory subDirectory)
    {
        subDirectory.SubfileOffset = Reader.ReadInt32();
        subDirectory.SubfileSize = Reader.ReadInt32();
        subDirectory.ZStart = Reader.ReadSingle();
    }

    public unsafe void ReadLog()
    {
        if (MainHeader.LogOffset <= 0)
        {
            return;
        }

        LogHeader logHeader = default;
        if (options.UnsafeRead)
        {
            byte* p = (byte*)&logHeader.DiskSize;
            ReadOnce(p, 64);
        }
        else
        {
            ReadLogStep(ref logHeader);
        }

        if (logHeader.BinaryAreaSize > 0)
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

        if (logHeader.LogTextSize > 0)
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

        LogHeader = logHeader;
    }

    private unsafe void ReadLogStep(ref LogHeader logHeader)
    {
        logHeader.DiskSize = Reader.ReadInt32();
        logHeader.MemorySize = Reader.ReadInt32();
        logHeader.TextOffset = Reader.ReadInt32();
        logHeader.BinaryAreaSize = Reader.ReadInt32();
        logHeader.DiskAreaSize = Reader.ReadInt32();
        fixed (byte* ptr = &logHeader._Spare[0])
        {
            Reader.ReadBytes(ptr, 44);
        }
    }

    public virtual float[] ReadSubXData(int fnpts)
    {
        return Reader.ReadData<float>(fnpts);
    }

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

    private unsafe void ReadOnce(byte* destination, int size)
    {
        Span<byte> buffer = stackalloc byte[size];
        if (Reader.Read(buffer) != size)
        {
            throw new InvalidDataException();
        }

        Buffer.MemoryCopy(Unsafe.AsPointer(ref Unsafe.AsRef(buffer[0])), destination, size, size);
    }

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