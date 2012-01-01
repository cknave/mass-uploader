namespace RadioPusher2
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxU = new System.Windows.Forms.TextBox();
            this.textBoxP = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboboxServer = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Password:";
            // 
            // textBoxU
            // 
            this.textBoxU.Location = new System.Drawing.Point(76, 6);
            this.textBoxU.Name = "textBoxU";
            this.textBoxU.Size = new System.Drawing.Size(226, 20);
            this.textBoxU.TabIndex = 1;
            // 
            // textBoxP
            // 
            this.textBoxP.Location = new System.Drawing.Point(76, 30);
            this.textBoxP.Name = "textBoxP";
            this.textBoxP.PasswordChar = '*';
            this.textBoxP.Size = new System.Drawing.Size(226, 20);
            this.textBoxP.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(244, 82);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(58, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Server:";
            // 
            // comboboxServer
            // 
            this.comboboxServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboboxServer.FormattingEnabled = true;
            this.comboboxServer.Items.AddRange(new object[] {
            "http://www.bigbeatradio.com/demovibes/",
            "http://www.cvgm.net/demovibes/",
            "http://www.scenemusic.net/demovibes/"});
            this.comboboxServer.Location = new System.Drawing.Point(76, 54);
            this.comboboxServer.Name = "comboboxServer";
            this.comboboxServer.Size = new System.Drawing.Size(226, 21);
            this.comboboxServer.TabIndex = 5;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 111);
            this.Controls.Add(this.comboboxServer);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxU);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxU;
        private System.Windows.Forms.TextBox textBoxP;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboboxServer;

    }
}