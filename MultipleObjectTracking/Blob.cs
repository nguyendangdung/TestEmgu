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
		public VectorOfPoint CurrentContour;
		public Rectangle CurrentBoundingRect;
		public List<Point> CenterPositions = new List<Point>();
		public double CurrentDiagonalSize;
		public double CurrentAspectRatio;
		public int CurrentRectArea;
		public bool CurrentMatchFoundOrNewBlob;
		public bool StillBeingTracked;
		public int NumOfConsecutiveFramesWithoutAMatch;
		public Point PredictedNextPosition;

		public Blob(VectorOfPoint contour)
		{
			CurrentContour = contour;
			CurrentBoundingRect = CvInvoke.BoundingRectangle(CurrentContour);
			var currentCenter = new Point
			{
				X = Convert.ToInt32(Convert.ToDouble(CurrentBoundingRect.X + CurrentBoundingRect.X + CurrentBoundingRect.Width) /
									2.0),
				Y = Convert.ToInt32(Convert.ToDouble(CurrentBoundingRect.Y + CurrentBoundingRect.Y + CurrentBoundingRect.Height) /
									2.0)
			};
			CenterPositions.Add(currentCenter);
			CurrentDiagonalSize = Math.Sqrt((Math.Pow(CurrentBoundingRect.Width, 2)) +
											(Math.Pow(CurrentBoundingRect.Height, 2)));
			CurrentAspectRatio = Convert.ToDouble(CurrentBoundingRect.Width) / Convert.ToDouble(CurrentBoundingRect.Height);
			CurrentRectArea = CurrentBoundingRect.Width * CurrentBoundingRect.Height;
			StillBeingTracked = true;
			CurrentMatchFoundOrNewBlob = true;
			NumOfConsecutiveFramesWithoutAMatch = 0;
		}

		public void PredictNextPosition()
		{
			var numPositions = CenterPositions.Count();
			switch (numPositions)
			{
				case 1:
					PredictedNextPosition.X = CenterPositions.Last().X;
					PredictedNextPosition.Y = CenterPositions.Last().Y;
					break;
				case 2:
					{
						var deltaX = CenterPositions[1].X - CenterPositions[0].X;
						var deltaY = CenterPositions[1].Y - CenterPositions[0].Y;
						PredictedNextPosition.X = CenterPositions.Last().X + deltaX;
						PredictedNextPosition.Y = CenterPositions.Last().Y + deltaY;
					}
					break;
				case 3:
				{
					var sumOfXChanges = ((CenterPositions[2].X - CenterPositions[1].X) * 2) +
										((CenterPositions[1].X - CenterPositions[0].X) * 1);
						var deltaX = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfXChanges / 3.0)));
					var sumOfYChanges = ((CenterPositions[2].Y - CenterPositions[1].Y) * 2) +
										((CenterPositions[1].Y - CenterPositions[0].Y) * 1);
						var deltaY = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfYChanges / 3.0)));
						PredictedNextPosition.X = CenterPositions.Last().X + deltaX;
						PredictedNextPosition.Y = CenterPositions.Last().Y + deltaY;
					}
					break;
				case 4:
				{
					var sumOfXChanges = ((CenterPositions[3].X - CenterPositions[2].X) * 3) +
										((CenterPositions[2].X - CenterPositions[1].X) * 2) + ((CenterPositions[1].X - CenterPositions[0].X) * 1);
						var deltaX = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfXChanges / 6.0)));
					var sumOfYChanges = ((CenterPositions[3].Y - CenterPositions[2].Y) * 3) +
										((CenterPositions[2].Y - CenterPositions[1].Y) * 2) + ((CenterPositions[1].Y - CenterPositions[0].Y) * 1);
						var deltaY = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfYChanges / 6.0)));
						PredictedNextPosition.X = CenterPositions.Last().X + deltaX;
						PredictedNextPosition.Y = CenterPositions.Last().Y + deltaY;
					}
					break;
				default:
					if ((numPositions >= 5))
					{
						var sumOfXChanges = ((CenterPositions[numPositions - 1].X - CenterPositions[numPositions - 2].X) * 4) +
											((CenterPositions[numPositions - 2].X - CenterPositions[numPositions - 3].X) * 3) +
											((CenterPositions[numPositions - 3].X - CenterPositions[numPositions - 4].X) * 2) +
											((CenterPositions[numPositions - 4].X - CenterPositions[numPositions - 5].X) * 1);
						var deltaX = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfXChanges / 10.0)));
						var sumOfYChanges = ((CenterPositions[numPositions - 1].Y - CenterPositions[numPositions - 2].Y) * 4) +
											((CenterPositions[numPositions - 2].Y - CenterPositions[numPositions - 3].Y) * 3) +
											((CenterPositions[numPositions - 3].Y - CenterPositions[numPositions - 4].Y) * 2) +
											((CenterPositions[numPositions - 4].Y - CenterPositions[numPositions - 5].Y) * 1);
						var deltaY = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfYChanges / 10.0)));
						PredictedNextPosition.X = CenterPositions.Last().X + deltaX;
						PredictedNextPosition.Y = CenterPositions.Last().Y + deltaY;
					}
					break;
			}
		}

	}
}