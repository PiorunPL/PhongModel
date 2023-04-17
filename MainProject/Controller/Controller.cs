using SkiaSharp;

namespace MainProject;

public class Controller
{
    public World World = new World();
    public Camera Camera = new Camera();

    public SKBitmap CreatePhoto()
    {
        Console.WriteLine("CREATING PHOTO!");
        Camera.Lines.AddRange(World.Lines);
        var result = Camera.CreatePhoto();
        Camera.Lines.Clear();
        return result;
    }

    public void ZoomIn(double t)
    {
        Console.WriteLine($"Before Zoom In: {Camera.ViewPort.Z} with t = {t}");
        Camera.ViewPort.Z += t;
        Console.WriteLine($"After Zoom In: {Camera.ViewPort.Z}");
       
    }

    public void ZoomOut(double t)
    {
        if (Camera.ViewPort.Z - t <= 0)
            return;
        Console.WriteLine($"Before Zoom Out: {Camera.ViewPort.Z} with t = {t}");
        Camera.ViewPort.Z -= t;
        Console.WriteLine($"After Zoom Out: {Camera.ViewPort.Z}");
    }

}