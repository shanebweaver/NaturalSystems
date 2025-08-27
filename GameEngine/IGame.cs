using System;

namespace NaturalSystems.GameEngine;

public interface IGame
{
    void UpdateState(TimeSpan deltaTime);

    GameFrame GetNextGameFrame();
}
