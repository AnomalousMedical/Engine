using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Platform.Input
{
    public abstract class GamepadHardware
    {
        private Gamepad pad;

        public GamepadHardware(Gamepad pad)
        {
            this.pad = pad;
        }

        protected void fireButtonDown(GamepadButtonCode button)
        {
            pad.fireButtonDown(button);
        }

        protected void fireButtonUp(GamepadButtonCode button)
        {
            pad.fireButtonUp(button);
        }
    }
}
