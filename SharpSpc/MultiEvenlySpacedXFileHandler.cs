using System.IO;

namespace SharpSpc;
internal class MultiEvenlySpacedXFileHandler(Stream stream, ReadOptions options) : AbstractFileHandler(stream, options)
{

    // Token: 0x06000051 RID: 81 RVA: 0x000031F0 File Offset: 0x000013F0
    public override void ReadSubfiles()
    {
        double[] xData = [];
        for (int i = 0; i < MainHeader.SubfileCount; i++)
        {
            SubHeader subHeader = ReadSubHeader();
            float[] yData = ReadSubYData(MainHeader.PointCount, MainHeader.YExponent);
            bool flag = i == 0;
            if (flag)
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