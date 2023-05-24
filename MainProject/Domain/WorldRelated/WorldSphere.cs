using MainProject.Domain.Basic;

namespace MainProject.Domain.WorldRelated;

public class WorldSphere
{
    public List<Triangle> Triangles = new List<Triangle>();
    public List<Point> Points = new List<Point>();

    public Point center = new Point(0, 0, 0);

    public List<int[]> colors = new List<int[]>()
    {
        new int[]{ 255, 40, 0 },
        new int[]{ 255, 80, 0 },
        new int[]{ 255, 120, 0 },
        new int[]{ 255, 160, 0 },
        new int[]{ 255, 200, 0 },
        new int[]{ 255, 240, 0 }
    };
    
    public WorldSphere()
    {
        var t = (1.0 + Math.Sqrt(5.0)) / 2.0;

        Point p0 = new Point(-1, t, 0);
        Point p1 = new Point(1, t, 0);
        Point p2 = new Point(-1, -t, 0);
        Point p3 = new Point(1, -t, 0);
        
        Point p4 = new Point(0,-1,t);
        Point p5 = new Point(0,1,t);
        Point p6 = new Point(0,-1,-t);
        Point p7 = new Point(0,1,-t);
        
        Point p8 = new Point(t,0,-1);
        Point p9 = new Point(t,0,1);
        Point p10 = new Point(-t,0,-1);
        Point p11 = new Point(-t,0,1);

        Random rand = new Random();

        Triangle t0 = new Triangle(p0, p11, p5, colors[rand.Next(6)]);
        Triangle t1 = new Triangle(p0, p5, p1, colors[rand.Next(6)]);
        Triangle t2 = new Triangle(p0, p1, p7, colors[rand.Next(6)]);
        Triangle t3 = new Triangle(p0, p7, p10, colors[rand.Next(6)]);
        Triangle t4 = new Triangle(p0, p10, p11, colors[rand.Next(6)]);
        Triangle t5 = new Triangle(p1, p5, p9, colors[rand.Next(6)]);
        Triangle t6 = new Triangle(p5, p11, p4, colors[rand.Next(6)]);
        Triangle t7 = new Triangle(p11, p10, p2, colors[rand.Next(6)]);
        Triangle t8 = new Triangle(p10, p7, p6, colors[rand.Next(6)]);
        Triangle t9 = new Triangle(p7, p1, p8, colors[rand.Next(6)]);
        Triangle t10 = new Triangle(p3, p9, p4, colors[rand.Next(6)]);
        Triangle t11 = new Triangle(p3, p4, p2, colors[rand.Next(6)]);
        Triangle t12 = new Triangle(p3, p2, p6, colors[rand.Next(6)]);
        Triangle t13 = new Triangle(p3, p6, p8, colors[rand.Next(6)]);
        Triangle t14 = new Triangle(p3, p8, p9, colors[rand.Next(6)]);
        Triangle t15 = new Triangle(p4, p9, p5, colors[rand.Next(6)]);
        Triangle t16 = new Triangle(p2, p4, p11, colors[rand.Next(6)]);
        Triangle t17 = new Triangle(p6, p2, p10, colors[rand.Next(6)]);
        Triangle t18 = new Triangle(p8, p6, p7, colors[rand.Next(6)]);
        Triangle t19 = new Triangle(p9, p8, p1, colors[rand.Next(6)]);
        
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

        double radius = Vector.GetVector(new Point3D(0, 0, 0), p0.CurrentPosition).GetLength();
        
        Console.WriteLine("Promien: " + radius);
        Console.WriteLine("T: " + t);
    }
}