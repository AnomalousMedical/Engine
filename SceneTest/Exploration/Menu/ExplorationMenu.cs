using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Exploration.Menu
{
    class ExplorationMenu : IExplorationMenu
    {
        private bool visible;
        private readonly ISharpGui sharpGui;
        private readonly IDebugGui debugGui;

        public ExplorationMenu(ISharpGui sharpGui, IDebugGui debugGui)
        {
            this.sharpGui = sharpGui;
            this.debugGui = debugGui;
        }

        /// <summary>
        /// Update the menu. Returns true if something was done. False if nothing was done and the menu wasn't shown
        /// </summary>
        /// <returns></returns>
        public bool Update(ExplorationGameState explorationGameState)
        {
            bool handled = visible;
            if (visible)
            {
                debugGui.Update(explorationGameState);
                if (sharpGui.GamepadButtonEntered == Engine.Platform.GamepadButtonCode.XInput_B || sharpGui.KeyEntered == Engine.Platform.KeyboardButtonCode.KC_ESCAPE)
                {
                    visible = false;
                }
            }
            else
            {
                if (sharpGui.GamepadButtonEntered == Engine.Platform.GamepadButtonCode.XInput_Y || sharpGui.KeyEntered == Engine.Platform.KeyboardButtonCode.KC_TAB)
                {
                    visible = true;
                    handled = true;
                }
            }
            return handled;
        }
    }
}
