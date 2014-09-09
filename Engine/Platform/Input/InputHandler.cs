using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    public abstract class InputHandler
    {
        /// <summary>
	    /// Creates a Keyboard object linked to the system keyboard.  This keyboard is valid
	    /// until the InputHandler is destroyed.
	    /// </summary>
	    /// <param name="buffered">True if the keyboard should be buffered, which allows the keyboard events to fire.</param>
	    /// <returns>The new keyboard.</returns>
        public abstract KeyboardHardware createKeyboard(bool buffered, EventManager eventManager);

	    /// <summary>
	    /// Destroys the given keyboard.  The keyboard will be disposed after this function
	    /// call and you will no longer be able to use it.
	    /// </summary>
	    /// <param name="keyboard">The keyboard to destroy.</param>
        public abstract void destroyKeyboard(KeyboardHardware keyboard);

	    /// <summary>
	    /// Creates a Mouse object linked to the system mouse.  This mouse is valid
	    /// until the InputHandler is destroyed.
	    /// </summary>
	    /// <param name="buffered">True if the mouse should be buffered, which allows the mouse events to fire.</param>
	    /// <returns>The new mouse.</returns>
        public abstract MouseHardware createMouse(bool buffered, Mouse mouse);

	    /// <summary>
	    /// Destroys the given mouse.  The mouse will be disposed after this function
	    /// call and you will no longer be able to use it.
	    /// </summary>
	    /// <param name="mouse">The mouse to destroy.</param>
        public abstract void destroyMouse(MouseHardware mouse);

        /// <summary>
        /// Create touch hardware for the EventManager, if touch hardware cannot be supported, return null.
        /// </summary>
        /// <param name="touches"></param>
        /// <returns></returns>
        public abstract TouchHardware createTouchHardware(Touches touches);

        /// <summary>
        /// Destroy touch hardware, if you returned null from the create function this will be null.
        /// </summary>
        /// <param name="touches"></param>
        public abstract void destroyTouchHardware(TouchHardware touchHardware);
    }
}
