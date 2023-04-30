namespace CGAA;


public static class SegmentsIntersection
{

    public static Dictionary<Point, List<Segment>> FindIntersections(IEnumerable<Segment> segments)
    {
        var eventPointsQueue = InitEventPointsQueue(segments);
        var sweepLineStatus = new SweepLineStatus();
        var result = new Dictionary<Point, List<Segment>>();

        while (eventPointsQueue.Points.Count > 0)
        {
            var eventPoint = eventPointsQueue.Dequeue();
            if (eventPoint is not null)
            {
                HandleEvent(eventPoint, eventPointsQueue, sweepLineStatus, ref result);
            }

        }
        return result;
    }

    private static void HandleEvent(Point point, EventsQueue eventsQueue, SweepLineStatus sweepLineStatus, ref Dictionary<Point, List<Segment>> output)
    {
        eventsQueue.WhereLower.TryGetValue(point, out List<Segment>? Lp);     // L(p)
        eventsQueue.WhereUpper.TryGetValue(point, out List<Segment>? Up);     // U(p)
        eventsQueue.WhereInside.TryGetValue(point, out List<Segment>? Cp); ;  // C(p)

        Lp ??= new List<Segment>();
        Up ??= new List<Segment>();
        Cp ??= new List<Segment>();

        var uAndCSegments = Up.Union(Cp).ToList();  // U(p) U C(p)
        var lAndCSegments = Lp.Union(Cp).ToList();  // L(p) U C(p)

        var allSegments = Lp.Union(uAndCSegments);  // L(p) U U(p) U C(p)

        if (allSegments.Count() > 1)
        {
            // report point as intersection point, with the L(p), U(p), and C(p) segments
            if(!output.TryAdd(point, allSegments.ToList()))
            {
                // processed
                return;
            }
        }

        // remove L(p) U C(p) from the status structure
        var (lowerBound, upperBound) = sweepLineStatus.Remove(lAndCSegments);

        if (uAndCSegments.Any())
        {
            // add U(p) U C(p) to the status structure slightly below the y coord
            sweepLineStatus.Update(uAndCSegments, point.Y - 0.001);
            var ordered = new SortedSet<Segment>(uAndCSegments, new SegmentComparer(point.Y - 0.001));
            var sleft = ordered.First();
            var sright = ordered.Last();
            var bleft = sweepLineStatus.GetPrevNeighbour(sleft);
            var bright = sweepLineStatus.GetNextNeighbour(sright);
            if(bleft != null)
                FindNewEvent(eventsQueue, sleft, bleft, point);
            if (bright != null)
                FindNewEvent(eventsQueue, sright, bright, point);

        }
        else
        {
            if (lowerBound != null && upperBound != null)
            {
                FindNewEvent(eventsQueue, lowerBound, upperBound, point);
            }

        }


    }

    private static void FindNewEvent(EventsQueue eventsQueue, Segment lowerBound, Segment upperBound, Point point)
    {
        if (IsIntersecting(lowerBound, upperBound, out Point? intersection) && intersection is not null)
        {
            if ((intersection.Y < point.Y) || intersection.X > point.X)
            {
                if (!eventsQueue.Points.Contains(intersection))
                {
                    eventsQueue.Points.Add(intersection);
                    bool isOnSegment(Point q, Segment segment)
                    {
                        Point p = segment.StartPoint;
                        Point r = segment.EndPoint;
                        if (q.X < Math.Max(p.X, r.X) && q.X > Math.Min(p.X, r.X) &&
                            q.Y < Math.Max(p.Y, r.Y) && q.Y > Math.Min(p.Y, r.Y))
                            return true;

                        return false;
                    }
                    if (isOnSegment(intersection, upperBound))
                        eventsQueue.AddInside(intersection, upperBound);
                    if (isOnSegment(intersection, lowerBound))
                        eventsQueue.AddInside(intersection, lowerBound);
                }
            }
        }
    }

    private static bool IsIntersecting(Segment s1, Segment s2, out Point? intersection)
    {
        Point p1 = s1.StartPoint, q1 = s1.EndPoint, p2 = s2.StartPoint, q2 = s2.EndPoint;
        intersection = null;

        // Find the four orientations needed for general and
        // special cases
        int o1 = GetOrientation(p1, q1, p2);
        int o2 = GetOrientation(p1, q1, q2);
        int o3 = GetOrientation(p2, q2, p1);
        int o4 = GetOrientation(p2, q2, q1);

        // General case
        if (o1 != o2 && o3 != o4)
        {
            try
            {
                intersection = FindIntersection(s1, s2);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Special Cases
        // p1, q1 and p2 are collinear and p2 lies on segment p1q1
        if (o1 == 0 && IsOnSegment(p1, p2, q1))
        {
            intersection = p2;
            return true;
        }

        // p1, q1 and q2 are collinear and q2 lies on segment p1q1
        if (o2 == 0 && IsOnSegment(p1, q2, q1))
        {
            intersection = q2;
            return true;
        }

        // p2, q2 and p1 are collinear and p1 lies on segment p2q2
        if (o3 == 0 && IsOnSegment(p2, p1, q2))
        {
            intersection = p1;
            return true;
        }

        // p2, q2 and q1 are collinear and q1 lies on segment p2q2
        if (o4 == 0 && IsOnSegment(p2, q1, q2))
        {
            intersection = q1;
            return true;
        }

        return false; // Doesn't fall in any of the above cases
    }

    public static Point? FindIntersection(Segment segment1, Segment segment2, double tolerance = 0.001)
    {
        double x1 = segment1.StartPoint.X, y1 = segment1.StartPoint.Y;
        double x2 = segment1.EndPoint.X, y2 = segment1.EndPoint.Y;

        double x3 = segment2.StartPoint.X, y3 = segment2.StartPoint.Y;
        double x4 = segment2.EndPoint.X, y4 = segment2.EndPoint.Y;

        // equations of the form x=c (two vertical lines) with overlapping
        if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance && Math.Abs(x1 - x3) < tolerance)
        {
            throw new Exception("Both lines overlap vertically, ambiguous intersection points.");
        }

        //equations of the form y=c (two horizontal lines) with overlapping
        if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance && Math.Abs(y1 - y3) < tolerance)
        {
            throw new Exception("Both lines overlap horizontally, ambiguous intersection points.");
        }

        //equations of the form x=c (two vertical parallel lines)
        if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance)
        {
            //return default (no intersection)
            return default(Point);
        }

        //equations of the form y=c (two horizontal parallel lines)
        if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance)
        {
            //return default (no intersection)
            return default(Point);
        }

        //general equation of line is y = mx + c where m is the slope
        //assume equation of line 1 as y1 = m1x1 + c1 
        //=> -m1x1 + y1 = c1 ----(1)
        //assume equation of line 2 as y2 = m2x2 + c2
        //=> -m2x2 + y2 = c2 -----(2)
        //if line 1 and 2 intersect then x1=x2=x & y1=y2=y where (x,y) is the intersection point
        //so we will get below two equations 
        //-m1x + y = c1 --------(3)
        //-m2x + y = c2 --------(4)

        double x, y;

        //lineA is vertical x1 = x2
        //slope will be infinity
        //so lets derive another solution
        if (Math.Abs(x1 - x2) < tolerance)
        {
            //compute slope of line 2 (m2) and c2
            double m2 = (y4 - y3) / (x4 - x3);
            double c2 = -m2 * x3 + y3;

            //equation of vertical line is x = c
            //if line 1 and 2 intersect then x1=c1=x
            //subsitute x=x1 in (4) => -m2x1 + y = c2
            // => y = c2 + m2x1 
            x = x1;
            y = c2 + m2 * x1;
        }
        //lineB is vertical x3 = x4
        //slope will be infinity
        //so lets derive another solution
        else if (Math.Abs(x3 - x4) < tolerance)
        {
            //compute slope of line 1 (m1) and c2
            double m1 = (y2 - y1) / (x2 - x1);
            double c1 = -m1 * x1 + y1;

            //equation of vertical line is x = c
            //if line 1 and 2 intersect then x3=c3=x
            //subsitute x=x3 in (3) => -m1x3 + y = c1
            // => y = c1 + m1x3 
            x = x3;
            y = c1 + m1 * x3;
        }
        //lineA & lineB are not vertical 
        //(could be horizontal we can handle it with slope = 0)
        else
        {
            //compute slope of line 1 (m1) and c2
            double m1 = (y2 - y1) / (x2 - x1);
            double c1 = -m1 * x1 + y1;

            //compute slope of line 2 (m2) and c2
            double m2 = (y4 - y3) / (x4 - x3);
            double c2 = -m2 * x3 + y3;

            //solving equations (3) & (4) => x = (c1-c2)/(m2-m1)
            //plugging x value in equation (4) => y = c2 + m2 * x
            x = (c1 - c2) / (m2 - m1);
            y = c2 + m2 * x;

            //verify by plugging intersection point (x, y)
            //in orginal equations (1) & (2) to see if they intersect
            //otherwise x,y values will not be finite and will fail this check
            if (!(Math.Abs(-m1 * x + y - c1) < tolerance
                && Math.Abs(-m2 * x + y - c2) < tolerance))
            {
                //return default (no intersection)
                return default(Point);
            }
        }

        //x,y can intersect outside the line segment since line is infinitely long
        //so finally check if x, y is within both the line segments
        if (IsInsideSegment(segment1, x, y) &&
            IsInsideSegment(segment2, x, y))
        {
            return new Point(x, y);
        }

        //return default (no intersection)
        return default(Point);

    }

    private static bool IsInsideSegment(Segment line, double x, double y)
    {
        return (x >= line.StartPoint.X && x <= line.EndPoint.X
                    || x >= line.EndPoint.X && x <= line.StartPoint.X)
               && (y >= line.StartPoint.Y && y <= line.EndPoint.Y
                    || y >= line.EndPoint.Y && y <= line.StartPoint.Y);
    }

    /// <summary>
    /// To find orientation of ordered triplet (p, q, r).
    /// The function returns following values
    /// 0 --> p, q and r are collinear
    /// 1 --> Clockwise
    /// 2 --> Counterclockwise
    /// </summary> 
    private static int GetOrientation(Point p, Point q, Point r)
    {
        // See https://www.geeksforgeeks.org/orientation-3-ordered-points/
        // for details of below formula.
        var val = (q.Y - p.Y) * (r.X - q.X) -
                  (q.X - p.X) * (r.Y - q.Y);

        if (val == 0) return 0;  // collinear

        return (val > 0) ? 1 : 2; // clock or counterclock wise
    }

    private static bool IsOnSegment(Point p, Point q, Point r)
    {
        if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
            q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
            return true;

        return false;
    }

    private static EventsQueue InitEventPointsQueue(IEnumerable<Segment> segments)
    {
        EventsQueue queue = new();

        foreach (Segment segment in segments)
        {
            if (!queue.Points.Contains(segment.StartPoint))
                queue.Points.Add(segment.StartPoint);
            if (!queue.Points.Contains(segment.EndPoint))
                queue.Points.Add(segment.EndPoint);

            if (segment.StartPoint > segment.EndPoint)
            {
                queue.AddUpper(segment.StartPoint, segment);
                queue.AddLower(segment.EndPoint, segment);
            }
            else
            {
                queue.AddUpper(segment.EndPoint, segment);
                queue.AddLower(segment.StartPoint, segment);
            }

        }

        return queue;
    }

}
