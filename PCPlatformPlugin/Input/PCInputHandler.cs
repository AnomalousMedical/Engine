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
        WindowsMessagePump windowsPump;

        public PCInputHandler(OSWindow windowHandle, bool foreground, bool exclusive, bool noWinKey, UpdateTimer updateTimer, WindowsMessagePump windowsPump)
        {
            this.windowsPump = windowsPump;
            windowsPump.MessageReceived += windowsPump_MessageReceived;
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
            windowsPump.MessageReceived -= windowsPump_MessageReceived;
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

        private void fireKeyDown(KeyboardButtonCode keyboardButtonCode, uint p)
        {
            if(createdKeyboard != null)
            {
                
            }
        }

        private void fireKeyUp(KeyboardButtonCode keyboardButtonCode)
        {
            
        }

        private void manageCapture(MouseButtonCode mouseButtonCode)
        {
            
        }

        private void fireMouseButtonDown(MouseButtonCode mouseButtonCode)
        {
            
        }

        private void fireMouseButtonUp(MouseButtonCode mouseButtonCode)
        {
            
        }

        private void manageRelease(MouseButtonCode mouseButtonCode)
        {
            
        }

        private void fireMouseMove(int p1, int p2)
        {
            
        }

        private void fireMouseWheel(short p)
        {
            
        }

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        private const int WM_SYSCOMMAND = 0x0112;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP = 0x0205;
        private const int WM_XBUTTONDOWN = 0x020B;
        private const int WM_XBUTTONUP = 0x020C;
        private const int WM_MBUTTONDOWN = 0x0207;
        private const int WM_MBUTTONUP = 0x0208;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_MOUSEWHEEL = 0x020A;

        private const int XBUTTON1 = 0x0001;
        private const int XBUTTON2 = 0x0002;

        private const int VK_SHIFT = 0x10;
        private const int VK_CONTROL = 0x11;
        private const int VK_MENU = 0x12;
        private const int SC_KEYMENU = 0xF100;

        ushort LOWORD(IntPtr l)
        {
            return ((ushort)(((l.ToInt64())) & 0xffff));
        }

        ushort HIWORD(IntPtr l)
        {
            return ((ushort)((((l.ToInt64())) >> 16) & 0xffff));
        }

        ushort GET_XBUTTON_WPARAM(IntPtr wparam)
        {
            return HIWORD(wparam);
        }

        int GET_X_LPARAM(IntPtr lp)
        {
            return ((int)(short)LOWORD(lp));
        }
        
        int GET_Y_LPARAM(IntPtr lp)
        {
            return ((int)(short)HIWORD(lp));
        }

        short GET_WHEEL_DELTA_WPARAM(IntPtr wParam)
        {
              return ((short)HIWORD(wParam));
        }

        void windowsPump_MessageReceived(ref WinMsg message)
        {
            switch(message.message)
            {
                //Keyboard
		    case WM_KEYDOWN:
		    case WM_SYSKEYDOWN:
			    switch (message.wParam.ToInt64())
			    {
			    case VK_MENU:
				    fireKeyDown(KeyboardButtonCode.KC_LMENU, 0);
				    break;
			    case VK_CONTROL:
                    fireKeyDown(KeyboardButtonCode.KC_LCONTROL, 0);
				    break;
			    case VK_SHIFT:
                    fireKeyDown(KeyboardButtonCode.KC_LSHIFT, 0);
				    break;
			    default:
                    fireKeyDown(InputManager_virtualKeyToKeyboardButtonCode(message.wParam), InputManager_getUtf32(message.wParam, message.lParam));
				    break;
			    }
			    break;
		    case WM_KEYUP:
		    case WM_SYSKEYUP:
                switch (message.wParam.ToInt64())
			    {
			    case VK_MENU:
                    fireKeyUp(KeyboardButtonCode.KC_LMENU);
				    break;
			    case VK_CONTROL:
                    fireKeyUp(KeyboardButtonCode.KC_LCONTROL);
				    break;
			    case VK_SHIFT:
                    fireKeyUp(KeyboardButtonCode.KC_LSHIFT);
				    break;
			    default:
                    fireKeyUp(InputManager_virtualKeyToKeyboardButtonCode(message.wParam));
				    break;
			    }
			    break;
		    case WM_SYSCOMMAND:
                switch (message.wParam.ToInt64())
			    {
			    case SC_KEYMENU:
				    if (message.lParam.ToInt64() >> 16 <= 0)
				    {
                        //In the c++ version this blocks the alt key from activating the menu, not really needed here.
					    //return 0;
				    }
				    break;
			    }
			    break;
			    //Mouse
		    case WM_LBUTTONDOWN:
			    manageCapture(MouseButtonCode.MB_BUTTON0);
                fireMouseButtonDown(MouseButtonCode.MB_BUTTON0);
			    break;
		    case WM_LBUTTONUP:
                fireMouseButtonUp(MouseButtonCode.MB_BUTTON0);
                manageRelease(MouseButtonCode.MB_BUTTON0);
			    break;
		    case WM_RBUTTONDOWN:
                manageCapture(MouseButtonCode.MB_BUTTON1);
                fireMouseButtonDown(MouseButtonCode.MB_BUTTON1);
			    break;
		    case WM_RBUTTONUP:
                fireMouseButtonUp(MouseButtonCode.MB_BUTTON1);
                manageRelease(MouseButtonCode.MB_BUTTON1);
			    break;
		    case WM_MBUTTONDOWN:
                manageCapture(MouseButtonCode.MB_BUTTON2);
                fireMouseButtonDown(MouseButtonCode.MB_BUTTON2);
			    break;
		    case WM_MBUTTONUP:
                fireMouseButtonUp(MouseButtonCode.MB_BUTTON2);
                manageRelease(MouseButtonCode.MB_BUTTON2);
			    break;
		    case WM_XBUTTONDOWN:
			    switch (GET_XBUTTON_WPARAM(message.wParam))
			    {
			    case XBUTTON1:
                    manageCapture(MouseButtonCode.MB_BUTTON3);
                    fireMouseButtonDown(MouseButtonCode.MB_BUTTON3);
				    break;
			    case XBUTTON2:
                    manageCapture(MouseButtonCode.MB_BUTTON4);
                    fireMouseButtonDown(MouseButtonCode.MB_BUTTON4);
				    break;
			    }
			    break;
		    case WM_XBUTTONUP:
			    switch (GET_XBUTTON_WPARAM(message.wParam))
			    {
			    case XBUTTON1:
                    fireMouseButtonUp(MouseButtonCode.MB_BUTTON3);
                    manageRelease(MouseButtonCode.MB_BUTTON3);
				    break;
			    case XBUTTON2:
                    fireMouseButtonUp(MouseButtonCode.MB_BUTTON4);
                    manageRelease(MouseButtonCode.MB_BUTTON4);
				    break;
			    }
			    break;
		    case WM_MOUSEMOVE:
                fireMouseMove(GET_X_LPARAM(message.lParam), GET_Y_LPARAM(message.lParam));
			    break;
		    case WM_MOUSEWHEEL:
                fireMouseWheel(GET_WHEEL_DELTA_WPARAM(message.wParam));
			    break;
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

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern uint InputManager_getUtf32(IntPtr wParam, IntPtr lParam);

        [DllImport("PCPlatform", CallingConvention = CallingConvention.Cdecl)]
        private static extern KeyboardButtonCode InputManager_virtualKeyToKeyboardButtonCode(IntPtr wParam);
    }
}
