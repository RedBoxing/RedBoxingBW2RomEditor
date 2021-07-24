using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace RedBoxingBW2RomEditor.Nitro
{
    /// <summary>
    /// File Name Table.
    /// </summary>
    public static class FNT
    {
        /// <summary>
        /// Devuelve el sistema de archivos internos de la ROM
        /// </summary>
        /// <param name="file">Archivo ROM</param>
        /// <param name="offset">Offset donde comienza la FNT</param>
        /// <returns></returns>
        public static Structures.sFolder ReadFNT(string file, UInt32 offset)
        {
            Structures.sFolder root = new Structures.sFolder();
            List<Structures.MainFNT> mains = new List<Structures.MainFNT>();

            BinaryReader br = new BinaryReader(File.OpenRead(file));
            br.BaseStream.Position = offset;

            long offsetSubTable = br.ReadUInt32();  // Offset donde comienzan las SubTable y terminan las MainTables.
            br.BaseStream.Position = offset;       // Volvemos al principio de la primera MainTable

            while (br.BaseStream.Position < offset + offsetSubTable)
            {
                Structures.MainFNT main = new Structures.MainFNT();
                main.offset = br.ReadUInt32();
                main.idFirstFile = br.ReadUInt16();
                main.idParentFolder = br.ReadUInt16();

                long currOffset = br.BaseStream.Position;           // Posición guardada donde empieza la siguienta maintable
                br.BaseStream.Position = offset + main.offset;      // SubTable correspondiente

                // SubTable
                byte id = br.ReadByte();                            // Byte que identifica si es carpeta o archivo.
                ushort idFile = main.idFirstFile;

                while (id != 0x0)   // Indicador de fin de la SubTable
                {
                    if (id < 0x80)  // Archivo
                    {
                        Structures.sFile currFile = new Structures.sFile();

                        if (!(main.subTable.files is List<Structures.sFile>))
                            main.subTable.files = new List<Structures.sFile>();

                        int lengthName = id;
                        currFile.name = new String(Encoding.GetEncoding("shift_jis").GetChars(br.ReadBytes(lengthName)));
                        currFile.id = idFile; idFile++;

                        main.subTable.files.Add(currFile);
                    }
                    if (id > 0x80)  // Directorio
                    {
                        Structures.sFolder currFolder = new Structures.sFolder();

                        if (!(main.subTable.folders is List<Structures.sFolder>))
                            main.subTable.folders = new List<Structures.sFolder>();

                        int lengthName = id - 0x80;
                        currFolder.name = new String(Encoding.GetEncoding("shift_jis").GetChars(br.ReadBytes(lengthName)));
                        currFolder.id = br.ReadUInt16();

                        main.subTable.folders.Add(currFolder);
                    }

                    id = br.ReadByte();
                }

                mains.Add(main);
                br.BaseStream.Position = currOffset;
            }

            root = Hierarchize_Folders(mains, 0, "root");
            root.id = 0xF000;

            br.Close();

            return root;
        }

        public static Structures.sFolder ReadFNT(string romFile, uint fntOffset, Structures.sFAT[] fat)
        {
            Structures.sFolder root = new Structures.sFolder();
            root.files = new List<Structures.sFile>();
            root.folders = new List<Structures.sFolder>();

            List<Structures.MainFNT> mains = new List<Structures.MainFNT>();

            BinaryReader br = new BinaryReader(File.OpenRead(romFile));
            br.BaseStream.Position = fntOffset;

            br.BaseStream.Position += 6;
            ushort number_directories = br.ReadUInt16();  // Get the total number of directories (mainTables)
            br.BaseStream.Position = fntOffset;

            for (int i = 0; i < number_directories; i++)
            {
                Structures.MainFNT main = new Structures.MainFNT();
                main.offset = br.ReadUInt32();
                main.idFirstFile = br.ReadUInt16();
                main.idParentFolder = br.ReadUInt16();

                if (i != 0)
                {
                    if (br.BaseStream.Position > fntOffset + mains[0].offset)
                    {                                      //  Error, in some cases the number of directories is wrong
                        number_directories--;              // Found in FF Four Heroes of Light, Tetris Party deluxe
                        i--;
                        continue;
                    }
                }

                long currOffset = br.BaseStream.Position;           // Posición guardada donde empieza la siguienta maintable
                br.BaseStream.Position = fntOffset + main.offset;      // SubTable correspondiente

                // SubTable
                byte id = br.ReadByte();                            // Byte que identifica si es carpeta o archivo.
                ushort idFile = main.idFirstFile;

                while (id != 0x0)   // Indicador de fin de la SubTable
                {
                    if (id < 0x80)  // File
                    {
                        Structures.sFile currFile = new Structures.sFile();

                        if (!(main.subTable.files is List<Structures.sFile>))
                            main.subTable.files = new List<Structures.sFile>();

                        int lengthName = id;
                        currFile.name = new String(Encoding.GetEncoding("shift_jis").GetChars(br.ReadBytes(lengthName)));
                        currFile.id = idFile; idFile++;

                        // FAT part
                        currFile.offset = fat[currFile.id].offset;
                        currFile.size = fat[currFile.id].size;
                        currFile.path = romFile;

                        main.subTable.files.Add(currFile);
                    }
                    if (id > 0x80)  // Directorio
                    {
                        Structures.sFolder currFolder = new Structures.sFolder();

                        if (!(main.subTable.folders is List<Structures.sFolder>))
                            main.subTable.folders = new List<Structures.sFolder>();

                        int lengthName = id - 0x80;
                        currFolder.name = new String(Encoding.GetEncoding("shift_jis").GetChars(br.ReadBytes(lengthName)));
                        currFolder.id = br.ReadUInt16();

                        main.subTable.folders.Add(currFolder);
                    }

                    id = br.ReadByte();
                }

                mains.Add(main);
                br.BaseStream.Position = currOffset;
            }

            root.folders.Add(Hierarchize_Folders(mains, mains.Count, "data"));
            root.id = number_directories;

            br.Close();
            return root;
        }

        public static Structures.sFolder Hierarchize_Folders(List<Structures.MainFNT> tables, int idFolder, string nameFolder)
        {
            Structures.sFolder currFolder = new Structures.sFolder();

            currFolder.name = nameFolder;
            currFolder.id = (ushort)idFolder;
            currFolder.files = tables[idFolder & 0xFFF].subTable.files;

            if (tables[idFolder & 0xFFF].subTable.folders is List<Structures.sFolder>) // Si tiene carpetas dentro.
            {
                currFolder.folders = new List<Structures.sFolder>();

                foreach (Structures.sFolder subFolder in tables[idFolder & 0xFFF].subTable.folders)
                    currFolder.folders.Add(Hierarchize_Folders(tables, subFolder.id, subFolder.name));
            }

            return currFolder;
        }

        private static void Obtener_Mains(Structures.sFolder currFolder, List<Structures.MainFNT> mains, int nTotalMains, ushort parent)
        {
            // Añadimos la carpeta actual al sistema
            Structures.MainFNT main = new Structures.MainFNT();
            main.offset = (uint)(nTotalMains * 0x08); // 0x08 == Tamaño de un Main sin SubTable
            main.idFirstFile = (ushort)Obtener_FirstID(currFolder);
            main.idParentFolder = parent;
            main.subTable = currFolder;
            mains.Add(main);

            // Seguimos buscando más carpetas
            if (currFolder.folders is List<Structures.sFolder>)
                foreach (Structures.sFolder subFolder in currFolder.folders)
                    Obtener_Mains(subFolder, mains, nTotalMains, currFolder.id);
        }
        private static int Obtener_FirstID(Structures.sFolder currFolder)
        {
            if (currFolder.folders is List<Structures.sFolder>)
            {
                for (int i = 0; i < currFolder.folders.Count; i++)
                {
                    int id = Obtener_FirstID(currFolder.folders[i]);
                    if (id != -1)
                        return id;
                }
            }

            if (currFolder.files is List<Structures.sFile>)
                return currFolder.files[0].id;

            return -1;
        }

        
    }
}
