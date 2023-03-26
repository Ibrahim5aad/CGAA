using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Pnt = System.Drawing.Point;


namespace CGAA.Visualizer
{
    /// <summary>
    /// Interaction logic for CGAAVisualizer.xaml
    /// </summary>
    public partial class CGAAVisualizer : Window
    {

        public CGAAVisualizer()
        {
            InitializeComponent();
        }

        public static void DrawConvexHull(IEnumerable<Pnt> points, IEnumerable<Pnt> convexHull, Color color = default, double thickness = 3)
        {
            if (color == default)
                color = Colors.DarkGreen;

            CGAAVisualizer visualizer = new();

            Polyline polyline = new()
            {
                Stroke = new SolidColorBrush(color),
                StrokeThickness = thickness,
                Points = new PointCollection(convexHull.Select(p => new Point(p.X, p.Y)))
            };

            if (convexHull.First() != convexHull.Last())
                polyline.Points.Add(new Point(convexHull.First().X, convexHull.First().Y));

            visualizer.canvas.Children.Add(polyline);

            foreach (var point in points)
            {
                visualizer.canvas.Children.Add(CreateEllipse(3, 3, point.X, point.Y));
            }

            visualizer.ShowDialog();
        }


        static Ellipse CreateEllipse(double width, double height, double desiredCenterX, double desiredCenterY)
        {
            double left = desiredCenterX - (width / 2);
            double top = desiredCenterY - (height / 2);
            Ellipse ellipse = new()
            {
                Width = width,
                Height = height,
                Stroke = new SolidColorBrush(Colors.Red),
                Margin = new Thickness(left, top, 0, 0)
            };
            return ellipse;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            // OnClosed logic
        }
    }
}
