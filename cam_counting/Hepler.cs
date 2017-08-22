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
		public bool PointInsidePolygon(List<PointF> polygon, PointF e)
		{
			var currentPoint = new PointF();//to store the mouse location
			{
				//Ray-cast algorithm is here onward
				int k, j = polygon.Count - 1;
				var oddNodes = false; //to check whether number of intersections is odd
				for (k = 0; k < polygon.Count; k++)
				{
					//fetch adjucent points of the polygon
					var polyK = polygon[k];
					var polyJ = polygon[j];

					//check the intersections
					if (((polyK.Y > currentPoint.Y) != (polyJ.Y > currentPoint.Y)) &&
						(currentPoint.X < (polyJ.X - polyK.X) * (currentPoint.Y - polyK.Y) / (polyJ.Y - polyK.Y) + polyK.X))
						oddNodes = !oddNodes; //switch between odd and even
					j = k;
				}

				//if odd number of intersections
				return oddNodes;
			}
		}
	}
}
