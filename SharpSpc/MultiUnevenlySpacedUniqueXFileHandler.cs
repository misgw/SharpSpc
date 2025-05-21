namespace SharpSpc;
internal class MultiUnevenlySpacedUniqueXFileHandler(Stream stream, ReadOptions options) : AbstractFileHandler(stream, options)
{
    public override void ReadSubfiles()
    {
        for (int i = 0; i < MainHeader.SubfileCount; i++)
        {
            SubHeader subHeader = ReadSubHeader();
            List<double> list = [.. ReadSubXData(subHeader.XYXYPointCount).Select(p => (double)p)];
            double[] xData = [.. list];
            float[] yData = ReadSubYData(subHeader.XYXYPointCount, subHeader.YExponent);
            Subfile subfile = new() {
                Header = subHeader,
                YData = yData,
                XData = xData
            };
            Subfiles[i] = subfile;
        }

        bool flag = MainHeader.PointCount > 0;
        if (flag)
        {
            Reader.BaseStream.Position = MainHeader.PointCount;
            SubDirectory subDirectory = ReadDirectory();
            Reader.BaseStream.Position = subDirectory.SubfileOffset;
            SubHeader subHeader2 = ReadSubHeader();
            List<double> list2 = [.. ReadSubXData(subHeader2.XYXYPointCount).Select(p => (double)p)];
            double[] xData2 = [.. list2];
            float[] yData2 = ReadSubYData(subHeader2.XYXYPointCount, subHeader2.YExponent);
            Subfile subfile2 = default;
            subfile2.Header = subHeader2;
            subfile2.YData = yData2;
            subfile2.XData = xData2;
        }
    }
}