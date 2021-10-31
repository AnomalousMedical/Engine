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
        private SharpButton fireButton = new SharpButton() { Text = "Fire" };

        public MagicAbilities(IBattleScreenLayout battleScreenLayout, IBattleManager battleManager)
        {
            this.battleScreenLayout = battleScreenLayout;
            this.battleManager = battleManager;
        }

        public bool UpdateGui(ISharpGui sharpGui, IScopedCoroutine coroutine, ref BattlePlayer.MenuMode menuMode, Action<IBattleTarget, ISpell> spellSelectedCb)
        {
            var didSomething = false;

            battleScreenLayout.LayoutBattleMenu(cureButton, fireButton);

            if (sharpGui.Button(cureButton, navUp: fireButton.Id, navDown: fireButton.Id))
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

            if (sharpGui.Button(fireButton, navUp: cureButton.Id, navDown: cureButton.Id))
            {
                coroutine.RunTask(async () =>
                {
                    var target = await battleManager.GetTarget(false);
                    if (target != null)
                    {
                        spellSelectedCb(target, new Spells.Fire());
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
