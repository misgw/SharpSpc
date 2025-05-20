using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpSpc;
internal class SingleUnevenlySpacedXFileHandler(Stream stream, ReadOptions options) : AbstractFileHandler(stream, options)
{
    // Token: 0x0600006B RID: 107 RVA: 0x0000367C File Offset: 0x0000187C
    public override void ReadSubfiles()
    {
        List<double> list =
        [
            .. from p in ReadSubXData(MainHeader.PointCount)
                          select (double)p,
        ];
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