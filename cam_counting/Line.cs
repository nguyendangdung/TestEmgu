using System;
using System.Windows;
using Point = System.Drawing.Point;

namespace cam_counting
{
	public enum PointLineEvaluateResult
	{
		Positive,
		Negative,
		Zero
	}

	public class Line
	{

		public Point First { get; }
		public Point Second { get; }

		public Line(Point first, Point second)
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

		public PointLineEvaluateResult PointEvaluate(Point point)
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
}
