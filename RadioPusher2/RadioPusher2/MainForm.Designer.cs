namespace RadioPusher2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewTracks = new System.Windows.Forms.DataGridView();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonLookupCompilation = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.buttonLookupArtist = new System.Windows.Forms.Button();
            this.buttonAddTracks = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trackBarVolume = new System.Windows.Forms.TrackBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStripArtistSearch = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripTextBoxQuery = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonStop = new System.Windows.Forms.Button();
            this.textboxDebug = new Utils.NRichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTracks)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStripArtistSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 98);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(993, 372);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Controls.Add(this.buttonUpload);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.buttonLookupCompilation);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.buttonLookupArtist);
            this.tabPage1.Controls.Add(this.buttonAddTracks);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(985, 346);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tracks";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(6, 35);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewTracks);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textboxDebug);
            this.splitContainer1.Size = new System.Drawing.Size(973, 276);
            this.splitContainer1.SplitterDistance = 194;
            this.splitContainer1.TabIndex = 5;
            // 
            // dataGridViewTracks
            // 
            this.dataGridViewTracks.AllowDrop = true;
            this.dataGridViewTracks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTracks.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTracks.Name = "dataGridViewTracks";
            this.dataGridViewTracks.Size = new System.Drawing.Size(973, 194);
            this.dataGridViewTracks.TabIndex = 0;
            this.dataGridViewTracks.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridViewTracks_DragDrop);
            this.dataGridViewTracks.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridViewTracks_DragEnter);
            // 
            // buttonUpload
            // 
            this.buttonUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpload.Location = new System.Drawing.Point(872, 317);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(75, 23);
            this.buttonUpload.TabIndex = 4;
            this.buttonUpload.Text = "Upload!";
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(474, 317);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Lookup Pouet Links";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(338, 317);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Lookup Youtube Links";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // buttonLookupCompilation
            // 
            this.buttonLookupCompilation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLookupCompilation.Enabled = false;
            this.buttonLookupCompilation.Location = new System.Drawing.Point(99, 317);
            this.buttonLookupCompilation.Name = "buttonLookupCompilation";
            this.buttonLookupCompilation.Size = new System.Drawing.Size(116, 23);
            this.buttonLookupCompilation.TabIndex = 3;
            this.buttonLookupCompilation.Text = "Lookup Compilations";
            this.buttonLookupCompilation.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(221, 317);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Lookup MixSongs";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // buttonLookupArtist
            // 
            this.buttonLookupArtist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLookupArtist.Enabled = false;
            this.buttonLookupArtist.Location = new System.Drawing.Point(6, 317);
            this.buttonLookupArtist.Name = "buttonLookupArtist";
            this.buttonLookupArtist.Size = new System.Drawing.Size(87, 23);
            this.buttonLookupArtist.TabIndex = 3;
            this.buttonLookupArtist.Text = "Lookup Artists";
            this.buttonLookupArtist.UseVisualStyleBackColor = true;
            this.buttonLookupArtist.Click += new System.EventHandler(this.buttonLookupArtist_Click);
            // 
            // buttonAddTracks
            // 
            this.buttonAddTracks.Location = new System.Drawing.Point(6, 6);
            this.buttonAddTracks.Name = "buttonAddTracks";
            this.buttonAddTracks.Size = new System.Drawing.Size(140, 23);
            this.buttonAddTracks.TabIndex = 1;
            this.buttonAddTracks.Text = "Add Tracks...";
            this.buttonAddTracks.UseVisualStyleBackColor = true;
            this.buttonAddTracks.Click += new System.EventHandler(this.buttonAddTracks_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 481);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1068, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip1_ItemClicked);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(63, 17);
            this.toolStripStatusLabel1.Text = "Progress: -";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1068, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.settingsToolStripMenuItem.Text = "Settings..";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // trackBarVolume
            // 
            this.trackBarVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarVolume.Location = new System.Drawing.Point(1011, 27);
            this.trackBarVolume.Name = "trackBarVolume";
            this.trackBarVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarVolume.Size = new System.Drawing.Size(45, 96);
            this.trackBarVolume.TabIndex = 3;
            this.trackBarVolume.ValueChanged += new System.EventHandler(this.trackBarVolume_ValueChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(809, 65);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // contextMenuStripArtistSearch
            // 
            this.contextMenuStripArtistSearch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBoxQuery,
            this.toolStripSeparator1});
            this.contextMenuStripArtistSearch.Name = "contextMenuStripArtistSearch";
            this.contextMenuStripArtistSearch.Size = new System.Drawing.Size(161, 35);
            this.contextMenuStripArtistSearch.Opened += new System.EventHandler(this.contextMenuStripArtistSearch_Opened);
            // 
            // toolStripTextBoxQuery
            // 
            this.toolStripTextBoxQuery.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.toolStripTextBoxQuery.Name = "toolStripTextBoxQuery";
            this.toolStripTextBoxQuery.Size = new System.Drawing.Size(100, 23);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // buttonStop
            // 
            this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStop.Location = new System.Drawing.Point(926, 65);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 27);
            this.buttonStop.TabIndex = 5;
            this.buttonStop.Text = "Stop Music";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // textboxDebug
            // 
            this.textboxDebug.AutoScroll1 = false;
            this.textboxDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textboxDebug.Location = new System.Drawing.Point(0, 0);
            this.textboxDebug.Name = "textboxDebug";
            this.textboxDebug.ReadOnly = true;
            this.textboxDebug.Size = new System.Drawing.Size(973, 78);
            this.textboxDebug.TabIndex = 0;
            this.textboxDebug.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1068, 503);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.trackBarVolume);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Demovibes Uploadtool";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTracks)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStripArtistSearch.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.Button buttonAddTracks;
        private System.Windows.Forms.DataGridView dataGridViewTracks;
        private System.Windows.Forms.TrackBar trackBarVolume;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonLookupCompilation;
        private System.Windows.Forms.Button buttonLookupArtist;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripArtistSearch;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxQuery;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Utils.NRichTextBox textboxDebug;
    }
}

