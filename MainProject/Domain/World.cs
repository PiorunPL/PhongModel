namespace MainProject;

public class World
{
    public List<Point> Points = new List<Point>();
    public List<Line> Lines = new List<Line>();
    
    

    public World()
    {
        //Points of cube
        // Front square
        Point p1 = new Point(-5, 5, 2);
        Point p2 = new Point(-2, 5, 2);
        Point p3 = new Point(-2, 2, 2);
        Point p4 = new Point(-5, 2, 2);
        
        // Backs square
        Point p5 = new Point(-5, 5, 5);
        Point p6 = new Point(-2, 5, 5);
        Point p7 = new Point(-2, 2, 5);
        Point p8 = new Point(-5, 2, 5);
        
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