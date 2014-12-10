using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Logging;

namespace PCPlatform
{
    /// <summary>
    /// This input handler uses some C# win32 processing to do input. Its pretty hacky and ok for our
    /// Windows.Forms based tools, but don't use it for anything production.
    /// </summary>
    public class PCInputHandler : InputHandler, IDisposable
    {
	    PCKeyboard createdKeyboard;
	    PCMouse createdMouse;
	    OSWindow window;
        WindowsMessagePump windowsPump;

        public PCInputHandler(OSWindow windowHandle, WindowsMessagePump windowsPump)
        {
            this.window = windowHandle;
            this.windowsPump = windowsPump;
            windowsPump.MessageReceived += windowsPump_MessageReceived;

	        //Log some info
            Log.Info("Using PCPlatform Input Handler");
        }

        public void Dispose()
        {
            windowsPump.MessageReceived -= windowsPump_MessageReceived;
            if( createdMouse != null )
	        {
		        destroyMouse(createdMouse);
	        }
	        if( createdKeyboard != null )
	        {
		        destroyKeyboard(createdKeyboard);
	        }
        }

        public override KeyboardHardware createKeyboard(Keyboard keyboard)
        {
            if( createdKeyboard == null )
	        {
		        Log.Info("Creating keyboard.");
                createdKeyboard = new PCKeyboard(keyboard);
	        }
	        return createdKeyboard;
        }

        public override void destroyKeyboard(KeyboardHardware keyboard)
        {
            if( createdKeyboard == keyboard )
	        {
		        Log.Info("Destroying keyboard.");
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
		        createdMouse = new PCMouse(mouse);
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

        private void fireKeyDown(KeyboardButtonCode keyboardButtonCode, uint keyChar)
        {
            if(createdKeyboard != null)
            {
                createdKeyboard._fireKeyPressed(keyboardButtonCode, keyChar);
            }
        }

        private void fireKeyUp(KeyboardButtonCode keyboardButtonCode)
        {
            if (createdKeyboard != null)
            {
                createdKeyboard._fireKeyReleased(keyboardButtonCode);
            }
        }

        private void fireMouseButtonDown(MouseButtonCode mouseButtonCode)
        {
            if(createdMouse != null)
            {
                createdMouse._fireButtonDown(mouseButtonCode);
            }
        }

        private void fireMouseButtonUp(MouseButtonCode mouseButtonCode)
        {
            if (createdMouse != null)
            {
                createdMouse._fireButtonUp(mouseButtonCode);
            }
        }

        private void fireMouseMove(int x, int y)
        {
            if (createdMouse != null)
            {
                createdMouse._fireMoved(x, y);
            }
        }

        private void fireMouseWheel(short z)
        {
            if (createdMouse != null)
            {
                createdMouse._fireWheel(z);
            }
        }

        private void manageCapture(MouseButtonCode mouseButtonCode)
        {

        }

        private void manageRelease(MouseButtonCode mouseButtonCode)
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
        private static extern uint InputManager_getUtf32(IntPtr wParam, IntPtr lParam);

        [DllImport("PCPlatform", CallingConvention = CallingConvention.Cdecl)]
        private static extern KeyboardButtonCode InputManager_virtualKeyToKeyboardButtonCode(IntPtr wParam);
    }
}
