using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    /// <summary>
    /// This class provides access to a keyboard plugged into the computer.
    /// </summary>
    public abstract class KeyboardHardware
    {
        private Keyboard keyboard;

        public KeyboardHardware(Keyboard keyboard)
        {
            this.keyboard = keyboard;
        }

        /// <summary>
        /// Fire a key pressed event.
        /// </summary>
        protected void fireKeyPressed(KeyboardButtonCode keyCode, uint keyChar)
        {
            keyboard.fireKeyPressed(keyCode, keyChar);
        }

        /// <summary>
        /// Fire a key released event.
        /// </summary>
        protected void fireKeyReleased(KeyboardButtonCode keyCode)
        {
            keyboard.fireKeyReleased(keyCode);
        }

        protected void fireReleaseAllKeys()
        {
            keyboard.releaseAllKeys();
        }
    }
}
