using System.Drawing;
using SkiaSharp;

namespace MainProject;

public class Camera
{
    public ViewPort ViewPort = new ViewPort();
    // public List<Point2D> ViewPortPoints = new List<Point2D>();
    public List<Line> Lines = new List<Line>();
    public BitmapUtil BitmapUtil;

    public Camera()
    {
        BitmapUtil = new BitmapUtil(ViewPort);
    }

    public SKBitmap CreatePhoto()
    {
        return BitmapUtil.getBitmapFromLines(Lines);
    }

    public void PassActualWorld(List<Line> lines)
    {
        Lines.Clear();
        Lines.AddRange(lines);
    }
}