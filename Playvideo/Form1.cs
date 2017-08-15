using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;

namespace Playvideo
{
	public partial class Form1 : Form
	{
		private bool blnFormClosing;
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			blnFormClosing = true;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var capVideo = new VideoCapture(@"C:\Users\Administrator\Documents\Visual Studio 2015\Projects\TestEmgu\TestEmgu\CarsDrivingUnderBridge.mp4");
			while (!blnFormClosing)
			{
				var imgFrame = capVideo.QueryFrame();
				if (imgFrame == null)
				{
					return;
				}
				imageBox1.Image = imgFrame;
				Application.DoEvents();
			}
		}
	}
}
