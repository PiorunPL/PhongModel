using MainProject.Domain.Basic;
using MainProject.Domain.BSPTree;

namespace MainProject.Utility;

public class BSPTreeBuilder
{
    public readonly List<Point> NewPointToWorld = new List<Point>();
    public readonly List<Triangle> NewTrianglesToWorld = new List<Triangle>();
    public readonly List<Triangle> TrianglesToRemoveFromWorld = new List<Triangle>();
    
    private const double Epsilon = 1.0E-10;
    
    public Node? GetBestBSPTree(List<Triangle> triangles, int numberOfTries)
    {
        if (numberOfTries < 0)
            return null;
        if (triangles.Count == 0)
            return null;
        
        NewPointToWorld.Clear();
        NewTrianglesToWorld.Clear();
        TrianglesToRemoveFromWorld.Clear();

        List<Node> roots = new List<Node>();

        for (int i = 0; i < numberOfTries; i++)
        {
            Node? newRoot = GenerateBSPTreeResursive(triangles.ToList());
            if(newRoot != null)
                roots.Add(newRoot);
        }
        
        return ChooseBestBSPTree(roots);
    }

    public Node? GenerateBSPTreeResursive(List<Triangle> triangles)
    {
        if (triangles.Count == 0)
            return null;
        
        Triangle randTriangle = PopRandomTriangle(triangles);
        Node node = new Node(randTriangle);
        

        List<Triangle> frontTriangles = new List<Triangle>();
        List<Triangle> backTriangles = new List<Triangle>();

        while (triangles.Count != 0)
        {
            Triangle toCheck = triangles[0];
            triangles.Remove(toCheck);

            TrianglePosition position = CheckTrianglePosition(toCheck, randTriangle);
            HandleTriangleBasedOnPosition(position, toCheck, randTriangle, node, backTriangles, frontTriangles, triangles);
        }

        node.Front = GenerateBSPTreeResursive(frontTriangles);
        node.Back = GenerateBSPTreeResursive(backTriangles);

        return node;
    }

    public void HandleTriangleBasedOnPosition(TrianglePosition position, Triangle toCheck, Triangle mainTriangle, Node node, List<Triangle> backTriangles, List<Triangle> frontTriangles, List<Triangle> allTriangles)
    {
        if(position == TrianglePosition.Behind)
            backTriangles.Add(toCheck);
        else if(position == TrianglePosition.Front)
            frontTriangles.Add(toCheck);
        else if(position == TrianglePosition.OnPlain)
            node.Triangles.Add(toCheck);
        else if(position == TrianglePosition.ToDivideEasy)
            allTriangles.AddRange(DivideTriangleEasy(toCheck, mainTriangle));
        else if(position == TrianglePosition.ToDivideHard)
            allTriangles.AddRange(DivideTriangleHard(toCheck, mainTriangle));
    }

    public List<Triangle> DivideTriangleEasy(Triangle toCheck, Triangle mainTriangle)
    {
        List<Triangle> newTriangles = new List<Triangle>();
        Point front, back, plain;
        (front, back, plain) = GetPreparedPointsForEasyDivide(toCheck, mainTriangle);

        Point3D? intersectionPointPosition = GetPoinfOfIntersection(front.CurrentPosition, back.CurrentPosition, mainTriangle);
        Point3D? intersectionPointOriginalPosition =
            GetPoinfOfIntersection(front.OriginalPosition, back.OriginalPosition, mainTriangle);

        if (intersectionPointPosition == null || intersectionPointOriginalPosition == null)
            return newTriangles;
        
        Point intersectionPoint = new Point(intersectionPointOriginalPosition, intersectionPointPosition);

        Triangle t1 = new Triangle(front, plain, intersectionPoint);
        // t1.color[0] = rand.Next(255);
        // t1.color[1] = rand.Next(255);
        // t1.color[2] = rand.Next(255);
        Triangle t2 = new Triangle(back, plain, intersectionPoint);
        // t2.color[0] = rand.Next(255);
        // t2.color[1] = rand.Next(255);
        // t2.color[2] = rand.Next(255);

        newTriangles.Add(t1);
        newTriangles.Add(t2);
        
        NewPointToWorld.Add(intersectionPoint);
        NewTrianglesToWorld.Add(t1);
        NewTrianglesToWorld.Add(t2);
        TrianglesToRemoveFromWorld.Add(toCheck);
        
        return newTriangles;
    }
    

    public static Point3D? GetPoinfOfIntersection(Point3D front, Point3D back, Triangle triangle)
    {
        Vector normal = triangle.GetNormalVector();
        double n = Vector.GetDotProduct(normal, Vector.GetVector(front, triangle.P1.CurrentPosition));
        Vector frontToBack = Vector.GetVector(front, back);
        double d = Vector.GetDotProduct(normal, frontToBack);

        if (d == 0) //Problem!
        {
            return null;
            // d = 0.001;
        }
        
        double u = n / d;
        
        
        Point3D resultPoint = new Point3D(
            front.X + u * frontToBack.X,
            front.Y + u * frontToBack.Y,
            front.Z + u * frontToBack.Z);
        return resultPoint;
    }
    public static (Point front, Point back, Point plain) GetPreparedPointsForEasyDivide(Triangle toCheck,
        Triangle mainTriangle)
    {
        //TODO: Validation
        Point front, back, plain;
        
        double pointPosition1 = mainTriangle.CheckPointPosition(toCheck.P1.CurrentPosition);
        double pointPosition2 = mainTriangle.CheckPointPosition(toCheck.P2.CurrentPosition);
        // double pointPosition3 = mainTriangle.CheckPointPosition(toCheck.P3.CurrentPosition);
        
        if (pointPosition1 < -Epsilon)
            front = toCheck.P1;
        else if (pointPosition2 < -Epsilon)
            front = toCheck.P2;
        else
            front = toCheck.P3;
        
        if (pointPosition1 > Epsilon)
            back = toCheck.P1;
        else if (pointPosition2 > Epsilon)
            back = toCheck.P2;
        else
            back = toCheck.P3;
        
        if (pointPosition1 > -Epsilon && pointPosition1 < Epsilon)
            plain = toCheck.P1;
        else if (pointPosition2 > -Epsilon && pointPosition2 < Epsilon)
            plain = toCheck.P2;
        else
            plain = toCheck.P3;

        return (front, back, plain);
    }
    
    public List<Triangle> DivideTriangleHard(Triangle toCheck, Triangle mainTriangle)
    {
        List<Triangle> newTriangles = new List<Triangle>();
        Point sideA1, sideA2, sideB1;
        (sideA1, sideA2, sideB1) = GetPreparedPointsForHardDivide(toCheck, mainTriangle);

        Point3D? intersectionPoint1Position = GetPoinfOfIntersection(sideA1.CurrentPosition, sideB1.CurrentPosition, mainTriangle);
        Point3D? intersectionPoint1OriginalPosition =
            GetPoinfOfIntersection(sideA1.OriginalPosition, sideB1.OriginalPosition, mainTriangle);
        if (intersectionPoint1Position == null || intersectionPoint1OriginalPosition == null)
                    return newTriangles;
        
        Point intersectionPoint1 = new Point(intersectionPoint1OriginalPosition, intersectionPoint1Position);
        
        Point3D? intersectionPoint2Position = GetPoinfOfIntersection(sideA2.CurrentPosition, sideB1.CurrentPosition, mainTriangle);
        Point3D? intersectionPoint2OriginalPosition =
            GetPoinfOfIntersection(sideA2.OriginalPosition, sideB1.OriginalPosition, mainTriangle);
        if (intersectionPoint2Position == null || intersectionPoint2OriginalPosition == null)
                    return newTriangles;
        
        Point intersectionPoint2 = new Point(intersectionPoint2OriginalPosition, intersectionPoint2Position);
        
        
        Triangle t1 = new Triangle(sideA1, intersectionPoint1, intersectionPoint2);
        // t1.color[0] = rand.Next(255);
        // t1.color[1] = rand.Next(255);
        // t1.color[2] = rand.Next(255);
        Triangle t2 = new Triangle(sideA1, sideA2, intersectionPoint2);
        // t2.color[1] = rand.Next(255);
        // t2.color[2] = rand.Next(255);
        // t2.color[0] = rand.Next(255);
        Triangle t3 = new Triangle(intersectionPoint1, intersectionPoint2, sideB1);
        // t3.color[0] = rand.Next(255);
        // t3.color[1] = rand.Next(255);
        // t3.color[2] = rand.Next(255);
        
        
        newTriangles.Add(t1);
        newTriangles.Add(t2);
        newTriangles.Add(t3);
        
        TrianglesToRemoveFromWorld.Add(toCheck);
        NewTrianglesToWorld.Add(t1);
        NewTrianglesToWorld.Add(t2);
        NewTrianglesToWorld.Add(t3);
        NewPointToWorld.Add(intersectionPoint1);
        NewPointToWorld.Add(intersectionPoint2);
        
        return newTriangles;
    }

    public static (Point sideA1, Point sideA2, Point sideB1) GetPreparedPointsForHardDivide(Triangle toCheck,
        Triangle mainTriangle)
    {
        Point sideA1, sideA2, sideB1;
        
        double pointPosition1 = mainTriangle.CheckPointPosition(toCheck.P1.CurrentPosition);
        double pointPosition2 = mainTriangle.CheckPointPosition(toCheck.P2.CurrentPosition);
        double pointPosition3 = mainTriangle.CheckPointPosition(toCheck.P3.CurrentPosition);

        sideA1 = toCheck.P1;
        if (pointPosition1 < -Epsilon && pointPosition2 < -Epsilon || pointPosition1 > Epsilon && pointPosition2 > Epsilon)
        {
            sideA2 = toCheck.P2;
            sideB1 = toCheck.P3;
        }
        else
        {
            sideA2 = toCheck.P3;
            
            if (pointPosition1 < -Epsilon && pointPosition3 < -Epsilon || pointPosition1 > Epsilon && pointPosition3 > Epsilon)
            {
                sideB1 = toCheck.P2;
            }
            else
            {
                sideA1 = toCheck.P2;
                sideB1 = toCheck.P1;
            }
        }

        return (sideA1, sideA2, sideB1);
    }
        

    public Triangle PopRandomTriangle(List<Triangle> triangles)
    {
        Random rnd = new();
        Triangle randTriangle = triangles[rnd.Next(triangles.Count)];
        triangles.Remove(randTriangle);
        return randTriangle;
    }

    public static TrianglePosition CheckTrianglePosition(Triangle toCheck, Triangle mainTriangle)
    {
        bool isFront = false;
        bool isBack = false;
        bool isOnPlain = false;

        double pointPosition = mainTriangle.CheckPointPosition(toCheck.P1.CurrentPosition);
        if (pointPosition < -Epsilon)
            isFront = true;
        else if (pointPosition > Epsilon)
            isBack = true;
        else
            isOnPlain = true;
        
        pointPosition = mainTriangle.CheckPointPosition(toCheck.P2.CurrentPosition);
        if (pointPosition < -Epsilon)
            isFront = true;
        else if (pointPosition > Epsilon)
            isBack = true;
        else
            isOnPlain = true;
        
        pointPosition = mainTriangle.CheckPointPosition(toCheck.P3.CurrentPosition);
        if (pointPosition < -Epsilon)
            isFront = true;
        else if (pointPosition > Epsilon)
            isBack = true;
        else
            isOnPlain = true;

        if (isFront && !isBack)
            return TrianglePosition.Front;
        if (!isFront && isBack)
            return TrianglePosition.Behind;
        if (!isFront && !isBack && isOnPlain)
            return TrianglePosition.OnPlain;
        if (isFront && isBack && !isOnPlain)
            return TrianglePosition.ToDivideHard;
        // if (isFront && isBack && isOnPlain)
        return TrianglePosition.ToDivideEasy;
    }
    
    

    public Node? ChooseBestBSPTree(List<Node> roots)
    {
        if (roots.Count == 0)
            return null;
        if (roots.Count == 1)
            return roots[0];
        
        Node bestRoot = roots[0];
        int depthOfBestRoot =bestRoot.GetMaxDepth();

        for (int i = 1; i < roots.Count; i++)
        {
            int currentDepth = roots[i].GetMaxDepth();
            if (currentDepth < depthOfBestRoot)
            {
                bestRoot = roots[i];
                depthOfBestRoot = currentDepth;
            }
        }
        Console.WriteLine("Depth of best root: " + depthOfBestRoot.ToString());
        return bestRoot;
    }

    public enum TrianglePosition
    {
        Front,
        Behind,
        OnPlain,
        ToDivideEasy,
        ToDivideHard
    }
    
}