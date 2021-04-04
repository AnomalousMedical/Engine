namespace SceneTest
{
    interface IDebugGui
    {
        enum Result
        {
            None,
            StartBattle
        }

        Result Update();
    }
}