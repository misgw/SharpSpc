namespace SharpSpc;
internal class SingleUnevenlySpacedXFileHandler(Stream stream, ReadOptions options) : AbstractFileHandler(stream, options)
{
    public override void ReadSubfiles()
    {
        List<double> list = [.. ReadSubXData(MainHeader.PointCount).Select(p => (double)p)];
        double[] xData = [.. list];
        SubHeader subHeader = ReadSubHeader();
        float[] yData = ReadSubYData(MainHeader.PointCount, MainHeader.YExponent);
        Subfile subfile = new() {
            Header = subHeader,
            YData = yData,
            XData = xData
        };
        Subfiles[0] = subfile;
    }
}