using MainProject.Domain.Basic;

namespace MainProject.Domain.BSPTree;

public class Node
{
    public List<Triangle> Triangles = new List<Triangle>();
    public Triangle Plane;
    public Node? Front;
    public Node? Back;

    public Node(Triangle plane)
    {
        Plane = plane;
    }

    public int GetMaxDepth()
    {
        int frontDepth = Front != null ? Front.GetMaxDepth(2) : 1;
        int backDepth = Back != null ? Back.GetMaxDepth(2) : 1;

        if (frontDepth > backDepth)
            return frontDepth;
        else
            return backDepth;
    }

    public int GetMaxDepth(int currentDepth)
    {
        int frontDepth = Front != null ? Front.GetMaxDepth(currentDepth+1) : currentDepth;
        int backDepth = Back != null ? Back.GetMaxDepth(currentDepth + 1) : currentDepth;

        if (frontDepth > backDepth)
            return frontDepth;
        else
            return backDepth;
    }

    public Boolean IsLeaf() 
    {
        return Front == null && Back == null;
    }

    public Boolean IsEmpty()
    {
        return Triangles == null || Triangles.Count == 0;
    }

    public override string ToString()
    {
        string trianglesInString = "  ";
        foreach(Triangle t in Triangles)
        {
            trianglesInString += t.ToString() + "\n  ";
        }
        return String.Format("Node\nTriangles count: {1}\n  {0}", trianglesInString, Triangles.Count);
    }

    // public static void TraverseAndWriteOutput(Node node)
    // {
    //     // Console.WriteLine(indent + node.ToString());
    //     if(!node.IsLeaf()){
    //         foreach (var child in new Node[]{node.Front, node.Back})
    //         {
    //             TraverseAndWriteOutput(child, indent + "  ");
    //         }
    //     }

    // }
}