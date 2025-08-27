using System;

namespace NaturalSystems.GameEngine;

public sealed class GameUpdateContext
{
    public TimeSpan DeltaTime { get; }

    public InputState Input { get; }

    public GameUpdateContext(TimeSpan deltaTime, InputState input)
    {
        DeltaTime = deltaTime;
        Input = input;
    }
}