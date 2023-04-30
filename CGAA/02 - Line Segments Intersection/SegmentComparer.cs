namespace CGAA;

public class SegmentComparer : IComparer<Segment>
{
    private readonly double _yCoord;

    public SegmentComparer(double yCoord)
    {
        _yCoord = yCoord;
    }

    public int Compare(Segment? seg1, Segment? seg2)
    {
        if (seg1 is null)
            throw new ArgumentNullException(nameof(seg1));
        if (seg2 is null)
            throw new ArgumentNullException(nameof(seg2));

        var xval1 = GetXVal(seg1, _yCoord);
        var xval2 = GetXVal(seg2, _yCoord);

        if (xval1 == xval2)
        {
            if (seg1.StartPoint.X == seg2.StartPoint.X)
            {
                return 0;
            }
            else if (seg1.StartPoint.X < seg2.StartPoint.X)
            {
                return -1;
            }
            else if (seg1.StartPoint.X > seg2.StartPoint.X)
            {
                return 1;
            }
        }
        else if (xval1 < xval2)
        {
            return -1;
        }
        else if (xval1 > xval2)
        {
            return 1;
        }

        throw new ArithmeticException();
    }

    private double GetXVal(Segment x, double yCoord)
    {
        if (yCoord == x.StartPoint.Y)
            return x.StartPoint.X;

        if (yCoord == x.EndPoint.Y)
            return x.EndPoint.X;

        if (x.EndPoint.X != x.StartPoint.X)
        {
            var m = (x.EndPoint.Y - x.StartPoint.Y) / (x.EndPoint.X - x.StartPoint.X);
            var c = x.StartPoint.Y - (m * x.StartPoint.X);
            return (yCoord - c) / m;
        }

        // vertical line
        return x.EndPoint.X;
    }
}