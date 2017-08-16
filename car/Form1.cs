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
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace car
{
	public partial class Form1 : Form
	{
		private MCvScalar SCALAR_BLACK = new MCvScalar(0.0, 0.0, 0.0);

		private MCvScalar SCALAR_WHITE = new MCvScalar(255.0, 255.0, 255.0);

		private MCvScalar SCALAR_BLUE = new MCvScalar(255.0, 0.0, 0.0);

		private MCvScalar SCALAR_GREEN = new MCvScalar(0.0, 200.0, 0.0);

		private MCvScalar SCALAR_RED = new MCvScalar(0.0, 0.0, 255.0);


		private VideoCapture capVideo;
		private Mat imgFrame1;
		private Mat imgFrame2;
		private List<Blob> blobs;

		private bool blnFormClosing;
		private bool blnFirstFrame = true;

		private Point[] crossingLine = new Point[2];
		private int horizontalLinePosition;


		private int carCount = 0;

		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var currentFrameBlobs = new List<Blob>();

			Mat imgFrame1Copy = imgFrame1.Clone();
			Mat imgFrame2Copy = imgFrame2.Clone();

			Mat imgDifference = new Mat(imgFrame1.Size, DepthType.Cv8U, 1);
			Mat imgThresh = new Mat(imgFrame1.Size, DepthType.Cv8U, 1);

			CvInvoke.CvtColor(imgFrame1Copy, imgFrame1Copy, ColorConversion.Bgr2Gray);
			CvInvoke.CvtColor(imgFrame2Copy, imgFrame2Copy, ColorConversion.Bgr2Gray);

			CvInvoke.GaussianBlur(imgFrame1Copy, imgFrame1Copy, new Size(5, 5), 0);
			CvInvoke.GaussianBlur(imgFrame2Copy, imgFrame2Copy, new Size(5, 5), 0);

			CvInvoke.AbsDiff(imgFrame1Copy, imgFrame2Copy, imgDifference);

			CvInvoke.Threshold(imgDifference, imgThresh, 30, 255.0, ThresholdType.Binary);


			Mat structuringElement5x5 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));

			for (int i = 0; i <= 1; i++)
			{
				CvInvoke.Dilate(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
				CvInvoke.Dilate(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
				CvInvoke.Erode(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
			}

			Mat imgThreshCopy = imgThresh.Clone();

			VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

			CvInvoke.FindContours(imgThreshCopy, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);


			VectorOfVectorOfPoint convexHulls = new VectorOfVectorOfPoint(contours.Size);

			for (int i = 0; i <= contours.Size - 1; i++)
			{
				CvInvoke.ConvexHull(contours[i], convexHulls[i]);
			}



			for (int i = 0; i <= contours.Size - 1; i++)
			{
				Blob possibleBlob = new Blob(convexHulls[i]);

				if ((possibleBlob.intCurrentRectArea > 400 & possibleBlob.dblCurrentAspectRatio > 0.2 & possibleBlob.dblCurrentAspectRatio < 4.0 & possibleBlob.currentBoundingRect.Width > 30 & possibleBlob.currentBoundingRect.Height > 30 & possibleBlob.dblCurrentDiagonalSize > 60.0 & (CvInvoke.ContourArea(possibleBlob.currentContour) / possibleBlob.intCurrentRectArea) > 0.5))
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

			var atLeastOneBlobCrossedTheLine = checkIfBlobsCrossedTheLine(blobs, horizontalLinePosition, carCount);

			if ((atLeastOneBlobCrossedTheLine))
			{
				CvInvoke.Line(imgFrame2Copy, crossingLine[0], crossingLine[1], SCALAR_GREEN, 2);
			}
			else
			{
				CvInvoke.Line(imgFrame2Copy, crossingLine[0], crossingLine[1], SCALAR_RED, 2);
			}

			drawCarCountOnImage(carCount, imgFrame2Copy);

			imageBox1.Image = imgFrame2Copy;



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
				return; // TODO: might not be correct. Was : Exit While
				//and jump out of while loop
			}

			blnFirstFrame = false;

			Application.DoEvents();

		}



		public void matchCurrentFrameBlobsToExistingBlobs(List<Blob> existingBlobs, List<Blob> currentFrameBlobs)
		{
			foreach (Blob existingBlob in existingBlobs)
			{
				existingBlob.blnCurrentMatchFoundOrNewBlob = false;
				existingBlob.predictNextPosition();
			}


			foreach (Blob currentFrameBlob in currentFrameBlobs)
			{
				int intIndexOfLeastDistance = 0;
				double dblLeastDistance = 1000000.0;


				for (int i = 0; i <= existingBlobs.Count() - 1; i++)
				{

					if ((existingBlobs[i].blnStillBeingTracked == true))
					{
						double dblDistance = distanceBetweenPoints(currentFrameBlob.centerPositions.Last(), existingBlobs[i].predictedNextPosition);

						if ((dblDistance < dblLeastDistance))
						{
							dblLeastDistance = dblDistance;
							intIndexOfLeastDistance = i;
						}

					}

				}

				if ((dblLeastDistance < currentFrameBlob.dblCurrentDiagonalSize * 0.5))
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
				if ((existingBlob.blnCurrentMatchFoundOrNewBlob == false))
				{
					existingBlob.intNumOfConsecutiveFramesWithoutAMatch = existingBlob.intNumOfConsecutiveFramesWithoutAMatch + 1;
				}

				if ((existingBlob.intNumOfConsecutiveFramesWithoutAMatch >= 5))
				{
					existingBlob.blnStillBeingTracked = false;
				}

			}

		}
		private void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			blnFormClosing = true;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			capVideo = new VideoCapture(
				@"C:\Users\Administrator\Documents\Visual Studio 2015\Projects\TestEmgu\ImageSubtraction\CarsDrivingUnderBridge.mp4");
			if (capVideo.GetCaptureProperty(CapProp.FrameCount) < 2)
			{
				MessageBox.Show("error");
			}
			imgFrame1 = capVideo.QueryFrame();
			imgFrame2 = capVideo.QueryFrame();
			blobs = new List<Blob>();
			horizontalLinePosition = (int)Math.Round((imgFrame1.Rows) * 0.35);
			crossingLine[0].X = 0;
			crossingLine[0].Y = horizontalLinePosition;
			crossingLine[1].X = imgFrame1.Cols - 1;
			crossingLine[1].Y = horizontalLinePosition;
			blnFirstFrame = true;
		}


		public double distanceBetweenPoints(Point point1, Point point2)
		{

			int intX = Math.Abs(point1.X - point2.X);
			int intY = Math.Abs(point1.Y - point2.Y);

			return Math.Sqrt((Math.Pow(intX, 2)) + (Math.Pow(intY, 2)));

		}



		public void addBlobToExistingBlobs(Blob currentFrameBlob, List<Blob> existingBlobs, int intIndex)
		{
			existingBlobs[intIndex].currentContour = currentFrameBlob.currentContour;
			existingBlobs[intIndex].currentBoundingRect = currentFrameBlob.currentBoundingRect;

			existingBlobs[intIndex].centerPositions.Add(currentFrameBlob.centerPositions.Last());

			existingBlobs[intIndex].dblCurrentDiagonalSize = currentFrameBlob.dblCurrentDiagonalSize;
			existingBlobs[intIndex].dblCurrentAspectRatio = currentFrameBlob.dblCurrentAspectRatio;

			existingBlobs[intIndex].blnStillBeingTracked = true;
			existingBlobs[intIndex].blnCurrentMatchFoundOrNewBlob = true;

		}



		public void addNewBlob(Blob currentFrameBlob, List<Blob> existingBlobs)
		{
			currentFrameBlob.blnCurrentMatchFoundOrNewBlob = true;

			existingBlobs.Add(currentFrameBlob);

		}



		public void drawBlobInfoOnImage(List<Blob> blobs, Mat imgFrame2Copy)
		{

			for (int i = 0; i <= blobs.Count - 1; i++)
			{

				if ((blobs[i].blnStillBeingTracked == true))
				{
					CvInvoke.Rectangle(imgFrame2Copy, blobs[i].currentBoundingRect, SCALAR_RED, 2);

					FontFace fontFace = FontFace.HersheySimplex;
					double dblFontScale = blobs[i].dblCurrentDiagonalSize / 60.0;
					int intFontThickness = Convert.ToInt32(Math.Round(dblFontScale * 1.0));

					CvInvoke.PutText(imgFrame2Copy, i.ToString(), blobs[i].centerPositions.Last(), fontFace, dblFontScale, SCALAR_GREEN, intFontThickness);

				}

			}

		}


		public bool checkIfBlobsCrossedTheLine(List<Blob> blobs, int horizontalLinePosition, int carCount)
		{

			bool atLeastOneBlobCrossedTheLine = false;
			//this will be the return value


			foreach (Blob blob in blobs)
			{

				if ((blob.blnStillBeingTracked == true & blob.centerPositions.Count() >= 2))
				{
					int prevFrameIndex = blob.centerPositions.Count() - 2;
					int currFrameIndex = blob.centerPositions.Count() - 1;

					if ((blob.centerPositions[prevFrameIndex].Y > horizontalLinePosition & blob.centerPositions[currFrameIndex].Y <= horizontalLinePosition))
					{
						carCount = carCount + 1;
						atLeastOneBlobCrossedTheLine = true;
					}

				}

			}

			return (atLeastOneBlobCrossedTheLine);

		}



		public void drawCarCountOnImage(int carCount, Mat imgFrame2Copy)
		{
			FontFace fontFace = FontFace.HersheySimplex;
			double dblFontScale = Convert.ToDouble(imgFrame2Copy.Rows * imgFrame2Copy.Cols) / 300000.0;
			int intFontThickness = Convert.ToInt32(Math.Round(dblFontScale * 1.5));

			Size textSize = getTextSize(carCount.ToString(), (int)fontFace, dblFontScale, intFontThickness);

			Point bottomLeftTextPosition = new Point();

			bottomLeftTextPosition.X = imgFrame2Copy.Cols - 1 - Convert.ToInt32(Convert.ToDouble(textSize.Width) * 1.3);
			bottomLeftTextPosition.Y = Convert.ToInt32(Convert.ToDouble(textSize.Height) * 1.3);

			CvInvoke.PutText(imgFrame2Copy, carCount.ToString(), bottomLeftTextPosition, fontFace, dblFontScale, SCALAR_GREEN, intFontThickness);
		}



		public Size getTextSize(string strText, int intFontFace, double dblFontScale, int intFontThickness)
		{

			Size textSize = new Size();
			//this will be the return value

			int intNumChars = strText.Count();

			textSize.Width = 55 * intNumChars;
			textSize.Height = 65;

			return (textSize);

		}
	}
}
