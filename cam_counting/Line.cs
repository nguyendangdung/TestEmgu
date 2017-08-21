using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace cam_counting
{
	//public class IntegerPoint
	//{
	//	public int X { get; }
	//	public int Y { get; }

	//	public Point Point { get; }

	//	public IntegerPoint(int x, int y)
	//	{
	//		X = x;
	//		Y = y;
	//		Point = new Point(X, Y);
	//	}
	//}
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

		public double A { get; }
		public double B { get; }
		public double C { get; }

		public PointLineEvaluateResult PointEvaluate(Point point)
		{
			var result = A * point.X + B * point.Y + C;
			if ((int)result == 0)
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

	public class Poligon : List<Point>
	{
		public Poligon(List<Point> points)
		{
			// check number of vertices
			if (points.Count < 3)
			{
				throw new Exception();
			}
			AddRange(points);

			// validate duplication vertices
			var gr = this.GroupBy(s => new {s.X, s.Y});
			if (gr.Any(s => s.Count() > 1))
			{
				throw new Exception();
			}

			// check simple/complex poligon
			var simplePoligon = true;
			if (!simplePoligon)
			{
				throw new Exception();
			}
		}
	}
}
