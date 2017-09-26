namespace Media.UI.Management
{
    partial class FormMain
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
            this.txtSourceLink = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSourceName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboSourceType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOutLink = new System.Windows.Forms.TextBox();
            this.txtSourceLink2 = new System.Windows.Forms.TextBox();
            this.chkSaveResult = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtArchiveFolder = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source stream";
            // 
            // txtSourceLink
            // 
            this.txtSourceLink.Location = new System.Drawing.Point(95, 11);
            this.txtSourceLink.Name = "txtSourceLink";
            this.txtSourceLink.Size = new System.Drawing.Size(327, 20);
            this.txtSourceLink.TabIndex = 1;
            this.txtSourceLink.Text = "rtsp://v4.cache5.c.youtube.com/CjYLENy73wIaLQlg0fcbksoOZBMYDSANFEIJbXYtZ29vZ2xlSA" +
    "RSBXdhdGNoYNWajp7Cv7WoUQw=/0/0/0/video.3gp";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Source name";
            // 
            // txtSourceName
            // 
            this.txtSourceName.Location = new System.Drawing.Point(95, 41);
            this.txtSourceName.Name = "txtSourceName";
            this.txtSourceName.Size = new System.Drawing.Size(98, 20);
            this.txtSourceName.TabIndex = 3;
            this.txtSourceName.Text = "LiveView";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(230, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Source type";
            // 
            // cboSourceType
            // 
            this.cboSourceType.FormattingEnabled = true;
            this.cboSourceType.Items.AddRange(new object[] {
            "JPEG",
            "MJPEG",
            "H.264"});
            this.cboSourceType.Location = new System.Drawing.Point(301, 42);
            this.cboSourceType.Name = "cboSourceType";
            this.cboSourceType.Size = new System.Drawing.Size(121, 21);
            this.cboSourceType.TabIndex = 5;
            this.cboSourceType.Text = "H.264";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Server port";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(95, 109);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(98, 20);
            this.txtServerPort.TabIndex = 7;
            this.txtServerPort.Text = "3012";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(347, 157);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 8;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(266, 157);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(233, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Output link";
            // 
            // txtOutLink
            // 
            this.txtOutLink.Location = new System.Drawing.Point(299, 109);
            this.txtOutLink.Name = "txtOutLink";
            this.txtOutLink.Size = new System.Drawing.Size(121, 20);
            this.txtOutLink.TabIndex = 11;
            // 
            // txtSourceLink2
            // 
            this.txtSourceLink2.Location = new System.Drawing.Point(95, 70);
            this.txtSourceLink2.Name = "txtSourceLink2";
            this.txtSourceLink2.Size = new System.Drawing.Size(327, 20);
            this.txtSourceLink2.TabIndex = 12;
            this.txtSourceLink2.Text = "http://extcam-16.se.axis.com/axis-cgi/mjpg/video.cgi?";
            // 
            // chkSaveResult
            // 
            this.chkSaveResult.AutoSize = true;
            this.chkSaveResult.Location = new System.Drawing.Point(95, 136);
            this.chkSaveResult.Name = "chkSaveResult";
            this.chkSaveResult.Size = new System.Drawing.Size(75, 17);
            this.chkSaveResult.TabIndex = 13;
            this.chkSaveResult.Text = "Save data";
            this.chkSaveResult.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(233, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Directory";
            // 
            // txtArchiveFolder
            // 
            this.txtArchiveFolder.Location = new System.Drawing.Point(299, 133);
            this.txtArchiveFolder.Name = "txtArchiveFolder";
            this.txtArchiveFolder.Size = new System.Drawing.Size(121, 20);
            this.txtArchiveFolder.TabIndex = 15;
            this.txtArchiveFolder.Text = "Archive";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 189);
            this.Controls.Add(this.txtArchiveFolder);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkSaveResult);
            this.Controls.Add(this.txtSourceLink2);
            this.Controls.Add(this.txtOutLink);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.txtServerPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboSourceType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSourceName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSourceLink);
            this.Controls.Add(this.label1);
            this.Name = "FormMain";
            this.Text = "Config management";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSourceLink;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSourceName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboSourceType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOutLink;
        private System.Windows.Forms.TextBox txtSourceLink2;
        private System.Windows.Forms.CheckBox chkSaveResult;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtArchiveFolder;
    }
}

