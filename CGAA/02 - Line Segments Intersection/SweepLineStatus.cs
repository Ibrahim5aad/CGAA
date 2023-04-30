namespace CGAA;

public class SweepLineStatus
{
    private SortedSet<Segment>? _segments = new();


    public SortedSet<Segment>? Segments { get => _segments; }


    public void Update(IEnumerable<Segment> segments, double sweepLineYCoord)
    {
        var statusStructure = new SortedSet<Segment>
            (new SegmentComparer(sweepLineYCoord));

        foreach (var seg in segments)
        {
            if (IsOverlapping(seg, sweepLineYCoord))
                statusStructure.Add(seg);
        }

        foreach (var seg in _segments)
        {
            if (IsOverlapping(seg, sweepLineYCoord))
                statusStructure.Add(seg);
        }

        _segments = statusStructure;
    }


    public (Segment? lowerBound, Segment? upperBound) Remove(IEnumerable<Segment> segments)
    {
        var l = _segments?.ToList();
        Segment? lowerBound = null;
        Segment? upperBound = null;

        if (!segments.Any())
            return (lowerBound, upperBound);

        var indices = segments.Select(s => l.IndexOf(s)).Where(i => i != -1);
        if (indices.Any())
        {
            var min = indices.Min() - 1;
            var max = indices.Max() + 1;
            lowerBound = min >= 0 ? l[min] : null;
            upperBound = max <= l.Count - 1 ? l[max] : null;
        }

        foreach (var segment in segments)
        {
            _segments?.Remove(segment);
        }

        return (lowerBound, upperBound);

    }

    public Segment? GetPrevNeighbour(Segment segment)
    {
        var l = _segments?.ToList();
        var i = l.IndexOf(segment) - 1;

        if (i < 0) return null;

        return l[i];
    }

    public Segment? GetNextNeighbour(Segment segment)
    {
        var l = _segments?.ToList();
        var i = l.IndexOf(segment) + 1;

        if (i > l.Count - 1) return null;

        return l[i];
    }

    public void Remove(Segment segment)
    {
        _segments?.Remove(segment);
    }

    private bool IsOverlapping(Segment segment, double sweepLineYCoord)
    {
        var ys = new List<double>() { segment.StartPoint.Y, segment.EndPoint.Y };
        var miny = ys.Min();
        var maxy = ys.Max();

        return sweepLineYCoord >= miny && sweepLineYCoord <= maxy;
    }
}
