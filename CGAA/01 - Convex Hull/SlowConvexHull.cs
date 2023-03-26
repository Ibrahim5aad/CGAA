using CGAA.Extensions;
using System.Drawing;

namespace CGAA.Convex_Hull
{
    /// <summary>
    /// A naive implementation of a convex hull algoritm with O(N^3) time complexity
    /// </summary>
    public class SlowConvexHull
    {
        /// <summary>
        /// Makes a convex hull out of a specified set of points.
        /// </summary>
        /// <input> A set P of points in the plane. </input>
        /// <output> A list L containing the vertices of CH(P) in clockwise order. </output>
        public static IEnumerable<Point> Make(IEnumerable<Point> points)
        {

            if (points.Count() < 3)
                return points;

            var convexHull = new List<(Point, Point)>();

            foreach (var point1 in points)
            {
                foreach (var point2 in points)
                {
                    if (point1 == point2)
                        continue;

                    bool valid = true;

                    foreach (var toCheckPoint in points)
                    {
                        if (toCheckPoint == point1 || toCheckPoint == point2)
                            continue;

                        if (!toCheckPoint.IsPointsLiesToTheRightOfLine(point1, point2))
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (valid)
                    {
                        convexHull.Add(new(point1, point2));
                    }
                }
            }
            

            

            return OrderPointPairs(convexHull);
        }


        /// <summary>
        /// Gets all the possible pairs of a set of points.
        /// </summary>
        /// <param name="points"> The points. </param>
        /// <returns> A set of point pairs. </returns>
        private static IEnumerable<(Point, Point)> PairsOf(IEnumerable<Point> points)
        {
            foreach (var point1 in points)
            {
                foreach (var point2 in points)
                {
                    if (point1 == point2)
                        continue;

                    yield return (point1, point2);
                }
            }
        }


        /// <summary>
        /// Orders a set of point pairs.
        /// </summary>
        /// <param name="pairs"> The point pairs. </param>
        /// <returns> A set of ordered points. </returns>
        private static IEnumerable<Point> OrderPointPairs(List<(Point, Point)> pairs)
        {
            var pairs2 = pairs.ToList();
            var p = pairs2.First();

            var result = new List<Point>()
            {
                p.Item1
            };

            while (pairs2.Any(x => x.Item1 == p.Item2))
            {
                p = pairs2.First(x => x.Item1 == p.Item2);
                result.Add(p.Item1);
                pairs2.Remove(p);
            }

            return result;
        }
    }
}
