using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Logging;

namespace PCPlatform
{
    class PCKeyboard : KeyboardHardware
    {
        public PCKeyboard(Keyboard keyboard)
            : base(keyboard)
        {
            
        }

        /// <summary>
        /// Fire a key pressed event.
        /// </summary>
        internal void _fireKeyPressed(KeyboardButtonCode keyCode, uint keyChar)
        {
            fireKeyPressed(keyCode, keyChar);
        }

        /// <summary>
        /// Fire a key released event.
        /// </summary>
        internal void _fireKeyReleased(KeyboardButtonCode keyCode)
        {
            fireKeyReleased(keyCode);
        }

        internal void _fireReleaseAllKeys()
        {
            fireReleaseAllKeys();
        }
    }
}
