namespace SharpSpc;
internal class MultiEvenlySpacedXFileHandler(Stream stream, ReadOptions options) : AbstractFileHandler(stream, options)
{
    public override void ReadSubfiles()
    {
        double[] xData = [];
        for (int i = 0; i < MainHeader.SubfileCount; i++)
        {
            SubHeader subHeader = ReadSubHeader();
            float[] yData = ReadSubYData(MainHeader.PointCount, MainHeader.YExponent);
            if (i == 0)
            {
                xData = GetXData(MainHeader.PointCount, MainHeader.FirstX, MainHeader.LastX);
            }

            Subfile subfile = new() {
                Header = subHeader,
                YData = yData,
                XData = xData
            };
            Subfiles[i] = subfile;
        }
    }
}