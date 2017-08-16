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

namespace ImageSubtraction
{
	public partial class Form2 : Form
	{
		private MCvScalar SCALAR_BLACK = new MCvScalar(0.0, 0.0, 0.0);

		private MCvScalar SCALAR_WHITE = new MCvScalar(255.0, 255.0, 255.0);

		private MCvScalar SCALAR_BLUE = new MCvScalar(255.0, 0.0, 0.0);

		private MCvScalar SCALAR_GREEN = new MCvScalar(0.0, 255.0, 0.0);

		private MCvScalar SCALAR_RED = new MCvScalar(0.0, 0.0, 255.0);


		private VideoCapture capVideo =
			new VideoCapture(
				@"C:\Users\Administrator\Documents\Visual Studio 2015\Projects\TestEmgu\ImageSubtraction\768x576.avi");
		public Form2()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var frame1 = capVideo.QueryFrame();
			var gray1 = frame1.Clone();
			CvInvoke.CvtColor(gray1, gray1, ColorConversion.Bgr2Gray);
			var blur1 = gray1.Clone();
			CvInvoke.GaussianBlur(blur1, blur1, new Size(5, 5), 0);
			imageBox1.Image = blur1;

			var frame2 = capVideo.QueryFrame();
			var gray2 = frame2.Clone();
			CvInvoke.CvtColor(gray2, gray2, ColorConversion.Bgr2Gray);
			var blur2 = gray2.Clone();
			CvInvoke.GaussianBlur(blur2, blur2, new Size(5, 5), 0);
			imageBox2.Image = blur2;

			var subtractionResultImg = new Mat(frame1.Size, DepthType.Cv8U, 1);
			CvInvoke.AbsDiff(blur1, blur2, subtractionResultImg);
			imageBox3.Image = subtractionResultImg;


			var segmentationResultImg = new Mat(frame1.Size, DepthType.Cv8U, 1);
			CvInvoke.Threshold(subtractionResultImg, segmentationResultImg, 30, 255.0, ThresholdType.Binary);
			imageBox4.Image = segmentationResultImg;



			//////////////////////////////
			var dilateErodeResultImg = segmentationResultImg.Clone();
			var structuringElement5X5 =
				CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));
			CvInvoke.Dilate(dilateErodeResultImg, dilateErodeResultImg, structuringElement5X5, new Point(-1, -1), 1, BorderType.Default,
				new MCvScalar(0, 0, 0));

			CvInvoke.Dilate(dilateErodeResultImg, dilateErodeResultImg, structuringElement5X5, new Point(-1, -1), 1, BorderType.Default,
				new MCvScalar(0, 0, 0));

			CvInvoke.Erode(dilateErodeResultImg, dilateErodeResultImg, structuringElement5X5, new Point(-1, -1), 1, BorderType.Default,
				new MCvScalar(0, 0, 0));
			imageBox5.Image = dilateErodeResultImg;


			/////////////////////////////////
			var dilateErodeResultImgClone = dilateErodeResultImg.Clone();
			var contours = new VectorOfVectorOfPoint();
			CvInvoke.FindContours(dilateErodeResultImgClone, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
			var findContoursResultImg = new Mat(segmentationResultImg.Size, DepthType.Cv8U, 3);
			CvInvoke.DrawContours(findContoursResultImg, contours, -1, SCALAR_WHITE, -1);
			imageBox6.Image = findContoursResultImg;


			/////////////////////////////////////
			// Find Convex hull
			var convexHulls = new VectorOfVectorOfPoint(contours.Size);
			for (int i = 0; i < contours.Size; i++)
			{
				CvInvoke.ConvexHull(contours[i], convexHulls[i]);
			}

			var blobs = new List<Blob>();
			for (int i = 0; i < convexHulls.Size; i++)
			{
				var possibleBlob = new Blob(convexHulls[i]);
				if (possibleBlob.RectArea > 100 &&
					possibleBlob.AspectRatio >= 0.2 &&
					possibleBlob.AspectRatio <= 1.2 &&
					possibleBlob.BoundingRect.Width > 15 &&
					possibleBlob.BoundingRect.Height > 20 &&
					possibleBlob.DiagonalSize > 30.0)
				{
					blobs.Add(possibleBlob);
				}
			}

			var convexHullsResultImg = new Mat(segmentationResultImg.Size, DepthType.Cv8U, 3);
			convexHulls = new VectorOfVectorOfPoint();
			blobs.ForEach(blob => convexHulls.Push(blob.Contour));
			CvInvoke.DrawContours(convexHullsResultImg, convexHulls, -1, SCALAR_WHITE, -1);
			imageBox1.Image = convexHullsResultImg;


			Application.DoEvents();
		}
	}
}
