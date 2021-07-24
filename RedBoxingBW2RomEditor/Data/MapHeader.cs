using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBoxingBW2RomEditor.Data
{
    public struct MapHeader
    {
        public int id;
        public byte type;
        public byte unk1;
        public ushort texture;
        public ushort matrix;
        public ushort scripts;
        public ushort level_scripts;
        public ushort texts;
        public ushort music_spring;
        public ushort music_summer;
        public ushort music_autumn;
        public ushort music_winter;
        public byte wild_pokemon;
        public byte unk2;
        public ushort map_id;
        public ushort parent_map_id;
        public string name;
        public byte name_style;
        public byte weather;
        public byte camera;
        public byte unk3;
        public byte flags;
        public ushort unk4;
        public byte name_icon;
        public byte unk5;
        public uint fly_x;
        public uint fly_y;
        public uint fly_z;
    }
}
