namespace CGAA;

public class Point : IEquatable<Point>, IComparable<Point>
{
    private readonly double _x;
    private readonly double _y;

    public Point(double x, double y)
    {
        _x = x;
        _y = y;
    }

    public double X => _x;
    public double Y => _y;

    public static bool operator <(Point p, Point q)
    {
        // if y-monotone points then right most point is considered lower/smaller
        return (p._y < q._y) || ((p._y == q._y) && p._x > q._x);
    }
    public static bool operator >(Point p, Point q)
    {
        return (p._y > q._y) || ((p._y == q._y) && p._x < q._x);

    }
    public static bool operator ==(Point p, Point q)
    {
        if (p is null || q is null) return false;
        return (p._x == q._x) && (p._y == q._y);
    }
    public static bool operator !=(Point p, Point q)
    {
        if (p is null || q is null) return true;
        return (p._x != q._x) || (p._y != q._y);
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

        if (obj is Point point)
        {
            return this == point;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this._x, this._y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public bool Equals(Point? other)
    {
        return Equals(other as object);
    }

    public int CompareTo(Point? other)
    {
        if (this == other)
        {
            return 0;
        }
        else if (this < other)
        {
            return -1;
        }
        else if (this > other)
        {
            return 1;
        }

        throw new ArithmeticException();
    }
}
