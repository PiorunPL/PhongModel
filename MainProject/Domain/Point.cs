namespace MainProject;

public class Point
{
    public Point3D CurrentPosition;
    public Point3D OriginalPosition;

    public List<Point> ConnectedPoints = new List<Point>();

    public Point(double x, double y, double z)
    {
        CurrentPosition = new Point3D(x,y,z);
        OriginalPosition = new Point3D(x,y,z);
    }

    public Point2D GetViewPortCoordinates(double d)
    {
        double viewPortX = (CurrentPosition.X * d) / CurrentPosition.Z;
        double viewPortY = (CurrentPosition.Y * d) / CurrentPosition.Z;

        return new Point2D(){X = viewPortX, Y = viewPortY};
    }
    
}