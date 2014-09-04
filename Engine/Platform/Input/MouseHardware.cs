using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    /// <summary>
    /// This class allows access to the state of a mouse.
    /// </summary>
    public abstract class MouseHardware
    {
        private EventManager eventManager;

        public MouseHardware(EventManager eventManager)
        {
            this.eventManager = eventManager;
        }

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
        /// Returns the absolute mouse position on the screen bounded by the mouse area
        /// and 0, 0.
        /// </summary>
        /// <returns>The position of the mouse.</returns>
        public abstract Vector3 AbsolutePosition { get; }

        /// <summary>
        /// Returns the relative mouse position from the last time capture was called.
        /// </summary>
        /// <returns>The relative mouse position.</returns>
        public abstract Vector3 RelativePosition { get; }

	    /// <summary>
	    /// Get the width that the mouse produces input for.
	    /// </summary>
	    /// <returns>The width of the mouse area.</returns>
        public abstract float AreaWidth { get; }

	    /// <summary>
	    /// Get the height that the mouse produces input for.
	    /// </summary>
	    /// <returns>The height of the mouse area.</returns>
        public abstract float AreaHeight { get; }

        /// <summary>
        /// This will be true if any mouse buttons are down.
        /// </summary>
        public bool AnyButtonsDown
        {
            get
            {
                return buttonDown(MouseButtonCode.MB_BUTTON0)
                    || buttonDown(MouseButtonCode.MB_BUTTON1)
                    || buttonDown(MouseButtonCode.MB_BUTTON2)
                    || buttonDown(MouseButtonCode.MB_BUTTON3)
                    || buttonDown(MouseButtonCode.MB_BUTTON4)
                    || buttonDown(MouseButtonCode.MB_BUTTON5)
                    || buttonDown(MouseButtonCode.MB_BUTTON6)
                    || buttonDown(MouseButtonCode.MB_BUTTON7);
            }
        }

        protected void fireButtonDown(MouseButtonCode button)
        {
            eventManager.fireButtonDown(button);
        }

        protected void fireButtonUp(MouseButtonCode button)
        {
            eventManager.fireButtonUp(button);
        }

        protected void fireMoved()
        {
            eventManager.fireMoved();
        }
    }
}
