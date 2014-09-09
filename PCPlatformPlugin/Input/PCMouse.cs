using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Engine;

namespace PCPlatform
{
    class PCMouse : MouseHardware, IDisposable
    {
        IntPtr mouse;
        private IntVector3 abs = new IntVector3();
        private IntVector3 rel = new IntVector3();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ButtonCallback(MouseButtonCode id);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void MovedCallback(int x, int y, int z);

        ButtonCallback buttonPressedCb;
        ButtonCallback buttonReleasedCb;
        MovedCallback movedCallback;

        private IntPtr mouseListener;

        public PCMouse(IntPtr mousePtr, int windowWidth, int windowHeight, Mouse mouse)
            : base(mouse)
        {
            this.mouse = mousePtr;
            oisMouse_setWindowSize(this.mouse, windowWidth, windowHeight);
            fireSizeChanged(windowWidth, windowHeight);

            buttonPressedCb = new ButtonCallback(fireButtonDown);
            buttonReleasedCb = new ButtonCallback(fireButtonUp);
            movedCallback = new MovedCallback(moved);

            mouseListener = oisMouse_createListener(mousePtr, buttonPressedCb, buttonReleasedCb, movedCallback);
        }

        public void Dispose()
        {
            oisMouse_destroyListener(mouse, mouseListener);
        }

        public void capture()
        {
            oisMouse_capture(mouse, ref abs, ref rel);
        }

        private void moved(int x, int y, int z)
        {
            if (x != 0 || y != 0)
            {
                fireMoved(x, y);
            }
            if (z != 0)
            {
                fireWheel(z);
            }
        }

        internal void windowResized(OSWindow window)
        {
            int windowWidth = window.WindowWidth;
            int windowHeight = window.WindowHeight;
            oisMouse_setWindowSize(mouse, windowWidth, windowHeight);
            fireSizeChanged(windowWidth, windowHeight);
        }

        internal IntPtr MouseHandle
        {
            get
            {
                return mouse;
            }
        }

        [DllImport("PCPlatform", CallingConvention = CallingConvention.Cdecl)]
        private static extern void oisMouse_setWindowSize(IntPtr mouse, int width, int height);

        [DllImport("PCPlatform", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool oisMouse_buttonDown(IntPtr mouse, MouseButtonCode button);

        [DllImport("PCPlatform", CallingConvention = CallingConvention.Cdecl)]
        private static extern void oisMouse_capture(IntPtr mouse, ref IntVector3 absPos, ref IntVector3 relPos);

        [DllImport("PCPlatform", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr oisMouse_createListener(IntPtr mouse, ButtonCallback buttonPressedCb, ButtonCallback buttonReleasedCb, MovedCallback movedCallback);

        [DllImport("PCPlatform", CallingConvention = CallingConvention.Cdecl)]
        private static extern void oisMouse_destroyListener(IntPtr mouse, IntPtr listener);
    }
}
