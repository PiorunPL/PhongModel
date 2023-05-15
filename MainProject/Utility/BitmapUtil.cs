using MainProject.Domain.Basic;
using MainProject.Domain.CameraRelated;
using SkiaSharp;
using Point = MainProject.Domain.Basic.Point;

namespace MainProject.Utility;

public class BitmapUtil
{
    public int targetWidth = 1900;
    public int targetHeight = 1000;

    public ViewPort ViewPort;

    public BitmapUtil(ViewPort viewPort)
    {
        ViewPort = viewPort;
    }
    
    public SKBitmap GetBitmapFromLines(List<Line> lines)
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

    public SKBitmap GetBitmapFromTriangles(List<Triangle> triangles)
    {
        SKBitmap bitmap = new SKBitmap(targetWidth, targetHeight, true);
        SKCanvas canvas = new SKCanvas(bitmap);

        foreach (var triangle in triangles)
        {
            var normal = triangle.GetNormalVector();

            byte light = 5;
            byte baselight = 0;
            int red = triangle.color[0];
            int green = triangle.color[1];
            int blue = triangle.color[2];
            
            var pathStroke = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
                Color = new SKColor(
                    (byte)(red * light + baselight),
                    (byte)(green * light+ baselight),
                    (byte)(blue * light+ baselight)),
                StrokeWidth = 1
            };

            (int x1, int y1) = triangle.P1.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
            (int x2, int y2) = triangle.P2.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
            (int x3, int y3) = triangle.P3.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);


            var path = new SKPath { FillType = SKPathFillType.EvenOdd };
            path.MoveTo(x1,y1);
            path.LineTo(x2,y2);
            path.LineTo(x3,y3);
            path.LineTo(x1,y1);
            path.Close();
            
            canvas.DrawPath(path, pathStroke);
        }
        
        return bitmap;
    }
}