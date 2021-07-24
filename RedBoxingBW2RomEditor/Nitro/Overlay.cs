using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RedBoxingBW2RomEditor.Nitro
{
    public static class Overlay
    {
        public static Structures.ARMOverlay ReadOverlay(string file, uint offset)
        {
            Structures.ARMOverlay overlay = new Structures.ARMOverlay();
            BinaryReader br = new BinaryReader(File.OpenRead(file));
            br.BaseStream.Position = offset;

            overlay.fileID = br.ReadUInt32();
            overlay.RAM_Adress = br.ReadUInt32();
            overlay.RAM_Size = br.ReadUInt32();
            overlay.BSS_Size = br.ReadUInt32();
            overlay.stInitStart = br.ReadUInt32();
            overlay.stInitEnd = br.ReadUInt32();
            overlay.fileID = br.ReadUInt32();
            overlay.reserved = br.ReadUInt32();

            br.Close();
            
            return overlay;
        }

        public static Structures.ARMOverlay[] LeerOverlays(string file, UInt32 offset, UInt32 size, bool arm9)
        {
            Structures.ARMOverlay[] overlays = new Structures.ARMOverlay[size / 0x20];
            BinaryReader br = new BinaryReader(File.OpenRead(file));

            for (int i = 0; i < overlays.Length; i++)
            {
                overlays[i] = new Structures.ARMOverlay();

                overlays[i].fileID = br.ReadUInt32();
                overlays[i].RAM_Adress = br.ReadUInt32();
                overlays[i].RAM_Size = br.ReadUInt32();
                overlays[i].BSS_Size = br.ReadUInt32();
                overlays[i].stInitStart = br.ReadUInt32();
                overlays[i].stInitEnd = br.ReadUInt32();
                overlays[i].fileID = br.ReadUInt32();
                overlays[i].reserved = br.ReadUInt32();
                overlays[i].ARM9 = arm9;
            }

            br.Close();

            return overlays;
        }

        public static Structures.sFile[] LeerOverlaysBasico(string file, UInt32 offset, UInt32 size, bool arm9)
        {
            Structures.sFile[] overlays = new Structures.sFile[size / 0x20];
            BinaryReader br = new BinaryReader(File.OpenRead(file));
            br.BaseStream.Position = offset;

            for (int i = 0; i < overlays.Length; i++)
            {
                overlays[i] = new Structures.sFile();
                overlays[i].name = "overlay" + (arm9 ? '9' : '7') + '_' + br.ReadUInt32();
                br.ReadBytes(20);
                overlays[i].id = (ushort)br.ReadUInt32();
                br.ReadBytes(4);

            }

            return overlays;

        }
        public static Structures.sFile[] ReadBasicOverlays(string romFile, UInt32 offset, UInt32 size, bool arm9, Structures.sFAT[] fat)
        {
            Structures.sFile[] overlays = new Structures.sFile[size / 0x20];
            BinaryReader br = new BinaryReader(File.OpenRead(romFile));
            br.BaseStream.Position = offset;

            for (int i = 0; i < overlays.Length; i++)
            {
                overlays[i] = new Structures.sFile();
                overlays[i].name = "overlay" + (arm9 ? '9' : '7') + '_' + br.ReadUInt32();
                br.ReadBytes(20);
                overlays[i].id = (ushort)br.ReadUInt32();
                br.ReadBytes(4);
                overlays[i].offset = fat[overlays[i].id].offset;
                overlays[i].size = fat[overlays[i].id].size;
                overlays[i].path = romFile;
                overlays[i].tag = arm9;
            }

            br.Close();
            return overlays;
        }

        public static uint Write_Y9(ref BinaryWriter bw, BinaryReader br, Structures.sFile[] overlays)
        {
            long start = bw.BaseStream.Position;
            for (int i = 0; i < overlays.Length; i++)
            {
                bw.Write(br.ReadBytes(0x1C));
                byte[] d = BitConverter.GetBytes(overlays[i].size);
                bw.Write(d[0]);
                bw.Write(d[1]);
                bw.Write(d[2]);
                br.BaseStream.Position += 3;
                bw.Write(br.ReadByte());
            }
            long end = bw.BaseStream.Position;
            return (uint)end - (uint)start;
        }

        public static void Write_Y9(BinaryWriter bw, BinaryReader br, uint[] overlays)
        {
            for (int i = 0; i < overlays.Length; i++)
            {
                bw.Write(br.ReadBytes(0x1C));
                byte[] d = BitConverter.GetBytes(overlays[i]);
                bw.Write(d[0]);
                bw.Write(d[1]);
                bw.Write(d[2]);
                br.BaseStream.Position += 3;
                bw.Write(br.ReadByte());
            }
        }

        public static void EscribirOverlays(string salida, List<Structures.sFile> overlays, string romFile)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(salida));
            BinaryReader br = new BinaryReader(File.OpenRead(romFile));

            for (int i = 0; i < overlays.Count; i++)
            {
                if (overlays[i].path == romFile)
                {
                    br.BaseStream.Position = overlays[i].offset;
                    bw.Write(br.ReadBytes((int)overlays[i].size));
                }
                else
                {
                    BinaryReader br2 = new BinaryReader(new FileStream(overlays[i].path, FileMode.Open));
                    br2.BaseStream.Position = overlays[i].offset;
                    bw.Write(br2.ReadBytes((int)overlays[i].size));
                    br2.Close();
                }

                uint rem = (uint)bw.BaseStream.Position % 0x200;
                if (rem != 0)
                {
                    while (rem < 0x200)
                    {
                        bw.Write((byte)0xFF);
                        rem++;
                    }
                }

            }

            br.Close();
            bw.Flush();
            bw.Close();
        }

        public static uint WriteOverlays(ref BinaryWriter bw, List<Structures.sFile> overlays, string romFile)
        {
            long start = bw.BaseStream.Position;
            BinaryReader br = new BinaryReader(File.OpenRead(romFile));

            for (int i = 0; i < overlays.Count; i++)
            {
                
                br.BaseStream.Position = overlays[i].offset;
                bw.Write(br.ReadBytes((int)overlays[i].size));
                
                uint rem = (uint)bw.BaseStream.Position % 0x200;
                if (rem != 0)
                {
                    while (rem < 0x200)
                    {
                        bw.Write(0xFF);
                        rem++;
                    }
                }

            }

            br.Close();
            bw.Flush();

            long end = bw.BaseStream.Position;
            return (uint) end - (uint) start;
        }
    }
}
