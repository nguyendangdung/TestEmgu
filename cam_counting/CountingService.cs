using System;
using System.Collections.Generic;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace cam_counting
{
	public class CountingService : ICountingService, IDisposable
	{
		private bool _isSetup = false;
		private bool _isFirstPush = true;
		public EventHandler Increment { get; set; }
		public EventHandler Decrement { get; set; }

		readonly Mat _temp = new Mat();

		public List<Rectangle> PushFrame(Mat mat)
		{
			if (mat == null) throw new ArgumentNullException("mat");
			if (!_isSetup) throw new Exception();

			//Cho size nhỏ lại để tăng tốc độ apply filter
			//CvInvoke.Resize(mat, _temp, new Size(400, 300));
			CvInvoke.CvtColor(mat, _temp, ColorConversion.Bgr2Gray);
			CvInvoke.GaussianBlur(_temp, _temp, new Size(5, 5), 0);
			_theSecondOriginal = _temp.Clone();
			_horizontalLinePosition = (int)Math.Round(mat.Rows * 0.35);
			if (_isFirstPush)
			{
				_theFirstOriginal = _theSecondOriginal;
				_isFirstPush = false;
			}
			else
			{
				ProcessCouting();
			}
			return _blobs.Where(s => s.StillBeingTracked).Select(s => s.CurrentBoundingRect).ToList();
		}

		public void Setup(List<PointF> polygon, List<PointF> line)
		{
			_isSetup = true;
		}

		private void ProcessCouting()
		{
			using (var result = new Mat(_theFirstOriginal.Size, DepthType.Cv8U, 1))
			{
				CvInvoke.AbsDiff(_theFirstOriginal, _theSecondOriginal, result);
				_theFirstOriginal.Dispose();
				_theFirstOriginal = _theSecondOriginal;
				CvInvoke.Threshold(result, result, 30, 255.0, ThresholdType.Binary);

				for (var i = 0; i <= 1; i++)
				{
					CvInvoke.Dilate(result, result, _structuringElement5X5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
					CvInvoke.Dilate(result, result, _structuringElement5X5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
					CvInvoke.Erode(result, result, _structuringElement5X5, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
				}

				using (var contours = new VectorOfVectorOfPoint())
				{
					CvInvoke.FindContours(result, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
					using (var convexHulls = new VectorOfVectorOfPoint(contours.Size))
					{
						var currentFrameBlobs = new List<Blob>();
						for (var i = 0; i <= contours.Size - 1; i++)
						{
							CvInvoke.ConvexHull(contours[i], convexHulls[i]);
							var possibleBlob = new Blob(convexHulls[i]);
							if (possibleBlob.CurrentRectArea > 400 & 
								possibleBlob.CurrentAspectRatio > 0.2 & 
								possibleBlob.CurrentAspectRatio < 4.0 & 
								possibleBlob.CurrentBoundingRect.Width > 30 & 
								possibleBlob.CurrentBoundingRect.Height > 30 & 
								possibleBlob.CurrentDiagonalSize > 60.0 & 
								CvInvoke.ContourArea(possibleBlob.CurrentContour) / possibleBlob.CurrentRectArea > 0.5)
							{
								currentFrameBlobs.Add(possibleBlob);
							}
						}


						if (_isFirstFrames)
						{
							_blobs.AddRange(currentFrameBlobs);
						}
						else
						{
							MatchCurrentFrameBlobsToExistingBlobs(_blobs, currentFrameBlobs);
						}

						var atLeastOneBlobCrossedTheLine = CheckIfBlobsCrossedTheLine(_blobs, _horizontalLinePosition, ref _objectCount);
						if (atLeastOneBlobCrossedTheLine)
						{
							if (Increment != null)
							{
								Increment.Invoke(this, null);
							}
						}
						currentFrameBlobs.Clear();
						_isFirstFrames = false;
					}
				}
			}
		}

		private static void MatchCurrentFrameBlobsToExistingBlobs(List<Blob> existingBlobs, List<Blob> currentFrameBlobs)
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

					if (existingBlobs[i].StillBeingTracked == true)
					{
						var dblDistance = DistanceBetweenPoints(currentFrameBlob.CenterPositions.Last(), existingBlobs[i].PredictedNextPosition);

						if (dblDistance < dblLeastDistance)
						{
							dblLeastDistance = dblDistance;
							intIndexOfLeastDistance = i;
						}

					}

				}

				if (dblLeastDistance < currentFrameBlob.CurrentDiagonalSize * 0.5)
				{
					AddBlobToExistingBlobs(currentFrameBlob, existingBlobs, intIndexOfLeastDistance);
				}
				else
				{
					AddNewBlob(currentFrameBlob, existingBlobs);
				}

			}


			foreach (var existingBlob in existingBlobs)
			{
				if (existingBlob.CurrentMatchFoundOrNewBlob == false)
				{
					existingBlob.NumOfConsecutiveFramesWithoutAMatch = existingBlob.NumOfConsecutiveFramesWithoutAMatch + 1;
				}

				if (existingBlob.NumOfConsecutiveFramesWithoutAMatch >= 5)
				{
					existingBlob.StillBeingTracked = false;
				}

			}

		}
		private static double DistanceBetweenPoints(Point point1, Point point2)
		{

			var intX = Math.Abs(point1.X - point2.X);
			var intY = Math.Abs(point1.Y - point2.Y);

			return Math.Sqrt(Math.Pow(intX, 2) + Math.Pow(intY, 2));

		}

		private static void AddBlobToExistingBlobs(Blob currentFrameBlob, List<Blob> existingBlobs, int intIndex)
		{
			existingBlobs[intIndex].CurrentContour = currentFrameBlob.CurrentContour;
			existingBlobs[intIndex].CurrentBoundingRect = currentFrameBlob.CurrentBoundingRect;

			existingBlobs[intIndex].CenterPositions.Add(currentFrameBlob.CenterPositions.Last());

			existingBlobs[intIndex].CurrentDiagonalSize = currentFrameBlob.CurrentDiagonalSize;
			existingBlobs[intIndex].CurrentAspectRatio = currentFrameBlob.CurrentAspectRatio;

			existingBlobs[intIndex].StillBeingTracked = true;
			existingBlobs[intIndex].CurrentMatchFoundOrNewBlob = true;

		}

		private static void AddNewBlob(Blob currentFrameBlob, List<Blob> existingBlobs)
		{
			currentFrameBlob.CurrentMatchFoundOrNewBlob = true;

			existingBlobs.Add(currentFrameBlob);

		}

		private static bool CheckIfBlobsCrossedTheLine(List<Blob> blobs, int horizontalLinePosition, ref int carCount)
		{
			var atLeastOneBlobCrossedTheLine = false;
			foreach (var blob in blobs)
			{

				if (blob.StillBeingTracked & blob.CenterPositions.Count >= 2)
				{
					var prevFrameIndex = blob.CenterPositions.Count - 2;
					var currFrameIndex = blob.CenterPositions.Count - 1;

					var a = blob.CenterPositions[prevFrameIndex].Y > horizontalLinePosition &&
							blob.CenterPositions[currFrameIndex].Y <= horizontalLinePosition;
					var b = blob.CenterPositions[prevFrameIndex].Y <= horizontalLinePosition &&
							blob.CenterPositions[currFrameIndex].Y > horizontalLinePosition;
					if (a || b)
					{
						carCount = carCount + 1;
						atLeastOneBlobCrossedTheLine = true;
					}

				}

			}

			return atLeastOneBlobCrossedTheLine;

		}

		private Mat _theFirstOriginal;
		private Mat _theSecondOriginal;

		private readonly Mat _structuringElement5X5 =
			CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));

		private bool _isFirstFrames = true;
		readonly List<Blob> _blobs = new List<Blob>();
		private int _horizontalLinePosition;
		private int _objectCount;


		public void Dispose()
		{
			if (_theFirstOriginal != null) _theFirstOriginal.Dispose();
			if (_theSecondOriginal != null) _theSecondOriginal.Dispose();
			if (_structuringElement5X5 != null) _structuringElement5X5.Dispose();
			_blobs.ForEach(c => c.Dispose());
			_blobs.Clear();
		}
	}
}
