using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Util;
namespace ImageSubtraction
{
	public class Blob
	{
		public VectorOfPoint Contour;
		public Rectangle BoundingRect;
		public Point CenterPosition;
		public double DiagonalSize;
		public double AspectRatio;
		public int RectArea;

		public Blob(VectorOfPoint contour)
		{
			Contour = contour;
			BoundingRect = CvInvoke.BoundingRectangle(Contour);
			CenterPosition.X = Convert.ToInt32((BoundingRect.Left + BoundingRect.Right) / 2);
			CenterPosition.Y = Convert.ToInt32((BoundingRect.Top + BoundingRect.Bottom) / 2);
			DiagonalSize = Math.Sqrt((Math.Pow(BoundingRect.Width, 2)) + (Math.Pow(BoundingRect.Height, 2)));
			AspectRatio = Convert.ToDouble(BoundingRect.Width) / Convert.ToDouble(BoundingRect.Height);
			RectArea = BoundingRect.Width * BoundingRect.Height;
		}
	}
}