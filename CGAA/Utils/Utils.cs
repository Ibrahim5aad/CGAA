﻿namespace CGAA
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

        public static List<Segment> GetSetOfRandomSegments(int count, int spanX, int spanY)
        {
            var segments = new List<Segment>();
            var pnts1 = GetSetOfRandomPoints(count, spanX, spanY);
            var pnts2 = GetSetOfRandomPoints(count, spanY, spanY);

            for (int i = 0; i < count; i++)
            {
                segments.Add(new Segment(pnts1[i], pnts2[count - i - 1]));
            }

            return segments;
        }

    }
}
