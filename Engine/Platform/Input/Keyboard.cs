using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    public delegate void KeyDownEvent(KeyboardButtonCode keyCode, uint keyChar);
    public delegate void KeyUpEvent(KeyboardButtonCode keyCode);

    public class Keyboard
    {
        private bool[] keysDown = new bool[256];
        bool altDown = false;
        bool ctrlDown = false;
        bool shiftDown = false;

        public Keyboard()
        {
            
        }

        /// <summary>
        /// This event is fired when a key is pressed. Will only be fired if the
        /// keyboard is created with buffered=true.
        /// </summary>
        public event KeyDownEvent KeyPressed;

        /// <summary>
        /// This event is fired when a key is released. Will only be fired if the
        /// keyboard is created with buffered=true.
        /// </summary>
        public event KeyUpEvent KeyReleased;
        
        /// <summary>
        /// Checks to see if the given key is pressed.
        /// </summary>
        /// <param name="keyCode">The KeyboardButtonCode to check.</param>
        /// <returns>True if the key is pressed.  False if it is not.</returns>
        public bool isKeyDown(KeyboardButtonCode keyCode)
        {
            return keysDown[(int)keyCode];
        }

        /// <summary>
        /// Checks to see if the given Modifier key is down.  This is Shift, Alt or Ctrl.
        /// </summary>
        /// <param name="keyCode">The Modifier key code to check.</param>
        /// <returns>True if the modifier is pressed down.  False if it is not.</returns>
        public bool isModifierDown(Modifier keyCode)
        {
            switch (keyCode)
            {
                case Modifier.Shift:
                    return shiftDown;
                case Modifier.Alt:
                    return altDown;
                case Modifier.Ctrl:
                    return ctrlDown;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Fire a key pressed event.
        /// </summary>
        internal void fireKeyPressed(KeyboardButtonCode keyCode, uint keyChar)
        {
            if (!keysDown[(int)keyCode])
            {
                keysDown[(int)keyCode] = true;
                switch (keyCode)
                {
                    case KeyboardButtonCode.KC_LSHIFT:
                        shiftDown = true;
                        break;

                    case KeyboardButtonCode.KC_LMENU:
                        altDown = true;
                        break;

                    case KeyboardButtonCode.KC_LCONTROL:
                        ctrlDown = true;
                        break;
                }

                if (KeyPressed != null)
                {
                    KeyPressed.Invoke(keyCode, keyChar);
                }
            }
        }

        /// <summary>
        /// Fire a key released event.
        /// </summary>
        internal void fireKeyReleased(KeyboardButtonCode keyCode)
        {
            if (keysDown[(int)keyCode])
            {
                keysDown[(int)keyCode] = false;
                switch (keyCode)
                {
                    case KeyboardButtonCode.KC_LSHIFT:
                        shiftDown = false;
                        break;

                    case KeyboardButtonCode.KC_LMENU:
                        altDown = false;
                        break;

                    case KeyboardButtonCode.KC_LCONTROL:
                        ctrlDown = false;
                        break;
                }

                if (KeyReleased != null)
                {
                    KeyReleased.Invoke(keyCode);
                }
            }            
        }

        /// <summary>
        /// Release all keys
        /// </summary>
        internal void releaseAllKeys()
        {
            for (int i = 0; i < keysDown.Length; ++i)
            {
                fireKeyReleased((KeyboardButtonCode)i);
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
