using System.Drawing;
using System.Drawing.Imaging;
using SkiaSharp;

namespace MainProject;

public class BitmapUtil
{
    public int targetWidth = 1900;
    public int targetHeight = 1000;

    public ViewPort ViewPort;

    public BitmapUtil(ViewPort viewPort)
    {
        ViewPort = viewPort;
    }
    
    public SKBitmap getBitmapFromLines(List<Line> lines)
    {
        SKBitmap bitmap = new SKBitmap(targetWidth, targetHeight, true);
        SKCanvas canvas = new SKCanvas(bitmap);
        SKPaint paint = new SKPaint();
        paint.Color = SKColors.Green;
        // Graphics graphics = Graphics.FromImage(bitmap);

        // Pen pen = new Pen(Color.Green, 2);
        
        foreach (var line in lines)
        {
            Point point1 = line.Point1;
            Point point2 = line.Point2;

            if (point1.CurrentPosition.Z <= 0 && point2.CurrentPosition.Z <= 0) 
                continue;
            
            (int x1, int y1) = point1.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
            (int x2, int y2) = point2.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
                
            canvas.DrawLine(x1, y1, x2, y2, paint);
        }
        
        //TODO: Change every point state not calculated!

        return bitmap;
    }
}