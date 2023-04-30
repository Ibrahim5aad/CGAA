namespace CGAA
{

    public class Segment
    {
        private Point _startPoint;
        private Point _endPoint;

        public Segment(Point startPoint, Point endPoint)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
        }

        public Point StartPoint { get => _startPoint; }
        public Point EndPoint { get => _endPoint; }

        public override string ToString()
        {
            return $"(Startpoint: {StartPoint.X}, {StartPoint.Y}) | Endpoint: {EndPoint.X}, {EndPoint.Y})";
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (obj is Segment other)
            {
                return this.StartPoint == other.StartPoint && this.EndPoint == other.EndPoint;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartPoint, EndPoint);
        }
    }
}
