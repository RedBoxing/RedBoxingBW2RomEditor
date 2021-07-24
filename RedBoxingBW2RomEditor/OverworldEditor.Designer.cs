namespace RedBoxingBW2RomEditor
{
    partial class OverworldEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sprite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Movement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unknown1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Flag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Script = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Face = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unknown2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unknown3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LeftRight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpDown = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unknown4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unknown5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unknown6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Z = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(11, 52);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1117, 386);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1109, 360);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Furniture";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1109, 360);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "NPC";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(11, 25);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Overworld:";
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1109, 360);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Warp";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1109, 360);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Trigger";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Sprite,
            this.Movement,
            this.Unknown1,
            this.Flag,
            this.Script,
            this.Face,
            this.Sight,
            this.Unknown2,
            this.Unknown3,
            this.LeftRight,
            this.UpDown,
            this.Unknown4,
            this.Unknown5,
            this.X,
            this.Y,
            this.Unknown6,
            this.Z});
            this.dataGridView1.Location = new System.Drawing.Point(9, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1094, 348);
            this.dataGridView1.TabIndex = 8;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // Sprite
            // 
            this.Sprite.HeaderText = "Sprite";
            this.Sprite.Name = "Sprite";
            // 
            // Movement
            // 
            this.Movement.HeaderText = "Movement";
            this.Movement.Name = "Movement";
            // 
            // Unknown1
            // 
            this.Unknown1.HeaderText = "???";
            this.Unknown1.Name = "Unknown1";
            // 
            // Flag
            // 
            this.Flag.HeaderText = "Flag";
            this.Flag.Name = "Flag";
            // 
            // Script
            // 
            this.Script.HeaderText = "Script";
            this.Script.Name = "Script";
            // 
            // Face
            // 
            this.Face.HeaderText = "Face";
            this.Face.Name = "Face";
            // 
            // Sight
            // 
            this.Sight.HeaderText = "Sight";
            this.Sight.Name = "Sight";
            // 
            // Unknown2
            // 
            this.Unknown2.HeaderText = "???";
            this.Unknown2.Name = "Unknown2";
            // 
            // Unknown3
            // 
            this.Unknown3.HeaderText = "???";
            this.Unknown3.Name = "Unknown3";
            // 
            // LeftRight
            // 
            this.LeftRight.HeaderText = "Left / Right";
            this.LeftRight.Name = "LeftRight";
            // 
            // UpDown
            // 
            this.UpDown.HeaderText = "Up / Down";
            this.UpDown.Name = "UpDown";
            // 
            // Unknown4
            // 
            this.Unknown4.HeaderText = "???";
            this.Unknown4.Name = "Unknown4";
            // 
            // Unknown5
            // 
            this.Unknown5.HeaderText = "???";
            this.Unknown5.Name = "Unknown5";
            // 
            // X
            // 
            this.X.HeaderText = "X";
            this.X.Name = "X";
            // 
            // Y
            // 
            this.Y.HeaderText = "Y";
            this.Y.Name = "Y";
            // 
            // Unknown6
            // 
            this.Unknown6.HeaderText = "???";
            this.Unknown6.Name = "Unknown6";
            // 
            // Z
            // 
            this.Z.HeaderText = "Z";
            this.Z.Name = "Z";
            // 
            // OverworldEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1140, 450);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Name = "OverworldEditor";
            this.Text = "Overworld Editor";
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sprite;
        private System.Windows.Forms.DataGridViewTextBoxColumn Movement;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unknown1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Flag;
        private System.Windows.Forms.DataGridViewTextBoxColumn Script;
        private System.Windows.Forms.DataGridViewTextBoxColumn Face;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sight;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unknown2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unknown3;
        private System.Windows.Forms.DataGridViewTextBoxColumn LeftRight;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpDown;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unknown4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unknown5;
        private System.Windows.Forms.DataGridViewTextBoxColumn X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unknown6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Z;
    }
}