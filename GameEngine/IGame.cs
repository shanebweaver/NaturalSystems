namespace NaturalSystems.GameEngine;

public interface IGame
{
    void UpdateState(GameUpdateContext context);

    GameFrame GetNextGameFrame();
}
