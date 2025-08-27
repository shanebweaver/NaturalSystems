using NaturalSystems.GameEngine;
using System.Collections.Generic;
using System.Drawing;

namespace NaturalSystems.Games;

public sealed partial class TestGame : IGame
{
    private static readonly Point DefaultPosition = new(0, 0);
    private static readonly Size DefaultSize = new(1, 1);

    private Rectangle _gameRect = new(DefaultPosition, DefaultSize);

    public void UpdateState(GameUpdateContext context)
    {
        _gameRect.Offset(1,0);
    }

    public GameFrame GetNextGameFrame()
    {
        List<IDrawable> sceneDrawables = [];

        sceneDrawables.Add(new RectangleDrawable(_gameRect, Color.Red));

        GameFrame nextFrame = new(sceneDrawables);
        return nextFrame;
    }
}
