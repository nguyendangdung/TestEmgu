namespace Emgu_ObjectTracking
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
			this.captureImageBox = new Emgu.CV.UI.ImageBox();
			this.grayscaleImageBox = new Emgu.CV.UI.ImageBox();
			this.btnStart = new System.Windows.Forms.Button();
			this.pyrScale = new System.Windows.Forms.TrackBar();
			this.levels = new System.Windows.Forms.TrackBar();
			this.winSize = new System.Windows.Forms.TrackBar();
			this.iterations = new System.Windows.Forms.TrackBar();
			this.polySigma = new System.Windows.Forms.TrackBar();
			this.polyN = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.threadhold = new System.Windows.Forms.TrackBar();
			this.label7 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.captureImageBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.grayscaleImageBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pyrScale)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.levels)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.winSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.iterations)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.polySigma)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.polyN)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.threadhold)).BeginInit();
			this.SuspendLayout();
			// 
			// captureImageBox
			// 
			this.captureImageBox.Location = new System.Drawing.Point(492, 326);
			this.captureImageBox.Name = "captureImageBox";
			this.captureImageBox.Size = new System.Drawing.Size(306, 295);
			this.captureImageBox.TabIndex = 2;
			this.captureImageBox.TabStop = false;
			// 
			// grayscaleImageBox
			// 
			this.grayscaleImageBox.Location = new System.Drawing.Point(492, 25);
			this.grayscaleImageBox.Name = "grayscaleImageBox";
			this.grayscaleImageBox.Size = new System.Drawing.Size(306, 295);
			this.grayscaleImageBox.TabIndex = 3;
			this.grayscaleImageBox.TabStop = false;
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(377, 416);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 4;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// pyrScale
			// 
			this.pyrScale.Location = new System.Drawing.Point(68, 25);
			this.pyrScale.Name = "pyrScale";
			this.pyrScale.Size = new System.Drawing.Size(226, 45);
			this.pyrScale.TabIndex = 5;
			this.pyrScale.Value = 5;
			// 
			// levels
			// 
			this.levels.Location = new System.Drawing.Point(68, 56);
			this.levels.Name = "levels";
			this.levels.Size = new System.Drawing.Size(226, 45);
			this.levels.TabIndex = 6;
			this.levels.Value = 3;
			// 
			// winSize
			// 
			this.winSize.Location = new System.Drawing.Point(68, 88);
			this.winSize.Maximum = 20;
			this.winSize.Name = "winSize";
			this.winSize.Size = new System.Drawing.Size(226, 45);
			this.winSize.TabIndex = 7;
			this.winSize.Value = 15;
			// 
			// iterations
			// 
			this.iterations.Location = new System.Drawing.Point(68, 121);
			this.iterations.Name = "iterations";
			this.iterations.Size = new System.Drawing.Size(226, 45);
			this.iterations.TabIndex = 8;
			this.iterations.Value = 3;
			// 
			// polySigma
			// 
			this.polySigma.Location = new System.Drawing.Point(68, 182);
			this.polySigma.Maximum = 20;
			this.polySigma.Name = "polySigma";
			this.polySigma.Size = new System.Drawing.Size(226, 45);
			this.polySigma.TabIndex = 10;
			this.polySigma.Value = 13;
			// 
			// polyN
			// 
			this.polyN.Location = new System.Drawing.Point(68, 151);
			this.polyN.Name = "polyN";
			this.polyN.Size = new System.Drawing.Size(226, 45);
			this.polyN.TabIndex = 9;
			this.polyN.Value = 6;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "pyrScale";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(34, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "levels";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 119);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(49, 13);
			this.label3.TabIndex = 14;
			this.label3.Text = "iterations";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 88);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(43, 13);
			this.label4.TabIndex = 13;
			this.label4.Text = "winSize";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 182);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(55, 13);
			this.label5.TabIndex = 16;
			this.label5.Text = "polySigma";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 151);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(34, 13);
			this.label6.TabIndex = 15;
			this.label6.Text = "polyN";
			// 
			// threadhold
			// 
			this.threadhold.Location = new System.Drawing.Point(68, 216);
			this.threadhold.Maximum = 20;
			this.threadhold.Name = "threadhold";
			this.threadhold.Size = new System.Drawing.Size(226, 45);
			this.threadhold.TabIndex = 10;
			this.threadhold.Value = 7;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 216);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(57, 13);
			this.label7.TabIndex = 18;
			this.label7.Text = "threadhold";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(826, 637);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.threadhold);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.polySigma);
			this.Controls.Add(this.polyN);
			this.Controls.Add(this.iterations);
			this.Controls.Add(this.winSize);
			this.Controls.Add(this.levels);
			this.Controls.Add(this.pyrScale);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.grayscaleImageBox);
			this.Controls.Add(this.captureImageBox);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.captureImageBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grayscaleImageBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pyrScale)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.levels)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.winSize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.iterations)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.polySigma)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.polyN)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.threadhold)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox captureImageBox;
        private Emgu.CV.UI.ImageBox grayscaleImageBox;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TrackBar pyrScale;
        private System.Windows.Forms.TrackBar levels;
        private System.Windows.Forms.TrackBar winSize;
        private System.Windows.Forms.TrackBar iterations;
        private System.Windows.Forms.TrackBar polySigma;
        private System.Windows.Forms.TrackBar polyN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar threadhold;
        private System.Windows.Forms.Label label7;
    }
}

