using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace RedBoxingBW2RomEditor.Compression
{
    public class STLibraryCompression
    {
        public class ZLIB
        {
            public static byte[] Decompress(byte[] b, bool hasMagic = true)
            {
                var br = new BinaryStream(b);
                
                    if (br.ReadString(4) == "ZCMP")
                    {
                        return DecompressZCMP(b);
                    }
                    else
                    {
                        var ms = new System.IO.MemoryStream();
                        if (hasMagic)
                        {
                            br.Position = 2;
                            using (var ds = new DeflateStream(new MemoryStream(br.ReadBytes((int)br.BaseStream.Length - 6)), CompressionMode.Decompress))
                                ds.CopyTo(ms);
                        }
                        else
                        {
                            using (var ds = new DeflateStream(new MemoryStream(b), CompressionMode.Decompress))
                                ds.CopyTo(ms);
                        }

                        return ms.ToArray();
                    }
                
            }

            public static Byte[] DecompressZCMP(byte[] b)
            {
                Console.WriteLine("DecompressZCMP");

                var br = new BinaryStream(b);
                
                    var ms = new System.IO.MemoryStream();
                    br.BaseStream.Position = 130;
                    using (var ds = new DeflateStream(new MemoryStream(br.ReadBytes((int)br.BaseStream.Length - 80)), CompressionMode.Decompress))
                        ds.CopyTo(ms);
                    return ms.ToArray();
                
            }

            public static byte[] Compress(byte[] b, uint Position = 0)
            {
                var output = new MemoryStream();
                output.Write(new byte[] { 0x78, 0xDA }, 0, 2);

                using (var zipStream = new DeflateStream(output, CompressionMode.Compress, true))
                    zipStream.Write(b, 0, b.Length);

                //Add this as it weirdly prevents the data getting corrupted
                //From https://github.com/IcySon55/Kuriimu/blob/f670c2719affc1eaef8b4c40e40985881247acc7/src/Kontract/Compression/ZLib.cs
                var adler = b.Aggregate(Tuple.Create(1, 0), (x, n) => Tuple.Create((x.Item1 + n) % 65521, (x.Item1 + x.Item2 + n) % 65521));
                output.Write(new[] { (byte)(adler.Item2 >> 8), (byte)adler.Item2, (byte)(adler.Item1 >> 8), (byte)adler.Item1 }, 0, 4);
                return output.ToArray();
            }

            public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
            {
                byte[] buffer = new byte[2000];
                int len;
                while ((len = input.Read(buffer, 0, 2000)) > 0)
                {
                    output.Write(buffer, 0, len);
                }
                output.Flush();
            }
        }

        public class GZIP
        {
            public static byte[] Decompress(byte[] b)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    using (GZipStream source = new GZipStream(new MemoryStream(b), CompressionMode.Decompress, false))
                    {
                        source.CopyTo(mem);
                    }
                    return mem.ToArray();
                }
            }

            public static byte[] Compress(byte[] b)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(mem,
                        CompressionMode.Compress))
                    {
                        gzip.Write(b, 0, b.Length);
                    }
                    return mem.ToArray();
                }
            }
        }
    }
}
