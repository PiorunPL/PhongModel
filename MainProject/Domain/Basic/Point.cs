using Accord.Math;

namespace MainProject.Domain.Basic;

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

    public Point(Point3D p)
    {
        CurrentPosition = new Point3D(p);
        OriginalPosition = new Point3D(p);
    }

    public Point(Point3D originalPosition, Point3D currentPosition)
    {
        CurrentPosition = currentPosition;
        OriginalPosition = originalPosition;
    }

    public Point2D GetViewPortCoordinates(double d)
    {
        var tempZ = CurrentPosition.Z;
        if (tempZ == 0)
            tempZ = 0.0001;
        if (tempZ < 0)
            d = -d * 1 ; // ???
        double viewPortX = (CurrentPosition.X * d) / tempZ;
        double viewPortY = (CurrentPosition.Y * d) / tempZ;

        while (viewPortX > Int16.MaxValue || viewPortY > Int16.MaxValue || viewPortX < Int16.MinValue ||
            viewPortY < Int16.MinValue)
        {
            viewPortX = viewPortX / 10;
            viewPortY = viewPortY / 10;
        }
        
        return new Point2D(){X = viewPortX, Y = viewPortY};
    }
    
    public (int x, int y) getPointCoordinatesBitmap(int width, int height, double d)
    {
        var pointTemp =GetViewPortCoordinates(d);
        var tempX = pointTemp.X;
        var tempY = pointTemp.Y;
        
        var widthHelp = width / 2;
        var heightHelp = height / 2;

        var x_draw = (int)(tempX * 50 + widthHelp);
        var y_draw = (int)(tempY * (-50) + heightHelp);

        return (x_draw, y_draw);
    }

    public Vector4 GetVector()
    {
        Vector4 vector = new Vector4((float)CurrentPosition.X, (float)CurrentPosition.Y, (float)CurrentPosition.Z,1);
        return vector;
    }

    public void LoadCoordinatesFromVector(Vector4 vector)
    {
        CurrentPosition.X = vector.X;
        CurrentPosition.Y = vector.Y;
        CurrentPosition.Z = vector.Z;
    }

    public override string ToString()
    {
        return String.Format("Point [{0}]", CurrentPosition.ToString());
    }
    
}