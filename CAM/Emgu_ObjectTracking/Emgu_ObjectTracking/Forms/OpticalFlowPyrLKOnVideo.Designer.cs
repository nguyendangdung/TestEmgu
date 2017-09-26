namespace Emgu_ObjectTracking.Forms
{
    partial class OpticalFlowPyrLKOnVideo
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
            this.label7 = new System.Windows.Forms.Label();
            this.eps = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.maxIteration = new System.Windows.Forms.TrackBar();
            this.subPixWinSize = new System.Windows.Forms.TrackBar();
            this.minDistance = new System.Windows.Forms.TrackBar();
            this.blockSize = new System.Windows.Forms.TrackBar();
            this.qualityLevel = new System.Windows.Forms.TrackBar();
            this.btnStart = new System.Windows.Forms.Button();
            this.ckCornerSubPix = new System.Windows.Forms.CheckBox();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.btnNextFrame = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.eps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxIteration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subPixWinSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blockSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 250);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "eps";
            // 
            // eps
            // 
            this.eps.Location = new System.Drawing.Point(68, 250);
            this.eps.Maximum = 20;
            this.eps.Minimum = 1;
            this.eps.Name = "eps";
            this.eps.Size = new System.Drawing.Size(226, 45);
            this.eps.TabIndex = 25;
            this.eps.Value = 7;
            this.eps.Scroll += new System.EventHandler(this.commonEvent);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 216);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 32;
            this.label5.Text = "maxIteration";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 185);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "subPixWinSize";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "minDistance";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "blockSize";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "qualityLevel";
            // 
            // maxIteration
            // 
            this.maxIteration.Location = new System.Drawing.Point(68, 216);
            this.maxIteration.Maximum = 20;
            this.maxIteration.Minimum = 1;
            this.maxIteration.Name = "maxIteration";
            this.maxIteration.Size = new System.Drawing.Size(226, 45);
            this.maxIteration.TabIndex = 26;
            this.maxIteration.Value = 13;
            this.maxIteration.Scroll += new System.EventHandler(this.commonEvent);
            // 
            // subPixWinSize
            // 
            this.subPixWinSize.Location = new System.Drawing.Point(68, 185);
            this.subPixWinSize.Minimum = 1;
            this.subPixWinSize.Name = "subPixWinSize";
            this.subPixWinSize.Size = new System.Drawing.Size(226, 45);
            this.subPixWinSize.TabIndex = 24;
            this.subPixWinSize.Value = 6;
            this.subPixWinSize.Scroll += new System.EventHandler(this.commonEvent);
            // 
            // minDistance
            // 
            this.minDistance.Location = new System.Drawing.Point(68, 108);
            this.minDistance.Maximum = 20;
            this.minDistance.Minimum = 1;
            this.minDistance.Name = "minDistance";
            this.minDistance.Size = new System.Drawing.Size(226, 45);
            this.minDistance.TabIndex = 23;
            this.minDistance.Value = 3;
            this.minDistance.Scroll += new System.EventHandler(this.commonEvent);
            // 
            // blockSize
            // 
            this.blockSize.Location = new System.Drawing.Point(68, 75);
            this.blockSize.Maximum = 20;
            this.blockSize.Minimum = 1;
            this.blockSize.Name = "blockSize";
            this.blockSize.Size = new System.Drawing.Size(226, 45);
            this.blockSize.TabIndex = 22;
            this.blockSize.Value = 3;
            this.blockSize.Scroll += new System.EventHandler(this.commonEvent);
            // 
            // qualityLevel
            // 
            this.qualityLevel.Location = new System.Drawing.Point(68, 43);
            this.qualityLevel.Maximum = 20;
            this.qualityLevel.Minimum = 1;
            this.qualityLevel.Name = "qualityLevel";
            this.qualityLevel.Size = new System.Drawing.Size(226, 45);
            this.qualityLevel.TabIndex = 21;
            this.qualityLevel.Value = 1;
            this.qualityLevel.Scroll += new System.EventHandler(this.commonEvent);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(121, 320);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 19;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // ckCornerSubPix
            // 
            this.ckCornerSubPix.AutoSize = true;
            this.ckCornerSubPix.Location = new System.Drawing.Point(68, 159);
            this.ckCornerSubPix.Name = "ckCornerSubPix";
            this.ckCornerSubPix.Size = new System.Drawing.Size(90, 17);
            this.ckCornerSubPix.TabIndex = 34;
            this.ckCornerSubPix.Text = "CornerSubPix";
            this.ckCornerSubPix.UseVisualStyleBackColor = true;
            this.ckCornerSubPix.CheckedChanged += new System.EventHandler(this.commonEvent);
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(327, 28);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(539, 489);
            this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // btnNextFrame
            // 
            this.btnNextFrame.Location = new System.Drawing.Point(121, 365);
            this.btnNextFrame.Name = "btnNextFrame";
            this.btnNextFrame.Size = new System.Drawing.Size(75, 23);
            this.btnNextFrame.TabIndex = 35;
            this.btnNextFrame.Text = "Next Frame";
            this.btnNextFrame.UseVisualStyleBackColor = true;
            this.btnNextFrame.Click += new System.EventHandler(this.btnNextFrame_Click);
            // 
            // OpticalFlowPyrLKOnVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 529);
            this.Controls.Add(this.btnNextFrame);
            this.Controls.Add(this.imageBox1);
            this.Controls.Add(this.ckCornerSubPix);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.eps);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.maxIteration);
            this.Controls.Add(this.subPixWinSize);
            this.Controls.Add(this.minDistance);
            this.Controls.Add(this.blockSize);
            this.Controls.Add(this.qualityLevel);
            this.Controls.Add(this.btnStart);
            this.Name = "OpticalFlowPyrLKOnVideo";
            this.Text = "OpticalFlowPyrLKOnVideo";
            this.Load += new System.EventHandler(this.OpticalFlowPyrLKOnVideo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.eps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxIteration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subPixWinSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blockSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar eps;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar maxIteration;
        private System.Windows.Forms.TrackBar subPixWinSize;
        private System.Windows.Forms.TrackBar minDistance;
        private System.Windows.Forms.TrackBar blockSize;
        private System.Windows.Forms.TrackBar qualityLevel;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.CheckBox ckCornerSubPix;
        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.Button btnNextFrame;
    }
}