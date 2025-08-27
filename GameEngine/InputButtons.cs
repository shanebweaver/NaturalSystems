using System;

namespace NaturalSystems.GameEngine;

[Flags]
public enum InputButtons
{
    None = 0,
    Up = 1 << 0,
    Down = 1 << 1,
    Left = 1 << 2,
    Right = 1 << 3,
    Action1 = 1 << 4,
    Action2 = 1 << 5
}
