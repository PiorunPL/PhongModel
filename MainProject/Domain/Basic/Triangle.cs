using MainProject.Domain.Materials;
using SkiaSharp;

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
    
    public Vector GetNormalVectorFromPoint(Point3D point)
    {
        Vector v1 = Vector.GetVector(point, P2.CurrentPosition);
        Vector v2 = Vector.GetVector(point, P3.CurrentPosition);
        return Vector.GetCrossProduct(v1, v2);
    }
    
    public Vector GetNormalisedNormalVectorFromPoint(Point3D point)
    {
        Vector normal = GetNormalVectorFromPoint(point);
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

     public List<int[]> GetAllPixelsInTriangle(int width, int height, double d, double z)
     {
         // SKBitmap bitmap = new SKBitmap(targetWidth, targetHeight, true);
         // SKCanvas canvas = new SKCanvas(bitmap);
         List<int[]> trianglePixels = new List<int[]>();
         var centralPointTriangle = GetCentralPoint();
         // if (centralPointTriangle.Z > z)
         // {
         //     return null;
         // }
         (int x1, int y1) = P1.getPointCoordinatesBitmap(width, height, d);
         (int x2, int y2) = P2.getPointCoordinatesBitmap(width, height, d);
         (int x3, int y3) = P3.getPointCoordinatesBitmap(width, height, d);
         var path = new SKPath { FillType = SKPathFillType.EvenOdd };
         path.MoveTo(x1,y1);
         path.LineTo(x2,y2);
         path.LineTo(x3,y3);
         path.LineTo(x1,y1);
         path.Close();
         
         var minX = new [] { x1, x2, x3 }.Min();
         var minY= new [] { y1, y2, y3 }.Min();
         var maxX = new [] { x1, x2, x3 }.Max();
         var maxY = new [] { y1, y2, y3 }.Max();
         for (int i = minX; i <= maxX; i++)
         {
             for (int j = minY; j <= maxY; j++)
             {
                 if (path.Contains(i, j))
                 {
                     trianglePixels.Add(new[]{i, j});
                 }
             }
         }

         return trianglePixels;
     }

     public double[] GetPlaneOfTriangle()
     {
         Vector ab = new Vector(P2.CurrentPosition.X - P1.CurrentPosition.X, P2.CurrentPosition.Y - P1.CurrentPosition.Y, P2.CurrentPosition.Z - P1.CurrentPosition.Z);
         Vector ac = new Vector(P3.CurrentPosition.X - P1.CurrentPosition.X, P3.CurrentPosition.Y - P1.CurrentPosition.Y, P3.CurrentPosition.Z - P1.CurrentPosition.Z);
         Vector abXac = new Vector(ab.Y*ac.Z - ab.Z*ac.Y, -(ab.X*ac.Z - ab.Z*ac.X), ab.X*ac.Y - ab.Y*ac.X);
         double d = -abXac.X * P1.CurrentPosition.X - abXac.Y * P1.CurrentPosition.Y - abXac.Z * P1.CurrentPosition.Z;
         return new[] { abXac.X, abXac.Y, abXac.Z, d };
     }
}