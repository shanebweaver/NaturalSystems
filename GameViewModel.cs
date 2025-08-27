using NaturalSystems.GameEngine;
using NaturalSystems.Games.RandomWalker;

namespace NaturalSystems;

public sealed class GameViewModel
{
    public IGame Game { get; } = new RandomWalkerGame();
}
