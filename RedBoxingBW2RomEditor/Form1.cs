using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RedBoxingBW2RomEditor.Helper;
using RedBoxingBW2RomEditor.Nitro;
using RedBoxingBW2RomEditor.Utils;
using static RedBoxingBW2RomEditor.Nitro.Structures;

namespace RedBoxingBW2RomEditor
{
    public partial class Form1 : Form
    {
        private OpenFileDialog openFileDialog1;
        private string workingFolder = "";
        private string ndsFileName = "";

        private RomInfo romInfo;
        private TextEditor textEditor;

        public Form1()
        {
            InitializeComponent();

            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Open ROM File";
            openFileDialog1.Filter = "Nintendo DS ROMs (*.nds)|*.nds|All files (*.*)|*.*";
        }

        private void ROM_ROMIsReady(long TimeTakenMS, string romTitle)
        {
            float num = Convert.ToSingle(TimeTakenMS);
            num /= 1000f;
            this.toolStripStatusLabel1.Text = "ROM loaded. Time taken: " + num.ToString("F2") + " seconds.";

            if (romTitle.StartsWith("POKEMON B2"))
            {
                this.Text = "RedBoxing's B2W2 Rom Editor - Pokémon Black 2";
            }
            else if (romTitle.StartsWith("POKEMON W2"))
            {
                this.Text = "RedBoxing's B2W2 Rom Editor - Pokémon White 2";
            }
            else
            {
                this.Text = "RedBoxing's B2W2 Rom Editor - Unsupported ROM : " + romTitle;
            }

            this.textEditor = new TextEditor(romInfo, false);

            gameTextBtn.Enabled = true;
            StoryTextBtn.Enabled = true;
            OverworldBtn.Enabled = true;
            ScriptsBtn.Enabled = true;
            SpritesBtn.Enabled = true;

            romfolderbox.Text = workingFolder;
        }

        private void openRomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                toolStripStatusLabel1.Text = "Loading ROM...";
                //ReadGame(this.openFileDialog1.FileName);

                this.romInfo = new RomInfo(openFileDialog1.FileName);
                string gameTitle = new string(romInfo.header.gameTitle);

                if (!gameTitle.StartsWith("POKEMON B2") && !gameTitle.StartsWith("POKEMON W2"))
                {
                    MessageBox.Show("Unsupported Pokemon game : " + gameTitle, "RedBoxing's B2W2 Rom Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                this.romInfo.workingFolder = Path.GetDirectoryName(openFileDialog1.FileName) + "\\" + Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                this.romInfo.LoadROM();
                /*long time = this.romInfo.LoadROM(this);
                if (time == -1)
                {
                    SetStatus("Failed to load ROM !");
                }
                else
                {
                    ROM_ROMIsReady(time, gameTitle);
                }*/
            }
        }

        private void saveRomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.romInfo.SaveROM();

           /* SaveFileDialog saveNDS = new SaveFileDialog();
            saveNDS.Title = "Save current game as...";
            saveNDS.Filter = "Nintendo DS ROM files (*.nds)|*.nds";
            saveNDS.FileName = Path.GetFileNameWithoutExtension(ndsFileName);
            if (saveNDS.ShowDialog() == DialogResult.OK)
            {
                toolStripStatusLabel1.Text = "Saving ROM...";

                this.romInfo.SaveROM(saveNDS.FileName);

                toolStripStatusLabel1.Text = "ROM saved to " + saveNDS.FileName;
            }*/
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void gameTextBtn_Click(object sender, EventArgs e)
        {
            this.textEditor.Show();
        }

        public void SetStatus(string status)
        {
            this.toolStripStatusLabel1.Text = status;
        }

        private void StoryTextBtn_Click(object sender, EventArgs e)
        {
            this.textEditor.Show();
        }
    }
}
