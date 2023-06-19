using MainProject.Domain.Basic;
using MainProject.Domain.WorldRelated;
using MainProject.Utility;
using SkiaSharp;

namespace MainProject.Domain.CameraRelated;

public class Camera
{
    public ViewPort ViewPort = new ViewPort();
    // public List<Point2D> ViewPortPoints = new List<Point2D>();
    public List<Line> Lines = new List<Line>();
    public List<Triangle> Triangles = new List<Triangle>();
    public List<Light> Lights = new List<Light>();
    public BitmapUtil BitmapUtil;
    private SKCanvas Canvas;

    public Camera(SKCanvas canvas)
    {
        BitmapUtil = new BitmapUtil(ViewPort);
        Canvas = canvas;
    }

    public SKBitmap CreatePhoto()
    {
        return BitmapUtil.GetBitmapFromLines(Lines);
    }

    public void CreatePhotoTriangles()
    {
        BitmapUtil.GetBitmapFromTriangles(Canvas, Triangles, Lights);
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

    public void PassAllLights(List<Light> lights)
    {
        Lights.Clear();
        Lights.AddRange(lights);
    }
}