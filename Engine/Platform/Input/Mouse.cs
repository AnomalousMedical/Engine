using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    public delegate void MouseButtonCallback(Mouse mouse, MouseButtonCode buttonCode);
    public delegate void MouseCallback(Mouse mouse);

    public class Mouse
    {
        private MouseHardware mouseHardware;

        public Mouse(MouseHardware mouseHardware)
        {
            this.mouseHardware = mouseHardware;
        }

        /// <summary>
        /// Called when a mouse button is pressed.
        /// </summary>
        public event MouseButtonCallback ButtonDown;

        /// <summary>
        /// Called when a mouse button is released.
        /// </summary>
        public event MouseButtonCallback ButtonUp;

        /// <summary>
        /// Called when the mouse moves in x or y direction. Or when the wheel is scrolled in the z direction.
        /// </summary>
        public event MouseCallback Moved;

        /// <summary>
        /// Determines if the specified button is pressed.
        /// </summary>
        /// <returns>True if the button is pressed.  False if it is not.</returns>
        public bool buttonDown(MouseButtonCode button)
        {
            return mouseHardware.buttonDown(button);
        }

        /// <summary>
        /// Returns the absolute mouse position on the screen bounded by the mouse area
        /// and 0, 0.
        /// </summary>
        /// <returns>The position of the mouse.</returns>
        public Vector3 AbsolutePosition
        {
            get
            {
                return mouseHardware.AbsolutePosition;
            }
        }

        /// <summary>
        /// Returns the relative mouse position from the last time capture was called.
        /// </summary>
        /// <returns>The relative mouse position.</returns>
        public Vector3 RelativePosition
        {
            get
            {
                return mouseHardware.RelativePosition;
            }
        }

        /// <summary>
        /// Get the width that the mouse produces input for.
        /// </summary>
        public float AreaWidth
        {
            get
            {
                return mouseHardware.AreaWidth;
            }
        }

        /// <summary>
        /// Get the height that the mouse produces input for.
        /// </summary>
        public float AreaHeight
        {
            get
            {
                return mouseHardware.AreaHeight;
            }
        }

        internal void fireButtonDown(MouseButtonCode button)
        {
            if (ButtonDown != null)
            {
                ButtonDown.Invoke(this, button);
            }
        }

        internal void fireButtonUp(MouseButtonCode button)
        {
            if (ButtonUp != null)
            {
                ButtonUp.Invoke(this, button);
            }
        }

        internal void fireMoved()
        {
            if (Moved != null)
            {
                Moved.Invoke(this);
            }
        }

        internal static string PrettyButtonName(MouseButtonCode button)
        {
            switch (button)
            {
                case MouseButtonCode.MB_BUTTON0:
                    return "Left Click";
                case MouseButtonCode.MB_BUTTON1:
                    return "Right Click";
                case MouseButtonCode.MB_BUTTON2:
                    return "Mouse Button 2";
                case MouseButtonCode.MB_BUTTON3:
                    return "Mouse Button 3";
                case MouseButtonCode.MB_BUTTON4:
                    return "Mouse Button 4";
                case MouseButtonCode.MB_BUTTON5:
                    return "Mouse Button 5";
                case MouseButtonCode.MB_BUTTON6:
                    return "Mouse Button 6";
                case MouseButtonCode.MB_BUTTON7:
                    return "Mouse Button 7";
                default:
                    return "Unknown Mouse Button";
            }
        }
    }
}
