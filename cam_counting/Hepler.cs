using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cam_counting
{
	class Hepler
	{
		public static bool PointInsidePolygon(List<PointF> polygon, PointF e)
		{

			//var currentPoint = new PointF(); //to store the mouse location

			//Ray-cast algorithm is here onward
			int k, j = polygon.Count - 1;
			var oddNodes = false; //to check whether number of intersections is odd
			for (k = 0; k < polygon.Count; k++)
			{
				//fetch adjucent points of the polygon
				var polyK = polygon[k];
				var polyJ = polygon[j];

				//check the intersections
				if (((polyK.Y > e.Y) != (polyJ.Y > e.Y)) &&
					(e.X < (polyJ.X - polyK.X) * (e.Y - polyK.Y) / (polyJ.Y - polyK.Y) + polyK.X))
					oddNodes = !oddNodes; //switch between odd and even
				j = k;
			}

			//if odd number of intersections
			return oddNodes;
		}
	}
}
