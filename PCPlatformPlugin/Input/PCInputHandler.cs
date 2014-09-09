using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Logging;

namespace PCPlatform
{
    public class PCInputHandler : InputHandler, IDisposable, UpdateListener
    {
        enum InputType : int
        {
            OISUnknown = 0,
            OISKeyboard = 1,
            OISMouse = 2,
            OISJoyStick = 3,
            OISTablet = 4
        };

        IntPtr nInputManager;
	    int numKeyboards;
	    int numMice;
	    int numJoysticks;
	    PCKeyboard createdKeyboard;
	    PCMouse createdMouse;
	    OSWindow window;
        UpdateTimer updateTimer;

        public PCInputHandler(OSWindow windowHandle, bool foreground, bool exclusive, bool noWinKey, UpdateTimer updateTimer)
        {
            this.window = windowHandle;
            this.updateTimer = updateTimer;
            updateTimer.addUpdateListener(this);

            nInputManager = InputManager_Create(windowHandle.WindowHandle.ToString(), foreground, exclusive, noWinKey);

	        numKeyboards = InputManager_getNumberOfDevices(nInputManager, InputType.OISKeyboard);
	        numMice = InputManager_getNumberOfDevices(nInputManager, InputType.OISMouse);
	        numJoysticks = InputManager_getNumberOfDevices(nInputManager, InputType.OISJoyStick);

	        //Log some info
	        uint v = InputManager_getVersionNumber(nInputManager);
            Log.Info("Using OIS Version: {0}.{1}.{2} \"{3}\"", (v>>16), ((v>>8) & 0x000000FF), (v & 0x000000FF), VersionName);
            Log.Info("Manager: {0}", InputSystemName);
	        Log.Info("Total Keyboards: {0}", numKeyboards);
	        Log.Info("Total Mice: {0}", numMice);
	        Log.Info("Total JoySticks: {0}", numJoysticks);
        }

        public void Dispose()
        {
            updateTimer.removeUpdateListener(this);
            if( createdMouse != null )
	        {
		        destroyMouse(createdMouse);
	        }
	        if( createdKeyboard != null )
	        {
		        destroyKeyboard(createdKeyboard);
	        }
            InputManager_Delete(nInputManager);
	        nInputManager = IntPtr.Zero;
        }

        public override KeyboardHardware createKeyboard(Keyboard keyboard)
        {
            if( createdKeyboard == null )
	        {
		        Log.Info("Creating keyboard.");
                createdKeyboard = new PCKeyboard(InputManager_createInputObject(nInputManager, InputType.OISKeyboard, true), keyboard);
                createdKeyboard.TranslationMode = PCKeyboard.TextTranslationMode.Unicode;
	        }
	        return createdKeyboard;
        }

        public override void destroyKeyboard(KeyboardHardware keyboard)
        {
            if( createdKeyboard == keyboard )
	        {
                PCKeyboard pcKeyboard = (PCKeyboard)keyboard;
                pcKeyboard.Dispose();
		        Log.Info("Destroying keyboard.");
                InputManager_destroyInputObject(nInputManager, pcKeyboard.KeyboardHandle);
		        createdKeyboard = null;
	        }
	        else
	        {
		        if( createdKeyboard == null )
		        {
			        Log.Error("OISKeyboard has already been destroyed.");
		        }
		        else
		        {
			        Log.Error("Attempted to erase keyboard that does not belong to this input manager. OISKeyboard not destroyed.");
		        }
	        }
        }

        public override MouseHardware createMouse(Mouse mouse)
        {
            if( createdMouse == null )
	        {
		        Log.Info("Creating mouse.");
		        createdMouse = new PCMouse(InputManager_createInputObject(nInputManager, InputType.OISMouse, true), window.WindowWidth, window.WindowHeight, mouse);
                window.Resized += window_Resized;
	        }
	        return createdMouse;
        }

        public override void destroyMouse(MouseHardware mouse)
        {
            if( createdMouse == mouse )
	        {
                var pcMouse = (PCMouse)mouse;
                pcMouse.Dispose();
                window.Resized -= window_Resized;
		        Log.Info("Destroying mouse.");
                InputManager_destroyInputObject(nInputManager, pcMouse.MouseHandle);
		        createdMouse = null;
	        }
	        else
	        {
		        if( createdMouse == null )
		        {
			        Log.Error("OISMouse has already been destroyed.");
		        }
		        else
		        {
			        Log.Error("Attempted to erase mouse that does not belong to this input manager. OISMouse not destroyed.");
		        }
	        }
        }

        public override TouchHardware createTouchHardware(Touches touches)
        {
            return null;
        }

        public override void destroyTouchHardware(TouchHardware touchHardware)
        {
            
        }

        void window_Resized(OSWindow window)
        {
            if (createdMouse != null)
            {
                createdMouse.windowResized(window);
            }
        }

        public String VersionName
        {
            get
            {
                return Marshal.PtrToStringAnsi(InputManager_getVersionName(nInputManager));
            }
        }

        public String InputSystemName
        {
            get
            {
                return Marshal.PtrToStringAnsi(InputManager_inputSystemName(nInputManager));
            }
        }

        public void sendUpdate(Clock clock)
        {
            if (createdKeyboard != null)
            {
                createdKeyboard.capture();
            }
            if (createdMouse != null)
            {
                createdMouse.capture();
            }
        }

        public void loopStarting()
        {

        }

        public void exceededMaxDelta()
        {

        }

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr InputManager_Create(String windowHandle, bool foreground, bool exclusive, bool noWinKey);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern void InputManager_Delete(IntPtr inputManager);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern int InputManager_getNumberOfDevices(IntPtr inputManager, InputType inputType);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern uint InputManager_getVersionNumber(IntPtr inputManager);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr InputManager_getVersionName(IntPtr inputManager);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr InputManager_inputSystemName(IntPtr inputManager);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr InputManager_createInputObject(IntPtr inputManager, InputType inputType, bool buffered);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern void InputManager_destroyInputObject(IntPtr inputManager, IntPtr inputObject);
    }
}
