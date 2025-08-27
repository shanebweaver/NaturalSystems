using System.Drawing;

namespace NaturalSystems.Games.RandomWalker;

public sealed partial class Walker
{
    public Size Dimensions { get; set; }
    public Point Position { get; set; }
    public Color Color { get; set; }

    public Walker(Size dimensions, Point position, Color color)
    {
        Dimensions = dimensions;
        Position = position;
        Color = color;
    }
}
