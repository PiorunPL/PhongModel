using MainProject.Domain.Materials;

namespace MainProject.Domain.Basic;

public class Triangle
{
    public Point P1;
    public Point P2;
    public Point P3;

    public Vector
        Normal; //Currently not using, problem with Normal changing, while moving/rotating (camera coordinate system)

    public Material Material;

    public Triangle(Point p1, Point p2, Point p3)
    {
        Random rand = new Random();
        P1 = p1;
        P2 = p2;
        P3 = p3;
        Material = Material.Bronze;
        Normal = GetNormalVector();
    }

    public Triangle(Point p1, Point p2, Point p3, Material material)
    {
        P1 = p1;
        P2 = p2;
        P3 = p3;
        Material = material;
        Normal = GetNormalVector();
    }

    public Vector GetNormalVector()
    {
        Vector v1 = Vector.GetVector(P1.CurrentPosition, P2.CurrentPosition);
        Vector v2 = Vector.GetVector(P1.CurrentPosition, P3.CurrentPosition);
        return Vector.GetCrossProduct(v1, v2);
    }

    public Vector GetNormalisedNormalVector()
    {
        Vector normal = GetNormalVector();
        double vectorLength = Math.Sqrt(Math.Pow(normal.X, 2) + Math.Pow(normal.Y, 2) + Math.Pow(normal.Z, 2));
        if (vectorLength == 0)
        {
            return normal;
        }

        return new Vector(normal.X / vectorLength, normal.Y / vectorLength, normal.Z / vectorLength);
    }

    // result > 0 - behind
    // result = 0 - on plain 
    // result < 0 - in front
    public double CheckPointPosition(Point3D toCheck)
    {
        return Vector.GetDotProduct(GetNormalVector(), Vector.GetVector(P1.CurrentPosition, toCheck));
    }

    public void SwapNormalVector()
    {
        (P2, P3) = (P3, P2);
    }

    public Point3D GetCentralPoint()
    {
        double xc = (P1.CurrentPosition.X + P2.CurrentPosition.X + P3.CurrentPosition.X) / 3;
        double yc = (P1.CurrentPosition.Y + P2.CurrentPosition.Y + P3.CurrentPosition.Y) / 3;
        double zc = (P1.CurrentPosition.Z + P2.CurrentPosition.Z + P3.CurrentPosition.Z) / 3;
        return new Point3D(xc, yc, zc);
    }
}