using MainProject.Domain.Basic;
using MainProject.Domain.BSPTree;
using MainProject.Domain.CameraRelated;
using MainProject.Domain.Materials;
using MainProject.Domain.WorldRelated;
using MainProject.Utility;
using SkiaSharp;
using Matrix4x4 = Accord.Math.Matrix4x4;

namespace MainProject.Controller;

public class Controller
{
    private readonly WorldSphere _world = new();
    private readonly Camera _camera = new Camera();
    private readonly List<Matrix4x4> _matrices = new List<Matrix4x4>();
    private readonly BSPTreeBuilder _bspTreeBuilder = new BSPTreeBuilder();
    private Node _BSPTreeRoot;
    private bool isSphere = false;

    public Controller()
    {
        // Triangle t = new Triangle(new Point(-1, 5, 3), new Point(1, 3, 1), new Point(0, 1, 5));
        // var plane = t.GetPlaneOfTriangle();
        
        Node? tempNode = _bspTreeBuilder.GetBestBSPTree(_world.Triangles, 1);
        
        if (tempNode == null)
            throw new ApplicationException();
        _BSPTreeRoot = tempNode;
        
        //Update world, after dividing triangles
        //Important! First Add new Triangles, then remove old ones
        _world.Triangles.AddRange(_bspTreeBuilder.NewTrianglesToWorld);
        _world.Points.AddRange(_bspTreeBuilder.NewPointToWorld);
        foreach (var triangle in _bspTreeBuilder.TrianglesToRemoveFromWorld)
        {
            _world.Triangles.Remove(triangle);
        }
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
        var orderedTriangles = PAO.Order;
        var chosenTriangles = TrianglesChooser.ChooseOnlyTrianglesAhead(orderedTriangles);
        
        _camera.PassActualWorld(chosenTriangles);
        _camera.PassAllLights(_world.Lights);
        var result = _camera.CreatePhotoTriangles(_world, isSphere);

        return result;
    }

    public void SwapSphere()
    {
        isSphere = !isSphere;
    }
    
    public void ChangeSphereMaterial()
    {
        var index = Material.AllMaterials.IndexOf(_world.Spheres[0].Material);
        index = (index + 1) % (Material.AllMaterials.Count);
            
        _world.Spheres[0].ChangeMaterial(Material.AllMaterials[index]);
    }

    public void LightXPlus(double diff)
    {
        _world.Lights[0].CenterPosition.CurrentPosition.X += diff;
    }
    
    public void LightXMinus(double diff)
    {
        _world.Lights[0].CenterPosition.CurrentPosition.X -= diff;
    }
    
    public void LightYPlus(double diff)
    {
        _world.Lights[0].CenterPosition.CurrentPosition.Y += diff;
    }
    
    public void LightYMinus(double diff)
    {
        _world.Lights[0].CenterPosition.CurrentPosition.Y -= diff;
    }
    
    public void LightZPlus(double diff)
    {
        _world.Lights[0].CenterPosition.CurrentPosition.Z += diff;
    }
    
    public void LightZMinus(double diff)
    {
        _world.Lights[0].CenterPosition.CurrentPosition.Z -= diff;
    }
    
    public void ZoomIn(double t)
    {
        Console.WriteLine($"Before Zoom In: {_camera.ViewPort.Z} with t = {t}");
        _camera.ViewPort.Z += t;
        Console.WriteLine($"After Zoom In: {_camera.ViewPort.Z}");
       
    }

    public void ZoomOut(double t)
    {
        if (_camera.ViewPort.Z - t <= 1)
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