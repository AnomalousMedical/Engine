using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    public delegate void MouseCallback(Mouse mouse, MouseButtonCode buttonCode);

    /// <summary>
    /// This class allows access to the state of a mouse.
    /// </summary>
    public abstract class Mouse
    {
        /// <summary>
        /// Called when a mouse button is pressed.
        /// </summary>
        public event MouseCallback ButtonDown;

        /// <summary>
        /// Called when a mouse button is released.
        /// </summary>
        public event MouseCallback ButtonUp;

        /// <summary>
        /// Called when the mouse moves. The scroll wheel moving counts as a movement.
        /// </summary>
        public event MouseCallback Moved;

	    /// <summary>
	    /// Returns the absolute mouse position on the screen bounded by the mouse area
	    /// and 0, 0.
	    /// </summary>
	    /// <returns>The position of the mouse.</returns>
        public abstract Vector3 getAbsMouse();

	    /// <summary>
	    /// Returns the relative mouse position from the last time capture was called.
	    /// </summary>
	    /// <returns>The relative mouse position.</returns>
        public abstract Vector3 getRelMouse();

	    /// <summary>
	    /// Determines if the specified button is pressed.
	    /// </summary>
	    /// <returns>True if the button is pressed.  False if it is not.</returns>
        public abstract bool buttonDown(MouseButtonCode button);

	    /// <summary>
	    /// Captures the current state of the mouse.
	    /// </summary>
        public abstract void capture();

	    /// <summary>
	    /// Set the sensitivity of the mouse.
	    /// </summary>
	    /// <param name="sensitivity">The sensitivity to set.</param>
        public abstract void setSensitivity(float sensitivity);

	    /// <summary>
	    /// Get the width that the mouse produces input for.
	    /// </summary>
	    /// <returns>The width of the mouse area.</returns>
        public abstract float getMouseAreaWidth();

	    /// <summary>
	    /// Get the height that the mouse produces input for.
	    /// </summary>
	    /// <returns>The height of the mouse area.</returns>
        public abstract float getMouseAreaHeight();

        protected void fireButtonDown(MouseButtonCode button)
        {
            if (ButtonDown != null)
            {
                ButtonDown.Invoke(this, button);
            }
        }

        protected void fireButtonUp(MouseButtonCode button)
        {
            if (ButtonUp != null)
            {
                ButtonUp.Invoke(this, button);
            }
        }

        protected void fireMoved(MouseButtonCode button)
        {
            if (Moved != null)
            {
                Moved.Invoke(this, button);
            }
        }

        internal static string PrettyButtonName(MouseButtonCode button)
        {
            switch(button)
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
