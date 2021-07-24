using System;
using System.Linq;
using System.IO;
using Microsoft.Win32;

namespace RedBoxingBW2RomEditor.Utils
{
    public static class IOUtils
    {
        /// <summary>
        /// Returns a 4-byte unsigned integer as used on the NDS converted from four bytes
        /// at a specified position in a byte array.
        /// </summary>
        /// <param name="buffer">The source of the data.</param>
        /// <param name="offset">The location of the data in the source.</param>
        /// <returns>The indicated 4 bytes converted to uint</returns>
        public static uint ToNDSu32(byte[] buffer, int offset)
        {
            return (uint)(buffer[offset]
                        | (buffer[offset + 1] << 8)
                        | (buffer[offset + 2] << 16)
                        | (buffer[offset + 3] << 24));
        }

        /// <summary>
        /// Returns a 4-byte signed integer as used on the NDS converted from four bytes
        /// at a specified position in a byte array.
        /// </summary>
        /// <param name="buffer">The source of the data.</param>
        /// <param name="offset">The location of the data in the source.</param>
        /// <returns>The indicated 4 bytes converted to int</returns>
        public static int ToNDSs32(byte[] buffer, int offset)
        {
            return (int)(buffer[offset]
                        | (buffer[offset + 1] << 8)
                        | (buffer[offset + 2] << 16)
                        | (buffer[offset + 3] << 24));
        }

        /// <summary>
        /// Converts a u32 value into a sequence of bytes that would make ToNDSu32 return
        /// the given input value.
        /// </summary>
        public static byte[] FromNDSu32(uint value)
        {
            return new byte[] {
                (byte)(value & 0xFF),
                (byte)((value >> 8) & 0xFF),
                (byte)((value >> 16) & 0xFF),
                (byte)((value >> 24) & 0xFF)
            };
        }

        /// <summary>
        /// Returns a 3-byte integer as used in the built-in compression
        /// formats in the DS, convrted from three bytes at a specified position in a byte array,
        /// </summary>
        /// <param name="buffer">The source of the data.</param>
        /// <param name="offset">The location of the data in the source.</param>
        /// <returns>The indicated 3 bytes converted to an integer.</returns>
        public static int ToNDSu24(byte[] buffer, int offset)
        {
            return (int)(buffer[offset]
                        | (buffer[offset + 1] << 8)
                        | (buffer[offset + 2] << 16));
        }

        public static uint Append(ref BinaryWriter bw, string file)
        {
            long start = bw.BaseStream.Position;
            BinaryReader br = new BinaryReader(File.OpenRead(file));
            Append(ref bw, ref br);

            br.Close();
            br = null;
            long end = start - bw.BaseStream.Position;
            return (uint)end - (uint)start;
        }
        public static void Append(ref BinaryWriter bw, ref BinaryReader br)
        {
            const int block_size = 0x80000; // 512 KB
            int size = (int)br.BaseStream.Length;

            while (br.BaseStream.Position + block_size < size)
            {
                bw.Write(br.ReadBytes(block_size));
                bw.Flush();
            }

            int rest = size - (int)br.BaseStream.Position;
            bw.Write(br.ReadBytes(rest));
            bw.Flush();
        }

        public static string LastSelectedFile()
        {
            string recent = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
            DirectoryInfo info = new DirectoryInfo(recent);
            FileInfo[] files = info.GetFiles().OrderBy(p => p.LastAccessTime).ToArray();

            if (files.Length > 0)
            {
                for (int i = 1; i <= files.Length; i++)
                {
                    LNK link = new LNK(files[files.Length - i].FullName);
                    if (!link.FileAttribute.archive)
                        continue;

                    return link.Path;
                }
            }

            return null;
        }
        public static string GetLastOpenSaveFile(string extention)
        {
            // IT DOESN'T WORK YET
            RegistryKey regKey = Registry.CurrentUser;
            string lastUsedFolder = string.Empty;
            regKey = regKey.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\ComDlg32\\OpenSavePidlMRU");

            if (string.IsNullOrEmpty(extention))
                return lastUsedFolder;

            RegistryKey myKey = regKey.OpenSubKey(extention);

            if (myKey == null && regKey.GetSubKeyNames().Length > 0)
                return lastUsedFolder;

            string[] names = myKey.GetValueNames();
            if (names != null && names.Length > 0)
            {
                File.WriteAllBytes("G:\\reg.bin", (byte[])myKey.GetValue(names[names.Length - 1]));
                //lastUsedFolder = new String(Encoding.ASCII.GetChars((byte[])myKey.GetValue(names[names.Length - 2])));
            }

            return lastUsedFolder;
        }
    }
}
