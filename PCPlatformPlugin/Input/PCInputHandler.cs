using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Logging;

namespace PCPlatform
{
    class PCInputHandler : InputHandler, OSWindowListener, IDisposable
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

        public override Keyboard createKeyboard(bool buffered)
        {
            if( createdKeyboard == null )
	        {
		        Log.Info("Creating keyboard.");
		        createdKeyboard = new PCKeyboard(InputManager_createInputObject(nInputManager, InputType.OISKeyboard, buffered));
	        }
	        return createdKeyboard;
        }

        public override void destroyKeyboard(Keyboard keyboard)
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

        public override Mouse createMouse(bool buffered)
        {
            if( createdMouse == null )
	        {
		        Log.Info("Creating mouse.");
		        createdMouse = new PCMouse(InputManager_createInputObject(nInputManager, InputType.OISMouse, buffered), window.WindowWidth, window.WindowHeight);
		        window.addListener(this);
	        }
	        return createdMouse;
        }

        public override void destroyMouse(Mouse mouse)
        {
            if( createdMouse == mouse )
	        {
		        window.removeListener(this);
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

        public void moved(OSWindow window)
        {
            
        }

        public void resized(OSWindow window)
        {
            if(createdMouse != null)
            {
                createdMouse.windowResized(window);
            }
        }

        public void closing(OSWindow window)
        {
            
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

        [DllImport("PCPlatform")]
        private static extern IntPtr InputManager_Create(String windowHandle, bool foreground, bool exclusive, bool noWinKey);

        [DllImport("PCPlatform")]
        private static extern void InputManager_Delete(IntPtr inputManager);

        [DllImport("PCPlatform")]
        private static extern int InputManager_getNumberOfDevices(IntPtr inputManager, InputType inputType);

        [DllImport("PCPlatform")]
        private static extern uint InputManager_getVersionNumber(IntPtr inputManager);

        [DllImport("PCPlatform")]
        private static extern IntPtr InputManager_getVersionName(IntPtr inputManager);

        [DllImport("PCPlatform")]
        private static extern IntPtr InputManager_inputSystemName(IntPtr inputManager);

        [DllImport("PCPlatform")]
        private static extern IntPtr InputManager_createInputObject(IntPtr inputManager, InputType inputType, bool buffered);

        [DllImport("PCPlatform")]
        private static extern void InputManager_destroyInputObject(IntPtr inputManager, IntPtr inputObject);
    }
}
