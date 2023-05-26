using MainProject.Domain.Basic;

namespace MainProject.Domain.WorldRelated;

public class WorldSphere
{
    public List<Triangle> Triangles = new List<Triangle>();
    public List<Point> Points = new List<Point>();
    public List<Sphere> Spheres = new List<Sphere>();

    public Point Center = new Point(0, 0, 20);
    const double Epsilon = 1.0E-10;
    
    
    
    public WorldSphere()
    {
        Sphere s1 = new Sphere(Center, 4, 5.0, Epsilon);
        Spheres.Add(s1);
        Triangles.AddRange(s1.Triangles);
        Points.AddRange(s1.Points);
    }

    
}