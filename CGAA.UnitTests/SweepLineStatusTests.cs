namespace CGAA.UnitTests
{
    public class SweepLineStatusTests
    {

        [Fact]
        public void CanUpdateSweepLineStatus()
        {
            const int sweepLineYCoord = 3;
            var status = new SweepLineStatus();

            var segments = new List<Segment>
            {
                new Segment(new(1, 5), new(10, 2)),
                new Segment(new(2, 5), new(10, 1)),
                new Segment(new(3, 2), new(10, 3)),
                new Segment(new(1, 2), new(9, 4)),
                new Segment(new(7, 1), new(9, 1)),
            };

            status.Update(segments, sweepLineYCoord);

            status.Segments.Should().HaveCount(4);
            status.Segments.ElementAt(0).Should().Be(segments[3]);
            status.Segments.ElementAt(1).Should().Be(segments[1]);
            status.Segments.ElementAt(2).Should().Be(segments[0]);
            status.Segments.ElementAt(3).Should().Be(segments[2]);

            var newSegs = new List<Segment> { new Segment(new(1, 2), new(1, 4)) };
            status.Update(newSegs, sweepLineYCoord);

            status.Segments.Should().HaveCount(5);
            status.Segments.ElementAt(0).Should().Be(newSegs[0]);
            status.Segments.ElementAt(1).Should().Be(segments[3]);
            status.Segments.ElementAt(2).Should().Be(segments[1]);
            status.Segments.ElementAt(3).Should().Be(segments[0]);
            status.Segments.ElementAt(4).Should().Be(segments[2]);
        }

        [Fact]
        public void CanRemove()
        {
            const int sweepLineYCoord = 3;
            var status = new SweepLineStatus();

            var segments = new List<Segment>
            {
                new Segment(new(1, 5), new(2, 1)),
                new Segment(new(2, 5), new(3, 2)),
                new Segment(new(3, 4), new(4, 3)),
                new Segment(new(5, 5), new(4, 3)),
                new Segment(new(6, 5), new(4, 3)),
                new Segment(new(7, 1), new(6, 6)),
                new Segment(new(10, 1), new(7, 6)),
            };

            status.Update(segments, sweepLineYCoord);

            var (lowerBound, upperBound) = status.Remove(segments.GetRange(2, 3));

            status.Segments.Should().HaveCount(4);
            lowerBound.Should().Be(segments.ElementAt(1));
            upperBound.Should().Be(segments.ElementAt(5));
        }
    }
}
