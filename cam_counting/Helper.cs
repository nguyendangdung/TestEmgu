using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cam_counting
{
	public class Helper
	{
		public List<PointF> Getintersections(Poligon poligon, Line line)
		{
			return new List<PointF>();

			for (int i = 0; i < poligon.Count(); i++)
			{
				var poligonLine = new Line(poligon[i], poligon[i+1]);

			}
		}


	}
}
