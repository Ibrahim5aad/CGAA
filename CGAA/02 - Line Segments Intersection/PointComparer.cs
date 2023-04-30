namespace CGAA;

public class PointComparer : IComparer<Point>
{
    public int Compare(Point? x, Point? y)
    {
        if (x is null)
            throw new ArgumentNullException(nameof(x));
        if (y is null)
            throw new ArgumentNullException(nameof(y));
        return x.CompareTo(y);
    }
}
