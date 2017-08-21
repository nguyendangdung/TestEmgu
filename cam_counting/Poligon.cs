using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace cam_counting
{
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