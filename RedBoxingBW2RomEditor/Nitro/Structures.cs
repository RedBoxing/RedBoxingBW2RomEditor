using System;
using System.Collections.Generic;

namespace RedBoxingBW2RomEditor.Nitro
{
    public static class Structures
    {
        public static Dictionary<string, string> makerCode;
        public static Dictionary<byte, string> unitCode;

        public struct ROMHeader
        {
            public char[] gameTitle;
            public char[] gameCode;
            public char[] makerCode;
            public byte unitCode;
            public byte encryptionSeed;
            public UInt32 size;
            public byte[] reserved;
            public byte ROMversion;
            public byte internalFlags;
            public UInt32 ARM9romOffset;
            public UInt32 ARM9entryAddress;
            public UInt32 ARM9ramAddress;
            public UInt32 ARM9size;
            public UInt32 ARM7romOffset;
            public UInt32 ARM7entryAddress;
            public UInt32 ARM7ramAddress;
            public UInt32 ARM7size;
            public UInt32 fileNameTableOffset;
            public UInt32 fileNameTableSize;
            public UInt32 FAToffset;            // File Allocation Table offset
            public UInt32 FATsize;              // File Allocation Table size
            public UInt32 ARM9overlayOffset;      // ARM9 overlay file offset
            public UInt32 ARM9overlaySize;
            public UInt32 ARM7overlayOffset;
            public UInt32 ARM7overlaySize;
            public UInt32 flagsRead;            // Control register flags for read
            public UInt32 flagsInit;            // Control register flags for init
            public UInt32 bannerOffset;           // Icon + titles offset
            public UInt16 secureCRC16;          // Secure area CRC16 0x4000 - 0x7FFF
            public UInt16 ROMtimeout;
            public UInt32 ARM9autoload;
            public UInt32 ARM7autoload;
            public UInt64 secureDisable;        // Magic number for unencrypted mode
            public UInt32 ROMsize;
            public UInt32 headerSize;
            public byte[] reserved2;            // 56 bytes
            //public byte[] logo;               // 156 bytes de un logo de nintendo usado para comprobaciones de seguridad
            public UInt16 logoCRC16;
            public UInt16 headerCRC16;
            public bool secureCRC;
            public bool logoCRC;
            public bool headerCRC;
            public UInt32 debug_romOffset;      // only if debug
            public UInt32 debug_size;           // version with
            public UInt32 debug_ramAddress;     // 0 = none, SIO and 8 MB
            public UInt32 reserved3;            // Zero filled transfered and stored but not used
                                                //public byte[] reserved4;          // 0x90 bytes => Zero filled transfered but not stored in RAM

        }
        public struct Banner
        {
            public UInt16 version;      // Always 1
            public UInt16 CRC16;        // CRC-16 of structure, not including first 32 bytes
            public bool checkCRC;
            public byte[] reserved;     // 28 bytes
            public byte[] tileData;     // 512 bytes
            public byte[] palette;      // 32 bytes
            public string japaneseTitle;// 256 bytes
            public string englishTitle; // 256 bytes
            public string frenchTitle;  // 256 bytes
            public string germanTitle;  // 256 bytes
            public string italianTitle; // 256 bytes
            public string spanishTitle; // 256 bytes
        }

        /// <summary>
        /// Estructura de las tablas principales dentro del archivos FNT. Cada una corresponde
        /// a un directorio.
        /// </summary>
        public struct MainFNT
        {
            public UInt32 offset;           // OffSet de la SubTable relativa al archivo FNT
            public UInt16 idFirstFile;      // ID del primer archivo que contiene. Puede corresponder a uno que contenga un directorio interno
            public UInt16 idParentFolder;   // ID del directorio padre de éste
            public sFolder subTable;         // SubTable que contiene los nombres de archivos y carpetas que contiene el directorio
        }

        public struct sFAT
        {
            public uint offset;
            public uint size;
        }

        public struct ARMOverlay
        {
            public UInt32 OverlayID;
            public UInt32 RAM_Adress;   // Point at which to load
            public UInt32 RAM_Size;     // Amount to load
            public UInt32 BSS_Size;     // Size of BSS data region
            public UInt32 stInitStart;  // Static initialiser start address
            public UInt32 stInitEnd;    // Static initialiser end address
            public UInt32 fileID;
            public UInt32 reserved;
            public bool ARM9;           // Si es true es ARM9, sino es ARM7
        }

        public struct sFile
        {
            public UInt32 offset;           // Offset where the files inside of the file in path
            public UInt32 size;             // Length of the file
            public string name;             // File name
            public UInt16 id;               // Internal id
            public string path;             // Path where the file is
            public Format format;           // Format file 
            public Object tag;              // Extra information
        }
        public struct sFolder
        {
            public List<sFile> files;           // List of files
            public List<sFolder> folders;      // List of folders
            public string name;                // Folder name
            public UInt16 id;                  // Internal id
            public Object tag;                 // Extra information
        }


        public enum Format
        {
            Palette,
            Tile,
            Map,
            Cell,
            Animation,
            FullImage,
            Text,
            Video,
            Sound,
            Font,
            Compressed,
            Unknown,
            System,
            Script,
            Pack,
            Model3D,
            Texture
        }
        public enum FormatCompress // From DSDecmp
        {
            LZOVL, // keep this as the first one, as only the end of a file may be LZ-ovl-compressed (and overlay files are oftenly double-compressed)
            LZ10,
            LZ11,
            HUFF4,
            HUFF8,
            RLE,
            HUFF,
            NDS,
            GBA,
            Invalid
        }

        public struct NTFS              // Nintedo Tile Format Screen
        {
            public byte nPalette;        // The parameters (two bytes) is PPPP Y X NNNNNNNNNN
            public byte xFlip;
            public byte yFlip;
            public ushort nTile;
        }
        public struct NTFT              // Nintendo Tile Format Tile
        {
            public byte[] tiles;
            public byte[] nPalette;     // Number of the palette that this tile uses
        }

        public struct NitroHeader    // Generic Header in Nitro formats
        {
            public char[] id;
            public UInt16 endianess;            // 0xFFFE -> little endian
            public UInt16 constant;             // Always 0x0100
            public UInt32 file_size;
            public UInt16 header_size;          // Always 0x10
            public UInt16 nSection;             // Number of sections
        }

    }
}
