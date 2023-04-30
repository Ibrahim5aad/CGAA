
namespace CGAA.UnitTests;


public class PointTests
{
    [Fact]
    public void CanDetectPointBiggerThanAnother()
    {
        Point p1 = new(1, 1);
        Point p2 = new(1, 2);

        var isP1BiggerThanP2 = p1 > p2;

        isP1BiggerThanP2.Should().BeFalse();
    }


    [Fact]
    public void CanDetectPointSmallerThanAnother()
    {
        Point p1 = new(1.1, 1);
        Point p2 = new(1, 1);

        var isP1SmallerThanP2 = p1 < p2;

        isP1SmallerThanP2.Should().BeTrue();
    }


    [Fact]
    public void CanOrderPoints()
    {
        Point p1 = new(1, 1);
        Point p2 = new(1, 2);
        Point p3 = new(1, 1.2);
        Point p4 = new(1.1, 1.1); // should appear before p5 in the sorted set
        Point p5 = new(1, 1.1);

        var points = new SortedSet<Point>(new PointComparer())
        {
            p1, p2, p3, p4, p5
        };

        points.ElementAt(0).Should().Be(p1);
        points.ElementAt(1).Should().Be(p4);
        points.ElementAt(2).Should().Be(p5);
        points.ElementAt(3).Should().Be(p3);
        points.ElementAt(4).Should().Be(p2);
    }
}
