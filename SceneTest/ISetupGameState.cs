namespace SceneTest
{
    interface ISetupGameState : IGameState
    {
        void Link(IGameState nextState);
    }
}