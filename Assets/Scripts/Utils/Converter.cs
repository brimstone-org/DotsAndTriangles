using System.Runtime.InteropServices;
using System;

namespace DotsTriangle.Utils
{
    public class Convert
    {
        public static byte[] ConvertIntToByteArray(int intValue)
        {
            byte[] intBytes = BitConverter.GetBytes(intValue);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            return intBytes;
        }

        public static int CovertByteArrayToInt(byte[] bytes, int startIndex)
        {
            return BitConverter.ToInt32(bytes, startIndex);
        }

        [StructLayout(LayoutKind.Explicit)]
        struct Int32Converter
        {
            [FieldOffset(0)] public int Value;
            [FieldOffset(0)] public byte Byte1;
            [FieldOffset(1)] public byte Byte2;
            [FieldOffset(2)] public byte Byte3;
            [FieldOffset(3)] public byte Byte4;

            public Int32Converter(int value)
            {
                Byte1 = Byte2 = Byte3 = Byte4 = 0;
                Value = value;
            }

            public static implicit operator Int32(Int32Converter value)
            {
                return value.Value;
            }

            public static implicit operator Int32Converter(int value)
            {
                return new Int32Converter(value);
            }
        }
    }
}
