using MainProject.Domain.Basic;

namespace MainProject.Utility;

public static class TrianglesChooser
{
    public static List<Triangle> ChooseOnlyTrianglesAhead(List<Triangle> triangles)
    {
        List<Triangle> chosenTriangles = new();
        List<Triangle> workingTriangles = triangles.ToList();

        Triangle cameraPlane = CreateCameraPlane();

        foreach (var currentTriangle in workingTriangles)
        {
            int pointsOfTriangleAhead = GetNumberOfPointsOfTriangleInFrontOfCamera(currentTriangle);
            
            if(pointsOfTriangleAhead == 3)
                chosenTriangles.Add(currentTriangle);
            else if (pointsOfTriangleAhead == 2)
            {
                BSPTreeBuilder.TrianglePosition position =
                    BSPTreeBuilder.CheckTrianglePosition(currentTriangle, cameraPlane);

                if (position == BSPTreeBuilder.TrianglePosition.ToDivideHard)
                {
                    Point sideA1, sideA2, sideB1;
                                    (sideA1, sideA2, sideB1) = BSPTreeBuilder.GetPreparedPointsForHardDivide(currentTriangle, cameraPlane);
                                    
                                    Point3D intersectionPoint1Position = BSPTreeBuilder.GetPoinfOfIntersection(sideA1.CurrentPosition, sideB1.CurrentPosition, cameraPlane);
                                    Point3D intersectionPoint1OriginalPosition =
                                        BSPTreeBuilder.GetPoinfOfIntersection(sideA1.OriginalPosition, sideB1.OriginalPosition, cameraPlane);
                                    Point intersectionPoint1 = new Point(intersectionPoint1OriginalPosition, intersectionPoint1Position);
                            
                                    Point3D intersectionPoint2Position = BSPTreeBuilder.GetPoinfOfIntersection(sideA2.CurrentPosition, sideB1.CurrentPosition, cameraPlane);
                                    Point3D intersectionPoint2OriginalPosition =
                                        BSPTreeBuilder.GetPoinfOfIntersection(sideA2.OriginalPosition, sideB1.OriginalPosition, cameraPlane);
                                    Point intersectionPoint2 = new Point(intersectionPoint2OriginalPosition, intersectionPoint2Position);
                                    
                                    Triangle t1 = new Triangle(sideA1, intersectionPoint1, intersectionPoint2);
                                    t1.color = currentTriangle.color;
                                    
                                    Triangle t2 = new Triangle(sideA1, sideA2, intersectionPoint2);
                                    t2.color = currentTriangle.color;
                                    
                                    chosenTriangles.Add(t1);
                                    chosenTriangles.Add(t2);
                }
                else
                {
                    chosenTriangles.Add(currentTriangle);
                }
            }
            else if (pointsOfTriangleAhead == 1)
            {
                BSPTreeBuilder.TrianglePosition position =
                    BSPTreeBuilder.CheckTrianglePosition(currentTriangle, cameraPlane);
                if (position == BSPTreeBuilder.TrianglePosition.ToDivideEasy)
                {
                    Point front, back, plane;
                    (front, back, plane) = BSPTreeBuilder.GetPreparedPointsForEasyDivide(currentTriangle, cameraPlane);

                    Point3D intersectionPointPosition = BSPTreeBuilder.GetPoinfOfIntersection(front.CurrentPosition, back.CurrentPosition, cameraPlane);
                    Point3D intersectionPointOriginalPosition =
                        BSPTreeBuilder.GetPoinfOfIntersection(front.OriginalPosition, back.OriginalPosition, cameraPlane);
                    Point intersectionPoint = new Point(intersectionPointOriginalPosition, intersectionPointPosition);

                    Triangle t1 = new Triangle(front, plane, intersectionPoint);
                    t1.color = currentTriangle.color;
                    
                    chosenTriangles.Add(t1);
                }
                else
                {
                    Point sideA1, sideA2, sideB1;
                    (sideA1, sideA2, sideB1) = BSPTreeBuilder.GetPreparedPointsForHardDivide(currentTriangle, cameraPlane);

                    Point3D intersectionPoint1Position = BSPTreeBuilder.GetPoinfOfIntersection(sideA1.CurrentPosition, sideB1.CurrentPosition, cameraPlane);
                    Point3D intersectionPoint1OriginalPosition =
                        BSPTreeBuilder.GetPoinfOfIntersection(sideA1.OriginalPosition, sideB1.OriginalPosition, cameraPlane);
                    Point intersectionPoint1 = new Point(intersectionPoint1OriginalPosition, intersectionPoint1Position);
        
                    Point3D intersectionPoint2Position = BSPTreeBuilder.GetPoinfOfIntersection(sideA2.CurrentPosition, sideB1.CurrentPosition, cameraPlane);
                    Point3D intersectionPoint2OriginalPosition =
                        BSPTreeBuilder.GetPoinfOfIntersection(sideA2.OriginalPosition, sideB1.OriginalPosition, cameraPlane);
                    Point intersectionPoint2 = new Point(intersectionPoint2OriginalPosition, intersectionPoint2Position);

                    Triangle t = new Triangle(intersectionPoint1, intersectionPoint2, sideB1);
                    t.color = currentTriangle.color;
                    
                    chosenTriangles.Add(t);
                }
            } 
        }

        return chosenTriangles;
    }

    private static int GetNumberOfPointsOfTriangleInFrontOfCamera(Triangle currentTriangle)
    {
        int result = 0;
        if (currentTriangle.P1.CurrentPosition.Z > 0.001)
            result++;
        if (currentTriangle.P2.CurrentPosition.Z > 0.001)
            result++;
        if (currentTriangle.P3.CurrentPosition.Z > 0.001)
            result++;

        return result;
    }

    public static Triangle CreateCameraPlane()
    {
        Point p1 = new Point(-1,-1,0.001);
        Point p2 = new Point(1,-1,0.001);
        Point p3 = new Point(0,1,0.001);
        
        Triangle result = new Triangle(p1,p2,p3);
        return result;
    }
}