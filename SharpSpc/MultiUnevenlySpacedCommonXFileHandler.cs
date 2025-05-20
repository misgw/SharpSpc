using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpSpc;
internal class MultiUnevenlySpacedCommonXFileHandler(Stream stream, ReadOptions options) : AbstractFileHandler(stream, options)
{

    // Token: 0x06000053 RID: 83 RVA: 0x000032BC File Offset: 0x000014BC
    public override void ReadSubfiles()
    {
        List<double> list =
        [
            .. from p in ReadSubXData(MainHeader.PointCount)
                          select (double)p,
        ];
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