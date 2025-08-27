using System.Collections.Generic;

namespace NaturalSystems.GameEngine;

public sealed class InputState
{
    public InputButtons Buttons { get; init; } = InputButtons.None;
    public HashSet<Windows.System.VirtualKey> Keys { get; init; } = [];

    public bool IsPressed(InputButtons button) => (Buttons & button) != 0;
}
