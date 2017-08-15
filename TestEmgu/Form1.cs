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
using Emgu.CV.UI;
using Emgu.CV.Util;
using System.Text.RegularExpressions;

namespace TestEmgu
{
    public partial class Form1 : Form
    {
	    private MCvScalar _scalarBlack = new MCvScalar(0.0, 0.0, 0.0);


	    private MCvScalar _scalarWhite = new MCvScalar(255.0, 255.0, 255.0);


	    private MCvScalar _scalarBlue = new MCvScalar(255.0, 0.0, 0.0);


	    private MCvScalar _scalarGreen = new MCvScalar(0.0, 200.0, 0.0);


	    private MCvScalar _scalarRed = new MCvScalar(0.0, 0.0, 255.0);


	    private VideoCapture _capVideo;


	    public bool BlnFormClosing;


		public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
	        BlnFormClosing = true;
	        CvInvoke.DestroyAllWindows();

        }

        private void BtnOpenFile(object sender, EventArgs e)
        {
	        if (openFileDialog.ShowDialog() != DialogResult.OK || openFileDialog.FileName == "")
	        {
		        lblChosenFile.Text = @"file not chosen";
	        }
	        else
	        {
				lblChosenFile.Text = openFileDialog.FileName;
		        try
		        {
			        _capVideo = new VideoCapture(openFileDialog.FileName);

		        }
		        catch (Exception exception)
		        {
			        MessageBox.Show(@"unable to read video file, error: " + exception.Message);
			        return;
		        }

		        if (_capVideo == null)
		        {
			        txtInfo.AppendText("unable to read video file");
		        }
		        else
		        {
			        if (_capVideo.GetCaptureProperty(CapProp.FrameCount) < 2)
			        {
				        txtInfo.AppendText("error: video file must have at least two frames");
			        }
			        else
			        {
				        TrackBlobsAndUpdateGui();
			        }
		        }
			}
        }

	    private void TrackBlobsAndUpdateGui()
	    {
		    throw new NotImplementedException();
	    }



	    public void trackBlobsAndUpdateGUI()
	    {
		    Mat imgFrame1 = default(Mat);
		    Mat imgFrame2 = default(Mat);

		    List<Blob> blobs = new List<Blob>();

		    Point[] crossingLine = new Point[3];

		    int carCount = 0;

		    imgFrame1 = _capVideo.QueryFrame();
		    imgFrame2 = _capVideo.QueryFrame();

		    int horizontalLinePosition = Convert.ToInt32(Math.Round(Convert.ToDouble(imgFrame1.Rows) * 0.35));

		    crossingLine[0].X = 0;
		    crossingLine[0].Y = horizontalLinePosition;

		    crossingLine[1].X = imgFrame1.Cols - 1;
		    crossingLine[1].Y = horizontalLinePosition;

		    bool blnFirstFrame = true;


		    while ((BlnFormClosing == false))
		    {
			    List<Blob> currentFrameBlobs = new List<Blob>();

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

			    CvInvoke.Imshow("imgThresh", imgThresh);

			    Mat structuringElement3x3 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
			    Mat structuringElement5x5 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));
			    Mat structuringElement7x7 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(7, 7), new Point(-1, -1));
			    Mat structuringElement9x9 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(9, 9), new Point(-1, -1));

			    for (int i = 0; i <= 1; i++)
			    {
				    CvInvoke.Dilate(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
				    CvInvoke.Dilate(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
				    CvInvoke.Erode(imgThresh, imgThresh, structuringElement5x5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
			    }

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


			    for (int i = 0; i <= contours.Size - 1; i++)
			    {
				    Blob possibleBlob = new Blob(convexHulls[i]);

				    if ((possibleBlob.IntCurrentRectArea > 400 & possibleBlob.DblCurrentAspectRatio > 0.2 & possibleBlob.DblCurrentAspectRatio < 4.0 & possibleBlob.CurrentBoundingRect.Width > 30 & possibleBlob.CurrentBoundingRect.Height > 30 & possibleBlob.DblCurrentDiagonalSize > 60.0 & (CvInvoke.ContourArea(possibleBlob.CurrentContour) / possibleBlob.IntCurrentRectArea) > 0.5))
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
				    matchCurrentFrameBlobsToExistingBlobs(ref blobs, ref currentFrameBlobs);
			    }

			    drawAndShowContours(imgThresh.Size, blobs, "imgBlobs");

			    imgFrame2Copy = imgFrame2.Clone();

			    drawBlobInfoOnImage(ref blobs, ref imgFrame2Copy);

			    dynamic atLeastOneBlobCrossedTheLine = checkIfBlobsCrossedTheLine(ref blobs, ref horizontalLinePosition, ref carCount);

			    if ((atLeastOneBlobCrossedTheLine))
			    {
				    CvInvoke.Line(imgFrame2Copy, crossingLine[0], crossingLine[1], _scalarGreen, 2);
			    }
			    else
			    {
				    CvInvoke.Line(imgFrame2Copy, crossingLine[0], crossingLine[1], _scalarRed, 2);
			    }

			    drawCarCountOnImage(ref carCount, ref imgFrame2Copy);

			    imageBox.Image = imgFrame2Copy;

			    //now we prepare for the next iteration

			    currentFrameBlobs.Clear();

			    imgFrame1 = imgFrame2.Clone();
			    //move frame 1 up to where frame 2 is

			    //if there is at least one more frame
			    if ((_capVideo.GetCaptureProperty(CapProp.PosFrames) + 1 < _capVideo.GetCaptureProperty(CapProp.FrameCount)))
			    {
				    imgFrame2 = _capVideo.QueryFrame();
				    //get the next frame
				    //else if there is not at least one more frame
			    }
			    else
			    {
				    txtInfo.AppendText("end of video");
				    //show end of video message
				    break; // TODO: might not be correct. Was : Exit While
				    //and jump out of while loop
			    }

			    blnFirstFrame = false;

			    Application.DoEvents();

		    }
	    }

	    public void drawAndShowContours(Size imageSize, VectorOfVectorOfPoint contours, string strImageName)
	    {
		    Mat image = new Mat(imageSize, DepthType.Cv8U, 3);

		    CvInvoke.DrawContours(image, contours, -1, _scalarWhite, -1);

		    CvInvoke.Imshow(strImageName, image);

	    }

		public void drawAndShowContours(Size imageSize, List<Blob> blobs, string strImageName)
	    {
		    Mat image = new Mat(imageSize, DepthType.Cv8U, 3);

		    VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

		    foreach (Blob blob in blobs)
		    {
			    if ((blob.BlnStillBeingTracked == true))
			    {
				    contours.Push(blob.CurrentContour);
			    }
		    }

		    CvInvoke.DrawContours(image, contours, -1, _scalarWhite, -1);

		    CvInvoke.Imshow(strImageName, image);

	    }



	    public void drawBlobInfoOnImage(ref List<Blob> blobs, ref Mat imgFrame2Copy)
	    {

		    for (int i = 0; i <= blobs.Count - 1; i++)
		    {

			    if ((blobs[i].BlnStillBeingTracked == true))
			    {
				    CvInvoke.Rectangle(imgFrame2Copy, blobs[i].CurrentBoundingRect, _scalarRed, 2);

				    FontFace fontFace = FontFace.HersheySimplex;
				    double dblFontScale = blobs[i].DblCurrentDiagonalSize / 60.0;
				    int intFontThickness = Convert.ToInt32(Math.Round(dblFontScale * 1.0));

				    CvInvoke.PutText(imgFrame2Copy, i.ToString(), blobs[i].CenterPositions.Last(), fontFace, dblFontScale, _scalarGreen, intFontThickness);

			    }

		    }

	    }



	    public void drawCarCountOnImage(ref int carCount, ref Mat imgFrame2Copy)
	    {
		    FontFace fontFace = FontFace.HersheySimplex;
		    double dblFontScale = Convert.ToDouble(imgFrame2Copy.Rows * imgFrame2Copy.Cols) / 300000.0;
		    int intFontThickness = Convert.ToInt32(Math.Round(dblFontScale * 1.5));

		    Size textSize = getTextSize(carCount.ToString(), (int)fontFace, dblFontScale, intFontThickness);

		    Point bottomLeftTextPosition = new Point();

		    bottomLeftTextPosition.X = imgFrame2Copy.Cols - 1 - Convert.ToInt32(Convert.ToDouble(textSize.Width) * 1.3);
		    bottomLeftTextPosition.Y = Convert.ToInt32(Convert.ToDouble(textSize.Height) * 1.3);

		    CvInvoke.PutText(imgFrame2Copy, carCount.ToString(), bottomLeftTextPosition, fontFace, dblFontScale, _scalarGreen, intFontThickness);
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



	    public void matchCurrentFrameBlobsToExistingBlobs(ref List<Blob> existingBlobs, ref List<Blob> currentFrameBlobs)
	    {
		    foreach (Blob existingBlob in existingBlobs)
		    {
			    existingBlob.BlnCurrentMatchFoundOrNewBlob = false;
			    existingBlob.PredictNextPosition();
		    }


		    foreach (Blob currentFrameBlob in currentFrameBlobs)
		    {
			    int intIndexOfLeastDistance = 0;
			    double dblLeastDistance = 1000000.0;


			    for (int i = 0; i <= existingBlobs.Count() - 1; i++)
			    {

				    if ((existingBlobs[i].BlnStillBeingTracked == true))
				    {
					    double dblDistance = distanceBetweenPoints(currentFrameBlob.CenterPositions.Last(), existingBlobs[i].PredictedNextPosition);

					    if ((dblDistance < dblLeastDistance))
					    {
						    dblLeastDistance = dblDistance;
						    intIndexOfLeastDistance = i;
					    }

				    }

			    }

			    if ((dblLeastDistance < currentFrameBlob.DblCurrentDiagonalSize * 0.5))
			    {
				    addBlobToExistingBlobs(ref currentFrameBlob, ref existingBlobs, ref intIndexOfLeastDistance);
			    }
			    else
			    {
				    addNewBlob(ref currentFrameBlob, ref existingBlobs);
			    }

		    }


		    foreach (Blob existingBlob in existingBlobs)
		    {
			    if ((existingBlob.BlnCurrentMatchFoundOrNewBlob == false))
			    {
				    existingBlob.IntNumOfConsecutiveFramesWithoutAMatch = existingBlob.IntNumOfConsecutiveFramesWithoutAMatch + 1;
			    }

			    if ((existingBlob.IntNumOfConsecutiveFramesWithoutAMatch >= 5))
			    {
				    existingBlob.BlnStillBeingTracked = false;
			    }

		    }

	    }


		public double distanceBetweenPoints(Point point1, Point point2)
	    {

		    int intX = Math.Abs(point1.X - point2.X);
		    int intY = Math.Abs(point1.Y - point2.Y);

		    return Math.Sqrt((Math.Pow(intX, 2)) + (Math.Pow(intY, 2)));

	    }



	    public void addBlobToExistingBlobs(ref Blob currentFrameBlob, ref List<Blob> existingBlobs, ref int intIndex)
	    {
		    existingBlobs[intIndex].CurrentContour = currentFrameBlob.CurrentContour;
		    existingBlobs[intIndex].CurrentBoundingRect = currentFrameBlob.CurrentBoundingRect;

		    existingBlobs[intIndex].CenterPositions.Add(currentFrameBlob.CenterPositions.Last());

		    existingBlobs[intIndex].DblCurrentDiagonalSize = currentFrameBlob.DblCurrentDiagonalSize;
		    existingBlobs[intIndex].DblCurrentAspectRatio = currentFrameBlob.DblCurrentAspectRatio;

		    existingBlobs[intIndex].BlnStillBeingTracked = true;
		    existingBlobs[intIndex].BlnCurrentMatchFoundOrNewBlob = true;

	    }



	    public void addNewBlob(ref Blob currentFrameBlob, ref List<Blob> existingBlobs)
	    {
		    currentFrameBlob.BlnCurrentMatchFoundOrNewBlob = true;

		    existingBlobs.Add(currentFrameBlob);

	    }


	    public bool checkIfBlobsCrossedTheLine(ref List<Blob> blobs, ref int horizontalLinePosition, ref int carCount)
	    {

		    bool atLeastOneBlobCrossedTheLine = false;
		    //this will be the return value


		    foreach (Blob blob in blobs)
		    {

			    if ((blob.BlnStillBeingTracked == true & blob.CenterPositions.Count() >= 2))
			    {
				    int prevFrameIndex = blob.CenterPositions.Count() - 2;
				    int currFrameIndex = blob.CenterPositions.Count() - 1;

				    if ((blob.CenterPositions[prevFrameIndex].Y > horizontalLinePosition & blob.CenterPositions[currFrameIndex].Y <= horizontalLinePosition))
				    {
					    carCount = carCount + 1;
					    atLeastOneBlobCrossedTheLine = true;
				    }

			    }

		    }

		    return (atLeastOneBlobCrossedTheLine);

	    }
	}
}
