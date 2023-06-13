namespace MainProject.Domain.Basic;

public class Vector
{
    public double X;
    public double Y;
    public double Z;

    public Vector() {
        
    }

    public Vector(double x, double y, double z) {
        X = x;
        Y = y;
        Z = z;
    }

    public void SetLength(double targetLength)
    {
        double length = GetLength();
        var normalized = GetNormalized();
        X = normalized.X * targetLength;
        Y = normalized.Y * targetLength;
        Z = normalized.Z * targetLength;
    }

    public Vector GetNormalized()
    {
        double length = GetLength();
        return new Vector(X / length, Y / length, Z / length);
    }
    
    
    public double GetLength()
    {
        return Math.Sqrt(X * X + Y * Y + Z * Z);
    }
    public static double GetDotProduct(Vector v1, Vector v2)
    {
        return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
    }

    public static Vector GetCrossProduct(Vector v1, Vector v2)
    {
        Vector result = new Vector();
        result.X = v1.Y * v2.Z - v1.Z * v2.Y;
        result.Y = v1.Z * v2.X - v1.X * v2.Z;
        result.Z = v1.X * v2.Y - v1.Y * v2.X;
        return result;
    }

    public static Vector GetVector(Point3D p1, Point3D p2)
    {
        Vector result = new Vector();
        result.X = p2.X - p1.X;
        result.Y = p2.Y - p1.Y;
        result.Z = p2.Z - p1.Z;
        return result;
    }

    public override string ToString()
    {
        return String.Format("[X: {0}; Y: {1}; Z: {2}]", X, Y, Z);
    }

    public void MultipleByScalar(double factor)
    {
        X = X * factor;
        Y = Y * factor;
        Z = Z * factor;
    }
}