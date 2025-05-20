using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpSpc;
internal class MultiUnevenlySpacedUniqueXFileHandler(Stream stream, ReadOptions options) : AbstractFileHandler(stream, options)
{

    // Token: 0x06000055 RID: 85 RVA: 0x00003390 File Offset: 0x00001590
    public override void ReadSubfiles()
    {
        for (int i = 0; i < MainHeader.SubfileCount; i++)
        {
            SubHeader subHeader = ReadSubHeader();
            List<double> list =
            [
                .. from p in ReadSubXData(subHeader.XYXYPointCount)
                              select (double)p,
            ];
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
            List<double> list2 =
            [
                .. from p in ReadSubXData(subHeader2.XYXYPointCount)
                               select (double)p,
            ];
            double[] xData2 = [.. list2];
            float[] yData2 = ReadSubYData(subHeader2.XYXYPointCount, subHeader2.YExponent);
            Subfile subfile2 = default(Subfile);
            subfile2.Header = subHeader2;
            subfile2.YData = yData2;
            subfile2.XData = xData2;
        }
    }
}