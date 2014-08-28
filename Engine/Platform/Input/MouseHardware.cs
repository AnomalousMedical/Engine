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
            eventManager.fireButtonDown(button);
        }

        protected void fireButtonUp(MouseButtonCode button)
        {
            eventManager.fireButtonUp(button);
        }

        protected void fireMoved(MouseButtonCode button)
        {
            eventManager.fireMoved(button);
        }
    }
}
