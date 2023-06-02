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
            Light light = lights[0];
            
            var normal = triangle.GetNormalVector();
            var normalisedNormal = triangle.GetNormalisedNormalVector();
            var centralPoint = triangle.GetCentralPoint();

            var lightVector = Vector.GetVector(centralPoint, light.CenterPosition.CurrentPosition).GetNormalized();
            double diffues_wsp = Vector.GetDotProduct(normalisedNormal, lightVector) ;
            
            var reflectionVector = new Vector()
            {
                X =  normalisedNormal.X * 2 * diffues_wsp - lightVector.X,
                Y =  normalisedNormal.Y * 2 * diffues_wsp - lightVector.Y,
                Z =  normalisedNormal.Z * 2 * diffues_wsp - lightVector.Z
            }.GetNormalized();

            var cameraVector = Vector.GetVector(centralPoint, new Point3D(0, 0, 0)).GetNormalized();
            
            // Console.WriteLine(normalisedNormal);

            // byte light = 5;
            // byte baselight = 0;
            

            
            double specular_wsp = Vector.GetDotProduct(reflectionVector, cameraVector);

            if (diffues_wsp < 0)
                diffues_wsp = 0;

            if (specular_wsp < 0)
                specular_wsp = 0;
            
            double ambient_wsp_red = 0.2125;
            double ambient_wsp_green = 0.1275;
            double ambient_wsp_blue = 0.054;


            double diffuse_wsp_red = 0.714;
            double diffuse_wsp_green = 0.4284;
            double diffuse_wsp_blue = 0.18144;

            double specular_wsp_red = 0.393548;
            double specular_wsp_green = 0.271906;
            double specular_wsp_blue = 0.166721;

            double shininess = 0.2 * 128;

            int triangle_red = 0;
            int triangle_green = 0;
            int triangle_blue = 0;

            specular_wsp = Math.Pow(specular_wsp, shininess);
            
            int red = (int)(
                triangle_red + 
                (light.Red * ambient_wsp_red) + 
                (light.Red * diffuse_wsp_red * diffues_wsp) + 
                (light.Red * specular_wsp_red * specular_wsp));
            int green = (int)(
                triangle_green + 
                (light.Green * ambient_wsp_green) + 
                (light.Green * diffuse_wsp_green * diffues_wsp) +
                (light.Green * specular_wsp_green * specular_wsp));
            int blue = (int)(
                triangle_blue + 
                (light.Blue * ambient_wsp_blue) + 
                (light.Blue * diffuse_wsp_blue * diffues_wsp) + 
                (light.Blue * specular_wsp_blue * specular_wsp));

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