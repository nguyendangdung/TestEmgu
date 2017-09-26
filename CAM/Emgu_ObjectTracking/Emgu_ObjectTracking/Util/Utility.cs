using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emgu_ObjectTracking.Util
{
    class Utility
    {
        public static void Swap<T>(ref T points0, ref T points1)
        {
            var tempPoints = points0;
            points0 = points1;
            points1 = tempPoints;
        }
    }
}
