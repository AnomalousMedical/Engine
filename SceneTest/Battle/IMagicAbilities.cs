using Engine;
using SharpGui;
using System;

namespace SceneTest.Battle
{
    interface IMagicAbilities
    {
        bool UpdateGui(ISharpGui sharpGui, IScopedCoroutine coroutine, ref BattlePlayer.MenuMode menuMode, Action<IBattleTarget, ISpell> spellSelectedCb);
    }
}