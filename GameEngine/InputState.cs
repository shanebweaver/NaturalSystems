using System.Collections.Generic;

namespace NaturalSystems.GameEngine;

public sealed class InputState
{
    private readonly HashSet<Windows.System.VirtualKey> _pressed;
    private readonly HashSet<Windows.System.VirtualKey> _justPressed;
    private readonly HashSet<Windows.System.VirtualKey> _justReleased;

    public InputButtons Buttons { get; init; } = InputButtons.None;

    public InputState(
        IEnumerable<Windows.System.VirtualKey> pressed,
        IEnumerable<Windows.System.VirtualKey> justPressed,
        IEnumerable<Windows.System.VirtualKey> justReleased,
        InputButtons buttons)
    {
        _pressed = [.. pressed];
        _justPressed = [.. justPressed];
        _justReleased = [.. justReleased];
        Buttons = buttons;
    }

    public bool IsPressed(InputButtons button) => (Buttons & button) != 0;

    public bool IsDown(Windows.System.VirtualKey key) => _pressed.Contains(key);
    public bool WasJustPressed(Windows.System.VirtualKey key) => _justPressed.Contains(key);
    public bool WasJustReleased(Windows.System.VirtualKey key) => _justReleased.Contains(key);
}
