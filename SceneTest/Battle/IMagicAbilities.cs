using Engine;
using SharpGui;

namespace SceneTest
{
    interface IMagicAbilities
    {
        bool UpdateGui(ISharpGui sharpGui, IScopedCoroutine coroutine, ref BattlePlayer.MenuMode menuMode);
    }
}