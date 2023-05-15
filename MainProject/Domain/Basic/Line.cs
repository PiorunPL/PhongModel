namespace MainProject.Domain.Basic;

public class Line
{
    public Point Point1;
    public Point Point2;

    public Line(Point point1, Point point2)
    {
        Point1 = point1;
        Point2 = point2;
    }
}