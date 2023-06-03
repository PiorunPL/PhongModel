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

    public Light(Point centerPosition, double red, double green, double blue)
    {
        CenterPosition = centerPosition;
        Red = red <= 255 ? (red >= 0 ? red : 0) : 255;
        Green = green <= 255 ? (green >= 0 ? green : 0) : 255;
        Blue = blue <= 255 ? (blue >= 0 ? blue : 0) : 255;
    }
}