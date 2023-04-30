namespace CGAA;

public class EventsQueue
{
    public EventsQueue()
    {
        Points = new SortedSet<Point>(new PointComparer());
        WhereLower = new Dictionary<Point, List<Segment>>();
        WhereUpper = new Dictionary<Point, List<Segment>>();
        WhereInside = new Dictionary<Point, List<Segment>>();
    }

    public SortedSet<Point> Points { get; }

    public Dictionary<Point, List<Segment>> WhereLower { get; }

    public Dictionary<Point, List<Segment>> WhereUpper { get; }

    public Dictionary<Point, List<Segment>> WhereInside { get; }


    public void AddLower(Point point, Segment segment)
    {
        if (WhereLower.ContainsKey(point))
            WhereLower[point].Add(segment);
        else
            WhereLower.Add(point, new List<Segment>() { segment });
    }
    public void AddUpper(Point point, Segment segment)
    {
        if (WhereUpper.ContainsKey(point))
            WhereUpper[point].Add(segment);
        else
            WhereUpper.Add(point, new List<Segment>() { segment });
    }
    public void AddInside(Point point, Segment segment)
    {
        if (WhereInside.ContainsKey(point))
            WhereInside[point].Add(segment);
        else
            WhereInside.Add(point, new List<Segment>() { segment });
    }

    public Point? Dequeue()
    {
        var p = Points.LastOrDefault();
        if (p is not null) Points.Remove(p);
        return p;
    }
}
