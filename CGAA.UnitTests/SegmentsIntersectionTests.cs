namespace CGAA.UnitTests
{
    public class SegmentsIntersectionTests
    {

        [Fact]
        public void CanDetectIntersections()
        {
            var segments = new List<Segment>
            {
                new Segment(new(1, 5), new(2, 1)),
                new Segment(new(2, 5), new(3, 2)),
                new Segment(new(3, 4), new(4, 3)),
                new Segment(new(5, 5), new(4, 3)),
                new Segment(new(6, 5), new(4, 3)),
                new Segment(new(7, 1), new(6, 6)),
                new Segment(new(10, 1), new(7, 6)),
                new Segment(new(4.1, 4), new(6, 2)),
            };

            var intersections = SegmentsIntersection.FindIntersections(segments);

            intersections.Should().HaveCount(3);
        }


    }
}
