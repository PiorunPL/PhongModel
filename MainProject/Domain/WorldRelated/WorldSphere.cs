using MainProject.Domain.Basic;

namespace MainProject.Domain.WorldRelated;

public class WorldSphere
{
    public List<Triangle> Triangles = new List<Triangle>();
    public List<Point> Points = new List<Point>();

    public Point center = new Point(0, 0, 20);

    public List<int[]> Colors = new List<int[]>()
    {
        new int[]{ 255, 40, 0 },
        new int[]{ 255, 80, 0 },
        new int[]{ 255, 120, 0 },
        new int[]{ 255, 160, 0 },
        new int[]{ 255, 200, 0 },
        new int[]{ 255, 240, 0 }
    };
    
    public WorldSphere(int levelOfTeselation, double size)
    {
        var t = (1.0 + Math.Sqrt(5.0)) / 2.0;

        Point p0 = new Point(center.CurrentPosition.X  -1*size,center.CurrentPosition.Y + t*size, center.CurrentPosition.Z +0);
        Point p1 = new Point(center.CurrentPosition.X +1*size, center.CurrentPosition.Y +t*size, center.CurrentPosition.Z +0);
        Point p2 = new Point(center.CurrentPosition.X -1*size, center.CurrentPosition.Y -t*size, center.CurrentPosition.Z +0);
        Point p3 = new Point(center.CurrentPosition.X +1*size, center.CurrentPosition.Y -t*size, center.CurrentPosition.Z +0);
        
        Point p4 = new Point(center.CurrentPosition.X +0*size,center.CurrentPosition.Y -1*size,center.CurrentPosition.Z +t*size);
        Point p5 = new Point(center.CurrentPosition.X +0*size,center.CurrentPosition.Y +1*size,center.CurrentPosition.Z +t*size);
        Point p6 = new Point(center.CurrentPosition.X +0*size,center.CurrentPosition.Y -1*size,center.CurrentPosition.Z -t*size);
        Point p7 = new Point(center.CurrentPosition.X +0*size,center.CurrentPosition.Y +1*size,center.CurrentPosition.Z -t*size);
        
        Point p8 = new Point(center.CurrentPosition.X +t*size,center.CurrentPosition.Y +0,center.CurrentPosition.Z -1*size);
        Point p9 = new Point(center.CurrentPosition.X +t*size,center.CurrentPosition.Y +0,center.CurrentPosition.Z +1*size);
        Point p10 = new Point(center.CurrentPosition.X -t*size,center.CurrentPosition.Y +0,center.CurrentPosition.Z -1*size);
        Point p11 = new Point(center.CurrentPosition.X -t*size,center.CurrentPosition.Y +0,center.CurrentPosition.Z +1*size);

        Random rand = new Random();

        Triangle t0 = new Triangle(p0, p11, p5, Colors[rand.Next(6)]);
        Triangle t1 = new Triangle(p0, p5, p1, Colors[rand.Next(6)]);
        Triangle t2 = new Triangle(p0, p1, p7, Colors[rand.Next(6)]);
        Triangle t3 = new Triangle(p0, p7, p10, Colors[rand.Next(6)]);
        Triangle t4 = new Triangle(p0, p10, p11, Colors[rand.Next(6)]);
        Triangle t5 = new Triangle(p1, p5, p9, Colors[rand.Next(6)]);
        Triangle t6 = new Triangle(p5, p11, p4, Colors[rand.Next(6)]);
        Triangle t7 = new Triangle(p11, p10, p2, Colors[rand.Next(6)]);
        Triangle t8 = new Triangle(p10, p7, p6, Colors[rand.Next(6)]);
        Triangle t9 = new Triangle(p7, p1, p8, Colors[rand.Next(6)]);
        Triangle t10 = new Triangle(p3, p9, p4, Colors[rand.Next(6)]);
        Triangle t11 = new Triangle(p3, p4, p2, Colors[rand.Next(6)]);
        Triangle t12 = new Triangle(p3, p2, p6, Colors[rand.Next(6)]);
        Triangle t13 = new Triangle(p3, p6, p8, Colors[rand.Next(6)]);
        Triangle t14 = new Triangle(p3, p8, p9, Colors[rand.Next(6)]);
        Triangle t15 = new Triangle(p4, p9, p5, Colors[rand.Next(6)]);
        Triangle t16 = new Triangle(p2, p4, p11, Colors[rand.Next(6)]);
        Triangle t17 = new Triangle(p6, p2, p10, Colors[rand.Next(6)]);
        Triangle t18 = new Triangle(p8, p6, p7, Colors[rand.Next(6)]);
        Triangle t19 = new Triangle(p9, p8, p1, Colors[rand.Next(6)]);
        
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

        double radius = Vector.GetVector(center.CurrentPosition, p0.CurrentPosition).GetLength();
        
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
                Vector centerToMiddlePoint1 = Vector.GetVector(center.CurrentPosition, middlePoint1.CurrentPosition);
                // Vector centerToMiddlePoint1 = Vector.GetVector( middlePoint1.CurrentPosition, center.CurrentPosition);
                centerToMiddlePoint1.SetLength(radius);
                middlePoint1 = new Point(center.CurrentPosition.X + centerToMiddlePoint1.X,
                    center.CurrentPosition.Y + centerToMiddlePoint1.Y,
                    center.CurrentPosition.Z + centerToMiddlePoint1.Z);
                
                Vector centerToMiddlePoint2 = Vector.GetVector(center.CurrentPosition, middlePoint2.CurrentPosition);
                // Vector centerToMiddlePoint2 = Vector.GetVector(middlePoint2.CurrentPosition, center.CurrentPosition );
                centerToMiddlePoint2.SetLength(radius);
                middlePoint2 = new Point(center.CurrentPosition.X + centerToMiddlePoint2.X,
                    center.CurrentPosition.Y + centerToMiddlePoint2.Y,
                    center.CurrentPosition.Z + centerToMiddlePoint2.Z);
                
                Vector centerToMiddlePoint3 = Vector.GetVector(center.CurrentPosition, middlePoint3.CurrentPosition);
                // Vector centerToMiddlePoint3 = Vector.GetVector(middlePoint3.CurrentPosition, center.CurrentPosition);
                centerToMiddlePoint3.SetLength(radius);
                middlePoint3 = new Point(center.CurrentPosition.X + centerToMiddlePoint3.X,
                    center.CurrentPosition.Y + centerToMiddlePoint3.Y,
                    center.CurrentPosition.Z + centerToMiddlePoint3.Z);

                Triangle nt1 = new Triangle(triangle.P1, middlePoint1, middlePoint2, new []{rand.Next(256), rand.Next(256), rand.Next(256)});
                Triangle nt2 = new Triangle(middlePoint1, middlePoint2, middlePoint3, new []{rand.Next(256), rand.Next(256), rand.Next(256)});
                Triangle nt3 = new Triangle(triangle.P2, middlePoint1, middlePoint3, new []{rand.Next(256), rand.Next(256), rand.Next(256)});
                Triangle nt4 = new Triangle(triangle.P3, middlePoint2, middlePoint3, new []{rand.Next(256), rand.Next(256), rand.Next(256)});
                
                Triangles.Add(nt1);
                Triangles.Add(nt2);
                Triangles.Add(nt3);
                Triangles.Add(nt4);
                
                Points.Add(middlePoint1);
                Points.Add(middlePoint2);
                Points.Add(middlePoint3);
            }
        }
        
        Console.WriteLine("ENDED");
        //TODO Merge points with same coordinations
        
    }

    private Point GetMiddlePoint(Point p1, Point p2)
    {
        var mpX = (p1.CurrentPosition.X - p2.CurrentPosition.X) / 2 + p2.CurrentPosition.X;
        var mpY = (p1.CurrentPosition.Y - p2.CurrentPosition.Y) / 2 + p2.CurrentPosition.Y;
        var mpZ = (p1.CurrentPosition.Z - p2.CurrentPosition.Z) / 2 + p2.CurrentPosition.Z;

        return new Point(mpX, mpY, mpZ);
    }
}