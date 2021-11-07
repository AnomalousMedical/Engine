namespace SceneTest.Exploration.Menu
{
    interface IExplorationMenu
    {
        IDebugGui DebugGui { get; }
        IRootMenu RootMenu { get; }

        void RequsetSubMenu(IExplorationSubMenu subMenu);
        bool Update(ExplorationGameState explorationGameState);
    }
}