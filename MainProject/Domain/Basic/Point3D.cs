namespace MainProject.Domain.Basic;

public class Point3D
{
    public double X, Y, Z;
    public Point3D(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Point3D(Point3D p)
    {
        X = p.X;
        Y = p.Y;
        Z = p.Z;
    }

    public override string ToString()
    {
        return String.Format("Point3D [X: {0}; Y: {1}; Z: {2}]", X, Y, Z);
    }
}