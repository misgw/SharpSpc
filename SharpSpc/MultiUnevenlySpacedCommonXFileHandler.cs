namespace SharpSpc;
internal class MultiUnevenlySpacedCommonXFileHandler(Stream stream, ReadOptions options) : AbstractFileHandler(stream, options)
{
    public override void ReadSubfiles()
    {
        List<double> list = [.. ReadSubXData(MainHeader.PointCount).Select(p => (double)p)];
        double[] xData = [.. list];
        for (int i = 0; i < MainHeader.SubfileCount; i++)
        {
            SubHeader subHeader = ReadSubHeader();
            float[] yData = ReadSubYData(MainHeader.PointCount, MainHeader.YExponent);
            Subfile subfile = new() {
                Header = subHeader,
                YData = yData,
                XData = xData
            };
            Subfiles[i] = subfile;
        }
    }
}