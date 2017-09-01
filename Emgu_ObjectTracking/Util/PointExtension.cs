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
    }
}
