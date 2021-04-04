using Engine;
using Engine.Platform;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class DebugGui : IDebugGui
    {
        private readonly ISharpGui sharpGui;
        private readonly IScaleHelper scaleHelper;
        private readonly IScreenPositioner screenPositioner;
        private readonly ILevelManager levelManager;
        private readonly ITimeClock timeClock;
        private readonly ICoroutineRunner coroutineRunner;
        SharpButton goNextLevel = new SharpButton() { Text = "Next Level" };
        SharpButton goPreviousLevel = new SharpButton() { Text = "Previous Level" };
        SharpButton toggleCamera = new SharpButton() { Text = "Toggle Camera" };
        SharpButton battle = new SharpButton() { Text = "Battle" };
        SharpSliderHorizontal currentHour;

        public DebugGui
        (
            ISharpGui sharpGui,
            IScaleHelper scaleHelper,
            IScreenPositioner screenPositioner,
            ILevelManager levelManager,
            ITimeClock timeClock,
            ICoroutineRunner coroutineRunner
        )
        {
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.screenPositioner = screenPositioner;
            this.levelManager = levelManager;
            this.timeClock = timeClock;
            this.coroutineRunner = coroutineRunner;
            currentHour = new SharpSliderHorizontal() { Rect = scaleHelper.Scaled(new IntRect(100, 10, 500, 35)), Max = 24 };
        }

        public IDebugGui.Result Update()
        {
            var result = IDebugGui.Result.None;

            var layout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(300),
                new ColumnLayout(battle, goNextLevel, goPreviousLevel, toggleCamera) { Margin = new IntPad(10) }
                ));
            var desiredSize = layout.GetDesiredSize(sharpGui);
            layout.SetRect(screenPositioner.GetBottomRightRect(desiredSize));

            if (sharpGui.Button(battle, navUp: toggleCamera.Id, navDown: goNextLevel.Id))
            {
                result = IDebugGui.Result.StartBattle;
            }

            if (!levelManager.ChangingLevels && sharpGui.Button(goNextLevel, navUp: battle.Id, navDown: goPreviousLevel.Id))
            {
                coroutineRunner.RunTask(levelManager.GoNextLevel());
            }

            if (!levelManager.ChangingLevels && sharpGui.Button(goPreviousLevel, navUp: goNextLevel.Id, navDown: toggleCamera.Id))
            {
                coroutineRunner.RunTask(levelManager.GoPreviousLevel());
            }

            if (sharpGui.Button(toggleCamera, navUp: goPreviousLevel.Id, navDown: battle.Id))
            {
                //useFirstPersonCamera = !useFirstPersonCamera;
            }

            int currentTime = (int)(timeClock.CurrentTimeMicro * Clock.MicroToSeconds / (60 * 60));
            if (sharpGui.Slider(currentHour, ref currentTime) || sharpGui.ActiveItem == currentHour.Id)
            {
                timeClock.CurrentTimeMicro = (long)currentTime * 60L * 60L * Clock.SecondsToMicro;
            }
            var time = TimeSpan.FromMilliseconds(timeClock.CurrentTimeMicro * Clock.MicroToMilliseconds);
            sharpGui.Text(currentHour.Rect.Right, currentHour.Rect.Top, timeClock.IsDay ? Engine.Color.Black : Engine.Color.White, $"Time: {time}");

            return result;
        }
    }
}
