using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PCPlatform
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WinMsg
    {
        public IntPtr hwnd;
        public int message;
        public IntPtr wParam;
        public IntPtr lParam;
        public int time;
        public int pt_x;
        public int pt_y;
    };

    public delegate void PumpMessageEvent(ref WinMsg message);

    /// <summary>
    /// This class provides a windows message pump. Use it for any applications
    /// that are windows specific and need to talk to the windows message loop.
    /// </summary>
    public class WindowsMessagePump : OSMessagePump
    {
        IntPtr msgHandle;
        WinMsg dispatchMessage = new WinMsg();

        public event PumpMessageEvent MessageReceived;

        public void loopStarting()
        {
            msgHandle = WindowsMessagePump_primeMessages();
        }

        public bool processMessages()
        {
            if(WindowsMessagePump_peekMessage(msgHandle, out dispatchMessage))
            {
                if(MessageReceived != null)
                {
                    MessageReceived.Invoke(ref dispatchMessage);
                }
                WindowsMessagePump_translateAndDispatchMessage(msgHandle);
                return true;
            }
            return false;
        }

        public void loopCompleted()
        {
            WindowsMessagePump_finishMessages(msgHandle);
        }

        [DllImport("PCPlatform")]
        private static extern IntPtr WindowsMessagePump_primeMessages();

        [DllImport("PCPlatform")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool WindowsMessagePump_peekMessage(IntPtr msgHandle, out WinMsg msg);

        [DllImport("PCPlatform")]
        private static extern void WindowsMessagePump_translateAndDispatchMessage(IntPtr msgHandle);

        [DllImport("PCPlatform")]
        private static extern void WindowsMessagePump_finishMessages(IntPtr msgHandle);
    }
}
