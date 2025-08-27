using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Drawing;

namespace NaturalSystems.GameEngine;

public class RectangleDrawable : IDrawable
{
    public Rectangle Dimensions { get; set; }
    public Color Color { get; set; }

    public RectangleDrawable(Rectangle rectangle, Color color)
    {
        Dimensions = rectangle;
        Color = color;
    }

    public void Draw(CanvasDrawingSession drawingSession)
    {
        Windows.Foundation.Rect rect = Dimensions.ToWindowsRect();
        Windows.UI.Color color = Color.ToWindowsColor();
        drawingSession.DrawRectangle(rect, new CanvasSolidColorBrush(drawingSession, color));
    }
}
