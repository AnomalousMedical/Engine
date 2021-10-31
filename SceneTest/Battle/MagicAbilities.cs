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
        private SharpButton cureButton = new SharpButton() { Text = "Cure" };

        public MagicAbilities(IBattleScreenLayout battleScreenLayout, IBattleManager battleManager)
        {
            this.battleScreenLayout = battleScreenLayout;
            this.battleManager = battleManager;
        }

        public bool UpdateGui(ISharpGui sharpGui, IScopedCoroutine coroutine, ref BattlePlayer.MenuMode menuMode, Action<IBattleTarget, ISpell> spellSelectedCb)
        {
            var didSomething = false;

            battleScreenLayout.LayoutBattleMenu(cureButton);

            if (sharpGui.Button(cureButton))
            {
                coroutine.RunTask(async () =>
                {
                    var target = await battleManager.GetTarget(true);
                    if (target != null)
                    {
                        spellSelectedCb(target, new Spells.Cure());
                    }
                });
                menuMode = BattlePlayer.MenuMode.Root;
                didSomething = true;
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
