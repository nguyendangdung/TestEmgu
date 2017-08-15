using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Util;
namespace ImageSubtraction
{
	///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
	public class Blob
	{

		// member variables ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		public VectorOfPoint contour;


		public Rectangle boundingRect;

		public Point centerPosition;

		public double dblDiagonalSize;

		public double dblAspectRatio;

		public int intRectArea;
		// constructor '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

		public Blob(VectorOfPoint _contour)
		{
			contour = _contour;

			boundingRect = CvInvoke.BoundingRectangle(contour);

			centerPosition.X = Convert.ToInt32((boundingRect.Left + boundingRect.Right) / 2);
			centerPosition.Y = Convert.ToInt32((boundingRect.Top + boundingRect.Bottom) / 2);

			dblDiagonalSize = Math.Sqrt((Math.Pow(boundingRect.Width, 2)) + (Math.Pow(boundingRect.Height, 2)));

			dblAspectRatio = Convert.ToDouble(boundingRect.Width) / Convert.ToDouble(boundingRect.Height);

			intRectArea = boundingRect.Width * boundingRect.Height;

		}
	}
}