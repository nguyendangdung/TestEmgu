using System;
using System.Collections.Generic;
using System.Windows;

namespace cam_counting
{
	public class IntegerPoint
	{
		public int X { get; }
		public int Y { get; }

		public Point Point { get; }

		public IntegerPoint(int x, int y)
		{
			X = x;
			Y = y;
			Point = new Point(X, Y);
		}
	}
	public enum PointLineEvaluateResult
	{
		Positive,
		Negative,
		Zero
	}

	public class Line
	{
		public IntegerPoint First { get; }
		public IntegerPoint Second { get; }

		public Line(IntegerPoint first, IntegerPoint second)
		{
			First = first;
			Second = second;

			if (first.X == second.X && first.Y == second.Y)
			{
				throw new Exception();
			}
			var vector = new Vector(first.X - second.X, first.Y - second.Y);
			vector.Normalize();
			vector = new Vector(-vector.Y, vector.X);
			A = (int)vector.X;
			B = (int)vector.Y;
			C = -(A * first.X + B * first.Y);
		}

		public int A { get; }
		public int B { get; }
		public int C { get; }

		public PointLineEvaluateResult PointEvaluate(IntegerPoint point)
		{
			var result = A * point.X + B * point.Y + C;
			if (result == 0)
			{
				return PointLineEvaluateResult.Zero;
			}
			if (result > 0)
			{
				return PointLineEvaluateResult.Positive;
			}
			return PointLineEvaluateResult.Negative;
		}
	}

	public class Poligon
	{
		public List<IntegerPoint> Points { get; }

		public Poligon(List<IntegerPoint> points)
		{
			Points = points;

			// validate

		}
	}
}
