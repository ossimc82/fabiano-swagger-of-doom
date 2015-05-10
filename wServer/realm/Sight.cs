#region

using System.Collections.Generic;

#endregion

namespace wServer.realm
{
    internal static class Sight
    {
        private static readonly Dictionary<int, IntPoint[]> points = new Dictionary<int, IntPoint[]>();

        public static IntPoint[] GetSightCircle(int radius)
        {
            IntPoint[] ret;
            if (!points.TryGetValue(radius, out ret))
            {
                List<IntPoint> pts = new List<IntPoint>();
                for (int y = -radius; y <= radius; y++)
                    for (int x = -radius; x <= radius; x++)
                    {
                        if (x*x + y*y <= radius*radius)
                            pts.Add(new IntPoint(x, y));
                    }
                ret = points[radius] = pts.ToArray();
            }
            return ret;
        }
    }
}