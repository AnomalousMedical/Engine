using Engine;
using Engine.Platform;
using RpgMath;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Exploration.Menu
{
    class DebugGui : IDebugGui, IExplorationSubMenu
    {
        private readonly ISharpGui sharpGui;
        private readonly IScaleHelper scaleHelper;
        private readonly IScreenPositioner screenPositioner;
        private readonly ILevelManager levelManager;
        private readonly ITimeClock timeClock;
        private readonly ICoroutineRunner coroutineRunner;
        private readonly Party party;
        private readonly ILevelCalculator levelCalculator;
        SharpButton goNextLevel = new SharpButton() { Text = "Next Stage" };
        SharpButton goPreviousLevel = new SharpButton() { Text = "Previous Stage" };
        SharpButton goStart = new SharpButton() { Text = "Go Start" };
        SharpButton goEnd = new SharpButton() { Text = "Go End" };
        //SharpButton toggleCamera = new SharpButton() { Text = "Toggle Camera" };
        SharpButton levelUp = new SharpButton() { Text = "Level Up" };
        SharpButton battle = new SharpButton() { Text = "Battle" };
        SharpText averageLevel = new SharpText() { Color = Color.White };
        SharpSliderHorizontal currentHour;

        public DebugGui
        (
            ISharpGui sharpGui,
            IScaleHelper scaleHelper,
            IScreenPositioner screenPositioner,
            ILevelManager levelManager,
            ITimeClock timeClock,
            ICoroutineRunner coroutineRunner,
            Party party,
            ILevelCalculator levelCalculator
        )
        {
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.screenPositioner = screenPositioner;
            this.levelManager = levelManager;
            this.timeClock = timeClock;
            this.coroutineRunner = coroutineRunner;
            this.party = party;
            this.levelCalculator = levelCalculator;
            currentHour = new SharpSliderHorizontal() { Rect = scaleHelper.Scaled(new IntRect(100, 10, 500, 35)), Max = 24 };
        }

        public void Update(IExplorationGameState explorationGameState, IExplorationMenu explorationMenu)
        {
            averageLevel.Text = $"Level: {party.ActiveCharacters.GetAverageLevel()}";

            var layout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(300),
                new ColumnLayout(averageLevel, battle, levelUp, goStart, goEnd, goNextLevel, goPreviousLevel /*, toggleCamera*/) { Margin = new IntPad(10) }
            ));
            var desiredSize = layout.GetDesiredSize(sharpGui);
            layout.SetRect(screenPositioner.GetBottomRightRect(desiredSize));

            sharpGui.Text(averageLevel);

            if (sharpGui.Button(battle, navUp: goPreviousLevel.Id, navDown: levelUp.Id))
            {
                explorationGameState.RequestBattle();
                explorationMenu.RequestSubMenu(null);
            }

            if (sharpGui.Button(levelUp, navUp: battle.Id, navDown: goStart.Id))
            {
                foreach(var c in party.ActiveCharacters)
                {
                    c.CharacterSheet.LevelUp(levelCalculator);
                }
            }

            if(sharpGui.Button(goStart, navUp: levelUp.Id, navDown: goEnd.Id))
            {
                levelManager.GoStartPoint();
                explorationMenu.RequestSubMenu(null);
            }

            if (sharpGui.Button(goEnd, navUp: goStart.Id, navDown: goNextLevel.Id))
            {
                levelManager.GoEndPoint();
                explorationMenu.RequestSubMenu(null);
            }

            if (!levelManager.ChangingLevels && sharpGui.Button(goNextLevel, navUp: goEnd.Id, navDown: goPreviousLevel.Id))
            {
                coroutineRunner.RunTask(levelManager.GoNextLevel());
                explorationMenu.RequestSubMenu(null);
            }

            if (!levelManager.ChangingLevels && sharpGui.Button(goPreviousLevel, navUp: goNextLevel.Id, navDown: battle.Id))
            {
                coroutineRunner.RunTask(levelManager.GoPreviousLevel());
                explorationMenu.RequestSubMenu(null);
            }

            //if (sharpGui.Button(toggleCamera, navUp: goPreviousLevel.Id, navDown: battle.Id))
            //{
            //    useFirstPersonCamera = !useFirstPersonCamera;
            //}

            int currentTime = (int)(timeClock.CurrentTimeMicro * Clock.MicroToSeconds / (60 * 60));
            if (sharpGui.Slider(currentHour, ref currentTime) || sharpGui.ActiveItem == currentHour.Id)
            {
                timeClock.CurrentTimeMicro = (long)currentTime * 60L * 60L * Clock.SecondsToMicro;
            }
            var time = TimeSpan.FromMilliseconds(timeClock.CurrentTimeMicro * Clock.MicroToMilliseconds);
            sharpGui.Text(currentHour.Rect.Right, currentHour.Rect.Top, timeClock.IsDay ? Engine.Color.Black : Engine.Color.White, $"Time: {time}");

            if (sharpGui.GamepadButtonEntered == GamepadButtonCode.XInput_B || sharpGui.KeyEntered == KeyboardButtonCode.KC_ESCAPE)
            {
                explorationMenu.RequestSubMenu(explorationMenu.RootMenu);
            }
        }
    }
}
