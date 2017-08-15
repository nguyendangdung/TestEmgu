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
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

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
			DetectBlobsAndUpdateGui();
		}

		private void DetectBlobsAndUpdateGui()
		{
			var imgFrame1 = capVideo.QueryFrame();
			var imgFrame2 = capVideo.QueryFrame();

			while (!blnFormClosing)
			{
				var frame1Copy = imgFrame1.Clone();
				var frame2Copy = imgFrame2.Clone();

				
				

				CvInvoke.CvtColor(frame1Copy, frame1Copy, ColorConversion.Bgr2Gray);
				CvInvoke.CvtColor(frame2Copy, frame2Copy, ColorConversion.Bgr2Gray);

				CvInvoke.GaussianBlur(frame1Copy, frame1Copy, new Size(5, 5), 0);
				CvInvoke.GaussianBlur(frame2Copy, frame2Copy, new Size(5, 5), 0);

				var imgDifference = new Mat(imgFrame1.Size, DepthType.Cv8U, 1);
				CvInvoke.AbsDiff(frame1Copy, frame2Copy, imgDifference);
				var imgThresh = new Mat(imgFrame1.Size, DepthType.Cv8U, 1);
				CvInvoke.Threshold(imgDifference, imgThresh, 30, 255.0, ThresholdType.Binary);

				Dosomething1(imgThresh);
				DoSomething2(imgThresh);

				imgFrame1 = imgFrame2.Clone();

				if (capVideo.GetCaptureProperty(CapProp.PosFrames) + 1 < capVideo.GetCaptureProperty(CapProp.FrameCount))
				{
					imgFrame2 = capVideo.QueryFrame();
				}
				else
				{
					break;
				}
				Application.DoEvents();
			}
		}


		private void Dosomething1(Mat imgThresh)
		{
			CvInvoke.Imshow("imgThresh", imgThresh);
		}

		private void DoSomething2(Mat imgThresh)
		{
			var structuringElement3x3 =
				CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));

			var structuringElement5x5 =
				CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));

			var structuringElement7x7 =
				CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(7, 7), new Point(-1, -1));

			var structuringElement9x9 =
				CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(9, 9), new Point(-1, -1));


			CvInvoke.Dilate(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default,
				new MCvScalar(0, 0, 0));

			CvInvoke.Dilate(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default,
				new MCvScalar(0, 0, 0));

			CvInvoke.Erode(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default,
				new MCvScalar(0, 0, 0));


			var imgThreshCopy = imgThresh.Clone();
			var contours = new VectorOfVectorOfPoint();
			CvInvoke.FindContours(imgThreshCopy, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
			var imgContours = new Mat(imgThresh.Size, DepthType.Cv8U, 3);
			CvInvoke.DrawContours(imgContours, contours, -1, SCALAR_WHITE, -1);
			CvInvoke.Imshow("imgContours", imgContours);
		}
	}
}
