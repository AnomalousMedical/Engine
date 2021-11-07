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
        private readonly ISharpGui sharpGui;
        private readonly IDebugGui debugGui;
        private readonly IRootMenu rootMenu;
        
        private IExplorationSubMenu currentMenu = null;

        public IDebugGui DebugGui => debugGui;
        public IRootMenu RootMenu => rootMenu;

        public ExplorationMenu(ISharpGui sharpGui, IDebugGui debugGui, IRootMenu rootMenu)
        {
            this.sharpGui = sharpGui;
            this.debugGui = debugGui;
            this.rootMenu = rootMenu;
        }

        /// <summary>
        /// Update the menu. Returns true if something was done. False if nothing was done and the menu wasn't shown
        /// </summary>
        /// <returns></returns>
        public bool Update(ExplorationGameState explorationGameState)
        {
            bool handled = false;
            if (currentMenu != null)
            {
                handled = true;
                currentMenu.Update(explorationGameState, this);
            }
            else
            {
                if (sharpGui.GamepadButtonEntered == Engine.Platform.GamepadButtonCode.XInput_Y || sharpGui.KeyEntered == Engine.Platform.KeyboardButtonCode.KC_TAB)
                {
                    RequsetSubMenu(rootMenu);
                    handled = true;
                }
            }
            return handled;
        }

        public void RequsetSubMenu(IExplorationSubMenu subMenu)
        {
            currentMenu = subMenu;
        }
    }
}
