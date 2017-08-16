﻿using System;
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
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

namespace MultipleObjectTracking
{
	public partial class Form3 : Form
	{

		MCvScalar SCALAR_BLACK = new MCvScalar(0.0, 0.0, 0.0);
		MCvScalar SCALAR_WHITE = new MCvScalar(255.0, 255.0, 255.0);
		MCvScalar SCALAR_BLUE = new MCvScalar(255.0, 0.0, 0.0);
		MCvScalar SCALAR_GREEN = new MCvScalar(0.0, 200.0, 0.0);
		MCvScalar SCALAR_RED = new MCvScalar(0.0, 0.0, 255.0);

		VideoCapture capVideo;
		bool blnFormClosing = false;
		private Mat imgFrame1;
		private Mat imgFrame2;
		private List<Blob> blobs = new List<Blob>();
		private bool blnFirstFrame = true;

		public Form3()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
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

			var structuringElement5x5 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));


			CvInvoke.Dilate(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default,
				new MCvScalar(0, 0, 0));

			CvInvoke.Dilate(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default,
				new MCvScalar(0, 0, 0));

			CvInvoke.Erode(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default,
				new MCvScalar(0, 0, 0));
			var imgThreshCopy = imgThresh.Clone();
			var contours = new VectorOfVectorOfPoint();
			CvInvoke.FindContours(imgThreshCopy, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

			var convexHulls = new VectorOfVectorOfPoint(contours.Size);

			for (int j = 0; j < contours.Size; j++)
			{
				CvInvoke.ConvexHull(contours[j], convexHulls[j]);
			}

			for (int i = 0; i < convexHulls.Size; i++)
			{
				var possibleBlob = new Blob(convexHulls[i]);
				var c = possibleBlob.CurrentRectArea > 100 &&
						possibleBlob.CurrentAspectRatio >= 0.2 &&
						possibleBlob.CurrentAspectRatio <= 1.25 &&
						possibleBlob.CurrentBoundingRect.Width > 20 &&
						possibleBlob.CurrentBoundingRect.Height > 20 &&
						possibleBlob.CurrentDiagonalSize > 30.0 &&
						(CvInvoke.ContourArea(possibleBlob.CurrentContour) / possibleBlob.CurrentRectArea) > 0.40;
				if (c)
				{
					currentFrameBlobs.Add(possibleBlob);
				}
			}

			if (blnFirstFrame)
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

			imgFrame2Copy = imgFrame2.Clone();


			drawBlobInfoOnImage(blobs, imgFrame2Copy);


			imageBox1.Image = imgFrame2Copy;
		}

		private void Form3_Load(object sender, EventArgs e)
		{
			capVideo = new VideoCapture(
				@"C:\Users\Administrator\Documents\Visual Studio 2015\Projects\TestEmgu\ImageSubtraction\768x576.avi");
			if (capVideo.GetCaptureProperty(CapProp.FrameCount) < 2)
			{
				MessageBox.Show("error");
			}

			imgFrame1 = capVideo.QueryFrame();
			imgFrame2 = capVideo.QueryFrame();
			blnFirstFrame = true;

		}



		public void matchCurrentFrameBlobsToExistingBlobs(List<Blob> existingBlobs, List<Blob> currentFrameBlobs)
		{
			foreach (Blob existingBlob in existingBlobs)
			{
				existingBlob.CurrentMatchFoundOrNewBlob = false;
				existingBlob.PredictNextPosition();
			}


			foreach (Blob currentFrameBlob in currentFrameBlobs)
			{
				int intIndexOfLeastDistance = 0;
				double dblLeastDistance = 1000000.0;


				for (int i = 0; i <= existingBlobs.Count() - 1; i++)
				{

					if (existingBlobs[i].StillBeingTracked)
					{
						double dblDistance = distanceBetweenPoints(currentFrameBlob.CenterPositions.Last(), existingBlobs[i].PredictedNextPosition);

						if ((dblDistance < dblLeastDistance))
						{
							dblLeastDistance = dblDistance;
							intIndexOfLeastDistance = i;
						}

					}

				}

				if ((dblLeastDistance < currentFrameBlob.CurrentDiagonalSize * 1.15))
				{
					addBlobToExistingBlobs(currentFrameBlob, existingBlobs, intIndexOfLeastDistance);
				}
				else
				{
					addNewBlob(currentFrameBlob, existingBlobs);
				}

			}


			foreach (Blob existingBlob in existingBlobs)
			{
				if ((existingBlob.CurrentMatchFoundOrNewBlob == false))
				{
					existingBlob.NumOfConsecutiveFramesWithoutAMatch = existingBlob.NumOfConsecutiveFramesWithoutAMatch + 1;
				}

				if ((existingBlob.NumOfConsecutiveFramesWithoutAMatch >= 5))
				{
					existingBlob.StillBeingTracked = false;
				}

			}

		}


		public double distanceBetweenPoints(Point point1, Point point2)
		{
			int intX = Math.Abs(point1.X - point2.X);
			int intY = Math.Abs(point1.Y - point2.Y);
			return Math.Sqrt((Math.Pow(intX, 2)) + (Math.Pow(intY, 2)));
		}



		public void addBlobToExistingBlobs(Blob currentFrameBlob, List<Blob> existingBlobs, int intIndex)
		{
			existingBlobs[intIndex].CurrentContour = currentFrameBlob.CurrentContour;
			existingBlobs[intIndex].CurrentBoundingRect = currentFrameBlob.CurrentBoundingRect;

			existingBlobs[intIndex].CenterPositions.Add(currentFrameBlob.CenterPositions.Last());

			existingBlobs[intIndex].CurrentDiagonalSize = currentFrameBlob.CurrentDiagonalSize;
			existingBlobs[intIndex].CurrentAspectRatio = currentFrameBlob.CurrentAspectRatio;

			existingBlobs[intIndex].StillBeingTracked = true;
			existingBlobs[intIndex].CurrentMatchFoundOrNewBlob = true;
		}



		public void addNewBlob(Blob currentFrameBlob, List<Blob> existingBlobs)
		{
			currentFrameBlob.CurrentMatchFoundOrNewBlob = true;

			existingBlobs.Add(currentFrameBlob);

		}



		public void drawBlobInfoOnImage(List<Blob> blobs, Mat imgFrame2Copy)
		{

			for (int i = 0; i <= blobs.Count - 1; i++)
			{

				if ((blobs[i].StillBeingTracked == true))
				{
					CvInvoke.Rectangle(imgFrame2Copy, blobs[i].CurrentBoundingRect, SCALAR_RED, 2);

					FontFace fontFace = FontFace.HersheySimplex;
					double dblFontScale = blobs[i].CurrentDiagonalSize / 60.0;
					int intFontThickness = Convert.ToInt32(Math.Round(dblFontScale * 1.0));

					CvInvoke.PutText(imgFrame2Copy, i.ToString(), blobs[i].CenterPositions.Last(), fontFace, dblFontScale, SCALAR_GREEN, intFontThickness);

				}

			}

		}
	}
}
