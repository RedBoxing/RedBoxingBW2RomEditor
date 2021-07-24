using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RedBoxingBW2RomEditor.Nitro;
using static RedBoxingBW2RomEditor.Nitro.Structures;

namespace RedBoxingBW2RomEditor
{
    public partial class TextEditor : Form
    {
        ResourceManager getChar = new ResourceManager("RedBoxingBW2RomEditor.Resources.ReadText", Assembly.GetExecutingAssembly());

        private RomInfo romInfo;
        private bool storyText;

        private ushort stringCount;
        private int initialKey;
        private int textSections;

        public TextEditor()
        {
            InitializeComponent();
        }

        public TextEditor(RomInfo romInfo, bool storyText)
        {
            InitializeComponent();
            this.romInfo = romInfo;

            this.Text = storyText ? "Story Text Editor" : "Game Text Editor";
            this.storyText = storyText;

        }

        private void TextEditor_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView6.Rows.Clear();
            string path;
            int mainKey = 31881;
            bool compressed = false;
            if (radioButton1.Checked)
            {
                path = "texts";
            }
            else
            {
                path = "texts2";
            }
            System.IO.BinaryReader readText = new System.IO.BinaryReader(File.OpenRead(romInfo.workingFolder + @"data\a\0\0\" + path + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            textSections = readText.ReadUInt16();
            numericUpDown7.Value = textSections;
            numericUpDown7_ValueChanged(null, null);
            uint[] sectionOffset = new uint[3];
            uint[] sectionSize = new uint[3];
            stringCount = readText.ReadUInt16();
            if (stringCount == 0) button4.Enabled = false;
            int stringOffset;
            int stringSize;
            int[] stringUnknown = new int[3];
            sectionSize[0] = readText.ReadUInt32();
            initialKey = (int)readText.ReadUInt32();
            int key;
            for (int i = 0; i < textSections; i++)
            {
                sectionOffset[i] = readText.ReadUInt32();
            }
            for (int j = 0; j < stringCount; j++)
            {
                #region Layer 1
                readText.BaseStream.Position = sectionOffset[0];
                sectionSize[0] = readText.ReadUInt32();
                readText.BaseStream.Position += j * 8;
                stringOffset = (int)readText.ReadUInt32();
                stringSize = readText.ReadUInt16();
                stringUnknown[0] = readText.ReadUInt16();
                string pokemonText = "";
                string pokemonText2 = "";
                string pokemonText3 = "";
                readText.BaseStream.Position = sectionOffset[0] + stringOffset;
                key = mainKey;
                for (int k = 0; k < stringSize; k++)
                {
                    int car = Convert.ToUInt16(readText.ReadUInt16() ^ key);
                    if (compressed)
                    {
                        #region Compressed String
                        int shift = 0;
                        int trans = 0;
                        string uncomp = "";
                        while (true)
                        {
                            int tmp = car >> shift;
                            int tmp1 = tmp;
                            if (shift >= 0x10)
                            {
                                shift -= 0x10;
                                if (shift > 0)
                                {
                                    tmp1 = (trans | ((car << (9 - shift)) & 0x1FF));
                                    if ((tmp1 & 0xFF) == 0xFF)
                                    {
                                        break;
                                    }
                                    if (tmp1 != 0x0 && tmp1 != 0x1)
                                    {
                                        uncomp += Convert.ToChar(tmp1);
                                    }
                                }
                            }
                            else
                            {
                                tmp1 = ((car >> shift) & 0x1FF);
                                if ((tmp1 & 0xFF) == 0xFF)
                                {
                                    break;
                                }
                                if (tmp1 != 0x0 && tmp1 != 0x1)
                                {
                                    uncomp += Convert.ToChar(tmp1);
                                }
                                shift += 9;
                                if (shift < 0x10)
                                {
                                    trans = ((car >> shift) & 0x1FF);
                                    shift += 9;
                                }
                                key = ((key << 3) | (key >> 13)) & 0xFFFF;
                                car = Convert.ToUInt16(readText.ReadUInt16() ^ key);
                                k++;
                            }
                        }
                        #endregion
                        pokemonText += uncomp;
                    }
                    else if (car == 0xFFFF)
                    {
                    }
                    else if (car == 0xF100)
                    {
                        compressed = true;
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
                compressed = false;
                #endregion
                #region Layer 2
                if (numericUpDown7.Value > 1)
                {
                    readText.BaseStream.Position = sectionOffset[1];
                    sectionSize[1] = readText.ReadUInt32();
                    readText.BaseStream.Position += j * 8;
                    stringOffset = (int)readText.ReadUInt32();
                    stringSize = readText.ReadUInt16();
                    stringUnknown[1] = readText.ReadUInt16();
                    readText.BaseStream.Position = sectionOffset[1] + stringOffset;
                    key = mainKey;
                    for (int k = 0; k < stringSize; k++)
                    {
                        int car = Convert.ToUInt16(readText.ReadUInt16() ^ key);
                        if (compressed)
                        {
                            #region Compressed String
                            int shift = 0;
                            int trans = 0;
                            string uncomp = "";
                            while (true)
                            {
                                int tmp = car >> shift;
                                int tmp1 = tmp;
                                if (shift >= 0x10)
                                {
                                    shift -= 0x10;
                                    if (shift > 0)
                                    {
                                        tmp1 = (trans | ((car << (9 - shift)) & 0x1FF));
                                        if ((tmp1 & 0xFF) == 0xFF)
                                        {
                                            break;
                                        }
                                        if (tmp1 != 0x0 && tmp1 != 0x1)
                                        {
                                            uncomp += Convert.ToChar(tmp1);
                                        }
                                    }
                                }
                                else
                                {
                                    tmp1 = ((car >> shift) & 0x1FF);
                                    if ((tmp1 & 0xFF) == 0xFF)
                                    {
                                        break;
                                    }
                                    if (tmp1 != 0x0 && tmp1 != 0x1)
                                    {
                                        uncomp += Convert.ToChar(tmp1);
                                    }
                                    shift += 9;
                                    if (shift < 0x10)
                                    {
                                        trans = ((car >> shift) & 0x1FF);
                                        shift += 9;
                                    }
                                    key = ((key << 3) | (key >> 13)) & 0xFFFF;
                                    car = Convert.ToUInt16(readText.ReadUInt16() ^ key);
                                    k++;
                                }
                            }
                            #endregion
                            pokemonText2 += uncomp;
                        }
                        else if (car == 0xFFFF)
                        {
                        }
                        else if (car == 0xF100)
                        {
                            compressed = true;
                        }
                        else if (car == 0xFFFE)
                        {
                            pokemonText2 += @"\n";
                        }
                        else if (car > 20 && car <= 0xFFF0 && car != 0xF000 && Char.GetUnicodeCategory(Convert.ToChar(car)) != UnicodeCategory.OtherNotAssigned)
                        {
                            pokemonText2 += Convert.ToChar(car);
                        }
                        else
                        {
                            pokemonText2 += @"\x" + car.ToString("X4");
                        }
                        key = ((key << 3) | (key >> 13)) & 0xFFFF;
                    }
                    compressed = false;
                }
                #endregion
                #region Layer 3
                if (numericUpDown7.Value > 2)
                {
                    readText.BaseStream.Position = sectionOffset[2];
                    sectionSize[2] = readText.ReadUInt32();
                    readText.BaseStream.Position += j * 8;
                    stringOffset = (int)readText.ReadUInt32();
                    stringSize = readText.ReadUInt16();
                    stringUnknown[2] = readText.ReadUInt16();
                    readText.BaseStream.Position = sectionOffset[2] + stringOffset;
                    key = mainKey;
                    for (int k = 0; k < stringSize; k++)
                    {
                        int car = Convert.ToUInt16(readText.ReadUInt16() ^ key);
                        if (compressed)
                        {
                            #region Compressed String
                            int shift = 0;
                            int trans = 0;
                            string uncomp = "";
                            while (true)
                            {
                                int tmp = car >> shift;
                                int tmp1 = tmp;
                                if (shift >= 0x10)
                                {
                                    shift -= 0x10;
                                    if (shift > 0)
                                    {
                                        tmp1 = (trans | ((car << (9 - shift)) & 0x1FF));
                                        if ((tmp1 & 0xFF) == 0xFF)
                                        {
                                            break;
                                        }
                                        if (tmp1 != 0x0 && tmp1 != 0x1)
                                        {
                                            uncomp += Convert.ToChar(tmp1);
                                        }
                                    }
                                }
                                else
                                {
                                    tmp1 = ((car >> shift) & 0x1FF);
                                    if ((tmp1 & 0xFF) == 0xFF)
                                    {
                                        break;
                                    }
                                    if (tmp1 != 0x0 && tmp1 != 0x1)
                                    {
                                        uncomp += Convert.ToChar(tmp1);
                                    }
                                    shift += 9;
                                    if (shift < 0x10)
                                    {
                                        trans = ((car >> shift) & 0x1FF);
                                        shift += 9;
                                    }
                                    key = ((key << 3) | (key >> 13)) & 0xFFFF;
                                    car = Convert.ToUInt16(readText.ReadUInt16() ^ key);
                                    k++;
                                }
                            }
                            #endregion
                            pokemonText3 += uncomp;
                        }
                        else if (car == 0xFFFF)
                        {
                        }
                        else if (car == 0xF100)
                        {
                            compressed = true;
                        }
                        else if (car == 0xFFFE)
                        {
                            pokemonText3 += @"\n";
                        }
                        else if (car > 20 && car <= 0xFFF0 && car != 0xF000 && Char.GetUnicodeCategory(Convert.ToChar(car)) != UnicodeCategory.OtherNotAssigned)
                        {
                            pokemonText3 += Convert.ToChar(car);
                        }
                        else
                        {
                            pokemonText3 += @"\x" + car.ToString("X4");
                        }
                        key = ((key << 3) | (key >> 13)) & 0xFFFF;
                    }
                    compressed = false;
                }
                #endregion
                dataGridView6.Rows.Add("", pokemonText, stringUnknown[0], pokemonText2, stringUnknown[1], pokemonText3, stringUnknown[2]);
                dataGridView6.Rows[j].HeaderCell.Value = j.ToString();
                mainKey += 0x2983;
                if (mainKey > 0xFFFF) mainKey -= 0x10000;
            }
            readText.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string path;
            int mainKey = 31881;
            if (radioButton1.Checked)
            {
                path = "texts";
            }
            else
            {
                path = "texts2";
            }
            BinaryWriter writeText = new BinaryWriter(File.Create(romInfo.workingFolder + @"data\a\0\0\" + path + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            writeText.Write(Convert.ToUInt16(numericUpDown7.Value));
            writeText.Write(Convert.ToUInt16(stringCount));
            int[] sectionSize = new int[3];
            int[] stringLength = new int[stringCount];
            int[] stringLength2 = new int[stringCount];
            int[] stringLength3 = new int[stringCount];
            int key;
            sectionSize[0] = 4 + (8 * stringCount);
            for (int i = 0; i < stringCount; i++)
            {
                stringLength[i] = getVStringLength(i, 1);
                sectionSize[0] += stringLength[i] * 2;
            }
            if (numericUpDown7.Value > 1)
            {
                sectionSize[1] = 4 + (8 * stringCount);
                for (int i = 0; i < stringCount; i++)
                {
                    stringLength2[i] = getVStringLength(i, 3);
                    sectionSize[1] += stringLength2[i] * 2;
                }
                if (numericUpDown7.Value > 2)
                {
                    sectionSize[2] = 4 + (8 * stringCount);
                    for (int i = 0; i < stringCount; i++)
                    {
                        stringLength3[i] = getVStringLength(i, 3);
                        sectionSize[2] += stringLength3[i] * 2;
                    }
                }
            }
            writeText.Write(sectionSize[0]);
            writeText.Write(initialKey);
            if (numericUpDown7.Value == 1)
            {
                writeText.Write(0x10);
            }
            else if (numericUpDown7.Value == 2)
            {
                writeText.Write(0x14);
                writeText.Write(0x14 + sectionSize[0]);
            }
            else
            {
                writeText.Write(0x18);
                writeText.Write(0x18 + sectionSize[0]);
                writeText.Write(0x18 + sectionSize[0] + sectionSize[1]);
            }

            #region Layer 1
            writeText.Write(sectionSize[0]);
            int offset = 4 + 8 * stringCount;
            for (int i = 0; i < stringCount; i++)
            {
                writeText.Write(offset);
                writeText.Write(Convert.ToUInt16(stringLength[i]));
                writeText.Write(Convert.ToUInt16(dataGridView6.Rows[i].Cells[2].Value));
                offset += stringLength[i] * 2;
            }
            for (int i = 0; i < stringCount; i++)
            {
                int[] currentString = EncodeVString(i, 1, stringLength[i]);
                key = mainKey;
                for (int j = 0; j < stringLength[i]; j++)
                {
                    if (j == stringLength[i] - 1)
                    {
                        writeText.Write(Convert.ToUInt16(key ^ 0xFFFF));
                        break;
                    }
                    writeText.Write(Convert.ToUInt16((currentString[j] ^ key) & 0xFFFF));
                    key = ((key << 3) | (key >> 13)) & 0xFFFF;
                }
                mainKey += 0x2983;
                if (mainKey > 0xFFFF) mainKey -= 0x10000;
            }
            #endregion
            #region Layer 2
            if (numericUpDown7.Value > 1)
            {
                mainKey = 31881;
                writeText.Write(sectionSize[1]);
                offset = 4 + 8 * stringCount;
                for (int i = 0; i < stringCount; i++)
                {
                    writeText.Write(offset);
                    writeText.Write(Convert.ToUInt16(stringLength2[i]));
                    writeText.Write(Convert.ToUInt16(dataGridView6.Rows[i].Cells[4].Value));
                    offset += stringLength2[i] * 2;
                }
                for (int i = 0; i < stringCount; i++)
                {
                    int[] currentString = EncodeVString(i, 3, stringLength2[i]);
                    key = mainKey;
                    for (int j = 0; j < stringLength2[i]; j++)
                    {
                        if (j == stringLength2[i] - 1)
                        {
                            writeText.Write(Convert.ToUInt16(key ^ 0xFFFF));
                            break;
                        }
                        writeText.Write(Convert.ToUInt16((currentString[j] ^ key) & 0xFFFF));
                        key = ((key << 3) | (key >> 13)) & 0xFFFF;
                    }
                    mainKey += 0x2983;
                    if (mainKey > 0xFFFF) mainKey -= 0x10000;
                }
                #region Layer 3
                if (numericUpDown7.Value > 2)
                {
                    mainKey = 31881;
                    writeText.Write(sectionSize[2]);
                    offset = 4 + 8 * stringCount;
                    for (int i = 0; i < stringCount; i++)
                    {
                        writeText.Write(offset);
                        writeText.Write(Convert.ToUInt16(stringLength3[i]));
                        writeText.Write(Convert.ToUInt16(dataGridView6.Rows[i].Cells[6].Value));
                        offset += stringLength3[i] * 2;
                    }
                    for (int i = 0; i < stringCount; i++)
                    {
                        int[] currentString = EncodeVString(i, 5, stringLength3[i]);
                        key = mainKey;
                        for (int j = 0; j < stringLength3[i]; j++)
                        {
                            if (j == stringLength3[i] - 1)
                            {
                                writeText.Write(Convert.ToUInt16(key ^ 0xFFFF));
                                break;
                            }
                            writeText.Write(Convert.ToUInt16((currentString[j] ^ key) & 0xFFFF));
                            key = ((key << 3) | (key >> 13)) & 0xFFFF;
                        }
                        mainKey += 0x2983;
                        if (mainKey > 0xFFFF) mainKey -= 0x10000;
                    }
                }
                #endregion
            }
            #endregion
            writeText.Close();
            #region Name List Updates
            if (comboBox1.SelectedIndex == 109 && radioButton1.Checked) // B2W2 Place Names
            {
                int[] index = new int[romInfo.mapHeaders.Length];
                for (int i = 0; i < romInfo.mapHeaders.Length; i++)
                {
                    //index[i] = romInfo.nameText.IndexOf(dataGridView7.Rows[i].Cells[16].Value.ToString());
                    index[i] = romInfo.nameText.IndexOf(romInfo.mapHeaders[i].name);
                }
                romInfo.nameText.Clear();
                for (int i = 0; i < dataGridView6.RowCount; i++)
                {
                    romInfo.nameText.Add(dataGridView6.Rows[i].Cells[1].Value.ToString());
                }
                for (int i = 0; i < romInfo.mapHeaders.Length; i++)
                {
                    // dataGridView7.Rows[i].Cells[16].Value = romInfo.nameText[index[i]];
                    romInfo.mapHeaders[i].name = romInfo.nameText[index[i]];
                }
            }
            #endregion
        }

        private int getVStringLength(int stringIndex, int column) // Calculates V string length
        {
            int count = 0;
            string currentMessage = "";
            if (dataGridView6[column, stringIndex].Value == null)
            {
                return 1;
            }
            currentMessage = dataGridView6[column, stringIndex].Value.ToString();
            var charArray = currentMessage.ToCharArray();
            for (int i = 0; i < currentMessage.Length; i++)
            {
                if (charArray[i] == '\\')
                {
                    if (charArray[i + 1] == 'n')
                    {
                        count++;
                        i++;
                    }
                    else
                    {
                        if (charArray[i + 1] == 'x')
                        {
                            count++;
                            i += 5;
                        }
                        else
                        {
                            count++;
                        }
                    }
                }
                else
                {
                    count++;
                }
            }
            count++;
            return count;
        }

        private int[] EncodeVString(int stringIndex, int column, int stringSize) // Converts V string to hex characters
        {
            int[] pokemonMessage = new int[stringSize - 1];
            string currentMessage = "";
            try { currentMessage = dataGridView6[column, stringIndex].Value.ToString(); }
            catch { }
            var charArray = currentMessage.ToCharArray();
            int count = 0;
            for (int i = 0; i < currentMessage.Length; i++)
            {
                if (charArray[i] == '\\')
                {
                    if (charArray[i + 1] == 'n')
                    {
                        pokemonMessage[count] = 0xFFFE;
                        i++;
                    }
                    else
                    {
                        if (charArray[i + 1] == 'x')
                        {
                            string characterID = ((char)charArray[i + 2]).ToString() + ((char)charArray[i + 3]).ToString() + ((char)charArray[i + 4]).ToString() + ((char)charArray[i + 5]).ToString();
                            pokemonMessage[count] = (int)Convert.ToUInt32(characterID, 16);
                            i += 5;
                        }
                        else
                        {
                            pokemonMessage[count] = (int)charArray[i];
                        }
                    }
                }
                else
                {
                    pokemonMessage[count] = (int)charArray[i];
                }
                count++;
            }
            return pokemonMessage;
        }


        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                romInfo.textCount = Directory.GetFiles(romInfo.workingFolder + @"data\a\0\0\texts").Length;
            }
            else
            {
                romInfo.textCount = Directory.GetFiles(romInfo.workingFolder + @"data\a\0\0\texts2").Length;
            }

            comboBox1.Items.Clear();
            for (int i = 0; i < romInfo.textCount; i++)
            {
                comboBox1.Items.Add("Text " + i);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int index = dataGridView6.Rows.Count;
            dataGridView6.Rows.Add("", "", 0, "", 0, "", 0);
            dataGridView6.Rows[index].HeaderCell.Value = index.ToString();
            dataGridView6.CurrentCell = dataGridView6.Rows[index].Cells[1];
            stringCount++;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int index = dataGridView6.Rows.Count - 1;
            dataGridView6.Rows.RemoveAt(index);
            stringCount--;
            if (stringCount == 0)
            {
                button4.Enabled = false;
            }
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown7.Value == 1)
            {
                Column51.Visible = false;
                Column52.Visible = false;
                Column53.Visible = false;
                Column54.Visible = false;
            }
            else if (numericUpDown7.Value == 2)
            {
                Column51.Visible = true;
                Column52.Visible = true;
                Column53.Visible = false;
                Column54.Visible = false;
            }
            else
            {
                Column51.Visible = true;
                Column52.Visible = true;
                Column53.Visible = true;
                Column54.Visible = true;
            }
            button3.Enabled = true;
            button5.Enabled = true;
            button4.Enabled = true;
        }
    }
}
