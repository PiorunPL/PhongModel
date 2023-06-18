using MainProject.Domain.Basic;
using MainProject.Domain.CameraRelated;
using MainProject.Domain.WorldRelated;
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

    public SKBitmap GetBitmapFromTriangles(List<Triangle> triangles, List<Light> lights)
    {
        SKBitmap bitmap = new SKBitmap(targetWidth, targetHeight, true);
        SKCanvas canvas = new SKCanvas(bitmap);
        
        var pathStroke = new SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.StrokeAndFill,
            StrokeWidth = 1
        };
        
        foreach (var triangle in triangles)
        {   
            
            int red = 0;
            int green = 0;
            int blue = 0;

            var normal = triangle.GetNormalVector();
            var normalisedNormal = triangle.GetNormalisedNormalVector();
            var centralPoint = triangle.GetCentralPoint();
            var cameraVector = Vector.GetVector(centralPoint, new Point3D(0, 0, 0)).GetNormalized();
            
            foreach (var light in lights)
            {


                var lightVector = Vector.GetVector(centralPoint, light.CenterPosition.CurrentPosition).GetNormalized();
                double diffues_wsp = Vector.GetDotProduct(normalisedNormal, lightVector);

                var reflectionVector = new Vector()
                {
                    X = normalisedNormal.X * 2 * diffues_wsp - lightVector.X,
                    Y = normalisedNormal.Y * 2 * diffues_wsp - lightVector.Y,
                    Z = normalisedNormal.Z * 2 * diffues_wsp - lightVector.Z
                }.GetNormalized();
                
                double specular_wsp = Vector.GetDotProduct(reflectionVector, cameraVector);

                if (diffues_wsp < 0)
                    diffues_wsp = 0;

                if (specular_wsp < 0)
                    specular_wsp = 0;

                specular_wsp = Math.Pow(specular_wsp, triangle.Material.Shininess);

                int redForLight = (int)(
                    (light.Red * triangle.Material.AmbientRed) +
                    (light.Red * triangle.Material.DiffuseRed * diffues_wsp) +
                    (light.Red * triangle.Material.SpecularRed * specular_wsp));
                int greenForLight = (int)(
                    (light.Green * triangle.Material.AmbientGreen) +
                    (light.Green * triangle.Material.DiffuseGreen * diffues_wsp) +
                    (light.Green * triangle.Material.SpecularGreen * specular_wsp));
                int blueForLight = (int)(
                    (light.Blue * triangle.Material.AmbientBlue) +
                    (light.Blue * triangle.Material.DiffuseBlue * diffues_wsp) +
                    (light.Blue * triangle.Material.SpecularBlue * specular_wsp));

                red += redForLight;
                green += greenForLight;
                blue += blueForLight;

            }

            if (red > 255)
                red = 255;
            if (green > 255)
                green = 255;
            if (blue > 255)
                blue = 255;

            pathStroke.Color = new SKColor(
                (byte)(red),
                (byte)(green),
                (byte)(blue));

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