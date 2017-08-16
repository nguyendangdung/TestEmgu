using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace MultipleObjectTracking
{
	public partial class Form1 : Form
	{

		MCvScalar SCALAR_BLACK = new MCvScalar(0.0, 0.0, 0.0);
		MCvScalar SCALAR_WHITE = new MCvScalar(255.0, 255.0, 255.0);
		MCvScalar SCALAR_BLUE = new MCvScalar(255.0, 0.0, 0.0);
		MCvScalar SCALAR_GREEN = new MCvScalar(0.0, 200.0, 0.0);
		MCvScalar SCALAR_RED = new MCvScalar(0.0, 0.0, 255.0);

		VideoCapture capVideo;

		bool blnFormClosing = false;

		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			capVideo = new VideoCapture(@"C:\Users\Administrator\Documents\Visual Studio 2015\Projects\TestEmgu\ImageSubtraction\768x576.avi");
			TrackBlobsAndUpdateGui();
		}



		public void TrackBlobsAndUpdateGui()
		{
			var blobs = new List<Blob>();

			var blnFirstFrame = true;

			var imgFrame1 = capVideo.QueryFrame();
			var imgFrame2 = capVideo.QueryFrame();


			while ((blnFormClosing == false))
			{
				var currentFrameBlobs = new List<Blob>();

				var imgFrame1Copy = imgFrame1.Clone();
				var imgFrame2Copy = imgFrame2.Clone();

				var imgDifference = new Mat(imgFrame1.Size, DepthType.Cv8U, 1);
				var imgThresh = new Mat(imgFrame1.Size, DepthType.Cv8U, 1);

				CvInvoke.CvtColor(imgFrame1Copy, imgFrame1Copy, ColorConversion.Bgr2Gray);
				CvInvoke.CvtColor(imgFrame2Copy, imgFrame2Copy, ColorConversion.Bgr2Gray);

				CvInvoke.GaussianBlur(imgFrame1Copy, imgFrame1Copy, new Size(5, 5), 0);
				CvInvoke.GaussianBlur(imgFrame2Copy, imgFrame2Copy, new Size(5, 5), 0);

				CvInvoke.AbsDiff(imgFrame1Copy, imgFrame2Copy, imgDifference);

				CvInvoke.Threshold(imgDifference, imgThresh, 30, 255.0, ThresholdType.Binary);

				//CvInvoke.Imshow("imgThresh", imgThresh);

				Mat structuringElement3x3 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
				Mat structuringElement5x5 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));
				Mat structuringElement7x7 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(7, 7), new Point(-1, -1));
				Mat structuringElement9x9 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(9, 9), new Point(-1, -1));

				CvInvoke.Dilate(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
				CvInvoke.Dilate(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
				CvInvoke.Erode(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));

				Mat imgThreshCopy = imgThresh.Clone();

				VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

				CvInvoke.FindContours(imgThreshCopy, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

				drawAndShowContours(imgThresh.Size, contours, "imgContours");

				VectorOfVectorOfPoint convexHulls = new VectorOfVectorOfPoint(contours.Size);

				for (int i = 0; i <= contours.Size - 1; i++)
				{
					CvInvoke.ConvexHull(contours[i], convexHulls[i]);
				}

				drawAndShowContours(imgThresh.Size, convexHulls, "imgConvexHulls");


				for (int i = 0; i <= convexHulls.Size - 1; i++)
				{
					Blob possibleBlob = new Blob(convexHulls[i]);

					if ((possibleBlob.CurrentRectArea > 100 & possibleBlob.CurrentAspectRatio >= 0.2 & possibleBlob.CurrentAspectRatio <= 1.25 & possibleBlob.CurrentBoundingRect.Width > 20 & possibleBlob.CurrentBoundingRect.Height > 20 & possibleBlob.CurrentDiagonalSize > 30.0 & (CvInvoke.ContourArea(possibleBlob.CurrentContour) / Convert.ToDouble(possibleBlob.CurrentRectArea)) > 0.4))
					{
						currentFrameBlobs.Add(possibleBlob);
					}

				}

				drawAndShowContours(imgThresh.Size, currentFrameBlobs, "imgCurrentFrameBlobs");

				if ((blnFirstFrame == true))
				{
					foreach (Blob currentFrameBlob in currentFrameBlobs)
					{
						blobs.Add(currentFrameBlob);
					}
				}
				else
				{
					matchCurrentFrameBlobsToExistingBlobs(blobs, currentFrameBlobs);
				}

				drawAndShowContours(imgThresh.Size, blobs, "imgBlobs");

				imgFrame2Copy = imgFrame2.Clone();

				drawBlobInfoOnImage(blobs, imgFrame2Copy);

				imageBox1.Image = imgFrame2Copy;

				//now we prepare for the next iteration

				currentFrameBlobs.Clear();

				imgFrame1 = imgFrame2.Clone();
				//move frame 1 up to where frame 2 is

				//if there is at least one more frame
				if ((capVideo.GetCaptureProperty(CapProp.PosFrames) + 1 < capVideo.GetCaptureProperty(CapProp.FrameCount)))
				{
					imgFrame2 = capVideo.QueryFrame();
					//get the next frame
					//else if there is not at least one more frame
				}
				else
				{
					MessageBox.Show("end of video");
					//show end of video message
					break; // TODO: might not be correct. Was : Exit While
					//and jump out of while loop
				}

				blnFirstFrame = false;

				Application.DoEvents();

			}

		}

		private void matchCurrentFrameBlobsToExistingBlobs(List<Blob> blobs, List<Blob> currentFrameBlobs)
		{
			throw new NotImplementedException();
		}

		private void drawBlobInfoOnImage(List<Blob> blobs, Mat imgFrame2Copy)
		{
			throw new NotImplementedException();
		}

		private void drawAndShowContours(object imgThreshSize, VectorOfVectorOfPoint convexHulls, string imgconvexhulls)
		{
			throw new NotImplementedException();
		}



		public void drawAndShowContours(Size imageSize, List<Blob> blobs, string strImageName)
		{
			Mat image = new Mat(imageSize, DepthType.Cv8U, 3);

			VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

			foreach (Blob blob in blobs)
			{
				if ((blob.StillBeingTracked == true))
				{
					contours.Push(blob.CurrentContour);
				}
			}

			CvInvoke.DrawContours(image, contours, -1, SCALAR_WHITE, -1);

			CvInvoke.Imshow(strImageName, image);

		}


		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			blnFormClosing = true;
			CvInvoke.DestroyAllWindows();
		}
	}
}
