using System.Diagnostics.CodeAnalysis;

namespace CGAA
{
    public class Event
    {
        private readonly Point _point;


        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="point">The point.</param>
        public Event([NotNull] Point point)
        {
            _point = point;
            Lp = new List<Point>();
            Up = new List<Point>();
            Cp = new List<Point>();

        }

        public List<Point> Lp { get; }
        public List<Point> Up { get; }
        public List<Point> Cp { get; }
    }
}
