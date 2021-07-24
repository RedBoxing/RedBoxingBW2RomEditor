using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedBoxingBW2RomEditor.Helper
{
    public static class BitsConverter
    {
        // From Byte
        public static Byte[] ByteToBits(Byte data)
        {
            List<Byte> bits = new List<byte>();

            for (int j = 7; j >= 0; j--)
                bits.Add((byte)((data >> j) & 1));

            return bits.ToArray();
        }
        public static Byte[] ByteToBit2(Byte data)
        {
            Byte[] bit2 = new byte[4];

            bit2[0] = (byte)(data & 0x3);
            bit2[1] = (byte)((data >> 2) & 0x3);
            bit2[2] = (byte)((data >> 4) & 0x3);
            bit2[3] = (byte)((data >> 6) & 0x3);

            return bit2;
        }
        public static Byte[] ByteToBit4(Byte data)
        {
            Byte[] bit4 = new Byte[2];

            bit4[0] = (byte)(data & 0x0F);
            bit4[1] = (byte)((data & 0xF0) >> 4);

            return bit4;
        }
        public static Byte[] BytesToBit4(Byte[] data)
        {
            byte[] bit4 = new byte[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                byte[] b4 = ByteToBit4(data[i]);
                bit4[i * 2] = b4[0];
                bit4[i * 2 + 1] = b4[1];
            }
            return bit4;
        }
        public static String BytesToHexString(Byte[] bytes)
        {
            string result = "";

            for (int i = 0; i < bytes.Length; i++)
                result += String.Format("{0:X}", bytes[i]);

            return result;
        }

        // To Byte
        public static Byte[] BitsToBytes(Byte[] bits)
        {
            List<Byte> bytes = new List<byte>();

            for (int i = 0; i < bits.Length; i += 8)
            {
                Byte newByte = 0;
                int b = 0;
                for (int j = 7; j >= 0; j--, b++)
                {
                    newByte += (byte)(bits[i + b] << j);
                }
                bytes.Add(newByte);
            }

            return bytes.ToArray();
        }
        public static Byte Bit4ToByte(Byte[] data)
        {
            return (byte)(data[0] + (data[1] << 4));
        }
        public static Byte Bit4ToByte(Byte b1, Byte b2)
        {
            return (byte)(b1 + (b2 << 4));
        }
        public static Byte[] Bits4ToByte(Byte[] data)
        {
            byte[] b = new byte[data.Length / 2];

            for (int i = 0; i < data.Length; i += 2)
                b[i / 2] = Bit4ToByte(data[i], data[i + 1]);

            return b;
        }
        public static Byte[] StringToBytes(String text, int num_bytes)
        {
            string hexText = text.Replace("-", "");
            hexText = hexText.PadRight(num_bytes * 2, '0');

            List<Byte> hex = new List<byte>();
            for (int i = 0; i < hexText.Length; i += 2)
                hex.Add(Convert.ToByte(hexText.Substring(i, 2), 16));

            return hex.ToArray();
        }

    }
}
