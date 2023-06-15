using MainProject.Domain.Basic;
using MainProject.Domain.Materials;

namespace MainProject.Domain.WorldRelated;

public class WorldSphere
{
    public List<Triangle> Triangles = new List<Triangle>();
    public List<Point> Points = new List<Point>();
    public List<Sphere> Spheres = new List<Sphere>();
    public List<Light> Lights = new List<Light>();

    public Point Center = new Point(0, 0, 20);
    const double Epsilon = 1.0E-10;
    
    
    
    public WorldSphere()
    {
        Sphere s1 = new Sphere(Center, 4, 5.0, Epsilon, Material.Gold);
        Spheres.Add(s1);
        Triangles.AddRange(s1.Triangles);
        Points.AddRange(s1.Points);

        Point light1Position = new Point(0, 100, 20);
        Light light1 = new Light(light1Position, 255, 255, 255);
        Lights.Add(light1);
        Points.Add(light1Position);
        
        Point light2Position = new Point(100, 50, 0);
        Light light2 = new Light(light2Position, 255, 0, 0);
        Lights.Add(light2);
        Points.Add(light2Position);
        //
        // Point light2Position = new Point(0, 100, 20);
        // Light light2 = new Light(light2Position);
        // Lights.Add(light2);
        // Points.Add(light2Position);
        //
        Point light3Position = new Point(-100, -20, 0);
        Light light3 = new Light(light3Position, 0, 255, 0);
        Lights.Add(light3);
        Points.Add(light3Position);
    }

    
}