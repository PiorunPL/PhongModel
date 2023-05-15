namespace MainProject.Domain.Basic;

public class Point2D
{
    public double X;
    public double Y;

    public int x_draw = 0;
    public int y_draw = 0;

    public bool isDrawedCalculated = false;

    public (int x, int y) getPointCoordinatesBitmap(int width, int height)
    {
        if (isDrawedCalculated)
            return (x_draw, y_draw);

        x_draw = (int)(X * 50 + width / 2);
        y_draw = (int)(Y * (-50) + height / 2);
        isDrawedCalculated = true;

        return (x_draw, y_draw);
    }
}