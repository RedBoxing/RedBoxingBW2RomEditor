using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RedBoxingBW2RomEditor.Nitro;
using RedBoxingBW2RomEditor.Formats;
using RedBoxingBW2RomEditor.Formats.Nitro;

using static RedBoxingBW2RomEditor.Nitro.Structures;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using RedBoxingBW2RomEditor.Data;
using RedBoxingBW2RomEditor.Helper;
using RedBoxingBW2RomEditor.Utils;

namespace RedBoxingBW2RomEditor
{
    public class RomInfo
    {
        public ROMHeader header;
        public Banner banner;

        private string file;
        private sFolder root;

        private ushort[] sortedIds;
        private int idSelect;
        private int lastFileId;
        private int lastFolderId;

        public string workingFolder;

        public int mapCount;
        public long headerCount;
        public int matrixCount;
        public int texturesCount;
        public int bldTexturesCount;
        public int bld2TexturesCount;
        public int textCount;
        public int scriptCount;
        public List<string> nameText = new List<string>();

        public MapHeader[] mapHeaders;

        public RomInfo(string file)
        {
            this.file = file;

            try
            {
                this.header = NDS.LoadRom(file);
                this.banner = NDS.ReadBanner(file, header.bannerOffset);
            }
            catch
            {
                MessageBox.Show("Error opening game. Are you sure it's a NDS game ?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public long LoadROM(Form1 form)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            workingFolder = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + "\\";
            form.SetStatus("Loading ROM...");
            LoadOrUnpackROM();

            form.SetStatus("Extracting packages...");

            Narc.Open(workingFolder + @"data\a\0\0\8").ExtractToFolder(workingFolder + @"data\a\0\0\maps", false);
            Narc.Open(workingFolder + @"data\a\0\1\2").ExtractToFolder(workingFolder + @"data\a\0\1\headers", false);
            Narc.Open(workingFolder + @"data\a\0\0\9").ExtractToFolder(workingFolder + @"data\a\0\0\matrix", false);
            Narc.Open(workingFolder + @"data\a\0\1\4").ExtractToFolder(workingFolder + @"data\a\0\1\tilesets", false);
            Narc.Open(workingFolder + @"data\a\1\7\4").ExtractToFolder(workingFolder + @"data\a\1\7\bldtilesets", false);
            Narc.Open(workingFolder + @"data\a\1\7\5").ExtractToFolder(workingFolder + @"data\a\1\7\bld2tilesets", false);
            Narc.Open(workingFolder + @"data\a\0\0\2").ExtractToFolder(workingFolder + @"data\a\0\0\texts", false);
            Narc.Open(workingFolder + @"data\a\0\0\3").ExtractToFolder(workingFolder + @"data\a\0\0\texts2", false);
            Narc.Open(workingFolder + @"data\a\0\5\6").ExtractToFolder(workingFolder + @"data\a\0\5\scripts", false);

            if (new FileInfo(workingFolder + @"arm9.bin").Length < 0xA0000)
            {
                BinaryWriter arm9Truncate = new BinaryWriter(File.OpenWrite(workingFolder + @"arm9.bin"));
                long arm9Length = new FileInfo(workingFolder + @"arm9.bin").Length;
                arm9Truncate.BaseStream.SetLength(arm9Length - 0xc);
                arm9Truncate.Close();
            }

            Process decompress = new Process();
            decompress.StartInfo.FileName = @"blz.exe";
            decompress.StartInfo.Arguments = @" -d " + '"' + workingFolder + "arm9.bin" + '"';
            decompress.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            decompress.StartInfo.CreateNoWindow = true;
            decompress.Start();
            decompress.WaitForExit();

            mapCount = Directory.GetFiles(workingFolder + @"data\a\0\0\maps").Length;
            headerCount = new FileInfo(workingFolder + @"data\a\0\1\headers\0000").Length / 48;
            matrixCount = Directory.GetFiles(workingFolder + @"data\a\0\0\matrix").Length;
            texturesCount = Directory.GetFiles(workingFolder + @"data\a\0\1\tilesets").Length;
            bldTexturesCount = Directory.GetFiles(workingFolder + @"data\a\1\7\bldtilesets").Length;
            bld2TexturesCount = Directory.GetFiles(workingFolder + @"data\a\1\7\bld2tilesets").Length;
            textCount = Directory.GetFiles(workingFolder + @"data\a\0\0\texts").Length;
            scriptCount = Directory.GetFiles(workingFolder + @"data\a\0\5\scripts").Length;

            //Map Names
            int mainKey = 31881;
            BinaryReader readText = new BinaryReader(File.OpenRead(workingFolder + @"data\a\0\0\texts\0109"));

            int nameSections = readText.ReadUInt16();
            uint[] sectionOffset = new uint[3];
            uint[] sectionSize = new uint[3];
            int nameCount = readText.ReadUInt16();
            int stringOffset;
            int stringSize;
            int[] stringUnknown = new int[3];

            sectionSize[0] = readText.ReadUInt32();
            readText.ReadUInt32();
            int key;

            for (int i = 0; i < nameSections; i++)
            {
                sectionOffset[i] = readText.ReadUInt32();
            }

            for (int j = 0; j < nameCount; j++)
            {
                readText.BaseStream.Position = sectionOffset[0];
                sectionSize[0] = readText.ReadUInt32();
                readText.BaseStream.Position += j * 8;
                stringOffset = (int)readText.ReadUInt32();
                stringSize = readText.ReadUInt16();
                stringUnknown[0] = readText.ReadUInt16();
                readText.BaseStream.Position = sectionOffset[0] + stringOffset;
                string pokemonText = "";
                key = mainKey;
                for (int k = 0; k < stringSize; k++)
                {
                    int car = Convert.ToUInt16(readText.ReadUInt16() ^ key);
                    if (car == 0xFFFF)
                    {
                    }
                    else if (car == 0xF100)
                    {
                        pokemonText += @"\xF100";
                    }
                    else if (car == 0xFFFE)
                    {
                        pokemonText += @"\n";
                    }
                    else if (car > 20 && car <= 0xFFF0 && car != 0xF000 && Char.GetUnicodeCategory(Convert.ToChar(car)) != UnicodeCategory.OtherNotAssigned)
                    {
                        pokemonText += Convert.ToChar(car);
                    }
                    else
                    {
                        pokemonText += @"\x" + car.ToString("X4");
                    }
                    key = ((key << 3) | (key >> 13)) & 0xFFFF;
                }
                mainKey += 0x2983;
                if (mainKey > 0xFFFF) mainKey -= 0x10000;
                nameText.Add(pokemonText);
            }
            readText.Close();

            this.mapHeaders = new MapHeader[headerCount];
           BinaryReader readHeader = new BinaryReader(File.OpenRead(workingFolder + @"data\a\0\1\headers\0000"));
            for (int i = 0; i < headerCount; i++)
            {
                /*dataGridView7.Rows.Add("", readHeader.ReadByte(), readHeader.ReadByte(), readHeader.ReadUInt16(), readHeader.ReadUInt16(), readHeader.ReadUInt16(), readHeader.ReadUInt16(), readHeader.ReadUInt16(), readHeader.ReadUInt16(), readHeader.ReadUInt16(), readHeader.ReadUInt16(), readHeader.ReadUInt16(), readHeader.ReadByte(), readHeader.ReadByte(), readHeader.ReadUInt16(), readHeader.ReadUInt16(), nameText[readHeader.ReadByte()], readHeader.ReadByte(), readHeader.ReadByte(), readHeader.ReadByte(), readHeader.ReadByte(), readHeader.ReadByte(), readHeader.ReadUInt16(), readHeader.ReadByte(), readHeader.ReadByte(), readHeader.ReadUInt32(), readHeader.ReadUInt32(), readHeader.ReadUInt32()); // Adds header data to grid
                dataGridView7.Rows[i].HeaderCell.Value = i.ToString();
                dataGridView7.Rows[i].ReadOnly = false;*/

                /*
                    MapType: readHeader.ReadByte(), 
                    ?: readHeader.ReadByte(), 
                    Texture: readHeader.ReadUInt16(), 
                    Matrix: readHeader.ReadUInt16(), 
                    Scripts: readHeader.ReadUInt16(), 
                    Level Scripts: readHeader.ReadUInt16(), 
                    Texts: readHeader.ReadUInt16(), 
                    Music (Spring): readHeader.ReadUInt16(), 
                    Music (Summer): readHeader.ReadUInt16(), 
                    Music (Autumn): readHeader.ReadUInt16(), 
                    Music (Winter): readHeader.ReadUInt16(), 
                    Wild Pokémon: readHeader.ReadByte(), 
                    ?: readHeader.ReadByte(), 
                    Map ID: readHeader.ReadUInt16(),
                    Parent Map ID: readHeader.ReadUInt16(), 
                    Name: nameText[readHeader.ReadByte()], 
                    Name Style: readHeader.ReadByte(), 
                    Weather: readHeader.ReadByte(), 
                    Camera: readHeader.ReadByte(), 
                    ?: readHeader.ReadByte(), 
                    Flags: readHeader.ReadByte(), 
                    ?: readHeader.ReadUInt16(),
                    Name Icon: readHeader.ReadByte(), 
                    ?: readHeader.ReadByte(), 
                    Fly X: readHeader.ReadUInt32(),
                    Fly Y: readHeader.ReadUInt32(), 
                    Fly Z: readHeader.ReadUInt32()); */
                MapHeader header = new MapHeader();
                header.id = i;
                header.type = readHeader.ReadByte();
                header.unk1 = readHeader.ReadByte();
                header.texture = readHeader.ReadUInt16();
                header.matrix = readHeader.ReadUInt16();
                header.scripts = readHeader.ReadUInt16();
                header.level_scripts = readHeader.ReadUInt16();
                header.texts = readHeader.ReadUInt16();
                header.music_spring = readHeader.ReadUInt16();
                header.music_summer = readHeader.ReadUInt16();
                header.music_autumn = readHeader.ReadUInt16();
                header.music_winter = readHeader.ReadUInt16();
                header.wild_pokemon = readHeader.ReadByte();
                header.unk2 = readHeader.ReadByte();
                header.map_id = readHeader.ReadUInt16(); ;
                header.parent_map_id = readHeader.ReadUInt16();
                header.name = nameText[readHeader.ReadByte()];
                header.name_style = readHeader.ReadByte();
                header.weather = readHeader.ReadByte();
                header.camera = readHeader.ReadByte();
                header.unk3 = readHeader.ReadByte();
                header.flags = readHeader.ReadByte();
                header.unk4 = readHeader.ReadUInt16();
                header.name_icon = readHeader.ReadByte();
                header.unk5 = readHeader.ReadByte();
                header.fly_x = readHeader.ReadUInt32();
                header.fly_y = readHeader.ReadUInt32();
                header.fly_z = readHeader.ReadUInt32();
                mapHeaders[i] = header;
            }
            readHeader.Close();
           /* comboBox6.Items.Clear();
            for (int i = 0; i < matrixCount; i++)
            {
                comboBox6.Items.Add(rm.GetString("matrix") + i);
            }
            comboBox6.SelectedIndex = 0;

            comboBox7.Items.Clear();
            listBox6.Items.Clear();
            comboBox7.Items.Add(rm.GetString("untextured"));
            for (int i = 0; i < texturesCount; i++)
            {
                comboBox7.Items.Add(rm.GetString("tileset") + i);
                listBox6.Items.Add(rm.GetString("tileset") + i);
            }
            listBox6.SelectedIndex = 0;
            comboBox8.Items.Clear();
            #region Read Map Names
            for (int i = 0; i < mapCount; i++)
            {
                string nsbmdName = "";
                System.IO.BinaryReader readNames = new System.IO.BinaryReader(File.OpenRead(workingFolder + @"data\a\0\0\maps" + "\\" + i.ToString("D4")));
                readNames.BaseStream.Position = 0x4;
                int offset = (int)readNames.ReadUInt32();
                readNames.BaseStream.Position = offset + 0x34;
                for (int nameLength = 0; nameLength < 16; nameLength++)
                {
                    int currentByte = readNames.ReadByte();
                    byte[] mapBytes = new Byte[] { Convert.ToByte(currentByte) }; // Reads map name
                    if (currentByte != 0) nsbmdName = nsbmdName + Encoding.UTF8.GetString(mapBytes);
                }
                comboBox8.Items.Add(i + ": " + nsbmdName);
                readNames.Close();
            }
            #endregion
            comboBox8.SelectedIndex = 0;

            comboBox5.Items.Clear();
            comboBox9.Items.Clear();
            for (int i = 0; i < scriptCount; i++)
            {
                comboBox5.Items.Add(rm.GetString("script") + i);
            }
            comboBox3.Items.Clear();
            for (int i = 0; i < textCount; i++)
            {
                comboBox3.Items.Add(rm.GetString("text") + i);
            }*/

            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        private void LoadOrUnpackROM()
        {
            if (Directory.Exists(workingFolder))
            {
                if (MessageBox.Show("An already existing project was found ! Do you want to load it ?", "RedBoxing's B2 Rom Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    return;
                }
                else
                {
                    Directory.Delete(workingFolder, true);
                }
            }

            Directory.CreateDirectory(workingFolder);

            Extract();
        }

        public void SaveROM(string destination)
        {
            Narc.FromFolder(workingFolder + @"data\a\0\0\maps\").Save(workingFolder + @"data\a\0\0\8");
            Narc.FromFolder(workingFolder + @"data\a\0\1\headers\").Save(workingFolder + @"data\a\0\1\2");
            Narc.FromFolder(workingFolder + @"data\a\0\0\matrix\").Save(workingFolder + @"data\a\0\0\9");
            Narc.FromFolder(workingFolder + @"data\a\0\1\tilesets\").Save(workingFolder + @"data\a\0\1\4");
            Narc.FromFolder(workingFolder + @"data\a\1\7\bldtilesets").Save(workingFolder + @"data\a\1\7\4");
            Narc.FromFolder(workingFolder + @"data\a\1\7\bld2tilesets").Save(workingFolder + @"data\a\1\7\5");
            Narc.FromFolder(workingFolder + @"data\a\0\0\texts\").Save(workingFolder + @"data\a\0\0\2");
            Narc.FromFolder(workingFolder + @"data\a\0\0\texts2\").Save(workingFolder + @"data\a\0\0\3");
            Narc.FromFolder(workingFolder + @"data\a\0\5\scripts\").Save(workingFolder + @"data\a\0\5\6");

            Directory.Delete(workingFolder + @"data\a\0\0\maps\", true);
            Directory.Delete(workingFolder + @"data\a\0\1\headers\", true);
            Directory.Delete(workingFolder + @"data\a\0\0\matrix\", true);
            Directory.Delete(workingFolder + @"data\a\0\1\tilesets\", true);
            Directory.Delete(workingFolder + @"data\a\1\7\bldtilesets", true);
            Directory.Delete(workingFolder + @"data\a\1\7\bld2tilesets", true);
            Directory.Delete(workingFolder + @"data\a\0\0\texts\", true);
            Directory.Delete(workingFolder + @"data\a\0\0\texts2\", true);
            Directory.Delete(workingFolder + @"data\a\0\5\scripts\", true);

            Process repack = new Process();
            repack.StartInfo.FileName = @"ndstool.exe";
            repack.StartInfo.Arguments = "-c " + '"' + destination + '"' + " -9 " + '"' + workingFolder + "arm9.bin" + '"' + " -7 " + '"' + workingFolder + "arm7.bin" + '"' + " -y9 " + '"' + workingFolder + "y9.bin" + '"' + " -y7 " + '"' + workingFolder + "y7.bin" + '"' + " -d " + '"' + workingFolder + "data" + '"' + " -y " + '"' + workingFolder + "overlay" + '"' + " -t " + '"' + workingFolder + "banner.bin" + '"' + " -h " + '"' + workingFolder + "header.bin" + '"';
            repack.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            repack.StartInfo.CreateNoWindow = true;
            MessageBox.Show(repack.StartInfo.Arguments);
            repack.Start();
            repack.WaitForExit();

            Narc.Open(workingFolder + @"data\a\0\0\8").ExtractToFolder(workingFolder + @"data\a\0\0\maps");
            Narc.Open(workingFolder + @"data\a\0\1\2").ExtractToFolder(workingFolder + @"data\a\0\1\headers");
            Narc.Open(workingFolder + @"data\a\0\0\9").ExtractToFolder(workingFolder + @"data\a\0\0\matrix");
            Narc.Open(workingFolder + @"data\a\0\1\4").ExtractToFolder(workingFolder + @"data\a\0\1\tilesets");
            Narc.Open(workingFolder + @"data\a\1\7\4").ExtractToFolder(workingFolder + @"data\a\1\7\bldtilesets");
            Narc.Open(workingFolder + @"data\a\1\7\5").ExtractToFolder(workingFolder + @"data\a\1\7\bld2tilesets");
            Narc.Open(workingFolder + @"data\a\0\0\2").ExtractToFolder(workingFolder + @"data\a\0\0\texts");
            Narc.Open(workingFolder + @"data\a\0\0\3").ExtractToFolder(workingFolder + @"data\a\0\0\texts2");
            Narc.Open(workingFolder + @"data\a\0\5\6").ExtractToFolder(workingFolder + @"data\a\0\5\scripts");
        }

        public void ExtractFiles(string outdir)
        {
            if (!Directory.Exists(outdir))
            {
                Directory.CreateDirectory(outdir);
            }

            ExtractDirectory("/", outdir, 0xF000);
        }

        public void ExtractOverlayFiles(string overlaydir)
        {
            if (!Directory.Exists(overlaydir))
            {
                Directory.CreateDirectory(overlaydir);
            }

            ExtractOverlayFiles(overlaydir, header.ARM9overlayOffset, header.ARM9overlaySize);
            ExtractOverlayFiles(overlaydir, header.ARM7overlayOffset, header.ARM7overlaySize);
        }

        public void Extract()
        {
            if (!Directory.Exists(workingFolder))
            {
                Directory.CreateDirectory(workingFolder);
            }

            Extract(workingFolder + @"arm9.bin", header.ARM9romOffset, header.ARM9size);
            Extract(workingFolder + @"arm7.bin", header.ARM7romOffset, header.ARM7size);
            Extract(workingFolder + @"banner.bin", header.bannerOffset, 0x840);
            Extract(workingFolder + @"header.bin", 0x0, header.headerSize);
            Extract(workingFolder + @"y9.bin", header.ARM9overlayOffset, header.ARM9overlaySize);
            Extract(workingFolder + @"y7.bin", header.ARM7overlayOffset, header.ARM7overlaySize);

            ExtractFiles(workingFolder + @"data");
            ExtractOverlayFiles(workingFolder + @"overlays");
        }

        private List<string> ParseText(BinaryStream bs)
        {
            ushort nSections = bs.ReadUInt16(), nEntries = bs.ReadUInt16();
            uint sectionSize = bs.ReadUInt32(), unknown = bs.ReadUInt32(), sectionOffset = bs.ReadUInt32();

            List<string> list = new List<string>();
            List<uint> tableOffsets = new List<uint>();
            List<ushort> characterCount = new List<ushort>();
            List<ushort> unknown2 = new List<ushort>();

            bs.BaseStream.Position += 0x4;

            for (int i = 0; i < nEntries; i++)
            {
                tableOffsets.Add(bs.ReadUInt32());
                characterCount.Add(bs.ReadUInt16());
                unknown2.Add(bs.ReadUInt16());
            }

            for (int i = 0; i < nEntries; i++)
            {
                StringBuilder s = new StringBuilder();
                bs.BaseStream.Position = sectionOffset + tableOffsets[i];

                List<ushort> encrypted_text = new List<ushort>();

                for (int j = 0; j < characterCount[i]; j++)
                    encrypted_text.Add(bs.ReadUInt16());

                int key = encrypted_text.Last() ^ 0xFFFF;

                for (int j = characterCount[i] - 1; j >= 0; j--)
                {
                    s.Insert(0, DecryptCharacter(encrypted_text[j], key));
                    if (DecryptCharacter(encrypted_text[j], key).Equals("\\n"))
                        s.Insert(2, "\",\n\"");
                    key = (key >> 3 | key << 13) & 0xFFFF;
                }

                // TODO: Implement text decompression for compresed text.


                // Stupid preprocessing for common things.
                s.Replace("\\xF000븁\\x0000", "\\c"); // Clear character for Gen V.
                s.Replace("\\xF000븀\\x0000", "\\l"); // Scroll to next line.
                s.Replace("\\xF000Ā\\x0001\\x0000", "{PLAYER}"); // Player name.
                s.Replace("\\xF000Ā\\x0001\\x0001", "{RIVAL}"); // Rival name.

                list.Add(s.ToString());
            }

            return list;
        }

        private string DecryptCharacter(ushort encrypted, int key)
        {
            switch (encrypted ^ key)
            {
                case 0xFFFF:
                    return "$";
                case 0xFFFE:
                    return "\\n";
                default:
                    if ((encrypted ^ key) > 0x14 && (encrypted ^ key) < 0xD800)
                        return Convert.ToChar(encrypted ^ key).ToString();
                    else
                        return $"\\x{(encrypted ^ key):X4}";
            }
        }

        private void ExtractDirectory(string prefix, string outdir, uint dir_id)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(file));

            br.BaseStream.Position = header.fileNameTableOffset + 8 * (dir_id & 0xFFF);
            uint entry_start = br.ReadUInt32();
            ushort top_file_id = br.ReadUInt16();
            ushort parent_id = br.ReadUInt16();
            br.BaseStream.Position = header.fileNameTableOffset + entry_start;

            for (uint file_id = top_file_id; ; file_id++)
            {
                byte id = br.ReadByte();
                if (id == 0) break;

                if (id > 0x80) // Folder
                {
                    string entry_name = new string(Encoding.GetEncoding("shift_jis").GetChars(br.ReadBytes(id - 0x80)));
                    ushort dirid = br.ReadUInt16();
                    Directory.CreateDirectory(outdir + prefix + entry_name);
                    ExtractDirectory(prefix + entry_name + "/", outdir, dirid);
                }
                else if (id < 0x80) // File
                {
                    string entry_name = new string(Encoding.GetEncoding("shift_jis").GetChars(br.ReadBytes(id)));
                    ExtractFile(outdir, prefix, entry_name, file_id);
                }
            }

            br.Close();
        }

        private void ExtractFile(string rootdir, string prefix, string entry_name, uint file_id)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(file));

            br.BaseStream.Position = header.FAToffset + 8 * file_id;
            uint start = br.ReadUInt32();
            uint end = br.ReadUInt32();
            uint size = end - start;

            string filename = rootdir + prefix + entry_name;
            br.BaseStream.Position = start;
            File.WriteAllBytes(filename, br.ReadBytes((int)size));
            br.Close();
        }

        private void ExtractOverlayFiles(string overlay_dir, uint overlay_offset, uint overlay_size)
        {
            ARMOverlay overlay;

            for (uint i = 0; i < overlay_size; i += 32)
            {
                overlay = Overlay.ReadOverlay(file, overlay_offset + i);
                uint file_id = overlay.fileID;
                ExtractFile(overlay_dir, "/", file_id.ToString(), file_id);
            }
        }

        private void Extract(string filename, uint offset, uint size)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(file));
            br.BaseStream.Position = offset;
            File.WriteAllBytes(filename, br.ReadBytes((int)size));
            br.Close();
        }

        private void Repack(string filename, string outfile, uint offset)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(outfile));
            bw.BaseStream.Position = offset;
            bw.Write(File.ReadAllBytes(filename));
            bw.Flush();
            bw.Close();
        }

        public void LoadROM()
        {
            sFAT[] fat = FAT.ReadFAT(file, header.FAToffset, header.FATsize);
            sFolder root = FNT.ReadFNT(file, header.fileNameTableOffset, fat);

            lastFileId = fat.Length;
            lastFolderId = root.id + 0xF000;
            root.id = 0xF000;
            this.root = root;

            Add_SystemFiles(fat);
            sortedIds = FAT.SortByOffset(fat);

            if (!Directory.Exists(workingFolder))
                Directory.CreateDirectory(workingFolder);

            ExtractFolder(root, workingFolder);
        }

        public void SaveROM()
        {
            /* ROM sections:
             * 
             * Header (0x0000-0x4000)
             * ARM9 Binary
             *   |_ARM9
             *   |_ARM9 Overlays Tables
             *   |_ARM9 Overlays
             * ARM7 Binary
             *   |_ARM7
             *   |_ARM7 Overlays Tables
             *   |_ARM7 Overlays
             * FNT (File Name Table)
             *   |_Main tables
             *   |_Subtables (names)
             * FAT (File Allocation Table)
             *   |_Files offset
             *     |_Start offset
             *     |_End offset
             * Banner
             *   |_Header 0x20
             *   |_Icon (Bitmap + palette) 0x200 + 0x20
             *   |_Game titles (Japanese, English, French, German, Italian, Spanish) 6 * 0x100
             * Files...
            */

            SaveFileDialog o = new SaveFileDialog();
            o.AddExtension = true;
            o.DefaultExt = ".nds";
            o.Filter = "Nintendo DS ROM (*.nds)|*.nds";
            o.OverwritePrompt = true;
            Open_Dialog:
            if (o.ShowDialog() == DialogResult.OK)
            {
                if (o.FileName == ROMFile)
                {
                    goto Open_Dialog;
                }

                int index = root.files.FindIndex(sFile => sFile.name == "y9.bin");
                Structures.sFile y9 = new Structures.sFile();
                List<Structures.sFile> ov9 = new List<Structures.sFile>();
                if (index != -1)
                {
                    y9 = root.files[index];
                    ov9 = root.files.FindAll(sFile => sFile.name.StartsWith("overlay9_"));
                }

                index = root.files.FindIndex(sFile => sFile.name == "y7.bin");
                List<Structures.sFile> ov7 = new List<Structures.sFile>();
                Structures.sFile y7 = new Structures.sFile();
                if (index != -1)
                {
                    y7 = root.files[index];
                    ov7 = root.files.FindAll(sFile => sFile.name.StartsWith("overlay7_"));
                }

                #region Get ROM sections
                BinaryReader br;

                // Write ARM9
                BinaryWriter bw = new BinaryWriter(new FileStream(o.FileName, FileMode.Create));
                bw.BaseStream.Position = header.headerSize;

                uint size = IOUtils.Append(ref bw, workingFolder + @"/arm9.bin");

                header.ARM9romOffset = (uint)bw.BaseStream.Position - size;
                header.ARM9size = size;
                header.ARM9overlayOffset = 0;
                uint arm9overlayOffset = 0;

                // Write the Nitrocode
                br = new BinaryReader(File.OpenRead(ROMFile));
                br.BaseStream.Position = bw.BaseStream.Position;
                if (br.ReadUInt32() == 0xDEC00621)
                {
                    // Nitrocode found
                    bw.Write(0xDEC00621);
                    bw.Write(br.ReadUInt32());
                    bw.Write(br.ReadUInt32()); ;
                    bw.Flush();
                }
                br.Close();

                uint rem = (uint)bw.BaseStream.Position % 0x200;
                if (rem != 0)
                {
                    while (rem < 0x200)
                    {
                        bw.Write((byte)0xFF);
                        rem++;
                    }
                }

                if (header.ARM9overlaySize != 0)
                {
                    // ARM9 Overlays Tables
                    header.ARM9overlayOffset = (uint)bw.BaseStream.Position;
                    br = new BinaryReader(File.OpenRead(y9.path));
                    br.BaseStream.Position = y9.offset;
                    size = Overlay.Write_Y9(ref bw, br, ov9.ToArray());
                    bw.Flush();
                    br.Close();
                    header.ARM9overlaySize = size;

                    rem = (uint)bw.BaseStream.Position % 0x200;
                    if (rem != 0)
                    {
                        while (rem < 0x200)
                        {
                            bw.Write((byte)0xFF);
                            rem++;
                        }
                    }

                    arm9overlayOffset = (uint)bw.BaseStream.Position;
                    size = Overlay.WriteOverlays(ref bw, ov9, ROMFile); // ARM9 Overlays
                }
                bw.Flush();


                // Escribismo el ARM7 Binary
                size = IOUtils.Append(ref bw, workingFolder + @"/arm7.bin");

                header.ARM7romOffset = (uint)bw.BaseStream.Position - size;
                header.ARM7size = size;
                header.ARM7overlayOffset = 0x00;
                uint arm7overlayOffset = 0x00;

                rem = (uint)bw.BaseStream.Position % 0x200;
                if (rem != 0)
                {
                    while (rem < 0x200)
                    {
                        bw.Write((byte)0xFF);
                        rem++;
                    }
                }

                if (header.ARM7overlaySize != 0x00)
                {
                    // ARM7 Overlays Tables
                    header.ARM7overlayOffset = (uint)bw.BaseStream.Position;
                    size = IOUtils.Append(ref bw, workingFolder + @"/y7.bin");
                    header.ARM7overlaySize = size;

                    rem = (uint)bw.BaseStream.Position % 0x200;
                    if (rem != 0)
                    {
                        while (rem < 0x200)
                        {
                            bw.Write((byte)0xFF);
                            rem++;
                        }
                    }

                    arm7overlayOffset = (uint)bw.BaseStream.Position;
                    size = Overlay.WriteOverlays(ref bw, ov7, ROMFile); // ARM7 Overlays
                }
                bw.Flush();


                // Escribimos el FNT (File Name Table)
                bw.BaseStream.Position = header.fileNameTableOffset;

                header.fileNameTableOffset = (uint)bw.BaseStream.Position;
                size = IOUtils.Append(ref bw, workingFolder + @"/fnt.bin");
                header.fileNameTableSize = size;

                rem = (uint)bw.BaseStream.Position % 0x200;
                if (rem != 0)
                {
                    while (rem < 0x200)
                    {
                        bw.Write((byte)0xFF);
                        rem++;
                    }
                }
                bw.Flush();

                // Escribimos el FAT (File Allocation Table)
                header.FAToffset = (uint)bw.BaseStream.Position;
                FAT.Write(ref bw, root, header.FAToffset, SortedIDs, arm9overlayOffset, arm7overlayOffset);

                // Escribimos el banner
                header.bannerOffset = (uint)bw.BaseStream.Position;
                NDS.SaveBanner(ref bw, this.banner);

                // Escribimos los archivos
                NDS.Write_Files(ref bw, ROMFile, root, SortedIDs);

                // Update the ROM size values of the header
                header.ROMsize = (uint)bw.BaseStream.Position;
                header.size = (uint)Math.Ceiling(Math.Log((uint)bw.BaseStream.Position, 2));
                header.size = (uint)Math.Pow(2, header.size);

                // Get Header CRC
                BinaryStream brHeader = NDS.SaveRom(header, ROMFile);
                header.headerCRC16 = (ushort)CRC16.Calculate(brHeader.ReadBytes(0x15E));
                brHeader.Close();
                brHeader = null;

                // Write header
                bw.BaseStream.Position = 0;
                NDS.SaveRom(ref bw, header, ROMFile);
                #endregion

               
                rem = header.size - (uint)bw.BaseStream.Position;
                while (rem > 0)
                {
                    bw.Write((byte)0xFF);
                    rem--;
                }
                bw.Flush();
                bw.Close();
            }                
        }

        private void Add_SystemFiles(sFAT[] fatTable)
        {
            sFolder overlays = new sFolder();
            overlays.name = "overlays";
            overlays.id = (ushort)LastFolderID;
            overlays.files = new List<sFile>();
            LastFolderID++;

            overlays.files.AddRange(Overlay.ReadBasicOverlays(file, this.header.ARM9overlayOffset, this.header.ARM9overlaySize, true, fatTable));
            overlays.files.AddRange(Overlay.ReadBasicOverlays(file, this.header.ARM7overlayOffset, this.header.ARM7overlaySize, false, fatTable));
            root.folders.Add(overlays);

            sFile fnt = new sFile();
            fnt.name = "fnt.bin";
            fnt.offset = this.header.fileNameTableOffset;
            fnt.size = this.header.fileNameTableSize;
            fnt.path = ROMFile;
            fnt.id = (ushort)LastFileID;
            LastFileID++;
            root.files.Add(fnt);

            sFile fat = new sFile();
            fat.name = "fat.bin";
            fat.offset = this.header.FAToffset;
            fat.size = this.header.FATsize;
            fat.path = ROMFile;
            fat.id = (ushort) LastFileID;
            LastFileID++;
            root.files.Add(fat);

            sFile banner = new sFile();
            banner.name = "banner.bin";
            banner.offset = this.header.bannerOffset;
            banner.size = 0x840;
            banner.path = ROMFile;
            banner.id = (ushort)LastFileID;
            LastFileID++;
            root.files.Add(banner);

            sFile header = new sFile();
            header.name = "header.bin";
            banner.offset = 0x0;
            header.size = this.header.headerSize;
            header.path = ROMFile;
            header.id = (ushort)LastFileID;
            LastFileID++;
            root.files.Add(header);

            sFile arm9 = new sFile();
            arm9.name = "arm9.bin";
            arm9.offset = this.header.ARM9romOffset;
            arm9.size = this.header.ARM9size;
            arm9.path = ROMFile;
            arm9.id = (ushort)LastFileID;
            LastFileID++;
            root.files.Add(arm9);

            sFile arm7 = new sFile();
            arm7.name = "arm7.bin";
            arm7.offset = this.header.ARM7romOffset;
            arm7.size = this.header.ARM7size;
            arm7.path = ROMFile;
            arm7.id = (ushort)LastFileID;
            LastFileID++;
            root.files.Add(arm7);

            if (this.header.ARM9overlaySize != 0)
            {
                sFile y9 = new sFile();
                y9.name = "y9.bin";
                y9.offset = this.header.ARM9overlayOffset;
                y9.size = this.header.ARM9overlaySize;
                y9.path = ROMFile;
                y9.id = (ushort)LastFileID;
                LastFileID++;
                root.files.Add(y9);
            }

            if (this.header.ARM7overlaySize != 0)
            {
                sFile y7 = new sFile();
                y7.name = "y7.bin";
                y7.offset = this.header.ARM7overlayOffset;
                y7.size = this.header.ARM7overlaySize;
                y7.path = ROMFile;
                y7.id = (ushort)LastFileID;
                LastFileID++;
                root.files.Add(y7);
            }
        }

        private void ExtractFolder(sFolder currFolder, String path)
        {
            if (currFolder.files is List<sFile>)
                foreach (sFile archivo in currFolder.files)
                {
                    string filePath = path + Path.DirectorySeparatorChar + archivo.name;
                    for (int i = 0; File.Exists(filePath); i++)
                    {
                        filePath = path + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(archivo.name) + " (" +
                            i.ToString() + ')' + Path.GetExtension(archivo.name);
                    }

                    BinaryReader br = new BinaryReader(File.OpenRead(archivo.path));
                    br.BaseStream.Position = archivo.offset;
                    File.WriteAllBytes(filePath, br.ReadBytes((int)archivo.size));
                    br.Close();
                }



            if (currFolder.folders is List<sFolder>)
            {
                foreach (sFolder subFolder in currFolder.folders)
                {
                    Directory.CreateDirectory(path + Path.DirectorySeparatorChar + subFolder.name);
                    ExtractFolder(subFolder, path + Path.DirectorySeparatorChar + subFolder.name);
                }
            }
        }

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
        public ushort[] SortedIDs
        {
            get { return sortedIds; }
            set { sortedIds = value; }
        }
    }
}
