using System.IO;

namespace SharpSpc;
internal class SingleEvenlySpacedXFileHandler(Stream stream, ReadOptions options) : AbstractFileHandler(stream, options)
{

    // Token: 0x06000069 RID: 105 RVA: 0x000035E8 File Offset: 0x000017E8
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