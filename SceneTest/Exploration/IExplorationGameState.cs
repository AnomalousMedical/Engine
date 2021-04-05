namespace SceneTest
{
    interface IExplorationGameState : IGameState
    {
        void Link(IGameState battleState);
    }
}