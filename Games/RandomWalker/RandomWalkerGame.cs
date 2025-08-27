using NaturalSystems.GameEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NaturalSystems.Games.RandomWalker;

public sealed partial class RandomWalkerGame : IGame
{
    private static readonly Size DefaultCanvasPixelSize = new(20, 20); // Each canvas pixel is 20x20 DIPs.
    private static readonly Size DefaultCanvasGridSize = new(50, 50); // The canvas is 20x20 canvas pixels.
    private static readonly Size DefaultWalkerSize = new(1, 1); // The walker is 1x1 canvas pixels.
    private static readonly Point DefaultWalkerPosition = Point.Empty;
    private static readonly Color DefaultCanvasBackgroundColor = Color.White;
    private static readonly Color DefaultWalkerBackgroundColor = Color.Red;
    private static readonly TimeSpan UpdateInterval = TimeSpan.FromMilliseconds(50); // one move every 200ms
    private static readonly int StepSize = 1;

    private readonly Size CanvasSize;
    private readonly Color CanvasBackgroundColor;
    private readonly Walker RandomWalker;
    private readonly List<MoveOperation> Moves = [];
    private readonly HashSet<IDrawable> _priorWalkers = [];

    private TimeSpan _timeSinceLastUpdate = TimeSpan.Zero;

    private bool HasNextMove => Moves.Count > 0;

    public RandomWalkerGame()
    {
        CanvasSize = DefaultCanvasGridSize;
        CanvasBackgroundColor = DefaultCanvasBackgroundColor;
        RandomWalker = new(DefaultWalkerSize, DefaultWalkerPosition, DefaultWalkerBackgroundColor);
    }

    public void UpdateState(GameUpdateContext context)
    {
        _timeSinceLastUpdate += context.DeltaTime;
        if (_timeSinceLastUpdate > UpdateInterval)
        {
            var input = context.Input;
            if (input.IsPressed(InputButtons.Left))
                Moves.Add(new(MoveDirection.Left, 1));
            if (input.IsPressed(InputButtons.Right))
                Moves.Add(new(MoveDirection.Right, 1));
            if (input.IsPressed(InputButtons.Up))
                Moves.Add(new(MoveDirection.Forward, 1));
            if (input.IsPressed(InputButtons.Down))
                Moves.Add(new(MoveDirection.Backward, 1));

            if (!HasNextMove)
            {
                //AddRandomMoveOperations();
                AddRandom8WayMoveOperation();
            }
        }

        bool moved = TryProcessNextMove();
        if (moved)
        {
            _timeSinceLastUpdate = TimeSpan.Zero;
        }
    }

    private bool TryProcessNextMove()
    {
        if (Moves.Count == 0)
        {
            return false;
        }

        MoveWalker(Moves.First());
        Moves.RemoveAt(0);

        return true;
    }

    private void Add4WayCoinFlipMoveOperation()
    {
        Random random = new();
        int flip1 = random.Next(0, 2);
        int flip2 = random.Next(0, 2);

        MoveDirection direction = isHeads(flip1)
            ? (isHeads(flip2) ? MoveDirection.Forward : MoveDirection.Right)
            : (isHeads(flip2) ? MoveDirection.Left : MoveDirection.Backward);

        Moves.Add(new(direction, 1));

        static bool isHeads(int c) => c == 0;
    }

    private void AddRandom8WayMoveOperation()
    {
        Random random = new();
        MoveDirection direction = (MoveDirection)random.Next(0, Enum.GetNames(typeof(MoveDirection)).Length);
        Moves.Add(new(direction, 1));
    }

    private void AddRandomMoveOperations()
    {
        // Which directions are valid?
        List<MoveDirection> validDirections = [];
        if (RandomWalker.Position.X > 0)
        {
            validDirections.Add(MoveDirection.Left);
        }
        if (RandomWalker.Position.Y > 0)
        {
            validDirections.Add(MoveDirection.Forward);
        }
        if (RandomWalker.Position.X < CanvasSize.Width - 1)
        {
            validDirections.Add(MoveDirection.Right);
        }
        if (RandomWalker.Position.Y < CanvasSize.Height - 1)
        {
            validDirections.Add(MoveDirection.Backward);
        }

        // Pick a direction (random)
        Random r = new();
        int directionIdx = r.Next(0, validDirections.Count);
        MoveDirection direction = validDirections[directionIdx];

        // How many steps are valid?
        int maxMoves = direction switch
        {
            MoveDirection.Left => RandomWalker.Position.X,
            MoveDirection.Right => (CanvasSize.Width - RandomWalker.Dimensions.Width) - RandomWalker.Position.X,
            MoveDirection.Forward => RandomWalker.Position.Y,
            MoveDirection.Backward => (CanvasSize.Height - RandomWalker.Dimensions.Height) - RandomWalker.Position.Y,
            _ => 0
        };

        // Pick a step amount
        int moves = r.Next(1, maxMoves + 1);

        for (int i = 0; i < moves; i++)
        {
            Moves.Add(new(direction, 1));
        }
    }

    private void MoveWalker(MoveOperation moveOperation)
    {
        MoveWalker(moveOperation.Direction, moveOperation.Distance);
    }
    
    private void MoveWalker(MoveDirection direction, int distance)
    {
        int steps = distance * StepSize;
        Point oldPosition = RandomWalker.Position;
        Point newPosition = direction switch
        {
            MoveDirection.Left => new Point(oldPosition.X - steps, oldPosition.Y),
            MoveDirection.Right => new Point(oldPosition.X + steps, oldPosition.Y),
            MoveDirection.Forward => new Point(oldPosition.X, oldPosition.Y - steps),
            MoveDirection.Backward => new Point(oldPosition.X, oldPosition.Y + steps),
            MoveDirection.ForwardLeft => new Point(oldPosition.X - steps, oldPosition.Y - steps),
            MoveDirection.ForwardRight => new Point(oldPosition.X + steps, oldPosition.Y - steps),
            MoveDirection.BackwardLeft => new Point(oldPosition.X - steps, oldPosition.Y + steps),
            MoveDirection.BackwardRight => new Point(oldPosition.X + steps, oldPosition.Y + steps),
            _ => oldPosition
        };

        int clampedX = Math.Clamp(newPosition.X, 0, CanvasSize.Width - RandomWalker.Dimensions.Width);
        int clampedY = Math.Clamp(newPosition.Y, 0, CanvasSize.Height - RandomWalker.Dimensions.Height);
        RandomWalker.Position = new Point(clampedX, clampedY);
    }

    public GameFrame GetNextGameFrame()
    {
        // Adjust to account for a simulated pixel size
        Size canvasDimensions = new(CanvasSize.Width * DefaultCanvasPixelSize.Width, CanvasSize.Height * DefaultCanvasPixelSize.Height);
        Point walkerPosition = new(RandomWalker.Position.X * DefaultCanvasPixelSize.Width, RandomWalker.Position.Y * DefaultCanvasPixelSize.Height);
        Size walkerDimensions = new(RandomWalker.Dimensions.Width * DefaultCanvasPixelSize.Width, RandomWalker.Dimensions.Height * DefaultCanvasPixelSize.Height);

        RectangleDrawable gameBorder = new(new(Point.Empty, canvasDimensions), CanvasBackgroundColor);
        RectangleDrawable walker = new(new(walkerPosition, walkerDimensions), RandomWalker.Color);
        GameFrame nextFrame = new([gameBorder, .. _priorWalkers, walker]);

        // Save a differently colored rect.
        walker.Color = RandomVibrantColor(new());
        _priorWalkers.Add(walker);

        return nextFrame;
    }

    public static Color RandomVibrantColor(Random random)
    {
        double hue = random.NextDouble() * 360.0;
        return FromHsv(hue, 0.8, 0.9); // high saturation, high brightness
    }

    private static Color FromHsv(double hue, double saturation, double value)
    {
        int hi = (int)(hue / 60) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);

        value *= 255;
        byte v = (byte)value;
        byte p = (byte)(value * (1 - saturation));
        byte q = (byte)(value * (1 - f * saturation));
        byte t = (byte)(value * (1 - (1 - f) * saturation));

        return hi switch
        {
            0 => Color.FromArgb(255, v, t, p),
            1 => Color.FromArgb(255, q, v, p),
            2 => Color.FromArgb(255, p, v, t),
            3 => Color.FromArgb(255, p, q, v),
            4 => Color.FromArgb(255, t, p, v),
            _ => Color.FromArgb(255, v, p, q),
        };
    }
}
