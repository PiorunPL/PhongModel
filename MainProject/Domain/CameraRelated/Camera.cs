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

    public Camera()
    {
        BitmapUtil = new BitmapUtil(ViewPort);
    }

    public SKBitmap CreatePhoto()
    {
        return BitmapUtil.GetBitmapFromLines(Lines);
    }

    public SKBitmap CreatePhotoTriangles(WorldSphere world)
    {
        // return BitmapUtil.GetBitmapFromTriangles(Triangles, Lights);
        return BitmapUtil.GetBitmapFromTrianglesPixels(Triangles, Lights, world);
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