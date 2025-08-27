using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using NaturalSystems.GameEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.System;

namespace NaturalSystems;

public sealed partial class GameCanvas : UserControl
{
    private readonly HashSet<VirtualKey> _keysDown = new();
    private readonly HashSet<VirtualKey> _keysPressedThisFrame = new();
    private readonly HashSet<VirtualKey> _keysReleasedThisFrame = new();

    public IGame? Game { get; set; }

    public GameCanvas()
    {
        InitializeComponent();
        CanvasControl.Loaded += OnCanvasControlLoaded;
        CanvasControl.Unloaded += OnCanvasControlUnloaded;
    }

    private void OnCanvasControlLoaded(object sender, RoutedEventArgs e)
    {
        CanvasControl.Focus(FocusState.Programmatic);
        StartGameLoop();
    }

    private void OnCanvasControlUnloaded(object sender, RoutedEventArgs e)
    {
        StopGameLoop();
    }

    private void OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (_keysDown.Add(e.Key))
        {
            _keysPressedThisFrame.Add(e.Key);
        }
    }

    private void OnKeyUp(object sender, KeyRoutedEventArgs e)
    {
        if (_keysDown.Remove(e.Key))
        {
            _keysReleasedThisFrame.Add(e.Key);
        }
    }

    private void StartGameLoop()
    {
        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp;
        CanvasControl.GameLoopStarting += OnGameLoopStarting;
        CanvasControl.GameLoopStopped += OnGameLoopStopped;
        CanvasControl.Draw += OnDraw;
        CanvasControl.Update += OnUpdate;
    }

    private void StopGameLoop()
    {
        CanvasControl.Draw -= OnDraw;
        CanvasControl.Update -= OnUpdate;
        CanvasControl.GameLoopStarting -= OnGameLoopStarting;
        CanvasControl.GameLoopStopped -= OnGameLoopStopped;
        KeyDown -= OnKeyDown;
        KeyUp -= OnKeyUp;
    }

    private void OnGameLoopStopped(ICanvasAnimatedControl sender, object args)
    {
        Debug.WriteLine("OnGameLoopStopped");
    }

    private void OnGameLoopStarting(ICanvasAnimatedControl sender, object args)
    {
        Debug.WriteLine("OnGameLoopStarting");
    }

    private void OnDraw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
    {
        if (Game != null)
        {
            GameFrame gameFrame = Game.GetNextGameFrame();
            GameRenderer.Render(gameFrame, args.DrawingSession);
        }
    }

    private void OnUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
    {
        if (Game != null)
        {
            TimeSpan deltaTime = args.Timing.ElapsedTime;

            InputButtons buttons = InputButtons.None;
            if (_keysDown.Contains(VirtualKey.Up)) buttons |= InputButtons.Up;
            if (_keysDown.Contains(VirtualKey.Down)) buttons |= InputButtons.Down;
            if (_keysDown.Contains(VirtualKey.Left)) buttons |= InputButtons.Left;
            if (_keysDown.Contains(VirtualKey.Right)) buttons |= InputButtons.Right;

            var input = new InputState(_keysDown, _keysPressedThisFrame, _keysReleasedThisFrame, buttons);
            var context = new GameUpdateContext(deltaTime, input);
            Game.UpdateState(context);

            _keysPressedThisFrame.Clear();
            _keysReleasedThisFrame.Clear();
        }
    }
}
