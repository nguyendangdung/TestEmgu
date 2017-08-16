namespace ImageSubtraction
{
	partial class Form2
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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.imageBox1 = new Emgu.CV.UI.ImageBox();
			this.imageBox2 = new Emgu.CV.UI.ImageBox();
			this.imageBox3 = new Emgu.CV.UI.ImageBox();
			this.imageBox4 = new Emgu.CV.UI.ImageBox();
			this.imageBox5 = new Emgu.CV.UI.ImageBox();
			this.imageBox6 = new Emgu.CV.UI.ImageBox();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox6)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(93, 12);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(174, 12);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 2;
			this.button3.Text = "button3";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(255, 12);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 23);
			this.button4.TabIndex = 3;
			this.button4.Text = "button4";
			this.button4.UseVisualStyleBackColor = true;
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(336, 12);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(75, 23);
			this.button5.TabIndex = 4;
			this.button5.Text = "button5";
			this.button5.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.45938F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.54062F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 339F));
			this.tableLayoutPanel1.Controls.Add(this.imageBox6, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.imageBox5, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.imageBox4, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.imageBox3, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.imageBox2, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.imageBox1, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1054, 526);
			this.tableLayoutPanel1.TabIndex = 5;
			// 
			// imageBox1
			// 
			this.imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imageBox1.Location = new System.Drawing.Point(3, 3);
			this.imageBox1.Name = "imageBox1";
			this.imageBox1.Size = new System.Drawing.Size(340, 257);
			this.imageBox1.TabIndex = 2;
			this.imageBox1.TabStop = false;
			// 
			// imageBox2
			// 
			this.imageBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imageBox2.Location = new System.Drawing.Point(349, 3);
			this.imageBox2.Name = "imageBox2";
			this.imageBox2.Size = new System.Drawing.Size(362, 257);
			this.imageBox2.TabIndex = 3;
			this.imageBox2.TabStop = false;
			// 
			// imageBox3
			// 
			this.imageBox3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imageBox3.Location = new System.Drawing.Point(717, 3);
			this.imageBox3.Name = "imageBox3";
			this.imageBox3.Size = new System.Drawing.Size(334, 257);
			this.imageBox3.TabIndex = 4;
			this.imageBox3.TabStop = false;
			// 
			// imageBox4
			// 
			this.imageBox4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imageBox4.Location = new System.Drawing.Point(3, 266);
			this.imageBox4.Name = "imageBox4";
			this.imageBox4.Size = new System.Drawing.Size(340, 257);
			this.imageBox4.TabIndex = 5;
			this.imageBox4.TabStop = false;
			// 
			// imageBox5
			// 
			this.imageBox5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imageBox5.Location = new System.Drawing.Point(349, 266);
			this.imageBox5.Name = "imageBox5";
			this.imageBox5.Size = new System.Drawing.Size(362, 257);
			this.imageBox5.TabIndex = 6;
			this.imageBox5.TabStop = false;
			// 
			// imageBox6
			// 
			this.imageBox6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imageBox6.Location = new System.Drawing.Point(717, 266);
			this.imageBox6.Name = "imageBox6";
			this.imageBox6.Size = new System.Drawing.Size(334, 257);
			this.imageBox6.TabIndex = 7;
			this.imageBox6.TabStop = false;
			// 
			// Form2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1054, 526);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "Form2";
			this.Text = "Form2";
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox6)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private Emgu.CV.UI.ImageBox imageBox6;
		private Emgu.CV.UI.ImageBox imageBox5;
		private Emgu.CV.UI.ImageBox imageBox4;
		private Emgu.CV.UI.ImageBox imageBox3;
		private Emgu.CV.UI.ImageBox imageBox2;
		private Emgu.CV.UI.ImageBox imageBox1;
	}
}