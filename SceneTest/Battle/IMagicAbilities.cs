using Engine;
using SharpGui;
using System;
using System.Collections.Generic;

namespace SceneTest.Battle
{
    interface IMagicAbilities
    {
        void AddSpell(ISpell spell);
        void AddSpells(IEnumerable<ISpell> spell);
        bool UpdateGui(ISharpGui sharpGui, IScopedCoroutine coroutine, ref BattlePlayer.MenuMode menuMode, Action<IBattleTarget, ISpell> spellSelectedCb);
    }
}