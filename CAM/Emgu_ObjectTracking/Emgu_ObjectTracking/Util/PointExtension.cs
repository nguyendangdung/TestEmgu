using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emgu_ObjectTracking
{
    public static class PointExtension
    {
        public static Point ToPoint(this PointF pointf)
        {
            return new Point((int)pointf.X, (int)pointf.Y);
        }

        public static bool InsideRectangle(this PointF point, Rectangle rec)
        {
            bool isInside = point.X >= rec.Left && point.X <= rec.Right
                            && point.Y >= rec.Top && point.Y <= rec.Bottom;
            return isInside;
        }

        public static bool IsSamePoint(this PointF pointf1, PointF pointf2, double eps)
        {
            var dx = Math.Abs(pointf2.X - pointf1.X);
            var dy = Math.Abs(pointf2.Y - pointf1.Y);

            bool isSame = (dx <= eps && dy <= eps);
            if (isSame)
                return true;
            else
                return false;
            return isSame;
        }

        public static Rectangle GetBoundary(PointF[] points)
        {
            var minX = points.Select(c => c.X).Min();
            var maxX = points.Select(c => c.X).Max();
            var minY = points.Select(c => c.Y).Min();
            var maxY = points.Select(c => c.Y).Max();

            Rectangle rec = new Rectangle();
            rec.X = (int)minX;
            rec.Y = (int)minY;
            rec.Width = (int)(maxX - minX);
            rec.Height = (int)(maxY - minY);
            return rec;
        }

        public static PointF GetCenterPoint(PointF[] points)
        {
            var x = points.Average(c => c.X);
            var y = points.Average(c => c.Y);
            return new PointF(x, y);
        }
    }
}
