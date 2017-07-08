using Engine.Platform.Input;
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
	    /// <returns>The new keyboard.</returns>
        public abstract KeyboardHardware createKeyboard(Keyboard keyboard);

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
	    /// <returns>The new mouse.</returns>
        public abstract MouseHardware createMouse(Mouse mouse);

	    /// <summary>
	    /// Destroys the given mouse.  The mouse will be disposed after this function
	    /// call and you will no longer be able to use it.
	    /// </summary>
	    /// <param name="mouse">The mouse to destroy.</param>
        public abstract void destroyMouse(MouseHardware mouse);

        /// <summary>
	    /// Creates a GamepadHardware object that manipulates the given Gamepad.
	    /// </summary>
	    /// <returns>The new gamepad.</returns>
        public abstract GamepadHardware createGamepad(Gamepad pad);

        /// <summary>
        /// Destroys the given gamepad.
        /// </summary>
        /// <param name="pad">The gamepad to destroy.</param>
        public abstract void destroyGamepad(GamepadHardware pad);

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

        /// <summary>
        /// Inject a mouse down event externally to the hardware mouse.
        /// 
        /// Since we need to treat touches as mouse events this input handler provides a way to inject
        /// mouse inputs that will follow the normal mouse path. This is only needed on touch only devices
        /// that need to simulate the mouse.
        /// </summary>
        /// <param name="code">The mouse button to simulate.</param>
        public abstract void injectButtonDown(MouseButtonCode code);

        /// <summary>
        /// Inject a mouse up event externally to the hardware mouse.
        /// 
        /// Since we need to treat touches as mouse events this input handler provides a way to inject
        /// mouse inputs that will follow the normal mouse path. This is only needed on touch only devices
        /// that need to simulate the mouse.
        /// </summary>
        /// <param name="code">The mouse button to simulate.</param>
        public abstract void injectButtonUp(MouseButtonCode code);

        /// <summary>
        /// Inject a mouse move event externally to the hardware mouse.
        /// 
        /// Since we need to treat touches as mouse events this input handler provides a way to inject
        /// mouse inputs that will follow the normal mouse path. This is only needed on touch only devices
        /// that need to simulate the mouse.
        /// </summary>
        /// <param name="x">x loc</param>
        /// <param name="y">y loc</param>
        public abstract void injectMoved(int x, int y);

        /// <summary>
        /// Inject a mouse wheel event externally to the hardware mouse.
        /// 
        /// Since we need to treat touches as mouse events this input handler provides a way to inject
        /// mouse inputs that will follow the normal mouse path. This is only needed on touch only devices
        /// that need to simulate the mouse.
        /// </summary>
        /// <param name="z">Mouse wheel</param>
        public abstract void injectWheel(int z);

        /// <summary>
        /// Inject a key pressed event. This allows us to inject keyboard info from managed code on platforms
        /// where input is handled in managed code.
        /// </summary>
        public abstract void injectKeyPressed(KeyboardButtonCode keyCode, uint keyChar);

        /// <summary>
        /// Inject a key released event. This allows us to inject keyboard info from managed code on platforms
        /// where input is handled in managed code.
        /// </summary>
        public abstract void injectKeyReleased(KeyboardButtonCode keyCode);
    }
}
