using RedBoxingBW2RomEditor.Nitro;
using RedBoxingBW2RomEditor.Formats;
using RedBoxingBW2RomEditor.Formats.Nitro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static RedBoxingBW2RomEditor.Nitro.Structures;
using System.Windows.Forms;

namespace RedBoxingBW2RomEditor
{
    public class Actions
    {
        string file;
        string gameCode;
        Structures.sFolder root;
        ushort[] sortedIds;
        int idSelect;
        int lastFileId;
        int lastFolderId;
        bool isNewRom;

        public Actions(string file, string name)
        {
            this.file = file;
            gameCode = name.Replace("\0", "");
        }

        #region Properties
        public Structures.sFolder Root
        {
            get { return root; }
            set { root = value; }
        }
        public int IDSelect
        {
            get { return idSelect; }
            set { idSelect = value; }
        }
        public String ROMFile
        {
            get { return file; }
        }
        public int LastFileID
        {
            get { return lastFileId; }
            set { lastFileId = value; }
        }
        public int LastFolderID
        {
            get { return lastFolderId; }
            set { lastFolderId = value; }
        }
        public bool IsNewRom
        {
            get { return isNewRom; }
            set { isNewRom = value; }
        }
        public ushort[] SortedIDs
        {
            get { return sortedIds; }
            set { sortedIds = value; }
        }
        #endregion

        public Structures.sFile Search_File(int id)
        {
            return Recursive_File(id, root);
        }

        public sFile Search_File(string name)
        {
            if (name.Contains("/"))
            {
                string[] array = name.Split('/');
                Array.Resize(ref array, array.Length - 1);

                sFolder folder = root;
                foreach (string sname in array)
                {
                    folder = folder.folders.Find(f => f.name == sname);
                }

                return folder.files.Find(f => f.name == name.Split('/').Last());
            }

            return root.files.Find(f => f.name == name);
        }

        private void Recursive_File(string name, sFolder currFolder, sFolder folder)
        {
            if (currFolder.files is List<sFile>)
                foreach (sFile archivo in currFolder.files)
                    if (archivo.name.ToLower().Contains(name.ToLower()))
                        folder.files.Add(archivo);


            if (currFolder.folders is List<sFolder>)
                foreach (sFolder subFolder in currFolder.folders)
                    Recursive_File(name, subFolder, folder);

        }

        private sFile Recursive_File(int id, sFolder currFolder)
        {
            if (currFolder.id == id) // Archivos descomprimidos
            {
                sFile folderFile = new sFile();
                folderFile.name = currFolder.name;
                folderFile.id = currFolder.id;
                folderFile.size = Convert.ToUInt32(((String)currFolder.tag).Substring(0, 8), 16);
                folderFile.offset = Convert.ToUInt32(((String)currFolder.tag).Substring(8, 8), 16);
                folderFile.path = ((string)currFolder.tag).Substring(16);
                folderFile.format = Get_Format(folderFile);
                folderFile.tag = "Descomprimido"; // Tag para indicar que ya ha sido procesado

                return folderFile;
            }

            if (currFolder.files is List<sFile>)
                foreach (sFile archivo in currFolder.files)
                    if (archivo.id == id)
                        return archivo;


            if (currFolder.folders is List<sFolder>)
            {
                foreach (sFolder subFolder in currFolder.folders)
                {
                    sFile currFile = Recursive_File(id, subFolder);
                    if (currFile.name is string)
                        return currFile;
                }
            }

            return new sFile();
        }

        public Structures.sFolder Search_Folder(string name)
        {
            return Recursive_Folder(name, root);
        }

        private Structures.sFolder Recursive_Folder(string name, Structures.sFolder currFolder)
        {
            // Not recommend method to use, can be more than one directory with the same folder
            if (currFolder.name == name)
                return currFolder;

            if (currFolder.folders is List<Structures.sFolder>)
            {
                foreach (Structures.sFolder subFolder in currFolder.folders)
                {
                    if (subFolder.name == name)
                        return subFolder;

                    Structures.sFolder folder = Recursive_Folder(name, subFolder);
                    if (folder.name is string)
                        return folder;
                }
            }

            return new Structures.sFolder();
        }

        public Byte[] Get_MagicID(Structures.sFile currFile)
        {
            byte[] ext;

            BinaryReader br = new BinaryReader(File.OpenRead(currFile.path));
            br.BaseStream.Position = currFile.offset;

            if (currFile.size < 0x04)
                ext = br.ReadBytes((int)currFile.size);
            else
                ext = br.ReadBytes(4);
            br.Close();

            return ext;
        }

        public String Get_MagicIDS(Stream stream, uint size)
        {
            Byte[] buffer = new byte[4];
            if (size < 0x04)
            {
                buffer = new byte[size];
                stream.Read(buffer, 0, (int)size);
            }
            else
                stream.Read(buffer, 0, 4);

            string ext = new String(Encoding.ASCII.GetChars(buffer));

            for (int i = 0; i < ext.Length; i++)
                if ((byte)ext[i] < 0x20 || (byte)ext[i] > 0x7D || ext[i] == '?')   // ASCII chars
                    return "";

            return ext;
        }

        public int ImageFormatFile(Format name)
        {
            switch (name)
            {
                case Format.Font:
                    return 16;
                case Format.Palette:
                    return 2;
                case Format.Tile:
                    return 3;
                case Format.Map:
                    return 9;
                case Format.Cell:
                    return 8;
                case Format.Animation:
                    return 15;
                case Format.FullImage:
                    return 10;
                case Format.Text:
                    return 4;
                case Format.Compressed:
                    return 5;
                case Format.Sound:
                    return 14;
                case Format.Video:
                    return 13;
                case Format.System:
                    return 20;
                case Format.Script:
                    return 17;
                case Format.Texture:
                    return 21;
                case Format.Model3D:
                    return 22;
                case Format.Pack:
                    return 6;
                case Format.Unknown:
                default:
                    return 1;
            }
        }

        public Format Get_Format(int id)
        {
            Format tipo = Format.Unknown;
            sFile currFile = Search_File(id);
            if (currFile.size == 0x00)
                return Format.Unknown;

            byte[] ext = Get_MagicID(currFile);

           /* #region Calling to plugins
            try
            {
                foreach (IGamePlugin format in gamePlugin)
                {
                    tipo = format.Get_Format(currFile, ext);
                    if (tipo != Format.Unknown)
                        return tipo;
                }

                foreach (IPlugin format in formatList)
                {
                    tipo = format.Get_Format(currFile, ext);
                    if (tipo != Format.Unknown)
                        return tipo;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return Format.Unknown;
            }
            #endregion*/

            currFile.name = currFile.name.ToUpper();
            if (currFile.name == "FNT.BIN" || currFile.name == "FAT.BIN" || currFile.name.StartsWith("OVERLAY9_") || currFile.name.StartsWith("OVERLAY7_") ||
                currFile.name == "ARM9.BIN" || currFile.name == "ARM7.BIN" || currFile.name == "Y9.BIN" || currFile.name == "Y7.BIN" || currFile.name == "BANNER.BIN" ||
                currFile.name.EndsWith(".SRL") || currFile.name.EndsWith(".NDS"))
                return Format.System;


            FileStream fs = File.OpenRead(currFile.path);
            fs.Position = currFile.offset;
            if (new String(Encoding.ASCII.GetChars(ext)) == "LZ77") // LZ77
                fs.Seek(4, SeekOrigin.Current);
            if (Get_Format(fs, (currFile.name == "ARM9.BIN" ? true : false)) != FormatCompress.Invalid)
            {
                fs.Close();
                fs.Dispose();
                return Format.Compressed;
            }
            fs.Close();
            fs.Dispose();

            return Format.Unknown;
        }

        public Structures.Format Get_Format(Structures.sFile currFile)
        {
            if (currFile.size == 0x00)
                return Structures.Format.Unknown;

            Structures.Format tipo = Structures.Format.Unknown;
            byte[] ext = Get_MagicID(currFile);

           /* #region Calling to plugins
            try
            {
                foreach (IGamePlugin plugin in gamePlugin)
                {
                    tipo = plugin.Get_Format(currFile, ext);
                    if (tipo != Format.Unknown)
                        return tipo;
                }

                foreach (IPlugin formato in formatList)
                {
                    tipo = formato.Get_Format(currFile, ext);
                    if (tipo != Format.Unknown)
                        return tipo;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return Format.Unknown;
            }
            #endregion*/

            currFile.name = currFile.name.ToUpper();
            if (currFile.name.EndsWith(".SRL") || currFile.name.EndsWith(".NDS"))
                return Structures.Format.System;

            FileStream fs = File.OpenRead(currFile.path);
            fs.Position = currFile.offset;
            if (new String(Encoding.ASCII.GetChars(ext)) == "LZ77") // LZ77
                fs.Seek(4, SeekOrigin.Current);
            if (Get_Format(fs, (currFile.name == "ARM9.BIN" ? true : false)) != Structures.FormatCompress.Invalid)
            {
                fs.Close();
                fs.Dispose();
                return Structures.Format.Compressed;
            }
            fs.Close();
            fs.Dispose();

            if (currFile.name == "FNT.BIN" || currFile.name == "FAT.BIN" || currFile.name.StartsWith("OVERLAY9_") || currFile.name.StartsWith("OVERLAY7_") ||
                currFile.name == "ARM9.BIN" || currFile.name == "ARM7.BIN" || currFile.name == "Y9.BIN" || currFile.name == "Y7.BIN" || currFile.name == "BANNER.BIN")
                return Structures.Format.System;


            return Structures.Format.Unknown;
        }

        public Structures.Format Get_Format(Stream stream, string name, int id, uint size)
        {
            if (size == 0x00)
                return Structures.Format.Unknown;

           /* Structures.sFile cfile = new Structures.sFile();
            cfile.name = name;
            cfile.id = (ushort)id;
            cfile.size = size;
            cfile.offset = (uint)stream.Position;
            cfile.path = file;      */                // When it's called it pass the stream of the rom file.

            //  Structures.Format tipo = Structures.Format.Unknown;
            // Extension
            Byte[] ext = new byte[4];
            if (size < 0x04)
            {
                ext = new byte[size];
                stream.Read(ext, 0, (int)size);
                stream.Position -= size;
            }
            else
            {
                stream.Read(ext, 0, 4);
                stream.Position -= 4;
            }

            /*  #region Calling to plugins
              try
              {
                  foreach (IGamePlugin plugin in gamePlugin)
                  {
                      tipo = plugin.Get_Format(cfile, ext);
                      if (tipo != Format.Unknown)
                          return tipo;
                  }

                  foreach (IPlugin formato in formatList)
                  {
                      tipo = formato.Get_Format(cfile, ext);
                      if (tipo != Format.Unknown)
                          return tipo;
                  }
              }
              catch (Exception e)
              {
                  Console.WriteLine(e.Message);
                  Console.WriteLine(e.StackTrace);
                  return Format.Unknown;
              }
              #endregion*/

            name = name.ToUpper();
            if (name.EndsWith(".SRL") || name.EndsWith(".NDS"))
                return Structures.Format.System;

            FileStream fs = (FileStream)stream;
            if (new String(Encoding.ASCII.GetChars(ext)) == "LZ77") // LZ77
                fs.Seek(4, SeekOrigin.Current);
            if (Get_Format(fs, (name == "ARM9.BIN" ? true : false)) != Structures.FormatCompress.Invalid)
                return Structures.Format.Compressed;

            if (name == "FNT.BIN" || name == "FAT.BIN" || name.StartsWith("OVERLAY9_") || name.StartsWith("OVERLAY7_") ||
                name == "ARM9.BIN" || name == "ARM7.BIN" || name == "Y9.BIN" || name == "Y7.BIN" || name == "BANNER.BIN")
                return Structures.Format.System;

            return Structures.Format.Unknown;
        }

        private Structures.FormatCompress Get_Format(FileStream input, bool arm9)
        {
            CompressionFormat fmt = null;

            foreach (Structures.FormatCompress f in Enum.GetValues(typeof(Structures.FormatCompress)))
            {
                switch (f)
                {
                    //case FormatCompress.LZOVL: fmt = new LZOvl(); break;
                    case Structures.FormatCompress.LZ10: fmt = new LZ10(); break;
                    case Structures.FormatCompress.LZ11: fmt = new LZ11(); break;
                    case Structures.FormatCompress.RLE: fmt = new RLE(); break;
                    case Structures.FormatCompress.HUFF: fmt = new Huffman(); break;
                }

                if (fmt == null)
                    continue;

                long fLength = input.Length;
                if (arm9)
                    fLength -= 0xC;

                if (fmt.Supports(input, fLength))
                    return f;
            }

            return Structures.FormatCompress.Invalid;
        }

        public static BinaryStream GetFile(sFile file)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(file.path));
            br.BaseStream.Position = file.offset;
            byte[] b = br.ReadBytes((int)file.size);
            br.Close();

            return new BinaryStream(b);
        }

        public string ReadFile(sFile file)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(file.path));
            br.BaseStream.Position = file.offset;
            byte[] b = br.ReadBytes((int)file.size);
            br.Close();

            return Encoding.UTF8.GetString(b, 0, b.Length);
        }
    }
}
