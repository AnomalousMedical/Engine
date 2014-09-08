using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Logging;

namespace PCPlatform
{
    class PCInputHandler : InputHandler, IDisposable
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

        public PCInputHandler(OSWindow windowHandle, bool foreground, bool exclusive, bool noWinKey)
        {
            this.window = windowHandle;

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

        public override KeyboardHardware createKeyboard(bool buffered, EventManager eventManager)
        {
            if( createdKeyboard == null )
	        {
		        Log.Info("Creating keyboard.");
		        createdKeyboard = new PCKeyboard(InputManager_createInputObject(nInputManager, InputType.OISKeyboard, buffered), eventManager);
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

        public override MouseHardware createMouse(bool buffered, EventManager eventManager)
        {
            if( createdMouse == null )
	        {
		        Log.Info("Creating mouse.");
		        createdMouse = new PCMouse(InputManager_createInputObject(nInputManager, InputType.OISMouse, buffered), window.WindowWidth, window.WindowHeight, eventManager);
                window.Resized += window_Resized;
	        }
	        return createdMouse;
        }

        public override void destroyMouse(MouseHardware mouse)
        {
            if( createdMouse == mouse )
	        {
                window.Resized -= window_Resized;
		        Log.Info("Destroying mouse.");
		        InputManager_destroyInputObject(nInputManager, ((PCMouse)mouse).MouseHandle);
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
