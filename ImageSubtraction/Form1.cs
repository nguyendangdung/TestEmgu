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
using Emgu.CV.Structure;

namespace ImageSubtraction
{
	public partial class Form1 : Form
	{
		private MCvScalar SCALAR_BLACK = new MCvScalar(0.0, 0.0, 0.0);

		private MCvScalar SCALAR_WHITE = new MCvScalar(255.0, 255.0, 255.0);

		private MCvScalar SCALAR_BLUE = new MCvScalar(255.0, 0.0, 0.0);

		private MCvScalar SCALAR_GREEN = new MCvScalar(0.0, 255.0, 0.0);

		private MCvScalar SCALAR_RED = new MCvScalar(0.0, 0.0, 255.0);


		private VideoCapture capVideo;


		private bool blnFormClosing;


		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			blnFormClosing = true;

			CvInvoke.DestroyAllWindows();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			capVideo = new VideoCapture(@"C:\Users\Administrator\Documents\Visual Studio 2015\Projects\TestEmgu\ImageSubtraction\768x576.avi");
			DetectBlobsAndUpdateGUI();
		}

		private void DetectBlobsAndUpdateGUI()
		{
			Mat imgFrame1;

			Mat imgFrame2;


			var blnFirstFrame = true;


			imgFrame1 = capVideo.QueryFrame();

			imgFrame2 = capVideo.QueryFrame();

			while (!blnFormClosing)
			{
				var blobs = new List<Blob>();
				var frame1Copy = imgFrame1.Clone();
				var frame2Copy = imgFrame2.Clone();

			}
		}
	}
}
