using MainProject.Domain.Basic;

namespace MainProject.Domain.WorldRelated;

public class World
{
    public List<Point> Points = new List<Point>();
    public List<Line> Lines = new List<Line>();

    public World()
    {
        //Front Row
        CreateCuboid(-5,2,6,-2,5,9);
        CreateCuboid(-5,-2,6,-2,-5,9);
        CreateCuboid(5,2,6,2,5,9);
        CreateCuboid(5,-2,6,2,-5,9);
        
        //Back Row
        CreateCuboid(-5,2,13,-2,5,16);
        CreateCuboid(-5,-2,13,-2,-5,16);
        CreateCuboid(5,2,13,2,5,16);
        CreateCuboid(5,-2,13,2,-5,16);
        
    }

    private void CreateCuboid(double x0, double y0, double z0, double x1, double y1, double z1)
    {
        //Points of cube
        // Front square
        Point p1 = new Point(x0, y1, z0);
        Point p2 = new Point(x1, y1, z0);
        Point p3 = new Point(x1, y0, z0);
        Point p4 = new Point(x0, y0, z0);

        // Backs square
        Point p5 = new Point(x0, y1, z1);
        Point p6 = new Point(x1, y1, z1);
        Point p7 = new Point(x1, y0, z1);
        Point p8 = new Point(x0, y0, z1);
        
        // Lines of cube
        //Front lines
        Line l12 = new Line(p1, p2);
        p1.ConnectedPoints.Add(p2);
        p2.ConnectedPoints.Add(p1);
        
        Line l23 = new Line(p2, p3);
        p2.ConnectedPoints.Add(p3);
        p3.ConnectedPoints.Add(p2);
        
        Line l34 = new Line(p3, p4);
        p3.ConnectedPoints.Add(p4);
        p4.ConnectedPoints.Add(p3);
        
        Line l14 = new Line(p1, p4);
        p1.ConnectedPoints.Add(p4);
        p4.ConnectedPoints.Add(p1);
        
        //Back lines
        Line l56 = new Line(p5, p6);
        p5.ConnectedPoints.Add(p6);
        p6.ConnectedPoints.Add(p5);

        Line l67 = new Line(p6, p7);
        p6.ConnectedPoints.Add(p7);
        p7.ConnectedPoints.Add(p6);
        
        Line l78 = new Line(p7, p8);
        p7.ConnectedPoints.Add(p8);
        p8.ConnectedPoints.Add(p7);
        
        Line l58 = new Line(p5, p8);
        p5.ConnectedPoints.Add(p8);
        p8.ConnectedPoints.Add(p5);
        
        //Diagonal lines
        Line l15 = new Line(p1, p5);
        p1.ConnectedPoints.Add(p5);
        p5.ConnectedPoints.Add(p1);
        
        Line l26 = new Line(p2, p6);
        p2.ConnectedPoints.Add(p6);
        p6.ConnectedPoints.Add(p2);
        
        Line l37 = new Line(p3, p7);
        p3.ConnectedPoints.Add(p7);
        p7.ConnectedPoints.Add(p3);
        
        Line l48 = new Line(p4, p8);
        p4.ConnectedPoints.Add(p8);
        p8.ConnectedPoints.Add(p4);
        
        
        // Add every point and line to World!
        Points.Add(p1);
        Points.Add(p2);
        Points.Add(p3);
        Points.Add(p4);
        Points.Add(p5);
        Points.Add(p6);
        Points.Add(p7);
        Points.Add(p8);
        
        Lines.Add(l12);
        Lines.Add(l23);
        Lines.Add(l34);
        Lines.Add(l14);
        Lines.Add(l56);
        Lines.Add(l67);
        Lines.Add(l78);
        Lines.Add(l58);
        Lines.Add(l15);
        Lines.Add(l26);
        Lines.Add(l37);
        Lines.Add(l48);
    }
    
}