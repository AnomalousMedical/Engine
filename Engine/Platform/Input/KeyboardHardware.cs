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
        private EventManager eventManager;

        public KeyboardHardware(EventManager eventManager)
        {
            this.eventManager = eventManager;
        }

	    /// <summary>
	    /// Checks to see if the given key is pressed.
	    /// </summary>
	    /// <param name="keyCode">The KeyboardButtonCode to check.</param>
	    /// <returns>True if the key is pressed.  False if it is not.</returns>
	    public abstract bool isKeyDown( KeyboardButtonCode keyCode );

	    /// <summary>
	    /// Checks to see if the given Modifier key is down.  This is Shift, Alt or Ctrl.
	    /// </summary>
	    /// <param name="keyCode">The Modifier key code to check.</param>
	    /// <returns>True if the modifier is pressed down.  False if it is not.</returns>
        public abstract bool isModifierDown(Modifier keyCode);

	    /// <summary>
	    /// Returns the keyboard button as a string for the given code.  If shift is pressed
	    /// the appropriate alt character will be returned.
	    /// </summary>
	    /// <param name="code">The code of the button to check for.</param>
	    /// <returns>The button as a string.</returns>
        public abstract String getAsString(KeyboardButtonCode code);
    	
	    /// <summary>
	    /// Reads the state of the keyboard.
	    /// </summary>
        public abstract void capture();

        /// <summary>
        /// Fire a key pressed event.
        /// </summary>
        protected void fireKeyPressed(KeyboardButtonCode keyCode, uint keyChar)
        {
            eventManager.fireKeyPressed(keyCode, keyChar);
        }

        /// <summary>
        /// Fire a key released event.
        /// </summary>
        protected void fireKeyReleased(KeyboardButtonCode keyCode, uint keyChar)
        {
            eventManager.fireKeyReleased(keyCode, keyChar);
        }
    }
}
