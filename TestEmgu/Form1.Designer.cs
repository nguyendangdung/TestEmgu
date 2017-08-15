namespace TestEmgu
{
    partial class Form1
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
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.txtInfo = new System.Windows.Forms.TextBox();
			this.lblChosenFile = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.imageBox = new Emgu.CV.UI.ImageBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.txtInfo);
			this.splitContainer1.Panel1.Controls.Add(this.lblChosenFile);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.imageBox);
			this.splitContainer1.Size = new System.Drawing.Size(780, 467);
			this.splitContainer1.SplitterDistance = 34;
			this.splitContainer1.TabIndex = 0;
			// 
			// txtInfo
			// 
			this.txtInfo.Location = new System.Drawing.Point(576, 8);
			this.txtInfo.Name = "txtInfo";
			this.txtInfo.Size = new System.Drawing.Size(100, 20);
			this.txtInfo.TabIndex = 3;
			// 
			// lblChosenFile
			// 
			this.lblChosenFile.AutoSize = true;
			this.lblChosenFile.Location = new System.Drawing.Point(93, 10);
			this.lblChosenFile.Name = "lblChosenFile";
			this.lblChosenFile.Size = new System.Drawing.Size(29, 13);
			this.lblChosenFile.TabIndex = 1;
			this.lblChosenFile.Text = "label";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 5);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Open File";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.BtnOpenFile);
			// 
			// imageBox
			// 
			this.imageBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imageBox.Location = new System.Drawing.Point(0, 0);
			this.imageBox.Name = "imageBox";
			this.imageBox.Size = new System.Drawing.Size(780, 429);
			this.imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.imageBox.TabIndex = 2;
			this.imageBox.TabStop = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(780, 467);
			this.Controls.Add(this.splitContainer1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button1;
        private Emgu.CV.UI.ImageBox imageBox;
		private System.Windows.Forms.Label lblChosenFile;
		private System.Windows.Forms.TextBox txtInfo;
	}
}

