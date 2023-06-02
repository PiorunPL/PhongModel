using MainProject.Domain.Basic;

namespace MainProject.Domain.WorldRelated;

public class Sphere
{
    public List<Triangle> Triangles = new List<Triangle>();
    public List<Point> Points = new List<Point>();
    public Point Center;

    public int[] Color;

    public Sphere(Point center, int levelOfTeselation, double size, double epsilon, int[] color)
    {
        Center = center;
        Points.Add(center);
        Color = color;
        GenerateSphere(levelOfTeselation, size, epsilon);
    }

    private void GenerateSphere(int levelOfTeselation, double size, double epsilon)
    {
        var t = (1.0 + Math.Sqrt(5.0)) / 2.0;
        
        Point p0 = new Point(Center.CurrentPosition.X  -1*size,Center.CurrentPosition.Y + t*size, Center.CurrentPosition.Z +0);
        Point p1 = new Point(Center.CurrentPosition.X +1*size, Center.CurrentPosition.Y +t*size, Center.CurrentPosition.Z +0);
        Point p2 = new Point(Center.CurrentPosition.X -1*size, Center.CurrentPosition.Y -t*size, Center.CurrentPosition.Z +0);
        Point p3 = new Point(Center.CurrentPosition.X +1*size, Center.CurrentPosition.Y -t*size, Center.CurrentPosition.Z +0);
        
        Point p4 = new Point(Center.CurrentPosition.X +0*size,Center.CurrentPosition.Y -1*size,Center.CurrentPosition.Z +t*size);
        Point p5 = new Point(Center.CurrentPosition.X +0*size,Center.CurrentPosition.Y +1*size,Center.CurrentPosition.Z +t*size);
        Point p6 = new Point(Center.CurrentPosition.X +0*size,Center.CurrentPosition.Y -1*size,Center.CurrentPosition.Z -t*size);
        Point p7 = new Point(Center.CurrentPosition.X +0*size,Center.CurrentPosition.Y +1*size,Center.CurrentPosition.Z -t*size);
        
        Point p8 = new Point(Center.CurrentPosition.X +t*size,Center.CurrentPosition.Y +0,Center.CurrentPosition.Z -1*size);
        Point p9 = new Point(Center.CurrentPosition.X +t*size,Center.CurrentPosition.Y +0,Center.CurrentPosition.Z +1*size);
        Point p10 = new Point(Center.CurrentPosition.X -t*size,Center.CurrentPosition.Y +0,Center.CurrentPosition.Z -1*size);
        Point p11 = new Point(Center.CurrentPosition.X -t*size,Center.CurrentPosition.Y +0,Center.CurrentPosition.Z +1*size);

        Random rand = new Random();

        Triangle t0 = new Triangle(p0, p11, p5, Color);
        Triangle t1 = new Triangle(p0, p5, p1, Color);
        Triangle t2 = new Triangle(p0, p1, p7, Color);
        Triangle t3 = new Triangle(p0, p7, p10, Color);
        var color = Color;
        Triangle t4 = new Triangle(p0, p10, p11, color);
        Triangle t5 = new Triangle(p1, p5, p9, color);
        Triangle t6 = new Triangle(p5, p11, p4, color);
        Triangle t7 = new Triangle(p11, p10, p2, color);
        Triangle t8 = new Triangle(p10, p7, p6, color);
        Triangle t9 = new Triangle(p7, p1, p8, color);
        Triangle t10 = new Triangle(p3, p9, p4, color);
        Triangle t11 = new Triangle(p3, p4, p2, color);
        Triangle t12 = new Triangle(p3, p2, p6, color);
        Triangle t13 = new Triangle(p3, p6, p8, color);
        Triangle t14 = new Triangle(p3, p8, p9, color);
        Triangle t15 = new Triangle(p4, p9, p5, color);
        Triangle t16 = new Triangle(p2, p4, p11, color);
        Triangle t17 = new Triangle(p6, p2, p10, color);
        Triangle t18 = new Triangle(p8, p6, p7, color);
        Triangle t19 = new Triangle(p9, p8, p1, color);
        
        Points.Add(p0);
        Points.Add(p1);
        Points.Add(p2);
        Points.Add(p3);
        Points.Add(p4);
        Points.Add(p5);
        Points.Add(p6);
        Points.Add(p7);
        Points.Add(p8);
        Points.Add(p9);
        Points.Add(p10);
        Points.Add(p11);
        
        Triangles.Add(t0);
        Triangles.Add(t1);
        Triangles.Add(t2);
        Triangles.Add(t3);
        Triangles.Add(t4);
        Triangles.Add(t5);
        Triangles.Add(t6);
        Triangles.Add(t7);
        Triangles.Add(t8);
        Triangles.Add(t9);
        Triangles.Add(t10);
        Triangles.Add(t11);
        Triangles.Add(t12);
        Triangles.Add(t13);
        Triangles.Add(t14);
        Triangles.Add(t15);
        Triangles.Add(t16);
        Triangles.Add(t17);
        Triangles.Add(t18);
        Triangles.Add(t19);

        double radius = Vector.GetVector(Center.CurrentPosition, p0.CurrentPosition).GetLength();
        
        Console.WriteLine("Promien: " + radius);
        Console.WriteLine("T: " + t);

        for (int i = 0; i < levelOfTeselation; i++)
        {
            var currentTriangles = Triangles.ToList();
            Triangles.Clear();
            foreach (var triangle in currentTriangles)
            {
                Point middlePoint1 = GetMiddlePoint(triangle.P1, triangle.P2);
                Point middlePoint2 = GetMiddlePoint(triangle.P1, triangle.P3);
                Point middlePoint3 = GetMiddlePoint(triangle.P2, triangle.P3);
                
                //Move point to be in specified length from center
                Vector centerToMiddlePoint1 = Vector.GetVector(Center.CurrentPosition, middlePoint1.CurrentPosition);
                // Vector centerToMiddlePoint1 = Vector.GetVector( middlePoint1.CurrentPosition, center.CurrentPosition);
                centerToMiddlePoint1.SetLength(radius);
                middlePoint1 = new Point(Center.CurrentPosition.X + centerToMiddlePoint1.X,
                    Center.CurrentPosition.Y + centerToMiddlePoint1.Y,
                    Center.CurrentPosition.Z + centerToMiddlePoint1.Z);
                
                Vector centerToMiddlePoint2 = Vector.GetVector(Center.CurrentPosition, middlePoint2.CurrentPosition);
                // Vector centerToMiddlePoint2 = Vector.GetVector(middlePoint2.CurrentPosition, center.CurrentPosition );
                centerToMiddlePoint2.SetLength(radius);
                middlePoint2 = new Point(Center.CurrentPosition.X + centerToMiddlePoint2.X,
                    Center.CurrentPosition.Y + centerToMiddlePoint2.Y,
                    Center.CurrentPosition.Z + centerToMiddlePoint2.Z);
                
                Vector centerToMiddlePoint3 = Vector.GetVector(Center.CurrentPosition, middlePoint3.CurrentPosition);
                // Vector centerToMiddlePoint3 = Vector.GetVector(middlePoint3.CurrentPosition, center.CurrentPosition);
                centerToMiddlePoint3.SetLength(radius);
                middlePoint3 = new Point(Center.CurrentPosition.X + centerToMiddlePoint3.X,
                    Center.CurrentPosition.Y + centerToMiddlePoint3.Y,
                    Center.CurrentPosition.Z + centerToMiddlePoint3.Z);

                Triangle nt1 = new Triangle(triangle.P1, middlePoint1, middlePoint2, Color);
                Triangle nt2 = new Triangle(middlePoint1, middlePoint2, middlePoint3, Color);
                Triangle nt3 = new Triangle(triangle.P2, middlePoint1, middlePoint3, Color);
                Triangle nt4 = new Triangle(triangle.P3, middlePoint2, middlePoint3, Color);
                
                Triangles.Add(nt1);
                Triangles.Add(nt2);
                Triangles.Add(nt3);
                Triangles.Add(nt4);
                
                Points.Add(middlePoint1);
                Points.Add(middlePoint2);
                Points.Add(middlePoint3);
            }
        }
        
        Console.WriteLine(Points.Count);
        for(int i = 0; i < Points.Count; i++)
        {
            Point point = Points[i];
            foreach (var triangle in Triangles)
            {
                ChangePointInTriangle(point, triangle, epsilon);
            }
        }
        Console.WriteLine(Points.Count);
        
        //TODO: Invert triangles if normal is towards center of sphere
        foreach (var triangle in Triangles)
        {
            if (triangle.CheckPointPosition(Center.CurrentPosition) > 0)
                triangle.SwapNormalVector();
        }
    }
    
    private void ChangePointInTriangle(Point point, Triangle triangle, double epsilon)
    {
        
        if(point == triangle.P1 || point == triangle.P2 || point == triangle.P3)
            return;

        if (Vector.GetVector(point.CurrentPosition, triangle.P1.CurrentPosition).GetLength() < epsilon)
        {
            Points.Remove(triangle.P1);
            triangle.P1 = point;
        }
        if (Vector.GetVector(point.CurrentPosition, triangle.P2.CurrentPosition).GetLength() < epsilon)
        {
            Points.Remove(triangle.P2);
            triangle.P2 = point;
        }
        if (Vector.GetVector(point.CurrentPosition, triangle.P3.CurrentPosition).GetLength() < epsilon)
        {
            Points.Remove(triangle.P3);
            triangle.P3 = point;
        }
    }

    private Point GetMiddlePoint(Point p1, Point p2)
    {
        var mpX = (p1.CurrentPosition.X - p2.CurrentPosition.X) / 2 + p2.CurrentPosition.X;
        var mpY = (p1.CurrentPosition.Y - p2.CurrentPosition.Y) / 2 + p2.CurrentPosition.Y;
        var mpZ = (p1.CurrentPosition.Z - p2.CurrentPosition.Z) / 2 + p2.CurrentPosition.Z;

        return new Point(mpX, mpY, mpZ);
    }
}