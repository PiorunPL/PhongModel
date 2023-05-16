using MainProject.Domain.Basic;
using MainProject.Domain.BSPTree;
using MainProject.Domain.CameraRelated;
using MainProject.Domain.WorldRelated;
using MainProject.Utility;
using SkiaSharp;
using Matrix4x4 = Accord.Math.Matrix4x4;

namespace MainProject.Controller;

public class Controller
{
    private readonly WorldTriangles _world = new();
    private readonly Camera _camera = new Camera();
    private readonly List<Matrix4x4> _matrices = new List<Matrix4x4>();
    private readonly BSPTreeBuilder _bspTreeBuilder = new BSPTreeBuilder();
    private Node _BSPTreeRoot;

    public Controller()
    {
        _world.SetUpBasicWorld();
        Console.WriteLine("before get best bsp: " + _world.Triangles.Count);
        Node? tempNode = _bspTreeBuilder.GetBestBSPTree(_world.Triangles, 80);
        // Console.WriteLine(tempNode.GetMaxDepth());s
        // Console.WriteLine("bsp tree: " + tempNode);
        // Console.WriteLine("bsp tree Back node: " + tempNode.Back);
        // Console.WriteLine("bsp tree Front node: " + tempNode.Front);
        
        // Node.TraverseAndWriteOutput(tempNode);
        if (tempNode == null)
            throw new ApplicationException();
        _BSPTreeRoot = tempNode;
        
        //Update world, after dividing triangles
        //Important! First Add new Triangles, then remove old ones
        Console.WriteLine("before add range: " + _world.Triangles.Count);
        _world.Triangles.AddRange(_bspTreeBuilder.newTrianglesToWorld);
        Console.WriteLine("after add range: " + _world.Triangles.Count);
        _world.Points.AddRange(_bspTreeBuilder.newPointToWorld);
        foreach (var triangle in _bspTreeBuilder.trianglesToRemoveFromWorld)
        {
            _world.Triangles.Remove(triangle);
        }
        Console.WriteLine("after remove: " + _world.Triangles.Count);
        
    }
    
    public SKBitmap CreatePhoto()
    {
        if (_matrices.Count != 0)
        {
            Matrix4x4 resultMatrix = _matrices[0];
            for (int i = 1; i < _matrices.Count; i++)
            { 
                resultMatrix = Matrix4x4.Multiply(resultMatrix, _matrices[i]);
            }
            
            List<Point> points = _world.Points;
            foreach (var point in points)
            {
                var vector = point.GetVector();

                var newVector = Matrix4x4.Multiply(resultMatrix, vector);
            
                point.LoadCoordinatesFromVector(newVector);
            }
            
            _matrices.Clear();
        }
        
        //TODO: Order Triangles
        PainingAlgorithOrder PAO = new PainingAlgorithOrder();
        PAO.CreateTrianglesOrder(_BSPTreeRoot);
        Console.WriteLine("before size: " + _world.Triangles.Count);
        Console.WriteLine("order size: " + PAO.Order.Count);
        _world.Triangles = PAO.Order;
        // _world.Triangles.Reverse();
        _camera.PassActualWorld(_world.Triangles);
        var result = _camera.CreatePhotoTriangles();

        return result;
    }

    public void ZoomIn(double t)
    {
        Console.WriteLine($"Before Zoom In: {_camera.ViewPort.Z} with t = {t}");
        _camera.ViewPort.Z += t;
        Console.WriteLine($"After Zoom In: {_camera.ViewPort.Z}");
       
    }

    public void ZoomOut(double t)
    {
        if (_camera.ViewPort.Z - t <= 0.2)
            return;
        Console.WriteLine($"Before Zoom Out: {_camera.ViewPort.Z} with t = {t}");
        _camera.ViewPort.Z -= t;
        Console.WriteLine($"After Zoom Out: {_camera.ViewPort.Z}");
    }

    public void GoForward(double t)
    {
        var matrix = Translation(0, 0, (float)-t);
        _matrices.Insert(0, matrix);
    }

    public void GoBackward(double t)
    {
        var matrix = Translation(0, 0, (float)t);
        _matrices.Insert(0, matrix);
    }

    public void GoLeft(double t)
    {
        var matrix = Translation((float)t,0 , 0);
        _matrices.Insert(0, matrix);
    }

    public void GoRight(double t)
    {
        var matrix = Translation((float)-t, 0, 0);
        _matrices.Insert(0, matrix);
    }

    public void GoDown(double t)
    {
        var matrix = Translation(0, (float)t,0 );
        _matrices.Insert(0, matrix);
    }

    public void GoUp(double t)
    {
        var matrix = Translation(0, (float)-t,0);
        _matrices.Insert(0, matrix);
    }

    public void TurnLeft(double t)
    {
        Matrix4x4 matrix = new Matrix4x4
        {
            V00 = (float)Math.Cos(t), V01 = 0, V02 = (float)Math.Sin(t), V03 = 0,
            V10 = 0, V11 = 1, V12 = 0, V13 = 0,
            V20 = (float)(-1*Math.Sin(t)), V21 = 0, V22 = (float)Math.Cos(t), V23 = 0,
            V30 = 0, V31 = 0, V32 = 0, V33 = 1
        };
        
        _matrices.Insert(0, matrix);
    }

    public void TurnRight(double t)
    {
        TurnLeft(-t);
    }

    public void TurnUp(double t)
    {
        Matrix4x4 matrix = new Matrix4x4
        {
            V00 = 1, V01 = 0, V02 = 0, V03 = 0,
            V10 = 0, V11 = (float)Math.Cos(t), V12 = (float)(-1 * Math.Sin(t)), V13 = 0,
            V20 = 0, V21 = (float)Math.Sin(t), V22 = (float)Math.Cos(t), V23 = 0,
            V30 = 0, V31 = 0, V32 = 0, V33 = 1
        };
        
        _matrices.Insert(0, matrix);
    }

    public void TurnDown(double t)
    {
        t = -t;
        
        Matrix4x4 matrix = new Matrix4x4
        {
            V00 = 1, V01 = 0, V02 = 0, V03 = 0,
            V10 = 0, V11 = (float)Math.Cos(t), V12 = (float)(-1 * Math.Sin(t)), V13 = 0,
            V20 = 0, V21 = (float)Math.Sin(t), V22 = (float)Math.Cos(t), V23 = 0,
            V30 = 0, V31 = 0, V32 = 0, V33 = 1
        };

        _matrices.Insert(0, matrix);
    }

    public void TurnClockwise(double t)
    {
        Matrix4x4 matrix = new Matrix4x4
        {
            V00 = (float)Math.Cos(t), V01 =(float)(-1 * Math.Sin(t)), V02 = 0, V03 = 0,
            V10 = (float)Math.Sin(t), V11 = (float)Math.Cos(t), V12 = 0, V13 = 0,
            V20 = 0, V21 = 0, V22 = 1, V23 = 0,
            V30 = 0, V31 = 0, V32 = 0, V33 = 1
        };
        
        _matrices.Insert(0, matrix);
    }

    public void TurnCounterClockwise(double t)
    {
        TurnClockwise(-t);
    }

    private Matrix4x4 Translation(float x, float y, float z)
    {
        Matrix4x4 matrix = new Matrix4x4
        {
            V00 = 1, V01 = 0, V02 = 0, V03 = x,
            V10 = 0, V11 = 1, V12 = 0, V13 = y,
            V20 = 0, V21 = 0, V22 = 1, V23 = z,
            V30 = 0, V31 = 0, V32 = 0, V33 = 1
            
        };
        return matrix;
    }
    

}