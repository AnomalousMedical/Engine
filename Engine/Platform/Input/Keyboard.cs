using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    public delegate void KeyEvent(KeyboardButtonCode keyCode, uint keyChar);

    public class Keyboard
    {
        private KeyboardHardware keyboardHardware;

        public Keyboard(KeyboardHardware keyboardHardware)
        {
            this.keyboardHardware = keyboardHardware;
        }

        /// <summary>
        /// This event is fired when a key is pressed. Will only be fired if the
        /// keyboard is created with buffered=true.
        /// </summary>
        public event KeyEvent KeyPressed;

        /// <summary>
        /// This event is fired when a key is released. Will only be fired if the
        /// keyboard is created with buffered=true.
        /// </summary>
        public event KeyEvent KeyReleased;
        
        /// <summary>
        /// Checks to see if the given key is pressed.
        /// </summary>
        /// <param name="keyCode">The KeyboardButtonCode to check.</param>
        /// <returns>True if the key is pressed.  False if it is not.</returns>
        public bool isKeyDown(KeyboardButtonCode keyCode)
        {
            return keyboardHardware.isKeyDown(keyCode);
        }

        /// <summary>
        /// Checks to see if the given Modifier key is down.  This is Shift, Alt or Ctrl.
        /// </summary>
        /// <param name="keyCode">The Modifier key code to check.</param>
        /// <returns>True if the modifier is pressed down.  False if it is not.</returns>
        public bool isModifierDown(Modifier keyCode)
        {
            return keyboardHardware.isModifierDown(keyCode);
        }

        /// <summary>
        /// Returns the keyboard button as a string for the given code.  If shift is pressed
        /// the appropriate alt character will be returned.
        /// </summary>
        /// <param name="code">The code of the button to check for.</param>
        /// <returns>The button as a string.</returns>
        public String getAsString(KeyboardButtonCode code)
        {
            return keyboardHardware.getAsString(code);
        }

        /// <summary>
        /// Fire a key pressed event.
        /// </summary>
        internal void fireKeyPressed(KeyboardButtonCode keyCode, uint keyChar)
        {
            if (KeyPressed != null)
            {
                KeyPressed.Invoke(keyCode, keyChar);
            }
        }

        /// <summary>
        /// Fire a key released event.
        /// </summary>
        internal void fireKeyReleased(KeyboardButtonCode keyCode, uint keyChar)
        {
            if (KeyReleased != null)
            {
                KeyReleased.Invoke(keyCode, keyChar);
            }
        }

        /// <summary>
        /// Get the nice name of a key
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static String PrettyKeyName(KeyboardButtonCode code)
        {
            switch (code)
            {
                case KeyboardButtonCode.KC_RMENU:
                case KeyboardButtonCode.KC_LMENU:
                    return "Alt";
                case KeyboardButtonCode.KC_RSHIFT:
                case KeyboardButtonCode.KC_LSHIFT:
                    return "Shift";
                case KeyboardButtonCode.KC_RCONTROL:
                case KeyboardButtonCode.KC_LCONTROL:
                    return "Ctrl";
                default:
                    try
                    {
                        return code.ToString().Substring(3);
                    }
                    catch (Exception)
                    {
                        return code.ToString();
                    }
            }
        }
    }
}
