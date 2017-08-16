using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Util;

namespace MultipleObjectTracking
{
	public class Blob
	{


		public VectorOfPoint currentContour = new VectorOfPoint();

		public Rectangle currentBoundingRect;

		public List<Point> centerPositions = new List<Point>();
		public double CurrentDiagonalSize;

		public double CurrentAspectRatio;
		public int CurrentRectArea;


		public bool CurrentMatchFoundOrNewBlob;

		public bool StillBeingTracked;

		public int NumOfConsecutiveFramesWithoutAMatch;

		public Point predictedNextPosition;

		public Blob(VectorOfPoint _contour)
		{
			currentContour = _contour;

			currentBoundingRect = CvInvoke.BoundingRectangle(currentContour);

			var currentCenter = new Point
			{
				X = Convert.ToInt32(Convert.ToDouble(currentBoundingRect.X + currentBoundingRect.X + currentBoundingRect.Width) /
									2.0),
				Y = Convert.ToInt32(Convert.ToDouble(currentBoundingRect.Y + currentBoundingRect.Y + currentBoundingRect.Height) /
									2.0)
			};


			centerPositions.Add(currentCenter);

			CurrentDiagonalSize = Math.Sqrt((Math.Pow(currentBoundingRect.Width, 2)) + (Math.Pow(currentBoundingRect.Height, 2)));

			CurrentAspectRatio = Convert.ToDouble(currentBoundingRect.Width) / Convert.ToDouble(currentBoundingRect.Height);

			CurrentRectArea = currentBoundingRect.Width * currentBoundingRect.Height;

			StillBeingTracked = true;
			CurrentMatchFoundOrNewBlob = true;

			NumOfConsecutiveFramesWithoutAMatch = 0;

		}

		///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

		public void PredictNextPosition()
		{
			var numPositions = centerPositions.Count();
			switch (numPositions)
			{
				case 1:
					predictedNextPosition.X = centerPositions.Last().X;
					predictedNextPosition.Y = centerPositions.Last().Y;
					break;
				case 2:
					{
						var deltaX = centerPositions[1].X - centerPositions[0].X;
						var deltaY = centerPositions[1].Y - centerPositions[0].Y;
						predictedNextPosition.X = centerPositions.Last().X + deltaX;
						predictedNextPosition.Y = centerPositions.Last().Y + deltaY;
					}
					break;
				case 3:
					{
						var sumOfXChanges = ((centerPositions[2].X - centerPositions[1].X) * 2) + ((centerPositions[1].X - centerPositions[0].X) * 1);
						var deltaX = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfXChanges / 3.0)));
						var sumOfYChanges = ((centerPositions[2].Y - centerPositions[1].Y) * 2) + ((centerPositions[1].Y - centerPositions[0].Y) * 1);
						var deltaY = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfYChanges / 3.0)));
						predictedNextPosition.X = centerPositions.Last().X + deltaX;
						predictedNextPosition.Y = centerPositions.Last().Y + deltaY;
					}
					break;
				case 4:
					{
						var sumOfXChanges = ((centerPositions[3].X - centerPositions[2].X) * 3) + ((centerPositions[2].X - centerPositions[1].X) * 2) + ((centerPositions[1].X - centerPositions[0].X) * 1);
						var deltaX = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfXChanges / 6.0)));
						var sumOfYChanges = ((centerPositions[3].Y - centerPositions[2].Y) * 3) + ((centerPositions[2].Y - centerPositions[1].Y) * 2) + ((centerPositions[1].Y - centerPositions[0].Y) * 1);
						var deltaY = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfYChanges / 6.0)));
						predictedNextPosition.X = centerPositions.Last().X + deltaX;
						predictedNextPosition.Y = centerPositions.Last().Y + deltaY;
					}
					break;
				default:
					if ((numPositions >= 5))
					{
						var sumOfXChanges = ((centerPositions[numPositions - 1].X - centerPositions[numPositions - 2].X) * 4) + ((centerPositions[numPositions - 2].X - centerPositions[numPositions - 3].X) * 3) + ((centerPositions[numPositions - 3].X - centerPositions[numPositions - 4].X) * 2) + ((centerPositions[numPositions - 4].X - centerPositions[numPositions - 5].X) * 1);
						var deltaX = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfXChanges / 10.0)));
						var sumOfYChanges = ((centerPositions[numPositions - 1].Y - centerPositions[numPositions - 2].Y) * 4) + ((centerPositions[numPositions - 2].Y - centerPositions[numPositions - 3].Y) * 3) + ((centerPositions[numPositions - 3].Y - centerPositions[numPositions - 4].Y) * 2) + ((centerPositions[numPositions - 4].Y - centerPositions[numPositions - 5].Y) * 1);
						var deltaY = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfYChanges / 10.0)));
						predictedNextPosition.X = centerPositions.Last().X + deltaX;
						predictedNextPosition.Y = centerPositions.Last().Y + deltaY;
					}
					break;
			}
		}

	}
}