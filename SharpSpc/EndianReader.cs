using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;

namespace SharpSpc;
internal class EndianReader(Stream stream, ReadOptions readOptions) : BinaryReader(stream, readOptions.Encoding)
{

    // Token: 0x06000037 RID: 55 RVA: 0x00002AE2 File Offset: 0x00000CE2
    public override short ReadInt16()
    {
        return (!readOptions.IsBigEndian) ? base.ReadInt16() : BinaryPrimitives.ReadInt16BigEndian(base.ReadBytes(2));
    }

    // Token: 0x06000038 RID: 56 RVA: 0x00002B0A File Offset: 0x00000D0A
    public override float ReadSingle()
    {
        return (!readOptions.IsBigEndian) ? base.ReadSingle() : BinaryPrimitives.ReadSingleBigEndian(base.ReadBytes(4));
    }

    // Token: 0x06000039 RID: 57 RVA: 0x00002B32 File Offset: 0x00000D32
    public override int ReadInt32()
    {
        return (!readOptions.IsBigEndian) ? base.ReadInt32() : BinaryPrimitives.ReadInt16BigEndian(base.ReadBytes(4));
    }

    // Token: 0x0600003A RID: 58 RVA: 0x00002B5A File Offset: 0x00000D5A
    public override double ReadDouble()
    {
        return (!readOptions.IsBigEndian) ? base.ReadDouble() : BinaryPrimitives.ReadDoubleBigEndian(base.ReadBytes(8));
    }

    // Token: 0x0600003B RID: 59 RVA: 0x00002B84 File Offset: 0x00000D84
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
                            ulong ret64 = BinaryPrimitives.ReverseEndianness(*Unsafe.As<T, ulong>(ref result[i]));
                            result[i] = *Unsafe.As<ulong, T>(ref ret64);
                        }
                    }
                    else
                    {
                        uint ret65 = BinaryPrimitives.ReverseEndianness(*Unsafe.As<T, uint>(ref result[i]));
                        result[i] = *Unsafe.As<uint, T>(ref ret65);
                    }
                }
                else
                {
                    ushort ret66 = BinaryPrimitives.ReverseEndianness(*Unsafe.As<T, ushort>(ref result[i]));
                    result[i] = *Unsafe.As<ushort, T>(ref ret66);
                }
            }
        }
        return result;
    }

    // Token: 0x0600003D RID: 61 RVA: 0x00002CE2 File Offset: 0x00000EE2
    public void SeekAtCurrent(int size)
    {
        base.BaseStream.Seek(size, SeekOrigin.Current);
    }
}
