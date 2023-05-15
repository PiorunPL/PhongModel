using MainProject.Domain.Basic;

namespace MainProject.Domain.BSPTree;

public class Node
{
    public List<Triangle> Triangles = new List<Triangle>();
    public Node? Front;
    public Node? Back;
}