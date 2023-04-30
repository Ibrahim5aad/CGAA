using CGAA.Visualizer;

namespace CGAA;


internal class Program
{
    [STAThread]
    static void Main(string[] args)
    {

        /// 01 - Convex Hull

        var input = Utils.GetSetOfRandomPoints(1400, 700, 500);

        var convexHull = GrahamScanConvexHull.Make(input);

        CGAAVisualizer.DrawConvexHull(
            input.Select(p => new System.Drawing.Point((int)p.X, (int)p.Y)),
            convexHull.Select(p => new System.Drawing.Point((int)p.X, (int)p.Y)));


        /// 02 - Segments Intersections

        var segments = Utils.GetSetOfRandomSegments(100, 1000, 500);

        var intersections = SegmentsIntersection.FindIntersections(segments);

        CGAAVisualizer.DrawSegments(segments.Select(s => new List<System.Drawing.Point> {
            new System.Drawing.Point((int)s.StartPoint.X, (int)s.StartPoint.Y),
            new System.Drawing.Point((int)s.EndPoint.X, (int)s.EndPoint.Y),
        })
        ,intersections.Keys.Select(p => new System.Drawing.Point((int)p.X, (int)p.Y)));

    }
}