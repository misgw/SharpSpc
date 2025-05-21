namespace SharpSpc;
internal class SingleEvenlySpacedXFileHandler(Stream stream, ReadOptions options) : AbstractFileHandler(stream, options)
{
    public override void ReadSubfiles()
    {
        SubHeader subHeader = ReadSubHeader();
        float[] yData = ReadSubYData(MainHeader.PointCount, MainHeader.YExponent);
        double[] xData = GetXData(MainHeader.PointCount, MainHeader.FirstX, MainHeader.LastX);
        Subfile subfile = new() {
            Header = subHeader,
            YData = yData,
            XData = xData
        };
        Subfiles[0] = subfile;
    }
}