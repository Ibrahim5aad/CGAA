using System.Drawing;

namespace CGAA
{
    public static class Utils
    {

        /// <summary>
        /// Gets a set of random points.
        /// </summary>
        /// <param name="count">The points count.</param>
        /// <param name="spanX">The span of points in x direction.</param>
        /// <param name="spanY">The span of points in y direction.</param>
        /// <returns></returns>
        public static List<Point> GetSetOfRandomPoints(int count, int spanX, int spanY)
        {
            var pnts = new List<Point>();
            Random rnd = new(100);

            for (int i = 0; i < count; i++)
            {
                int x = rnd.Next(spanX - 100);
                int y = rnd.Next(spanY - 100);
                pnts.Add(new Point(x, y));
            }

            return pnts;
        }

    }
}
