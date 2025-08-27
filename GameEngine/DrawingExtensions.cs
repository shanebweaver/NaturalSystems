using System.Drawing;

namespace NaturalSystems.GameEngine;

public static class DrawingExtensions
{
    public static Windows.Foundation.Rect ToWindowsRect(this Rectangle systemDrawingRectangle)
    {
        return new(systemDrawingRectangle.X, systemDrawingRectangle.Y, systemDrawingRectangle.Width, systemDrawingRectangle.Height);
    }

    public static Windows.Foundation.Point ToWindowsPoint(this Point systemDrawingPoint)
    {
        return new(systemDrawingPoint.X, systemDrawingPoint.Y);
    }

    public static Windows.Foundation.Size ToWindowsSize(this Size systemDrawingSize)
    {
        return new(systemDrawingSize.Width, systemDrawingSize.Height);
    }

    public static Windows.UI.Color ToWindowsColor(this Color systemDrawingColor)
    {
        return Windows.UI.Color.FromArgb(
            systemDrawingColor.A,
            systemDrawingColor.R,
            systemDrawingColor.G,
            systemDrawingColor.B);
    }
}
