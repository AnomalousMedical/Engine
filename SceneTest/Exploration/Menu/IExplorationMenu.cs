namespace SceneTest.Exploration.Menu
{
    interface IExplorationMenu
    {
        IDebugGui DebugGui { get; }
        IRootMenu RootMenu { get; }

        void RequestSubMenu(IExplorationSubMenu subMenu);
        bool Update(ExplorationGameState explorationGameState);
    }
}