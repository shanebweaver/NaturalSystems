using System.Collections.Generic;

namespace NaturalSystems.GameEngine;

public readonly struct GameFrame
{
    public IEnumerable<IDrawable> CanvasDrawables { get; }

    public GameFrame(IEnumerable<IDrawable> drawables)
    {
        CanvasDrawables = drawables;
    }
}
