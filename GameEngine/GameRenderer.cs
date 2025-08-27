using Microsoft.Graphics.Canvas;

namespace NaturalSystems.GameEngine;

public static partial class GameRenderer
{
    public static void Render(GameFrame gameFrame, CanvasDrawingSession drawingSession)
    {
        foreach (IDrawable drawable in gameFrame.CanvasDrawables)
        {
            drawable.Draw(drawingSession);
        }
    }
}
