using NetTopologySuite.Geometries;
using System;
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
        private static Random rnd = new Random();


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
                Points = new PointCollection(convexHull.Select(p => new System.Windows.Point(p.X, p.Y)))
            };

            if (convexHull.First() != convexHull.Last())
                polyline.Points.Add(new System.Windows.Point(convexHull.First().X, convexHull.First().Y));

            visualizer.canvas.Children.Add(polyline);

            foreach (var point in points)
            {
                visualizer.canvas.Children.Add(CreateEllipse(3, 3, point.X, point.Y));
            }

            visualizer.ShowDialog();
        }

        public static void DrawMultiPolygons(IEnumerable<MultiPolygon> multiPolygons)
        {
            CGAAVisualizer visualizer = new();

            foreach (var multiPolygon in multiPolygons)
            {
                foreach (var geometry in multiPolygon)
                {
                    Color randomColor = Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256));

                    if (geometry is NetTopologySuite.Geometries.Polygon polygon)
                    {
                        var pol = new System.Windows.Shapes.Polygon()
                        {
                            Stroke = new SolidColorBrush(randomColor),
                        };
                        foreach (var coordinate in polygon.Coordinates)
                        {
                            pol.Points.Add(new System.Windows.Point(coordinate.X, coordinate.Y));
                        }
                        visualizer.canvas.Children.Add(pol);

                    }

                }

            }

            visualizer.ShowDialog();
        }

        public static void DrawSegments(IEnumerable<IEnumerable<Pnt>> segments, IEnumerable<Pnt> points = null, Color color = default, double thickness = 3)
        {
            if (color == default)
                color = Colors.DarkGreen;

            CGAAVisualizer visualizer = new();

            foreach (var segment in segments)
            {
                Line line = new Line()
                {
                    Stroke = new SolidColorBrush(color),
                    StrokeThickness = thickness,
                    X1 = segment.First().X, 
                    X2 = segment.Last().X,
                    Y1 = segment.First().Y,
                    Y2 = segment.Last().Y,
                };

                visualizer.canvas.Children.Add(line);
            } 

            if(points != null)
            {
                foreach (var point in points)
                {
                    visualizer.canvas.Children.Add(CreateEllipse(10, 10, point.X, point.Y));
                }
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
