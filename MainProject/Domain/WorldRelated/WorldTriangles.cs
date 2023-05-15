using MainProject.Domain.Basic;

namespace MainProject.Domain.WorldRelated;

public class WorldTriangles
{
    public List<Triangle> Triangles = new List<Triangle>();
    public List<Point> Points = new List<Point>();
    int[] color1 = new int[]{255, 40, 0};
    int[] color2 = new int[]{255, 80, 0};
    int[] color3 = new int[]{255, 120, 0};
    int[] color4 = new int[]{255, 160, 0};
    int[] color5 = new int[]{255, 200, 0};
    int[] color6 = new int[]{255, 240, 0};

    public void SetUpBasicWorld()
    {
        //Front
        Point3D a1 = new Point3D(-5, 2, 6);
        Point3D b1 = new Point3D(-2, 5, 9);

        Point3D a2 = new Point3D(-5, -2, 6);
        Point3D b2 = new Point3D(-2, -5, 9);

        Point3D a3 = new Point3D(5, 2, 6);
        Point3D b3 = new Point3D(2, 5, 9);

        Point3D a4 = new Point3D(5, -2, 6);
        Point3D b4 = new Point3D(2, -5, 9);
        
        
        //Back
        Point3D a5 = new Point3D(-5, 2, 13);
        Point3D b5 = new Point3D(-2, 5, 16);

        Point3D a6 = new Point3D(-5, -2, 13);
        Point3D b6 = new Point3D(-2, -5, 16);

        Point3D a7 = new Point3D(5, 2, 13);
        Point3D b7 = new Point3D(2, 5, 16);

        Point3D a8 = new Point3D(5, -2, 13);
        Point3D b8 = new Point3D(2, -5, 16);
        
        CreateCuboid(a1,b1);
        CreateCuboid(a2,b2);
        CreateCuboid(a3,b3);
        CreateCuboid(a4,b4);
        CreateCuboid(a5,b5);
        CreateCuboid(a6,b6);
        CreateCuboid(a7,b7);
        CreateCuboid(a8,b8);
    }

    public void CreateCuboid(Point3D a, Point3D b)
    {
        Point p1 = new Point(a.X, b.Y, a.Z);
        Point p2 = new Point(b.X, b.Y, a.Z);
        Point p3 = new Point(b.X, a.Y, a.Z);
        Point p4 = new Point(a);

        Point p5 = new Point(a.X, b.Y, b.Z);
        Point p6 = new Point(b);
        Point p7 = new Point(b.X, a.Y, b.Z);
        Point p8 = new Point(a.X, a.Y, b.Z);
        
        Points.Add(p1);
        Points.Add(p2);
        Points.Add(p3);
        Points.Add(p4);
        Points.Add(p5);
        Points.Add(p6);
        Points.Add(p7);
        Points.Add(p8);

        //Front
        Triangle t1 = new Triangle(p1, p2, p3, color1);
        Triangle t2 = new Triangle(p3, p4, p1, color1);
        
        //Back
        Triangle t3 = new Triangle(p5, p6, p7, color2);
        Triangle t4 = new Triangle(p7, p8, p5, color2);
        
        //Top
        Triangle t5 = new Triangle(p1, p2, p5, color3);
        Triangle t6 = new Triangle(p2, p5, p6, color3);
        
        //Bottom
        Triangle t7 = new Triangle(p3, p4, p8, color4);
        Triangle t8 = new Triangle(p3, p8, p7, color4);
        
        //Right
        Triangle t9 = new Triangle(p2, p3, p7, color5);
        Triangle t10 = new Triangle(p2, p6, p7, color5);
        
        //Left
        Triangle t11 = new Triangle(p1, p4, p5, color6);
        Triangle t12 = new Triangle(p4, p5, p8, color6);

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
    }
}