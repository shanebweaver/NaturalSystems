namespace NaturalSystems.Games.RandomWalker;

public readonly partial struct MoveOperation
{
    public MoveDirection Direction { get; }
    public int Distance { get; }

    public MoveOperation(MoveDirection direction, int distance)
    {
        Direction = direction;
        Distance = distance;
    }
}
