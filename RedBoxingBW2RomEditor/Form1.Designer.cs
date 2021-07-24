namespace RedBoxingBW2RomEditor
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveRomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.romfolderbox = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.SpritesBtn = new System.Windows.Forms.Button();
            this.ScriptsBtn = new System.Windows.Forms.Button();
            this.OverworldBtn = new System.Windows.Forms.Button();
            this.StoryTextBtn = new System.Windows.Forms.Button();
            this.gameTextBtn = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(393, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openRomToolStripMenuItem,
            this.saveRomToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openRomToolStripMenuItem
            // 
            this.openRomToolStripMenuItem.Name = "openRomToolStripMenuItem";
            this.openRomToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openRomToolStripMenuItem.Text = "Open Rom";
            this.openRomToolStripMenuItem.Click += new System.EventHandler(this.openRomToolStripMenuItem_Click);
            // 
            // saveRomToolStripMenuItem
            // 
            this.saveRomToolStripMenuItem.Name = "saveRomToolStripMenuItem";
            this.saveRomToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveRomToolStripMenuItem.Text = "Save Rom";
            this.saveRomToolStripMenuItem.Click += new System.EventHandler(this.saveRomToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // romfolderbox
            // 
            this.romfolderbox.BackColor = System.Drawing.SystemColors.Control;
            this.romfolderbox.Location = new System.Drawing.Point(112, 4);
            this.romfolderbox.Name = "romfolderbox";
            this.romfolderbox.ReadOnly = true;
            this.romfolderbox.Size = new System.Drawing.Size(269, 20);
            this.romfolderbox.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 30);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(369, 147);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SpritesBtn);
            this.tabPage1.Controls.Add(this.ScriptsBtn);
            this.tabPage1.Controls.Add(this.OverworldBtn);
            this.tabPage1.Controls.Add(this.StoryTextBtn);
            this.tabPage1.Controls.Add(this.gameTextBtn);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(361, 121);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // SpritesBtn
            // 
            this.SpritesBtn.Enabled = false;
            this.SpritesBtn.Location = new System.Drawing.Point(126, 35);
            this.SpritesBtn.Name = "SpritesBtn";
            this.SpritesBtn.Size = new System.Drawing.Size(110, 23);
            this.SpritesBtn.TabIndex = 4;
            this.SpritesBtn.Text = "Sprites";
            this.SpritesBtn.UseVisualStyleBackColor = true;
            // 
            // ScriptsBtn
            // 
            this.ScriptsBtn.Enabled = false;
            this.ScriptsBtn.Location = new System.Drawing.Point(6, 35);
            this.ScriptsBtn.Name = "ScriptsBtn";
            this.ScriptsBtn.Size = new System.Drawing.Size(110, 23);
            this.ScriptsBtn.TabIndex = 3;
            this.ScriptsBtn.Text = "Scripts";
            this.ScriptsBtn.UseVisualStyleBackColor = true;
            // 
            // OverworldBtn
            // 
            this.OverworldBtn.Enabled = false;
            this.OverworldBtn.Location = new System.Drawing.Point(245, 6);
            this.OverworldBtn.Name = "OverworldBtn";
            this.OverworldBtn.Size = new System.Drawing.Size(110, 23);
            this.OverworldBtn.TabIndex = 2;
            this.OverworldBtn.Text = "Overworld ";
            this.OverworldBtn.UseVisualStyleBackColor = true;
            // 
            // StoryTextBtn
            // 
            this.StoryTextBtn.Enabled = false;
            this.StoryTextBtn.Location = new System.Drawing.Point(126, 6);
            this.StoryTextBtn.Name = "StoryTextBtn";
            this.StoryTextBtn.Size = new System.Drawing.Size(110, 23);
            this.StoryTextBtn.TabIndex = 1;
            this.StoryTextBtn.Text = "Story Text";
            this.StoryTextBtn.UseVisualStyleBackColor = true;
            this.StoryTextBtn.Click += new System.EventHandler(this.StoryTextBtn_Click);
            // 
            // gameTextBtn
            // 
            this.gameTextBtn.Enabled = false;
            this.gameTextBtn.Location = new System.Drawing.Point(6, 6);
            this.gameTextBtn.Name = "gameTextBtn";
            this.gameTextBtn.Size = new System.Drawing.Size(110, 23);
            this.gameTextBtn.TabIndex = 0;
            this.gameTextBtn.Text = "Game Text";
            this.gameTextBtn.UseVisualStyleBackColor = true;
            this.gameTextBtn.Click += new System.EventHandler(this.gameTextBtn_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.treeView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(361, 121);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(6, 6);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(349, 109);
            this.treeView1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 186);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(393, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(105, 17);
            this.toolStripStatusLabel1.Text = "Waiting for ROM...";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 208);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.romfolderbox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "RBW2RE";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openRomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveRomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.TextBox romfolderbox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button SpritesBtn;
        private System.Windows.Forms.Button ScriptsBtn;
        private System.Windows.Forms.Button OverworldBtn;
        private System.Windows.Forms.Button StoryTextBtn;
        private System.Windows.Forms.Button gameTextBtn;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TreeView treeView1;
    }
}

