using System.Buffers.Binary;

namespace SharpSpc;
internal class EndianReader(Stream stream, ReadOptions readOptions) : BinaryReader(stream, readOptions.Encoding)
{
    public override short ReadInt16()
    {
        return (!readOptions.IsBigEndian) ? base.ReadInt16() : BinaryPrimitives.ReadInt16BigEndian(base.ReadBytes(2));
    }

    public override float ReadSingle()
    {
        return (!readOptions.IsBigEndian) ? base.ReadSingle() : BinaryPrimitives.ReadSingleBigEndian(base.ReadBytes(4));
    }

    public override int ReadInt32()
    {
        return (!readOptions.IsBigEndian) ? base.ReadInt32() : BinaryPrimitives.ReadInt16BigEndian(base.ReadBytes(4));
    }

    public override double ReadDouble()
    {
        return (!readOptions.IsBigEndian) ? base.ReadDouble() : BinaryPrimitives.ReadDoubleBigEndian(base.ReadBytes(8));
    }

    public unsafe void ReadBytes(byte* destination, int size)
    {
        fixed (byte* ptr = &base.ReadBytes(size)[0])
        {
            byte* source = ptr;
            Buffer.MemoryCopy(source, destination, size, size);
        }
    }

    public unsafe T[] ReadData<T>(int count) where T : unmanaged
    {
        int size = sizeof(T) * count;
        byte[] buffer = new byte[size];
        bool flag = base.Read(buffer, 0, size) != size;
        if (flag)
        {
            throw new InvalidDataException();
        }

        T[] result = new T[count];
        Buffer.BlockCopy(buffer, 0, result, 0, size);
        bool isBigEndian = readOptions.IsBigEndian;
        if (isBigEndian)
        {
            for (int i = 0; i < result.Length; i++)
            {
                int num = sizeof(T);
                int num2 = num;
                if (num2 != 2)
                {
                    if (num2 != 4)
                    {
                        if (num2 == 8)
                        {
                            ulong ret64 = BinaryPrimitives.ReverseEndianness(Unsafe.As<T, ulong>(ref result[i]));
                            result[i] = Unsafe.As<ulong, T>(ref ret64);
                        }
                    }
                    else
                    {
                        uint ret65 = BinaryPrimitives.ReverseEndianness(Unsafe.As<T, uint>(ref result[i]));
                        result[i] = Unsafe.As<uint, T>(ref ret65);
                    }
                }
                else
                {
                    ushort ret66 = BinaryPrimitives.ReverseEndianness(Unsafe.As<T, ushort>(ref result[i]));
                    result[i] = Unsafe.As<ushort, T>(ref ret66);
                }
            }
        }

        return result;
    }

    public void SeekAtCurrent(int size)
    {
        base.BaseStream.Seek(size, SeekOrigin.Current);
    }
}
