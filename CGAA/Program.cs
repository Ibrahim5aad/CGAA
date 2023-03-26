using CGAA.Visualizer;

namespace CGAA
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            /// 01 - Conex Hull

            var input = Utils.GetSetOfRandomPoints(1400, 700, 500);

            var convexHull = ConvexHull.Make(input);

            CGAAVisualizer.DrawConvexHull(input, convexHull);

        }
    }
}