using CGAA.Extensions;
using System.Drawing;

namespace CGAA
{
    public class ConvexHull
    {

        public static IEnumerable<Point> Make(IEnumerable<Point> points)
        {
            return UpperHull(points).Union(LowerHull(points));
        }

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

                while (upperHull.Count > 2 && !IsLastThreePointsMakingRightTurn(upperHull))
                {
                    upperHull.Remove(upperHull[upperHull.Count - 2]);
                }
            }

            return upperHull;
        }


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

                while (lowerHull.Count > 2 && !IsLastThreePointsMakingRightTurn(lowerHull))
                {
                    lowerHull.Remove(lowerHull[lowerHull.Count - 2]);
                }
            }
            return lowerHull;
        }


        private static bool IsLastThreePointsMakingRightTurn(List<Point> points)
        {
            return points.Last().IsPointsLiesToTheRightOfLine(points[points.Count - 2], points[points.Count - 3]);
        }
    }
}
