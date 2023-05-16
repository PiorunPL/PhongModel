namespace MainProject.Domain.Basic;

public class Triangle
{
    public Point P1;
    public Point P2;
    public Point P3;
    public Vector Normal; //Currently not using, problem with Normal changing, while moving/rotating (camera coordinate system)
    public int[] color;
    public string position;
    
    public Triangle(Point p1, Point p2, Point p3)
    {
        P1 = p1;
        P2 = p2;
        P3 = p3;
        this.color = new int[]{255, 0, 0};
        Normal = GetNormalVector();
    }

    public Triangle(Point p1, Point p2, Point p3, int[] color, string position)
    {
        P1 = p1;
        P2 = p2;
        P3 = p3;
        this.color = color;
        this.position = position;
        Normal = GetNormalVector();
    }

    public Vector GetNormalVector()
    {
        Vector v1 = Vector.GetVector(P1.CurrentPosition, P2.CurrentPosition);
        Vector v2 = Vector.GetVector(P1.CurrentPosition, P3.CurrentPosition);
        return Vector.GetCrossProduct(v1,v2);
    }

    public Vector GetNormalisedNormalVector()
    {
        Vector normal = GetNormalVector();
        double vectorLength = Math.Sqrt(Math.Pow(normal.X, 2) + Math.Pow(normal.Y, 2) + Math.Pow(normal.Z, 2));
        if(vectorLength == 0) {
            return normal;
        }
        return new Vector(normal.X/vectorLength, normal.Y/vectorLength, normal.Z/vectorLength);
    }
    
    // result > 0 - behind
    // result = 0 - on plain 
    // result < 0 - in front
    public double CheckPointPosition(Point3D toCheck)
    {
        return Vector.GetDotProduct(GetNormalisedNormalVector(), Vector.GetVector(P1.CurrentPosition, toCheck));
    }

    public override string ToString()
    {
        // return String.Format("Triangle [P1: {0}; P2: {1}; P3: {2}]", P1, P2, P3);
        return position;
    }
}