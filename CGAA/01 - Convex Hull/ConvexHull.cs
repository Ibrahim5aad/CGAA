using CGAA.Extensions;
using System.Drawing;

namespace CGAA
{
    public class ConvexHull
    {

        /// <summary>
        /// Makes a convex hull out of a specified set of points.
        /// </summary>
        /// <input> A set P of points in the plane. </input>
        /// <output> A list L containing the vertices of CH(P) in clockwise order. </output>
        public static IEnumerable<Point> Make(IEnumerable<Point> points)
        {
            return UpperHull(points).Union(LowerHull(points));
        }


        /// <summary>
        /// Makes an upper hull out of a specified set of points.
        /// </summary>
        /// <input> A set P of points in the plane. </input>
        /// <output> A list L containing the vertices of the upper hull. </output>
        public static IEnumerable<Point> UpperHull(IEnumerable<Point> points)
        {

            if (points.Count() < 3)
                return points;

            var sortedPoints = points.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
            var upperHull = new List<Point>();

            for (int i = 0; i < sortedPoints.Count; i++)
            {
                var point = sortedPoints[i];
                upperHull.Add(point);

                while (upperHull.Count > 2 && !AreLastThreePointsMakingRightTurn(upperHull))
                {
                    upperHull.Remove(upperHull[upperHull.Count - 2]);
                }
            }

            return upperHull;
        }


        /// <summary>
        /// Makes a lower hull out of a specified set of points.
        /// </summary>
        /// <input> A set P of points in the plane. </input>
        /// <output> A list L containing the vertices of the lower hull. </output>
        public static IEnumerable<Point> LowerHull(IEnumerable<Point> points)
        {
            if (points.Count() < 3)
                return points;

            var sortedPoints = points.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
            var lowerHull = new List<Point>();

            for (int i = sortedPoints.Count - 1; i > -1; i--)
            {
                var point = sortedPoints[i];
                lowerHull.Add(point);

                while (lowerHull.Count > 2 && !AreLastThreePointsMakingRightTurn(lowerHull))
                {
                    lowerHull.Remove(lowerHull[lowerHull.Count - 2]);
                }
            }
            return lowerHull;
        }


        /// <summary>
        /// Determines whether [the last three points of a list making a right turn].
        /// </summary>
        /// <param name="points">The list of points.</param>
        /// <returns>
        ///   <c>true</c> if [the last three points making right turn]; otherwise, <c>false</c>.
        /// </returns>
        private static bool AreLastThreePointsMakingRightTurn(List<Point> points)
        {
            return points.Last().IsPointsLiesToTheRightOfLine(points[points.Count - 2], points[points.Count - 3]);
        }
    }
}
