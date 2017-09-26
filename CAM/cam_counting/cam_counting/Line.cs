using System;
using System.Drawing;
using System.Windows;
using Point = System.Drawing.Point;

namespace cam_counting
{
	public class Line
	{
        public PointF First { get; private set; }
        public PointF Second { get; private set; }
        public Vector Vector { get; private set; }
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
			A = Vector.X;
			B = Vector.Y;
			C = -(A * first.X + B * first.Y);
		}

        public double A { get; private set; }
        public double B { get; private set; }
        public double C { get; private set; }

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