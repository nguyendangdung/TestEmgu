namespace Test
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
			this.imageBox1 = new Emgu.CV.UI.ImageBox();
			this.outLabel = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.inLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// imageBox1
			// 
			this.imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imageBox1.Location = new System.Drawing.Point(0, 0);
			this.imageBox1.Name = "imageBox1";
			this.imageBox1.Size = new System.Drawing.Size(1262, 622);
			this.imageBox1.TabIndex = 2;
			this.imageBox1.TabStop = false;
			// 
			// outLabel
			// 
			this.outLabel.AutoSize = true;
			this.outLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.outLabel.Location = new System.Drawing.Point(12, 9);
			this.outLabel.Name = "outLabel";
			this.outLabel.Size = new System.Drawing.Size(132, 46);
			this.outLabel.TabIndex = 3;
			this.outLabel.Text = "label1";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(20, 58);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// inLabel
			// 
			this.inLabel.AutoSize = true;
			this.inLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.inLabel.Location = new System.Drawing.Point(1118, 9);
			this.inLabel.Name = "inLabel";
			this.inLabel.Size = new System.Drawing.Size(132, 46);
			this.inLabel.TabIndex = 5;
			this.inLabel.Text = "label2";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1262, 622);
			this.Controls.Add(this.inLabel);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.outLabel);
			this.Controls.Add(this.imageBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Emgu.CV.UI.ImageBox imageBox1;
		private System.Windows.Forms.Label outLabel;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label inLabel;
	}
}

