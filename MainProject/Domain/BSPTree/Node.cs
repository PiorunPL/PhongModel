using MainProject.Domain.Basic;

namespace MainProject.Domain.BSPTree;

public class Node
{
    public List<Triangle> Triangles = new List<Triangle>();
    public Node? Front;
    public Node? Back;

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
}