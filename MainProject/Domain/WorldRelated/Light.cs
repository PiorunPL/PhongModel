using MainProject.Domain.Basic;

namespace MainProject.Domain.WorldRelated;

public class Light
{
    public double Red = 255;
    public double Green = 255;
    public double Blue = 255;

    public Point CenterPosition;

    public Light(Point centerPosition)
    {
        this.CenterPosition = centerPosition;
    }
}