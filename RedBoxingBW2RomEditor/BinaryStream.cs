using System;
using System.IO;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;
using System.Text;

namespace RedBoxingBW2RomEditor
{
    public class BinaryStream
    {
        private Stream stream;
        private ByteOrder byteOrder = ByteOrder.Little;

        public BinaryStream()
        {
            stream = new MemoryStream();
        }

        public BinaryStream(byte[] buffer)
        {
            stream = new MemoryStream(buffer);
        }

        public BinaryStream(Stream str)
        {
            stream = str;
        }

        public long Length
        {
            get { return stream.Length; }
        }

        public long Position
        {
            get { return stream.Position; }
            set { stream.Position = value; }
        }

        public ByteOrder ByteOrder
        {
            get { return byteOrder; }
            set { byteOrder = value; }
        }

        public Stream BaseStream
        {
            get { return stream; }
        }

        public byte[] ReadBytes(int length, int offset = -1)
        {
            if (offset == -1) offset = (int)stream.Position;
            Seek(offset);

            byte[] array = new byte[length];
            stream.Read(array, 0, length);
            //stream.Position += length;

            if (this.byteOrder == ByteOrder.Big)
                Array.Reverse(array);

            return array;
        }

        public void WriteBytes(byte[] array, int offset = -1)
        {
            if (offset == -1) offset = (int)stream.Position;
            Seek(offset);

            if (this.byteOrder == ByteOrder.Big)
                Array.Reverse(array);

            stream.Write(array, 0, array.Length);
            //stream.Position += array.Length;
        }

        public byte ReadByte(int offset = -1)
        {
            if (offset == -1) offset = (int)stream.Position;
            Seek(offset);

            return (byte)stream.ReadByte();
        }

        public ushort ReadUInt16(int offset = -1)
        {
            return BitConverter.ToUInt16(ReadBytes(2, offset), 0);
        }

        public uint ReadUInt32(int offset = -1)
        {
            return BitConverter.ToUInt32(ReadBytes(4, offset), 0);
        }

        public ulong ReadUInt64(int offset = -1)
        {
            return BitConverter.ToUInt64(ReadBytes(8, offset), 0);
        }

        public int ReadInt32(int offset = -1)
        {
            return BitConverter.ToInt32(ReadBytes(4, offset), 0);
        }

        public string ReadString(int length, Encoding encoding = null, int offset = -1)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            byte[] bytes = ReadBytes(length, offset);
            return encoding.GetString(bytes);
        }

        public string ReadSignature(int length)
        {
            string RealSignature = ReadString(length, Encoding.ASCII);
            return RealSignature;
        }

        public string ReadSignature(int length, string ExpectedSignature, bool TrimEnd = false)
        {
            string RealSignature = ReadString(length, Encoding.ASCII);

            if (TrimEnd) RealSignature = RealSignature.TrimEnd(' ');

            if (RealSignature != ExpectedSignature)
                throw new Exception($"Invalid signature {RealSignature}! Expected {ExpectedSignature}.");

            return RealSignature;
        }

        public void WriteByte(byte value, int offset = -1)
        {
            if (offset == -1) offset = (int)stream.Position;
            Seek(offset);

            stream.WriteByte(value);
        }

        public void WriteUInt16(ushort value, int offset = -1)
        {
            WriteBytes(BitConverter.GetBytes(value), offset);
        }

        public void WriteUInt32(uint value, int offset = -1)
        {
            WriteBytes(BitConverter.GetBytes(value), offset);
        }

        public void WriteUInt64(ulong value, int offset = -1)
        {
            WriteBytes(BitConverter.GetBytes(value), offset);
        }

        public void WriteString(string value, Encoding encoding = null, int offset = -1)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            WriteBytes(encoding.GetBytes(value), offset);
        }

        public void WriteSignature(string value)
        {
            WriteString(value, Encoding.ASCII);
        }

        public void WriteChars(char[] value, int offset = -1)
        {
            WriteString(new string(value), Encoding.GetEncoding("UTF-8"));
        }

        public byte[] GetBytes()
        {
            var pos = stream.Position;

            byte[] data = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(data, 0, (int)stream.Length);

            stream.Position = pos;
            return data;
        }

        public byte[] getSection(uint offset, uint size)
        {
            Position = offset;
            return ReadBytes((int)size);
        }

        public byte[] getSection(int offset, int size)
        {
            Position = offset;
            return ReadBytes(size);
        }

        public void Seek(long offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            stream.Seek(offset, origin);
        }

        public void Dispose()
        {
            stream.Dispose();
        }

        public void Flush()
        {
            stream.Flush();
        }

        public void Close()
        {
            stream.Close();
        }

        public void CheckByteOrderMark(uint ByteOrderMark)
        {
            SetByteOrder(ByteOrderMark == 0xFEFF);
        }

        public void SetByteOrder(bool IsBigEndian)
        {
            if (IsBigEndian)
                this.byteOrder = ByteOrder.Big;
            else
                this.byteOrder = ByteOrder.Little;
        }
    }

    public enum ByteOrder
    {
        Little,
        Big
    }
}