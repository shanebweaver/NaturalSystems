using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NaturalSystems.GameEngine;
using System.Diagnostics;

namespace NaturalSystems;

public sealed partial class GameCanvas : UserControl
{
    public IGame? Game { get; set; }

    public GameCanvas()
    {
        InitializeComponent();
        CanvasControl.Loaded += OnCanvasControlLoaded;
        CanvasControl.Unloaded += OnCanvasControlUnloaded;
    }

    private void OnCanvasControlLoaded(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("OnLoaded");

        CanvasControl.GameLoopStarting += OnGameLoopStarting;
        CanvasControl.GameLoopStopped += OnGameLoopStopped;
        StartGameLoop();
    }

    private void OnCanvasControlUnloaded(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("OnUnloaded");
        StopGameLoop();
        CanvasControl.GameLoopStarting -= OnGameLoopStarting;
        CanvasControl.GameLoopStopped -= OnGameLoopStopped;
    }

    private void StartGameLoop()
    {
        Debug.WriteLine("StartGameLoop");
        CanvasControl.Draw += OnDraw;
        CanvasControl.Update += OnUpdate;
    }

    private void StopGameLoop()
    {
        Debug.WriteLine("StopGameLoop");
        CanvasControl.Draw -= OnDraw;
        CanvasControl.Update -= OnUpdate;
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
        Debug.WriteLine("OnDraw");
        if (Game != null)
        {
            GameFrame gameFrame = Game.GetNextGameFrame();
            GameRenderer.Render(gameFrame, args.DrawingSession);
        }
    }

    private void OnUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
    {
        Debug.WriteLine("OnUpdate");

        if (Game != null)
        {
            Game.UpdateState(args.Timing.ElapsedTime);
        }
    }
}
