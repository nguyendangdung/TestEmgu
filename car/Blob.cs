using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Util;
namespace car
{
	///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
	public class Blob
	{


		public VectorOfPoint currentContour = new VectorOfPoint();

		public Rectangle currentBoundingRect;

		public List<Point> centerPositions = new List<Point>();
		public double dblCurrentDiagonalSize;

		public double dblCurrentAspectRatio;
		public int intCurrentRectArea;


		public bool blnCurrentMatchFoundOrNewBlob;

		public bool blnStillBeingTracked;

		public int intNumOfConsecutiveFramesWithoutAMatch;

		public Point predictedNextPosition;
		// constructor '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

		public Blob(VectorOfPoint _contour)
		{
			currentContour = _contour;

			currentBoundingRect = CvInvoke.BoundingRectangle(currentContour);

			Point currentCenter = new Point();

			currentCenter.X = Convert.ToInt32(Convert.ToDouble(currentBoundingRect.X + currentBoundingRect.X + currentBoundingRect.Width) / 2.0);
			currentCenter.Y = Convert.ToInt32(Convert.ToDouble(currentBoundingRect.Y + currentBoundingRect.Y + currentBoundingRect.Height) / 2.0);

			centerPositions.Add(currentCenter);

			dblCurrentDiagonalSize = Math.Sqrt((Math.Pow(currentBoundingRect.Width, 2)) + (Math.Pow(currentBoundingRect.Height, 2)));

			dblCurrentAspectRatio = Convert.ToDouble(currentBoundingRect.Width) / Convert.ToDouble(currentBoundingRect.Height);

			intCurrentRectArea = currentBoundingRect.Width * currentBoundingRect.Height;

			blnStillBeingTracked = true;
			blnCurrentMatchFoundOrNewBlob = true;

			intNumOfConsecutiveFramesWithoutAMatch = 0;

		}

		///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

		public void predictNextPosition()
		{
			int numPositions = centerPositions.Count();


			if ((numPositions == 1))
			{
				predictedNextPosition.X = centerPositions.Last().X;
				predictedNextPosition.Y = centerPositions.Last().Y;


			}
			else if ((numPositions == 2))
			{
				int deltaX = centerPositions[1].X - centerPositions[0].X;
				int deltaY = centerPositions[1].Y - centerPositions[0].Y;

				predictedNextPosition.X = centerPositions.Last().X + deltaX;
				predictedNextPosition.Y = centerPositions.Last().Y + deltaY;


			}
			else if ((numPositions == 3))
			{
				int sumOfXChanges = ((centerPositions[2].X - centerPositions[1].X) * 2) + ((centerPositions[1].X - centerPositions[0].X) * 1);

				int deltaX = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfXChanges / 3.0)));

				int sumOfYChanges = ((centerPositions[2].Y - centerPositions[1].Y) * 2) + ((centerPositions[1].Y - centerPositions[0].Y) * 1);

				int deltaY = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfYChanges / 3.0)));

				predictedNextPosition.X = centerPositions.Last().X + deltaX;
				predictedNextPosition.Y = centerPositions.Last().Y + deltaY;


			}
			else if ((numPositions == 4))
			{
				int sumOfXChanges = ((centerPositions[3].X - centerPositions[2].X) * 3) + ((centerPositions[2].X - centerPositions[1].X) * 2) + ((centerPositions[1].X - centerPositions[0].X) * 1);

				int deltaX = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfXChanges / 6.0)));

				int sumOfYChanges = ((centerPositions[3].Y - centerPositions[2].Y) * 3) + ((centerPositions[2].Y - centerPositions[1].Y) * 2) + ((centerPositions[1].Y - centerPositions[0].Y) * 1);

				int deltaY = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfYChanges / 6.0)));

				predictedNextPosition.X = centerPositions.Last().X + deltaX;
				predictedNextPosition.Y = centerPositions.Last().Y + deltaY;


			}
			else if ((numPositions >= 5))
			{
				int sumOfXChanges = ((centerPositions[numPositions - 1].X - centerPositions[numPositions - 2].X) * 4) + ((centerPositions[numPositions - 2].X - centerPositions[numPositions - 3].X) * 3) + ((centerPositions[numPositions - 3].X - centerPositions[numPositions - 4].X) * 2) + ((centerPositions[numPositions - 4].X - centerPositions[numPositions - 5].X) * 1);

				int deltaX = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfXChanges / 10.0)));

				int sumOfYChanges = ((centerPositions[numPositions - 1].Y - centerPositions[numPositions - 2].Y) * 4) + ((centerPositions[numPositions - 2].Y - centerPositions[numPositions - 3].Y) * 3) + ((centerPositions[numPositions - 3].Y - centerPositions[numPositions - 4].Y) * 2) + ((centerPositions[numPositions - 4].Y - centerPositions[numPositions - 5].Y) * 1);

				int deltaY = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfYChanges / 10.0)));

				predictedNextPosition.X = centerPositions.Last().X + deltaX;
				predictedNextPosition.Y = centerPositions.Last().Y + deltaY;

			}
			else
			{
				//should never get here
			}

		}

	}
}