using Engine;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Exploration.Menu
{
    class RootMenu : IRootMenu
    {
        private readonly ISharpGui sharpGui;
        private readonly IScaleHelper scaleHelper;
        private readonly IScreenPositioner screenPositioner;
        SharpButton debug = new SharpButton() { Text = "Debug" };

        public RootMenu(
            ISharpGui sharpGui,
            IScaleHelper scaleHelper,
            IScreenPositioner screenPositioner)
        {
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.screenPositioner = screenPositioner;
        }

        public void Update(IExplorationGameState explorationGameState, IExplorationMenu explorationMenu)
        {
            var layout =
               new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
               new MaxWidthLayout(scaleHelper.Scaled(300),
               new ColumnLayout(debug) { Margin = new IntPad(10) }
            ));

            var desiredSize = layout.GetDesiredSize(sharpGui);
            layout.SetRect(screenPositioner.GetBottomRightRect(desiredSize));

            if (sharpGui.Button(debug))
            {
                explorationMenu.RequsetSubMenu(explorationMenu.DebugGui);
            }
            else if (sharpGui.GamepadButtonEntered == Engine.Platform.GamepadButtonCode.XInput_B || sharpGui.KeyEntered == Engine.Platform.KeyboardButtonCode.KC_ESCAPE)
            {
                explorationMenu.RequsetSubMenu(null);
            }
        }
    }
}
