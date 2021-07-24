using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using RedBoxingBW2RomEditor.Helper;

namespace RedBoxingBW2RomEditor.Nitro
{
    public static class NDS
    {
        public static Structures.ROMHeader LoadRom(string path)
        {
            Structures.ROMHeader nds = new Structures.ROMHeader();
            BinaryReader br = new BinaryReader(File.OpenRead(path));

            Fill_MakerCodes();
            Fill_UnitCodes();

            nds.gameTitle = br.ReadChars(12);
            nds.gameCode = br.ReadChars(4);
            nds.makerCode = br.ReadChars(2);
            nds.unitCode = br.ReadByte();
            nds.encryptionSeed = br.ReadByte();
            nds.size = (UInt32)Math.Pow(2, 17 + br.ReadByte());
            nds.reserved = br.ReadBytes(9);
            nds.ROMversion = br.ReadByte();
            nds.internalFlags = br.ReadByte();
            nds.ARM9romOffset = br.ReadUInt32();
            nds.ARM9entryAddress = br.ReadUInt32();
            nds.ARM9ramAddress = br.ReadUInt32();
            nds.ARM9size = br.ReadUInt32();
            nds.ARM7romOffset = br.ReadUInt32();
            nds.ARM7entryAddress = br.ReadUInt32();
            nds.ARM7ramAddress = br.ReadUInt32();
            nds.ARM7size = br.ReadUInt32();
            nds.fileNameTableOffset = br.ReadUInt32();
            nds.fileNameTableSize = br.ReadUInt32();
            nds.FAToffset = br.ReadUInt32();
            nds.FATsize = br.ReadUInt32();
            nds.ARM9overlayOffset = br.ReadUInt32();
            nds.ARM9overlaySize = br.ReadUInt32();
            nds.ARM7overlayOffset = br.ReadUInt32();
            nds.ARM7overlaySize = br.ReadUInt32();
            nds.flagsRead = br.ReadUInt32();
            nds.flagsInit = br.ReadUInt32();
            nds.bannerOffset = br.ReadUInt32();
            nds.secureCRC16 = br.ReadUInt16();
            nds.ROMtimeout = br.ReadUInt16();
            nds.ARM9autoload = br.ReadUInt32();
            nds.ARM7autoload = br.ReadUInt32();
            nds.secureDisable = br.ReadUInt64();
            nds.ROMsize = br.ReadUInt32();
            nds.headerSize = br.ReadUInt32();
            nds.reserved2 = br.ReadBytes(56);
            br.BaseStream.Seek(156, SeekOrigin.Current); //nds.logo = br.ReadBytes(156); Logo de Nintendo utilizado para comprobaciones
            nds.logoCRC16 = br.ReadUInt16();
            nds.headerCRC16 = br.ReadUInt16();
            nds.debug_romOffset = br.ReadUInt32();
            nds.debug_size = br.ReadUInt32();
            nds.debug_ramAddress = br.ReadUInt32();
            nds.reserved3 = br.ReadUInt32();

            br.BaseStream.Position = 0x4000;
            nds.secureCRC = (CRC16.Calculate(br.ReadBytes(0x4000)) == nds.secureCRC16) ? true : false;
            br.BaseStream.Position = 0xC0;
            nds.logoCRC = (CRC16.Calculate(br.ReadBytes(156)) == nds.logoCRC16) ? true : false;
            br.BaseStream.Position = 0x0;
            nds.headerCRC = (CRC16.Calculate(br.ReadBytes(0x15E)) == nds.headerCRC16) ? true : false;

            br.Close();

            return nds;
        }

        public static void SaveRom(string destination, Structures.ROMHeader nds, string romFile)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(destination));
            BinaryReader br = new BinaryReader(File.OpenRead(romFile));
            br.BaseStream.Position = 0xC0;

            bw.Write(nds.gameTitle);
            bw.Write(nds.gameCode);
            bw.Write(nds.makerCode);
            bw.Write(nds.unitCode);
            bw.Write(nds.encryptionSeed);
            bw.Write((byte)(Math.Log(nds.size, 2) - 17));
            bw.Write(nds.reserved);
            bw.Write(nds.ROMversion);
            bw.Write(nds.internalFlags);
            bw.Write(nds.ARM9romOffset);
            bw.Write(nds.ARM9entryAddress);
            bw.Write(nds.ARM9ramAddress);
            bw.Write(nds.ARM9size);
            bw.Write(nds.ARM7romOffset);
            bw.Write(nds.ARM7entryAddress);
            bw.Write(nds.ARM7ramAddress);
            bw.Write(nds.ARM7size);
            bw.Write(nds.fileNameTableOffset);
            bw.Write(nds.fileNameTableSize);
            bw.Write(nds.FAToffset);
            bw.Write(nds.FATsize);
            bw.Write(nds.ARM9overlayOffset);
            bw.Write(nds.ARM9overlaySize);
            bw.Write(nds.ARM7overlayOffset);
            bw.Write(nds.ARM7overlaySize);
            bw.Write(nds.flagsRead);
            bw.Write(nds.flagsInit);
            bw.Write(nds.bannerOffset);
            bw.Write(nds.secureCRC16);
            bw.Write(nds.ROMtimeout);
            bw.Write(nds.ARM9autoload);
            bw.Write(nds.ARM7autoload);
            bw.Write(nds.secureDisable);
            bw.Write(nds.ROMsize);
            bw.Write(nds.headerSize);
            bw.Write(nds.reserved2);
            bw.Write(br.ReadBytes(0x9C));
            bw.Write(nds.logoCRC16);
            bw.Write(nds.headerCRC16);
            bw.Write(nds.debug_romOffset);
            bw.Write(nds.debug_size);
            bw.Write(nds.debug_ramAddress);
            bw.Write(nds.reserved3);
            bw.Flush();

            int relleno = (int)(nds.headerSize - bw.BaseStream.Length);
            br.BaseStream.Position = bw.BaseStream.Position;
            for (int i = 0; i < relleno; i++)
                bw.Write(br.ReadByte());

            bw.Flush();
            bw.Close();
            br.Close();
        }

        public static void SaveRom(ref BinaryWriter bw, Structures.ROMHeader nds, string romFile)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(romFile));
            br.BaseStream.Position = 0xC0;

            bw.Write(nds.gameTitle);
            bw.Write(nds.gameCode);
            bw.Write(nds.makerCode);
            bw.Write(nds.unitCode);
            bw.Write(nds.encryptionSeed);
            bw.Write((byte)(Math.Log(nds.size, 2) - 17));
            bw.Write(nds.reserved);
            bw.Write(nds.ROMversion);
            bw.Write(nds.internalFlags);
            bw.Write(nds.ARM9romOffset);
            bw.Write(nds.ARM9entryAddress);
            bw.Write(nds.ARM9ramAddress);
            bw.Write(nds.ARM9size);
            bw.Write(nds.ARM7romOffset);
            bw.Write(nds.ARM7entryAddress);
            bw.Write(nds.ARM7ramAddress);
            bw.Write(nds.ARM7size);
            bw.Write(nds.fileNameTableOffset);
            bw.Write(nds.fileNameTableSize);
            bw.Write(nds.FAToffset);
            bw.Write(nds.FATsize);
            bw.Write(nds.ARM9overlayOffset);
            bw.Write(nds.ARM9overlaySize);
            bw.Write(nds.ARM7overlayOffset);
            bw.Write(nds.ARM7overlaySize);
            bw.Write(nds.flagsRead);
            bw.Write(nds.flagsInit);
            bw.Write(nds.bannerOffset);
            bw.Write(nds.secureCRC16);
            bw.Write(nds.ROMtimeout);
            bw.Write(nds.ARM9autoload);
            bw.Write(nds.ARM7autoload);
            bw.Write(nds.secureDisable);
            bw.Write(nds.ROMsize);
            bw.Write(nds.headerSize);
            bw.Write(nds.reserved2);
            bw.Write(br.ReadBytes(0x9C));
            bw.Write(nds.logoCRC16);
            bw.Write(nds.headerCRC16);
            bw.Write(nds.debug_romOffset);
            bw.Write(nds.debug_size);
            bw.Write(nds.debug_ramAddress);
            bw.Write(nds.reserved3);
            bw.Flush();

            int relleno = (int)(nds.headerSize - bw.BaseStream.Length);
            br.BaseStream.Position = bw.BaseStream.Position;
            for (int i = 0; i < relleno; i++)
                bw.Write(br.ReadByte());

            bw.Flush();
            br.Close();
        }

        public static void SaveRom(string destination, Structures.ROMHeader header, byte[] nintendoLogo)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(destination));

            bw.Write(header.gameTitle);
            bw.Write(header.gameCode);
            bw.Write(header.makerCode);
            bw.Write(header.unitCode);
            bw.Write(header.encryptionSeed);
            bw.Write((byte)(Math.Log(header.size, 2) - 17));
            bw.Write(header.reserved);
            bw.Write(header.ROMversion);
            bw.Write(header.internalFlags);
            bw.Write(header.ARM9romOffset);
            bw.Write(header.ARM9entryAddress);
            bw.Write(header.ARM9ramAddress);
            bw.Write(header.ARM9size);
            bw.Write(header.ARM7romOffset);
            bw.Write(header.ARM7entryAddress);
            bw.Write(header.ARM7ramAddress);
            bw.Write(header.ARM7size);
            bw.Write(header.fileNameTableOffset);
            bw.Write(header.fileNameTableSize);
            bw.Write(header.FAToffset);
            bw.Write(header.FATsize);
            bw.Write(header.ARM9overlayOffset);
            bw.Write(header.ARM9overlaySize);
            bw.Write(header.ARM7overlayOffset);
            bw.Write(header.ARM7overlaySize);
            bw.Write(header.flagsRead);
            bw.Write(header.flagsInit);
            bw.Write(header.bannerOffset);
            bw.Write(header.secureCRC16);
            bw.Write(header.ROMtimeout);
            bw.Write(header.ARM9autoload);
            bw.Write(header.ARM7autoload);
            bw.Write(header.secureDisable);
            bw.Write(header.ROMsize);
            bw.Write(header.headerSize);
            bw.Write(header.reserved2);
            bw.Write(nintendoLogo);
            bw.Write(header.logoCRC16);
            bw.Write(header.headerCRC16);
            bw.Write(header.debug_romOffset);
            bw.Write(header.debug_size);
            bw.Write(header.debug_ramAddress);
            bw.Write(header.reserved3);
            bw.Flush();

            int relleno = (int)(header.headerSize - bw.BaseStream.Length);
            for (int i = 0; i < relleno; i++)
                bw.Write((byte)0x00);

            bw.Flush();
            bw.Close();
        }

        public static BinaryStream SaveRom(Structures.ROMHeader header, string romFile)
        {
            BinaryStream bw = new BinaryStream();
            BinaryReader br = new BinaryReader(File.OpenRead(romFile));
            br.BaseStream.Position = 0xC0;

            bw.WriteChars(header.gameTitle);
            bw.WriteChars(header.gameCode);
            bw.WriteChars(header.makerCode);
            bw.WriteByte(header.unitCode);
            bw.WriteByte(header.encryptionSeed);
            bw.WriteByte((byte)(Math.Log(header.size, 2) - 17));
            bw.WriteBytes(header.reserved);
            bw.WriteByte(header.ROMversion);
            bw.WriteByte(header.internalFlags);
            bw.WriteUInt32(header.ARM9romOffset);
            bw.WriteUInt32(header.ARM9entryAddress);
            bw.WriteUInt32(header.ARM9ramAddress);
            bw.WriteUInt32(header.ARM9size);
            bw.WriteUInt32(header.ARM7romOffset);
            bw.WriteUInt32(header.ARM7entryAddress);
            bw.WriteUInt32(header.ARM7ramAddress);
            bw.WriteUInt32(header.ARM7size);
            bw.WriteUInt32(header.fileNameTableOffset);
            bw.WriteUInt32(header.fileNameTableSize);
            bw.WriteUInt32(header.FAToffset);
            bw.WriteUInt32(header.FATsize);
            bw.WriteUInt32(header.ARM9overlayOffset);
            bw.WriteUInt32(header.ARM9overlaySize);
            bw.WriteUInt32(header.ARM7overlayOffset);
            bw.WriteUInt32(header.ARM7overlaySize);
            bw.WriteUInt32(header.flagsRead);
            bw.WriteUInt32(header.flagsInit);
            bw.WriteUInt32(header.bannerOffset);
            bw.WriteUInt16(header.secureCRC16);
            bw.WriteUInt16(header.ROMtimeout);
            bw.WriteUInt32(header.ARM9autoload);
            bw.WriteUInt32(header.ARM7autoload);
            bw.WriteUInt64(header.secureDisable);
            bw.WriteUInt32(header.ROMsize);
            bw.WriteUInt32(header.headerSize);
            bw.WriteBytes(header.reserved2);
            bw.WriteBytes(br.ReadBytes(0x9C));
            bw.WriteUInt16(header.logoCRC16);
            bw.WriteUInt16(header.headerCRC16);
            bw.WriteUInt32(header.debug_romOffset);
            bw.WriteUInt32(header.debug_size);
            bw.WriteUInt32(header.debug_ramAddress);
            bw.WriteUInt32(header.reserved3);
            bw.Flush();

            int relleno = (int)(header.headerSize - bw.BaseStream.Length);
            for (int i = 0; i < relleno; i++)
                bw.WriteByte((byte)0x00);

            bw.Flush();
            br.Close();

            bw.BaseStream.Position = 0x0;
            return bw;
        }

        public static Structures.Banner ReadBanner(string file, UInt32 offset)
        {
            Structures.Banner bn = new Structures.Banner();
            BinaryReader br = new BinaryReader(File.OpenRead(file));
            br.BaseStream.Position = offset;

            bn.version = br.ReadUInt16();
            bn.CRC16 = br.ReadUInt16();
            bn.reserved = br.ReadBytes(0x1C);
            bn.tileData = br.ReadBytes(0x200);
            bn.palette = br.ReadBytes(0x20);
            bn.japaneseTitle = TitleToString(br.ReadBytes(0x100));
            bn.englishTitle = TitleToString(br.ReadBytes(0x100));
            bn.frenchTitle = TitleToString(br.ReadBytes(0x100));
            bn.germanTitle = TitleToString(br.ReadBytes(0x100));
            bn.italianTitle = TitleToString(br.ReadBytes(0x100));
            bn.spanishTitle = TitleToString(br.ReadBytes(0x100));

            br.BaseStream.Position = offset + 0x20;
            bn.checkCRC = (CRC16.Calculate(br.ReadBytes(0x820)) == bn.CRC16) ? true : false;

            br.Close();

            Console.WriteLine(bn.englishTitle.Replace("\0", ""));

            return bn;
        }
        public static void SaveBanner(string salida, Structures.Banner banner)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(salida, FileMode.Create));

            bw.Write(banner.version);
            bw.Write(banner.CRC16);
            bw.Write(banner.reserved);
            bw.Write(banner.tileData);
            bw.Write(banner.palette);
            bw.Write(StringToTitle(banner.japaneseTitle));
            bw.Write(StringToTitle(banner.englishTitle));
            bw.Write(StringToTitle(banner.frenchTitle));
            bw.Write(StringToTitle(banner.germanTitle));
            bw.Write(StringToTitle(banner.italianTitle));
            bw.Write(StringToTitle(banner.spanishTitle));
            bw.Flush();

            int rem = (int)bw.BaseStream.Position % 0x200;
            if (rem != 0)
            {
                while (rem < 0x200)
                {
                    bw.Write((byte)0xFF);
                    rem++;
                }
            }
            bw.Flush();

            bw.Close();
        }

        public static void SaveBanner(ref BinaryWriter bw, Structures.Banner banner)
        {
            bw.Write(banner.version);
            bw.Write(banner.CRC16);
            bw.Write(banner.reserved);
            bw.Write(banner.tileData);
            bw.Write(banner.palette);
            bw.Write(StringToTitle(banner.japaneseTitle));
            bw.Write(StringToTitle(banner.englishTitle));
            bw.Write(StringToTitle(banner.frenchTitle));
            bw.Write(StringToTitle(banner.germanTitle));
            bw.Write(StringToTitle(banner.italianTitle));
            bw.Write(StringToTitle(banner.spanishTitle));
            bw.Flush();

            int rem = (int)bw.BaseStream.Position % 0x200;
            if (rem != 0)
            {
                while (rem < 0x200)
                {
                    bw.Write((byte)0xFF);
                    rem++;
                }
            }
            bw.Flush();
        }
        public static string TitleToString(byte[] data)
        {
            string title = "";
            title = new String(Encoding.Unicode.GetChars(data));
            title = title.Replace("\n", "\r\n");

            return title;
        }
        public static byte[] StringToTitle(string title)
        {
            List<byte> data = new List<byte>();

            title = title.Replace("\r", "");
            data.AddRange(Encoding.Unicode.GetBytes(title));

            int relleno = 0x100 - data.Count;
            for (int i = 0; i < relleno; i++)
                data.Add(0x00);

            return data.ToArray();
        }
        public static Bitmap IconoToBitmap(byte[] tileData, byte[] paletteData)
        {
            Bitmap imagen = new Bitmap(32, 32);
            Color[] paleta = Images.Actions.BGR555ToColor(paletteData);

            tileData = BitsConverter.BytesToBit4(tileData);
            int i = 0;
            for (int hi = 0; hi < 4; hi++)
            {
                for (int wi = 0; wi < 4; wi++)
                {
                    for (int h = 0; h < 8; h++)
                    {
                        for (int w = 0; w < 8; w++)
                        {
                            imagen.SetPixel(w + wi * 8, h + hi * 8, paleta[tileData[i]]);
                            i++;
                        }
                    }
                }
            }

            return imagen;
        }

        public static void Write_Files(string fileOut, string romFile, Structures.sFolder root, ushort[] sortedIDs)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileOut));
            BinaryReader br = new BinaryReader(File.OpenRead(romFile));

            for (int i = 0; i < sortedIDs.Length; i++)
            {
                if (i == 0 & sortedIDs[i] > sortedIDs.Length)
                    continue;

                Structures.sFile currFile = SearchFile(sortedIDs[i], root);
                if (!(currFile.name is string) || currFile.name.StartsWith("overlay")) // Los overlays no van en esta sección
                    continue;

                if (currFile.path == romFile)
                {
                    br.BaseStream.Position = currFile.offset;
                    bw.Write(br.ReadBytes((int)currFile.size));
                    bw.Flush();
                }
                else
                {
                    BinaryReader br2 = new BinaryReader(File.OpenRead(currFile.path));
                    br2.BaseStream.Position = currFile.offset;
                    bw.Write(br2.ReadBytes((int)currFile.size));

                    br2.Close();
                    bw.Flush();
                }

                // Padd for next file.
                // There is no need to padd last file since no more data will be
                // after it. A full padding of the ROM will be applied later.
                int rem = (int)bw.BaseStream.Position % 0x200;
                if (rem != 0 && i != sortedIDs.Length - 1)
                {
                    while (rem < 0x200)
                    {
                        bw.Write((byte)0xFF);
                        rem++;
                    }
                }
            }

            bw.Flush();
            bw.Close();
            br.Close();
        }

        public static void Write_Files(ref BinaryWriter bw, string romFile, Structures.sFolder root, ushort[] sortedIDs)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(romFile));

            for (int i = 0; i < sortedIDs.Length; i++)
            {
                if (i == 0 & sortedIDs[i] > sortedIDs.Length)
                    continue;

                Structures.sFile currFile = SearchFile(sortedIDs[i], root);
                if (!(currFile.name is string) || currFile.name.StartsWith("overlay")) // Los overlays no van en esta sección
                    continue;

                if (currFile.path == romFile)
                {
                    br.BaseStream.Position = currFile.offset;
                    bw.Write(br.ReadBytes((int)currFile.size));
                    bw.Flush();
                }
                else
                {
                    BinaryReader br2 = new BinaryReader(File.OpenRead(currFile.path));
                    br2.BaseStream.Position = currFile.offset;
                    bw.Write(br2.ReadBytes((int)currFile.size));

                    br2.Close();
                    bw.Flush();
                }

                // Padd for next file.
                // There is no need to padd last file since no more data will be
                // after it. A full padding of the ROM will be applied later.
                int rem = (int)bw.BaseStream.Position % 0x200;
                if (rem != 0 && i != sortedIDs.Length - 1)
                {
                    while (rem < 0x200)
                    {
                        bw.Write((byte)0xFF);
                        rem++;
                    }
                }
            }

            bw.Flush();
            br.Close();
        }

        private static Structures.sFile SearchFile(int id, Structures.sFolder currFolder)
        {
            if (currFolder.id == id) // Archivos descomprimidos
            {
                Structures.sFile folderFile = new Structures.sFile();
                folderFile.name = currFolder.name;
                folderFile.id = currFolder.id;
                folderFile.size = Convert.ToUInt32(((String)currFolder.tag).Substring(0, 8), 16);
                folderFile.offset = Convert.ToUInt32(((String)currFolder.tag).Substring(8, 8), 16);
                folderFile.path = ((string)currFolder.tag).Substring(16);

                return folderFile;
            }

            if (currFolder.files is List<Structures.sFile>)
                foreach (Structures.sFile archivo in currFolder.files)
                    if (archivo.id == id)
                        return archivo;


            if (currFolder.folders is List<Structures.sFolder>)
            {
                foreach (Structures.sFolder subFolder in currFolder.folders)
                {
                    Structures.sFile currFile = SearchFile(id, subFolder);
                    if (currFile.name is string)
                        return currFile;
                }
            }

            return new Structures.sFile();
        }

        private static void Fill_MakerCodes()
        {
            Structures.makerCode = new Dictionary<string, string>();
            Dictionary<string, string> dictionary = Structures.makerCode;

            dictionary.Add("01", "Nintendo");
            dictionary.Add("02", "Rocket Games");
            dictionary.Add("03", "Imagineer Zoom");
            dictionary.Add("04", "Gray Matter");
            dictionary.Add("05", "Zamuse");
            dictionary.Add("06", "Falcom");
            dictionary.Add("07", "Enix");
            dictionary.Add("08", "Capcom");
            dictionary.Add("09", "Hot B");
            dictionary.Add("0A", "Jaleco");

            dictionary.Add("13", "Electronic Arts Japan");
            dictionary.Add("18", "Hudson Entertainment");
            dictionary.Add("20", "Destination Software");
            dictionary.Add("36", "Codemasters");
            dictionary.Add("41", "Ubisoft");
            dictionary.Add("4A", "Gakken");
            dictionary.Add("4F", "Eidos");
            dictionary.Add("4Q", "Disney Interactive Studios");
            dictionary.Add("4Z", "Crave Entertainment");
            dictionary.Add("52", "Activision");
            dictionary.Add("54", "ROCKSTAR GAMES");
            dictionary.Add("5D", "Midway");
            dictionary.Add("5G", "Majesco Entertainment");
            dictionary.Add("64", "LucasArts Entertainment");
            dictionary.Add("69", "Electronic Arts Inc.");
            dictionary.Add("6K", "UFO Interactive");
            dictionary.Add("6V", "JoWooD Entertainment");
            dictionary.Add("70", "Atari");
            dictionary.Add("78", "THQ");
            dictionary.Add("7D", "Vivendi Universal Games");
            dictionary.Add("7J", "Zoo Digital Publishing Ltd");
            dictionary.Add("7N", "Empire Interactive");
            dictionary.Add("7U", "Ignition Entertainment");
            dictionary.Add("7V", "Summitsoft Entertainment");
            dictionary.Add("8J", "General Entertainment");
            dictionary.Add("8P", "SEGA");
            dictionary.Add("99", "Rising Star Games");
            dictionary.Add("A4", "Konami Digital Entertainment");
            dictionary.Add("AF", "Namco");
            dictionary.Add("B2", "Bandai");
            dictionary.Add("E9", "Natsume");
            dictionary.Add("EB", "Atlus");
            dictionary.Add("FH", "Foreign Media Games");
            dictionary.Add("FK", "The Game Factory");
            dictionary.Add("FP", "Mastiff");
            dictionary.Add("FR", "dtp young");
            dictionary.Add("G9", "D3Publisher of America");
            dictionary.Add("GD", "SQUARE ENIX");
            dictionary.Add("GL", "gameloft");
            dictionary.Add("GN", "Oxygen Interactive");
            dictionary.Add("GR", "GSP");
            dictionary.Add("GT", "505 Games");
            dictionary.Add("GQ", "Engine Software");
            dictionary.Add("GY", "The Game Factory");
            dictionary.Add("H3", "Zen");
            dictionary.Add("H4", "SNK PLAYMORE");
            dictionary.Add("H6", "MYCOM");
            dictionary.Add("HC", "Plato");
            dictionary.Add("HF", "Level 5");
            dictionary.Add("HG", "Graffiti Entertainment");
            dictionary.Add("HM", "HMH - INTERACTIVE");
            dictionary.Add("HV", "bhv Software GmbH");
            dictionary.Add("LR", "Asylum Entertainment");
            dictionary.Add("KJ", "Gamebridge");
            dictionary.Add("KM", "Deep Silver");
            dictionary.Add("MJ", "MumboJumbo");
            dictionary.Add("MT", "Blast Entertainment");
            dictionary.Add("NK", "Neko Entertainment");
            dictionary.Add("NP", "Nobilis Publishing");
            dictionary.Add("PG", "Phoenix Games");
            dictionary.Add("PL", "Playlogic");
            dictionary.Add("SU", "Slitherine Software UK Ltd");
            dictionary.Add("SV", "SevenOne Intermedia GmbH");
            dictionary.Add("RM", "rondomedia");
            dictionary.Add("RT", "RTL Games");
            dictionary.Add("TK", "Tasuke");
            dictionary.Add("TR", "Tetris Online");
            dictionary.Add("TV", "Tivola Publishing");
            dictionary.Add("VP", "Virgin Play");
            dictionary.Add("WP", "White Park Bay");
            dictionary.Add("WR", "Warner Bros");
            dictionary.Add("XS", "Aksys Games");
        }

        private static void Fill_UnitCodes()
        {
            Structures.unitCode = new Dictionary<byte, string>();

            Structures.unitCode.Add(0x00, "Nintendo DS");
        }
    }
}
