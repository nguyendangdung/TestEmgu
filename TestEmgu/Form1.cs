using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace TestEmgu
{
    public partial class Form1 : Form
    {
	    private MCvScalar _scalarBlack = new MCvScalar(0.0, 0.0, 0.0);


	    private readonly MCvScalar _scalarWhite = new MCvScalar(255.0, 255.0, 255.0);


	    private MCvScalar _scalarBlue = new MCvScalar(255.0, 0.0, 0.0);


	    private readonly MCvScalar _scalarGreen = new MCvScalar(0.0, 200.0, 0.0);


	    private readonly MCvScalar _scalarRed = new MCvScalar(0.0, 0.0, 255.0);


	    private VideoCapture _capVideo;


	    public bool IsFormClosing;


		public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
	        IsFormClosing = true;
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

	    //private void TrackBlobsAndUpdateGui()
	    //{
		   // throw new NotImplementedException();
	    //}



	    public void TrackBlobsAndUpdateGui()
	    {
		    var blobs = new List<Blob>();
		    var carCount = 0;
			var crossingLine = new Point[3];

		    var imgFrame1 = _capVideo.QueryFrame();
		    var imgFrame2 = _capVideo.QueryFrame();

		    var horizontalLinePosition = Convert.ToInt32(Math.Round(Convert.ToDouble(imgFrame1.Rows) * 0.35));

		    crossingLine[0].X = 0;
		    crossingLine[0].Y = horizontalLinePosition;

		    crossingLine[1].X = imgFrame1.Cols - 1;
		    crossingLine[1].Y = horizontalLinePosition;

		    var isFirstFrame = true;


		    while (!IsFormClosing)
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

			    CvInvoke.Imshow("imgThresh", imgThresh);

			    // var structuringElement3X3 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
			    var structuringElement5X5 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));
			    //var structuringElement7X7 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(7, 7), new Point(-1, -1));
			    //var structuringElement9X9 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(9, 9), new Point(-1, -1));

			    for (var i = 0; i <= 1; i++)
			    {
				    CvInvoke.Dilate(imgThresh, imgThresh, structuringElement5X5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
				    CvInvoke.Dilate(imgThresh, imgThresh, structuringElement5X5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
				    CvInvoke.Erode(imgThresh, imgThresh, structuringElement5X5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
			    }

			    var imgThreshCopy = imgThresh.Clone();

			    var contours = new VectorOfVectorOfPoint();

			    CvInvoke.FindContours(imgThreshCopy, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

			    DrawAndShowContours(imgThresh.Size, contours, "imgContours");

			    var convexHulls = new VectorOfVectorOfPoint(contours.Size);

			    for (var i = 0; i <= contours.Size - 1; i++)
			    {
				    CvInvoke.ConvexHull(contours[i], convexHulls[i]);
			    }

			    DrawAndShowContours(imgThresh.Size, convexHulls, "imgConvexHulls");


			    for (var i = 0; i <= contours.Size - 1; i++)
			    {
				    var possibleBlob = new Blob(convexHulls[i]);

				    if ((possibleBlob.CurrentRectArea > 400 & possibleBlob.CurrentAspectRatio > 0.2 & possibleBlob.CurrentAspectRatio < 4.0 & possibleBlob.CurrentBoundingRect.Width > 30 & possibleBlob.CurrentBoundingRect.Height > 30 & possibleBlob.CurrentDiagonalSize > 60.0 & (CvInvoke.ContourArea(possibleBlob.CurrentContour) / possibleBlob.CurrentRectArea) > 0.5))
				    {
					    currentFrameBlobs.Add(possibleBlob);
				    }

			    }

			    DrawAndShowContours(imgThresh.Size, currentFrameBlobs, "imgCurrentFrameBlobs");

			    if (isFirstFrame)
			    {
				    foreach (var currentFrameBlob in currentFrameBlobs)
				    {
					    blobs.Add(currentFrameBlob);
				    }
			    }
			    else
			    {
				    MatchCurrentFrameBlobsToExistingBlobs(ref blobs, ref currentFrameBlobs);
			    }

			    DrawAndShowContours(imgThresh.Size, blobs, "imgBlobs");

			    imgFrame2Copy = imgFrame2.Clone();

			    DrawBlobInfoOnImage(ref blobs, ref imgFrame2Copy);

			    dynamic atLeastOneBlobCrossedTheLine = CheckIfBlobsCrossedTheLine(ref blobs, ref horizontalLinePosition, ref carCount);

			    CvInvoke.Line(imgFrame2Copy, crossingLine[0], crossingLine[1],
				    (atLeastOneBlobCrossedTheLine) ? _scalarGreen : _scalarRed, 2);

			    DrawCarCountOnImage(ref carCount, ref imgFrame2Copy);

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

			    isFirstFrame = false;

			    Application.DoEvents();

		    }
	    }

	    public void DrawAndShowContours(Size imageSize, VectorOfVectorOfPoint contours, string strImageName)
	    {
		    var image = new Mat(imageSize, DepthType.Cv8U, 3);

		    CvInvoke.DrawContours(image, contours, -1, _scalarWhite, -1);

		    CvInvoke.Imshow(strImageName, image);

	    }

		public void DrawAndShowContours(Size imageSize, List<Blob> blobs, string strImageName)
	    {
		    var image = new Mat(imageSize, DepthType.Cv8U, 3);

		    var contours = new VectorOfVectorOfPoint();

		    foreach (var blob in blobs)
		    {
			    if (blob.StillBeingTracked)
			    {
				    contours.Push(blob.CurrentContour);
			    }
		    }

		    CvInvoke.DrawContours(image, contours, -1, _scalarWhite, -1);

		    CvInvoke.Imshow(strImageName, image);

	    }



	    public void DrawBlobInfoOnImage(ref List<Blob> blobs, ref Mat imgFrame2Copy)
	    {

		    for (var i = 0; i <= blobs.Count - 1; i++)
		    {

			    if (blobs[i].StillBeingTracked)
			    {
				    CvInvoke.Rectangle(imgFrame2Copy, blobs[i].CurrentBoundingRect, _scalarRed, 2);

				    var fontFace = FontFace.HersheySimplex;
				    var dblFontScale = blobs[i].CurrentDiagonalSize / 60.0;
				    var intFontThickness = Convert.ToInt32(Math.Round(dblFontScale * 1.0));

				    CvInvoke.PutText(imgFrame2Copy, i.ToString(), blobs[i].CenterPositions.Last(), fontFace, dblFontScale, _scalarGreen, intFontThickness);

			    }

		    }

	    }



	    public void DrawCarCountOnImage(ref int carCount, ref Mat imgFrame2Copy)
	    {
		    var fontFace = FontFace.HersheySimplex;
		    var dblFontScale = Convert.ToDouble(imgFrame2Copy.Rows * imgFrame2Copy.Cols) / 300000.0;
		    var intFontThickness = Convert.ToInt32(Math.Round(dblFontScale * 1.5));

		    var textSize = GetTextSize(carCount.ToString(), (int)fontFace, dblFontScale, intFontThickness);

		    var bottomLeftTextPosition = new Point
		    {
			    X = imgFrame2Copy.Cols - 1 - Convert.ToInt32(Convert.ToDouble(textSize.Width) * 1.3),
			    Y = Convert.ToInt32(Convert.ToDouble(textSize.Height) * 1.3)
		    };


		    CvInvoke.PutText(imgFrame2Copy, carCount.ToString(), bottomLeftTextPosition, fontFace, dblFontScale, _scalarGreen, intFontThickness);
	    }


		public Size GetTextSize(string strText, int intFontFace, double dblFontScale, int intFontThickness)
	    {

		    var textSize = new Size();
		    //this will be the return value

		    var intNumChars = strText.Length;

		    textSize.Width = 55 * intNumChars;
		    textSize.Height = 65;

		    return (textSize);

	    }



	    public void MatchCurrentFrameBlobsToExistingBlobs(ref List<Blob> existingBlobs, ref List<Blob> currentFrameBlobs)
	    {
		    foreach (var existingBlob in existingBlobs)
		    {
			    existingBlob.CurrentMatchFoundOrNewBlob = false;
			    existingBlob.PredictNextPosition();
		    }


		    foreach (var currentFrameBlob in currentFrameBlobs)
		    {
			    var intIndexOfLeastDistance = 0;
			    var dblLeastDistance = 1000000.0;


			    for (var i = 0; i <= existingBlobs.Count - 1; i++)
			    {
				    if (!existingBlobs[i].StillBeingTracked) continue;
				    var dblDistance = DistanceBetweenPoints(currentFrameBlob.CenterPositions.Last(), existingBlobs[i].PredictedNextPosition);

				    if ((!(dblDistance < dblLeastDistance))) continue;
				    dblLeastDistance = dblDistance;
				    intIndexOfLeastDistance = i;
			    }

			    if ((dblLeastDistance < currentFrameBlob.CurrentDiagonalSize * 0.5))
			    {
				    AddBlobToExistingBlobs(currentFrameBlob, existingBlobs, ref intIndexOfLeastDistance);
			    }
			    else
			    {
				    AddNewBlob(currentFrameBlob, existingBlobs);
			    }

		    }


		    foreach (var blob in existingBlobs)
		    {
			    if ((blob.CurrentMatchFoundOrNewBlob == false))
			    {
				    blob.IntNumOfConsecutiveFramesWithoutAMatch = blob.IntNumOfConsecutiveFramesWithoutAMatch + 1;
			    }

			    if ((blob.IntNumOfConsecutiveFramesWithoutAMatch >= 5))
			    {
				    blob.StillBeingTracked = false;
			    }

		    }

	    }


		public double DistanceBetweenPoints(Point point1, Point point2)
	    {
		    var intX = Math.Abs(point1.X - point2.X);
		    var intY = Math.Abs(point1.Y - point2.Y);
		    return Math.Sqrt((Math.Pow(intX, 2)) + (Math.Pow(intY, 2)));
	    }



	    public void AddBlobToExistingBlobs(Blob currentFrameBlob, List<Blob> existingBlobs, ref int intIndex)
	    {
		    existingBlobs[intIndex].CurrentContour = currentFrameBlob.CurrentContour;
		    existingBlobs[intIndex].CurrentBoundingRect = currentFrameBlob.CurrentBoundingRect;

		    existingBlobs[intIndex].CenterPositions.Add(currentFrameBlob.CenterPositions.Last());

		    existingBlobs[intIndex].CurrentDiagonalSize = currentFrameBlob.CurrentDiagonalSize;
		    existingBlobs[intIndex].CurrentAspectRatio = currentFrameBlob.CurrentAspectRatio;

		    existingBlobs[intIndex].StillBeingTracked = true;
		    existingBlobs[intIndex].CurrentMatchFoundOrNewBlob = true;

	    }



	    public void AddNewBlob(Blob currentFrameBlob, List<Blob> existingBlobs)
	    {
		    currentFrameBlob.CurrentMatchFoundOrNewBlob = true;

		    existingBlobs.Add(currentFrameBlob);

	    }


	    public bool CheckIfBlobsCrossedTheLine(ref List<Blob> blobs, ref int horizontalLinePosition, ref int carCount)
	    {

		    var atLeastOneBlobCrossedTheLine = false;
		    //this will be the return value


		    foreach (var blob in blobs)
		    {
			    if ((!(blob.StillBeingTracked & blob.CenterPositions.Count >= 2))) continue;
			    var prevFrameIndex = blob.CenterPositions.Count - 2;
			    var currFrameIndex = blob.CenterPositions.Count - 1;

			    if ((!(blob.CenterPositions[prevFrameIndex].Y > horizontalLinePosition &
						blob.CenterPositions[currFrameIndex].Y <= horizontalLinePosition))) continue;
			    carCount = carCount + 1;
			    atLeastOneBlobCrossedTheLine = true;
		    }

		    return atLeastOneBlobCrossedTheLine;
	    }
	}
}
