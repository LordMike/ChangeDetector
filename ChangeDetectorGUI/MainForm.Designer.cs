namespace ChangeDetectorGUI
{
    partial class MainForm
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
            this.tabFilesystemSnapshot = new System.Windows.Forms.TabPage();
            this.tabRegistrySnapshot = new System.Windows.Forms.TabPage();
            this.tabCompareSnapshot = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnStartCompare = new System.Windows.Forms.Button();
            this.btnBrowseFSSnapshotTwo = new System.Windows.Forms.Button();
            this.txtSnapshotFile2 = new System.Windows.Forms.TextBox();
            this.txtSnapshotFile1 = new System.Windows.Forms.TextBox();
            this.btnBrowseFSSnapshotOne = new System.Windows.Forms.Button();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkHashingAlgorithm = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboHashingAlgorithm = new System.Windows.Forms.ComboBox();
            this.snapshotLayout1 = new ChangeDetectorGUI.SnapshotLayout();
            this.snapshotLayout2 = new ChangeDetectorGUI.SnapshotLayout();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabFilesystemSnapshot.SuspendLayout();
            this.tabRegistrySnapshot.SuspendLayout();
            this.tabCompareSnapshot.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabFilesystemSnapshot);
            this.tabControl1.Controls.Add(this.tabRegistrySnapshot);
            this.tabControl1.Controls.Add(this.tabCompareSnapshot);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(557, 273);
            this.tabControl1.TabIndex = 0;
            // 
            // tabFilesystemSnapshot
            // 
            this.tabFilesystemSnapshot.Controls.Add(this.snapshotLayout1);
            this.tabFilesystemSnapshot.Location = new System.Drawing.Point(4, 22);
            this.tabFilesystemSnapshot.Name = "tabFilesystemSnapshot";
            this.tabFilesystemSnapshot.Padding = new System.Windows.Forms.Padding(3);
            this.tabFilesystemSnapshot.Size = new System.Drawing.Size(549, 247);
            this.tabFilesystemSnapshot.TabIndex = 0;
            this.tabFilesystemSnapshot.Text = "Filesystem Snapshot ";
            this.tabFilesystemSnapshot.UseVisualStyleBackColor = true;
            // 
            // tabRegistrySnapshot
            // 
            this.tabRegistrySnapshot.Controls.Add(this.snapshotLayout2);
            this.tabRegistrySnapshot.Location = new System.Drawing.Point(4, 22);
            this.tabRegistrySnapshot.Name = "tabRegistrySnapshot";
            this.tabRegistrySnapshot.Padding = new System.Windows.Forms.Padding(3);
            this.tabRegistrySnapshot.Size = new System.Drawing.Size(549, 247);
            this.tabRegistrySnapshot.TabIndex = 1;
            this.tabRegistrySnapshot.Text = "Registry Snapshot";
            this.tabRegistrySnapshot.UseVisualStyleBackColor = true;
            // 
            // tabCompareSnapshot
            // 
            this.tabCompareSnapshot.Controls.Add(this.groupBox2);
            this.tabCompareSnapshot.Location = new System.Drawing.Point(4, 22);
            this.tabCompareSnapshot.Name = "tabCompareSnapshot";
            this.tabCompareSnapshot.Padding = new System.Windows.Forms.Padding(3);
            this.tabCompareSnapshot.Size = new System.Drawing.Size(549, 247);
            this.tabCompareSnapshot.TabIndex = 2;
            this.tabCompareSnapshot.Text = "Compare snapshots";
            this.tabCompareSnapshot.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.progressBar1);
            this.groupBox2.Controls.Add(this.btnStartCompare);
            this.groupBox2.Controls.Add(this.btnBrowseFSSnapshotTwo);
            this.groupBox2.Controls.Add(this.txtSnapshotFile2);
            this.groupBox2.Controls.Add(this.txtSnapshotFile1);
            this.groupBox2.Controls.Add(this.btnBrowseFSSnapshotOne);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(537, 233);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Compare snapshot";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "File 2:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "File 1:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 103);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(438, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // btnStartCompare
            // 
            this.btnStartCompare.Location = new System.Drawing.Point(450, 103);
            this.btnStartCompare.Name = "btnStartCompare";
            this.btnStartCompare.Size = new System.Drawing.Size(75, 23);
            this.btnStartCompare.TabIndex = 5;
            this.btnStartCompare.Text = "Start";
            this.btnStartCompare.UseVisualStyleBackColor = true;
            // 
            // btnBrowseFSSnapshotTwo
            // 
            this.btnBrowseFSSnapshotTwo.Location = new System.Drawing.Point(450, 58);
            this.btnBrowseFSSnapshotTwo.Name = "btnBrowseFSSnapshotTwo";
            this.btnBrowseFSSnapshotTwo.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseFSSnapshotTwo.TabIndex = 4;
            this.btnBrowseFSSnapshotTwo.Text = "Browse";
            this.btnBrowseFSSnapshotTwo.UseVisualStyleBackColor = true;
            this.btnBrowseFSSnapshotTwo.Click += new System.EventHandler(this.btnBrowseFSSnapshotTwo_Click);
            // 
            // txtSnapshotFile2
            // 
            this.txtSnapshotFile2.Location = new System.Drawing.Point(47, 58);
            this.txtSnapshotFile2.Name = "txtSnapshotFile2";
            this.txtSnapshotFile2.Size = new System.Drawing.Size(397, 20);
            this.txtSnapshotFile2.TabIndex = 3;
            // 
            // txtSnapshotFile1
            // 
            this.txtSnapshotFile1.Location = new System.Drawing.Point(47, 32);
            this.txtSnapshotFile1.Name = "txtSnapshotFile1";
            this.txtSnapshotFile1.Size = new System.Drawing.Size(397, 20);
            this.txtSnapshotFile1.TabIndex = 1;
            // 
            // btnBrowseFSSnapshotOne
            // 
            this.btnBrowseFSSnapshotOne.Location = new System.Drawing.Point(450, 29);
            this.btnBrowseFSSnapshotOne.Name = "btnBrowseFSSnapshotOne";
            this.btnBrowseFSSnapshotOne.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseFSSnapshotOne.TabIndex = 0;
            this.btnBrowseFSSnapshotOne.Text = "Browse";
            this.btnBrowseFSSnapshotOne.UseVisualStyleBackColor = true;
            this.btnBrowseFSSnapshotOne.Click += new System.EventHandler(this.btnBrowseFSSnapshotOne_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.groupBox1);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(549, 247);
            this.tabSettings.TabIndex = 3;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkHashingAlgorithm);
            this.groupBox1.Controls.Add(this.checkBox4);
            this.groupBox1.Controls.Add(this.checkBox3);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.comboHashingAlgorithm);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(533, 233);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File change properties";
            // 
            // chkHashingAlgorithm
            // 
            this.chkHashingAlgorithm.AutoSize = true;
            this.chkHashingAlgorithm.Location = new System.Drawing.Point(6, 111);
            this.chkHashingAlgorithm.Name = "chkHashingAlgorithm";
            this.chkHashingAlgorithm.Size = new System.Drawing.Size(133, 17);
            this.chkHashingAlgorithm.TabIndex = 12;
            this.chkHashingAlgorithm.Text = "Use hashing algorithm:";
            this.chkHashingAlgorithm.UseVisualStyleBackColor = true;
            this.chkHashingAlgorithm.CheckedChanged += new System.EventHandler(this.chkHashingAlgorithm_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(6, 88);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(82, 17);
            this.checkBox4.TabIndex = 11;
            this.checkBox4.Text = "Use file size";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(6, 65);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(107, 17);
            this.checkBox3.TabIndex = 10;
            this.checkBox3.Text = "Use file attributes";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(6, 42);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(156, 17);
            this.checkBox2.TabIndex = 9;
            this.checkBox2.Text = "Use last modified timestamp";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(151, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Use last access timestamp";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboHashingAlgorithm
            // 
            this.comboHashingAlgorithm.FormattingEnabled = true;
            this.comboHashingAlgorithm.Items.AddRange(new object[] {
            "MD4",
            "MD5",
            "SHA1"});
            this.comboHashingAlgorithm.Location = new System.Drawing.Point(142, 109);
            this.comboHashingAlgorithm.Name = "comboHashingAlgorithm";
            this.comboHashingAlgorithm.Size = new System.Drawing.Size(121, 21);
            this.comboHashingAlgorithm.TabIndex = 7;
            // 
            // snapshotLayout1
            // 
            this.snapshotLayout1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.snapshotLayout1.Location = new System.Drawing.Point(3, 3);
            this.snapshotLayout1.Name = "snapshotLayout1";
            this.snapshotLayout1.Size = new System.Drawing.Size(543, 241);
            this.snapshotLayout1.TabIndex = 0;
            // 
            // snapshotLayout2
            // 
            this.snapshotLayout2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.snapshotLayout2.Location = new System.Drawing.Point(3, 3);
            this.snapshotLayout2.Name = "snapshotLayout2";
            this.snapshotLayout2.Size = new System.Drawing.Size(543, 241);
            this.snapshotLayout2.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 273);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "Change Detector";
            this.tabControl1.ResumeLayout(false);
            this.tabFilesystemSnapshot.ResumeLayout(false);
            this.tabRegistrySnapshot.ResumeLayout(false);
            this.tabCompareSnapshot.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabFilesystemSnapshot;
        private System.Windows.Forms.TabPage tabRegistrySnapshot;
        private SnapshotLayout snapshotLayout1;
        private SnapshotLayout snapshotLayout2;
        private System.Windows.Forms.TabPage tabCompareSnapshot;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnStartCompare;
        private System.Windows.Forms.Button btnBrowseFSSnapshotTwo;
        private System.Windows.Forms.TextBox txtSnapshotFile2;
        private System.Windows.Forms.TextBox txtSnapshotFile1;
        private System.Windows.Forms.Button btnBrowseFSSnapshotOne;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkHashingAlgorithm;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboHashingAlgorithm;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

