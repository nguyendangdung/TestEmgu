namespace OpenCV_emgu
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
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStartRecording = new System.Windows.Forms.Button();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartPlaying = new System.Windows.Forms.Button();
            this.btnStopRecording = new System.Windows.Forms.Button();
            this.videoSourcePlayer = new Emgu.CV.UI.ImageBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ckUseOpenCL = new System.Windows.Forms.CheckBox();
            this.chkUseGLView = new System.Windows.Forms.CheckBox();
            this.chkShowImage = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.videoSourcePlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(332, 55);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 0;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStartRecording
            // 
            this.btnStartRecording.Location = new System.Drawing.Point(113, 55);
            this.btnStartRecording.Name = "btnStartRecording";
            this.btnStartRecording.Size = new System.Drawing.Size(99, 23);
            this.btnStartRecording.TabIndex = 1;
            this.btnStartRecording.Text = "Start recording";
            this.btnStartRecording.UseVisualStyleBackColor = true;
            this.btnStartRecording.Click += new System.EventHandler(this.btnStartRecording_Click);
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(113, 29);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(575, 20);
            this.txtSource.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Source Url/File";
            // 
            // btnStartPlaying
            // 
            this.btnStartPlaying.Location = new System.Drawing.Point(218, 55);
            this.btnStartPlaying.Name = "btnStartPlaying";
            this.btnStartPlaying.Size = new System.Drawing.Size(99, 23);
            this.btnStartPlaying.TabIndex = 4;
            this.btnStartPlaying.Text = "Start playing";
            this.btnStartPlaying.UseVisualStyleBackColor = true;
            this.btnStartPlaying.Click += new System.EventHandler(this.btnStartPlaying_Click);
            // 
            // btnStopRecording
            // 
            this.btnStopRecording.Location = new System.Drawing.Point(113, 84);
            this.btnStopRecording.Name = "btnStopRecording";
            this.btnStopRecording.Size = new System.Drawing.Size(99, 23);
            this.btnStopRecording.TabIndex = 6;
            this.btnStopRecording.Text = "Stop recording";
            this.btnStopRecording.UseVisualStyleBackColor = true;
            this.btnStopRecording.Click += new System.EventHandler(this.btnStopRecording_Click);
            // 
            // videoSourcePlayer
            // 
            this.videoSourcePlayer.Location = new System.Drawing.Point(-1, 6);
            this.videoSourcePlayer.Name = "videoSourcePlayer";
            this.videoSourcePlayer.Size = new System.Drawing.Size(797, 484);
            this.videoSourcePlayer.TabIndex = 2;
            this.videoSourcePlayer.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(110, 234);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(575, 153);
            this.textBox1.TabIndex = 7;
            // 
            // ckUseOpenCL
            // 
            this.ckUseOpenCL.AutoSize = true;
            this.ckUseOpenCL.Location = new System.Drawing.Point(547, 60);
            this.ckUseOpenCL.Name = "ckUseOpenCL";
            this.ckUseOpenCL.Size = new System.Drawing.Size(87, 17);
            this.ckUseOpenCL.TabIndex = 8;
            this.ckUseOpenCL.Text = "Use OpenCL";
            this.ckUseOpenCL.UseVisualStyleBackColor = true;
            this.ckUseOpenCL.CheckedChanged += new System.EventHandler(this.ckUseOpenCL_CheckedChanged);
            // 
            // chkUseGLView
            // 
            this.chkUseGLView.AutoSize = true;
            this.chkUseGLView.Location = new System.Drawing.Point(429, 59);
            this.chkUseGLView.Name = "chkUseGLView";
            this.chkUseGLView.Size = new System.Drawing.Size(85, 17);
            this.chkUseGLView.TabIndex = 9;
            this.chkUseGLView.Text = "Use GLView";
            this.chkUseGLView.UseVisualStyleBackColor = true;
            // 
            // chkShowImage
            // 
            this.chkShowImage.AutoSize = true;
            this.chkShowImage.Location = new System.Drawing.Point(429, 82);
            this.chkShowImage.Name = "chkShowImage";
            this.chkShowImage.Size = new System.Drawing.Size(84, 17);
            this.chkShowImage.TabIndex = 10;
            this.chkShowImage.Text = "Show image";
            this.chkShowImage.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 488);
            this.Controls.Add(this.chkShowImage);
            this.Controls.Add(this.chkUseGLView);
            this.Controls.Add(this.ckUseOpenCL);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnStopRecording);
            this.Controls.Add(this.btnStartPlaying);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.btnStartRecording);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.videoSourcePlayer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.videoSourcePlayer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStartRecording;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStartPlaying;
        private System.Windows.Forms.Button btnStopRecording;
        private Emgu.CV.UI.ImageBox videoSourcePlayer;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox ckUseOpenCL;
        private System.Windows.Forms.CheckBox chkUseGLView;
        private System.Windows.Forms.CheckBox chkShowImage;
    }
}

