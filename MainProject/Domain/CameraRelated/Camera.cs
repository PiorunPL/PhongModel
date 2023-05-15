using MainProject.Domain.Basic;
using MainProject.Utility;
using SkiaSharp;

namespace MainProject.Domain.CameraRelated;

public class Camera
{
    public ViewPort ViewPort = new ViewPort();
    // public List<Point2D> ViewPortPoints = new List<Point2D>();
    public List<Line> Lines = new List<Line>();
    public List<Triangle> Triangles = new List<Triangle>();
    public BitmapUtil BitmapUtil;

    public Camera()
    {
        BitmapUtil = new BitmapUtil(ViewPort);
    }

    public SKBitmap CreatePhoto()
    {
        return BitmapUtil.GetBitmapFromLines(Lines);
    }

    public SKBitmap CreatePhotoTriangles()
    {
        return BitmapUtil.GetBitmapFromTriangles(Triangles);
    }

    public void PassActualWorld(List<Line> lines)
    {
        Lines.Clear();
        Lines.AddRange(lines);
    }

    public void PassActualWorld(List<Triangle> triangles)
    {
        Triangles.Clear();
        Triangles.AddRange(triangles);
    }
}