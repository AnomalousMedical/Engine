using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine.Platform.Input;

namespace Anomalous.OSPlatform
{
    public class NativeInputHandler : InputHandler, IDisposable
    {
        NativeOSWindow window;

        private NativeKeyboard createdKeyboard;
        private NativeMouse createdMouse;
        private bool enableMultitouch;

        public NativeInputHandler(NativeOSWindow window, bool enableMultitouch)
        {
            this.window = window;
            this.enableMultitouch = enableMultitouch;
        }

        public void Dispose()
        {
            if (createdKeyboard != null)
            {
                destroyKeyboard(createdKeyboard);
            }
            if (createdMouse != null)
            {
                destroyMouse(createdMouse);
            }
        }

        /// <summary>
        /// Inject a mouse down event externally to the hardware mouse.
        /// 
        /// Since we need to treat touches as mouse events this input handler provides a way to inject
        /// mouse inputs that will follow the normal mouse path. This is only needed on touch only devices
        /// that need to simulate the mouse.
        /// </summary>
        /// <param name="code">The mouse button to simulate.</param>
        public override void injectButtonDown(MouseButtonCode code)
        {
            createdMouse.injectButtonDown(code);
        }

        /// <summary>
        /// Inject a mouse up event externally to the hardware mouse.
        /// 
        /// Since we need to treat touches as mouse events this input handler provides a way to inject
        /// mouse inputs that will follow the normal mouse path. This is only needed on touch only devices
        /// that need to simulate the mouse.
        /// </summary>
        /// <param name="code">The mouse button to simulate.</param>
        public override void injectButtonUp(MouseButtonCode code)
        {
            createdMouse.injectButtonUp(code);
        }

        /// <summary>
        /// Inject a mouse move event externally to the hardware mouse.
        /// 
        /// Since we need to treat touches as mouse events this input handler provides a way to inject
        /// mouse inputs that will follow the normal mouse path. This is only needed on touch only devices
        /// that need to simulate the mouse.
        /// </summary>
        /// <param name="x">x loc</param>
        /// <param name="y">y loc</param>
        public override void injectMoved(int x, int y)
        {
            createdMouse.injectMoved(x, y);
        }

        /// <summary>
        /// Inject a mouse wheel event externally to the hardware mouse.
        /// 
        /// Since we need to treat touches as mouse events this input handler provides a way to inject
        /// mouse inputs that will follow the normal mouse path. This is only needed on touch only devices
        /// that need to simulate the mouse.
        /// </summary>
        /// <param name="z">Mouse wheel</param>
        public override void injectWheel(int z)
        {
            createdMouse.injectWheel(z);
        }

        /// <summary>
        /// Inject a key pressed event. This allows us to inject keyboard info from managed code on platforms
        /// where input is handled in managed code.
        /// </summary>
        public override void injectKeyPressed(KeyboardButtonCode keyCode, uint keyChar)
        {
            createdKeyboard.injectKeyPressed(keyCode, keyChar);
        }

        /// <summary>
        /// Inject a key released event. This allows us to inject keyboard info from managed code on platforms
        /// where input is handled in managed code.
        /// </summary>
        public override void injectKeyReleased(KeyboardButtonCode keyCode)
        {
            createdKeyboard.injectKeyReleased(keyCode);
        }

        public override KeyboardHardware createKeyboard(Keyboard keyboard)
        {
            if (createdKeyboard == null)
            {
                createdKeyboard = new NativeKeyboard(window, keyboard);
            }
            return createdKeyboard;
        }

        public override MouseHardware createMouse(Mouse mouse)
        {
            if (createdMouse == null)
            {
                createdMouse = new NativeMouse(window, mouse);
            }
            return createdMouse;
        }

        public override GamepadHardware createGamepad(Gamepad pad)
        {
            return RuntimePlatformInfo.CreateGamepadHardware(pad);
        }

        public override void destroyGamepad(GamepadHardware pad)
        {
            pad.Dispose();
        }

        public override void destroyKeyboard(KeyboardHardware keyboard)
        {
            ((NativeKeyboard)keyboard).Dispose();
            if (keyboard == createdKeyboard)
            {
                createdKeyboard = null;
            }
        }

        public override void destroyMouse(MouseHardware mouse)
        {
            ((NativeMouse)mouse).Dispose();
            if (mouse == createdMouse)
            {
                createdMouse = null;
            }
        }

        public override TouchHardware createTouchHardware(Touches touches)
        {
            if(enableMultitouch && MultiTouch.IsAvailable)
            {
                return new MultiTouch(window, touches);
            }
            return null;
        }

        public override void destroyTouchHardware(TouchHardware touchHardware)
        {
            if(touchHardware != null)
            {
                ((MultiTouch)touchHardware).Dispose();
            }
        }
    }
}
