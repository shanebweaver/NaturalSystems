using Microsoft.Graphics.Canvas;

namespace NaturalSystems.GameEngine;

public interface IDrawable
{
    void Draw(CanvasDrawingSession drawingSession);
}
