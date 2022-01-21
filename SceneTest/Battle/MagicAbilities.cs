using Engine;
using Engine.Platform;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Battle
{
    class MagicAbilities : IMagicAbilities
    {
        private readonly IBattleScreenLayout battleScreenLayout;
        private readonly IBattleManager battleManager;


        private List<SharpButton<ISpell>> spells = new List<SharpButton<ISpell>>();

        public MagicAbilities(IBattleScreenLayout battleScreenLayout, IBattleManager battleManager)
        {
            this.battleScreenLayout = battleScreenLayout;
            this.battleManager = battleManager;
        }

        public void AddSpell(ISpell spell)
        {
            var button = new SharpButton<ISpell>() { Text = spell.Name, UserObject = spell };
            this.spells.Add(button);
        }

        public void AddSpells(IEnumerable<ISpell> spells)
        {
            foreach(var spell in spells)
            {
                AddSpell(spell);
            }
        }

        public bool UpdateGui(ISharpGui sharpGui, IScopedCoroutine coroutine, ref BattlePlayer.MenuMode menuMode, Action<IBattleTarget, ISpell> spellSelectedCb)
        {
            var didSomething = false;

            var spellCount = spells.Count;
            if (spellCount > 0)
            {
                var previous = spellCount - 1;
                var next = spells.Count > 1 ? 1 : 0;

                battleScreenLayout.LayoutBattleMenu(spells);

                for (var i = 0; i < spellCount; ++i)
                {
                    if (sharpGui.Button(spells[i], navUp: spells[previous].Id, navDown: spells[next].Id))
                    {
                        var spell = spells[i].UserObject;
                        coroutine.RunTask(async () =>
                        {
                            var target = await battleManager.GetTarget(spell.DefaultTargetPlayers);
                            if (target != null)
                            {
                                spellSelectedCb(target, spell);
                            }
                        });
                        menuMode = BattlePlayer.MenuMode.Root;
                        didSomething = true;
                    }

                    previous = i;
                    next = (i + 2) % spellCount;
                }
            }

            if (!didSomething)
            {
                switch (sharpGui.GamepadButtonEntered)
                {
                    case GamepadButtonCode.XInput_B:
                        menuMode = BattlePlayer.MenuMode.Root;
                        break;
                    default:
                        //Handle keyboard
                        switch (sharpGui.KeyEntered)
                        {
                            case KeyboardButtonCode.KC_ESCAPE:
                                menuMode = BattlePlayer.MenuMode.Root;
                                break;
                        }
                        break;
                }
            }

            return didSomething;
        }
    }
}
