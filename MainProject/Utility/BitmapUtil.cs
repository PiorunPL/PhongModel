using System.Collections.Concurrent;
using System.Net.Sockets;
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
            // var normalisedNormal = triangle.GetNormalisedNormalVector();
            var centralPoint = triangle.GetCentralPoint();
            var normalisedNormal = normal.GetNormalized();

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

                var cameraVector = Vector.GetVector(centralPoint, new Point3D(0, 0, 0)).GetNormalized();

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

            // var poinTest1 = new Point(-19, 10, 15);
            // var poinTest2 = new Point(19, -10, 15);
            // var poinTest3 = new Point(10, 10, 15);
            // var poinTest4 = new Point(50, 50, 15);
            // var poinTest5 = new Point(100, 100, 15);

            // (int x11, int y11) = poinTest1.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
            // (int x22, int y22) = poinTest2.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
            // (int x33, int y33) = poinTest3.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
            // (int x44, int y44) = poinTest4.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
            // (int x55, int y55) = poinTest5.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);

            
            // triangle.GetAllPixelsInTriangle(targetWidth, targetHeight, ViewPort.Z);
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
    
    public SKBitmap GetBitmapFromTrianglesPixels(List<Triangle> triangles, List<Light> lights, WorldSphere world)
    {
        // TODO: change values to static
        SKBitmap bitmap = new SKBitmap(1900, 1000, true);
        // SKCanvas canvas = new SKCanvas(bitmap);
        
        // var pathStroke = new SKPaint
        // {
        //     IsAntialias = true,
        //     Style = SKPaintStyle.StrokeAndFill,
        //     StrokeWidth = 1
        // };

        IDictionary<int[], SKColor> outputPixels = new ConcurrentDictionary<int[], SKColor>();
        
        foreach (var triangle in triangles)
        // Parallel.ForEach(triangles, triangle =>

        {
            var centralPointTriangle = triangle.GetCentralPoint();
            // if (centralPointTriangle.Z > world.Center.CurrentPosition.Z)
            // {
            //     continue;
            // }

            var sphere = world.Spheres[0];
            Point3D center = sphere.Center.CurrentPosition;
            
            var planeOfTriangle = triangle.GetPlaneOfTriangle();
            List<int[]> trianglePixels = triangle.GetAllPixelsInTriangle(targetWidth, targetHeight, ViewPort.Z, sphere.Center.CurrentPosition.Z);
            foreach (var pixel in trianglePixels)
            {
                Point pointOfPixel = GetPointFromPixelOnBitmap(pixel[0], pixel[1], targetWidth, targetHeight);
                Vector vectorFromObserverToPointOfPixel = GetVectorFromObserverToPointOfPixel(pointOfPixel);
                // Point3D pointOnTriangle =
                //     GetCommonPointOfTriangleAndStraight(vectorFromObserverToPointOfPixel, planeOfTriangle);

                // vectorFromCenterToPointOnTriangle.SetLength(sphere.Radius);
                // Point pointOnSphere = new Point(sphere.Center.CurrentPosition.X + vectorFromCenterToPointOnTriangle.X,
                //     sphere.Center.CurrentPosition.Y + vectorFromCenterToPointOnTriangle.Y,
                //     sphere.Center.CurrentPosition.Z + vectorFromCenterToPointOnTriangle.Z);
                Point pointOnSphere = GetPointOnSphere(center.X, center.Y, center.Z, sphere.Radius,
                    vectorFromObserverToPointOfPixel.X, vectorFromObserverToPointOfPixel.Y,
                    vectorFromObserverToPointOfPixel.Z);
                if (pointOnSphere == null)
                {
                    continue;
                }
                Vector vectorFromCenterToPointOnTriangle = Vector.GetVector(
                    world.Center.CurrentPosition, pointOnSphere.CurrentPosition);
                // double lenFromCenterToPointOnTriangle = vectorFromCenterToPointOnTriangle.GetLength();
                // double lackOfLen = 1 - lenFromCenterToPointOnTriangle / world.Spheres[0].Radius;
                //
                // Point3D pointOnSphere = new Point3D(pointOnTriangle.X + lackOfLen * vectorFromCenterToPointOnTriangle.X,
                //     pointOnTriangle.Y + lackOfLen * vectorFromCenterToPointOnTriangle.Y,
                //     pointOnTriangle.Z + lackOfLen * vectorFromCenterToPointOnTriangle.Z);

                
                
                // double xx = planeOfTriangle[0] * pointOnTriangle.X;
                // double yy = planeOfTriangle[1] * pointOnTriangle.Y;
                // double zz = planeOfTriangle[2] * pointOnTriangle.Z;
                // double ott = xx + yy + zz + planeOfTriangle[3];
                // Point3D pointONTriangle = ;


                int red = 0;
                int green = 0;
                int blue = 0;

                // var normalisedNormal = triangle.GetNormalisedNormalVectorFromPoint(pointOnTriangle);
                // var normalisedNormal = triangle.GetNormalisedNormalVector();
                var normalisedNormal = vectorFromCenterToPointOnTriangle.GetNormalized();

                foreach (var light in lights)
                {
                    var lightVector = Vector.GetVector(pointOnSphere.CurrentPosition, light.CenterPosition.CurrentPosition)
                        .GetNormalized();
                    double diffues_wsp = Vector.GetDotProduct(normalisedNormal, lightVector);

                    var reflectionVector = new Vector()
                    {
                        X = normalisedNormal.X * 2 * diffues_wsp - lightVector.X,
                        Y = normalisedNormal.Y * 2 * diffues_wsp - lightVector.Y,
                        Z = normalisedNormal.Z * 2 * diffues_wsp - lightVector.Z
                    }.GetNormalized();

                    var cameraVector = Vector.GetVector(pointOnSphere.CurrentPosition, new Point3D(0, 0, 0)).GetNormalized();

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

                var color = new SKColor(
                    (byte)(red),
                    (byte)(green),
                    (byte)(blue));

                // (int x1, int y1) = triangle.P1.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
                // (int x2, int y2) = triangle.P2.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
                // (int x3, int y3) = triangle.P3.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);

                // var poinTest1 = new Point(-19, 10, 15);
                // var poinTest2 = new Point(19, -10, 15);
                // var poinTest3 = new Point(10, 10, 15);
                // var poinTest4 = new Point(50, 50, 15);
                // var poinTest5 = new Point(100, 100, 15);

                // (int x11, int y11) = poinTest1.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
                // (int x22, int y22) = poinTest2.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
                // (int x33, int y33) = poinTest3.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
                // (int x44, int y44) = poinTest4.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
                // (int x55, int y55) = poinTest5.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);

                // Point p = new Point(pointOnSphere.CurrentPosition.X, pointOnSphere.CurrentPosition.Y, pointOnSphere.CurrentPosition.Z);
                // (int x, int y) = p.getPointCoordinatesBitmap(targetWidth, targetHeight, ViewPort.Z);
                // // triangle.GetAllPixelsInTriangle(targetWidth, targetHeight, ViewPort.Z);
                outputPixels.Add(new[] { pixel[0], pixel[1] }, color);
                // bitmap.SetPixel(pixel[0], pixel[1], color);
                if (pixel[0] < 0 || pixel[0] > 1900 || pixel[1] < 0 || pixel[1] > 1000)
                {
                    continue;
                }
                bitmap.SetPixel(pixel[0], pixel[1], color);
            }
        }
        // #endregion

        var bitmapKeys = outputPixels.Keys;
        foreach(var key in bitmapKeys)
        {
            // try
            // {
            // if (key[0] < 0 || key[0] > 1900 || key[1] < 0 || key[1] > 1000)
            // {
            //     continue;
            // }
            // bitmap.SetPixel(key[0], key[1], outputPixels[key]);
            // }
            // catch(Exception e)
            // {
            //     Console.WriteLine(key[0]);
            //     Console.WriteLine(key[1]);
            //
            //     throw e;
            // }
        }
        return bitmap;
    }
    
    private Point GetPointFromPixelOnBitmap(int pixelX, int pixelY, int targetWidth, int targetHeight)
    {
        double x = (double)(pixelX - targetWidth/2.0) / 50.0;
        double y = (double)(pixelY - targetHeight/2.0) / -50.0;
        // TODO: change everywhere to static
        double z = ViewPort.Z;
        return new Point(x, y, z);
    }

    private Vector GetVectorFromObserverToPointOfPixel(Point pointOfPixel)
    {
        return new Vector(pointOfPixel.CurrentPosition.X, pointOfPixel.CurrentPosition.Y,
            pointOfPixel.CurrentPosition.Z);
    }

    private Point3D GetCommonPointOfTriangleAndStraight(Vector vector, double[] plane)
    {
        double t = -plane[3] / (plane[0] * vector.X + plane[1] * vector.Y + plane[2] * vector.Z);
        return new Point3D(vector.X*t, vector.Y*t, vector.Z*t);
    }

    private Point GetPointOnSphere(double x, double y, double z, double r, double a, double b, double c)
    {
        double[] primary = new double[10];
        double[] factors = new double[3];
        primary[0] = a * a;
        primary[1] = (-2) * a * x;
        primary[2] = x * x;
        primary[3] = b * b;
        primary[4] = (-2) * b * y;
        primary[5] = y * y;
        primary[6] = c * c;
        primary[7] = (-2) * c * z;
        primary[8] = z * z;
        primary[9] = r * r;

        factors[0] = primary[0] + primary[3] + primary[6];
        factors[1] = primary[1] + primary[4] + primary[7];
        factors[2] = primary[2] + primary[5] + primary[8] - primary[9];

        double delta = factors[1] * factors[1] - 4 * factors[0] * factors[2];
        double t1 = 0.0;
        double t2 = 0.0;
        Boolean oneT = false;
        if (delta < 0)
        {
            return null;
        }

        if (delta < BSPTreeBuilder.Epsilon && delta > -BSPTreeBuilder.Epsilon)
        {
            t1 = -factors[1] / (2 * factors[0]);
            oneT = true;
        }
        else
        {
            t1 = (-factors[1] - Math.Sqrt(delta))/(2*factors[0]);
            t2 = (-factors[1] + Math.Sqrt(delta))/(2*factors[0]);
        }

        if (oneT)
        {
            return new Point(a * t1, b * t1, c * t1);
        }
        
        Point p1 = new Point(a * t1, b * t1, c * t1);
        Point p2 = new Point(a * t2, b * t2, c * t2);

        if (p1.CurrentPosition.Z > p2.CurrentPosition.Z)
        {
            return p2;
        }
        else
        {
            return p1;
        }
    }
}