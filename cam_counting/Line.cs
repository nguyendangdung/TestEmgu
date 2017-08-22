using System;
using System.Drawing;
using System.Windows;
using Point = System.Drawing.Point;

namespace cam_counting
{
	public class Line
	{
		public PointF First { get; }
		public PointF Second { get; }
		public Vector Vector { get; }
		public Line(PointF first, PointF second)
		{
			First = first;
			Second = second;

			if (first.X == second.X && first.Y == second.Y)
			{
				throw new Exception();
			}
			var vector = new Vector(first.X - second.X, first.Y - second.Y);
			vector.Normalize();
			Vector = new Vector(-vector.Y, vector.X);
			A = (int)Vector.X;
			B = (int)Vector.Y;
			C = -(A * first.X + B * first.Y);
		}

		public float A { get; }
		public float B { get; }
		public float C { get; }

		public double PointEvaluate(PointF point)
		{
			var result = A * point.X + B * point.Y + C;
			return result;
			//if (result == 0)
			//{
			//	return PointLineEvaluateResult.Zero;
			//}
			//if (result > 0)
			//{
			//	return PointLineEvaluateResult.Positive;
			//}
			//return PointLineEvaluateResult.Negative;
		}
	}
}