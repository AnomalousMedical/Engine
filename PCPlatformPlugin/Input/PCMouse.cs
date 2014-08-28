using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Engine;

namespace PCPlatform
{
    class PCMouse : MouseHardware
    {
        IntPtr mouse;
        int windowWidth;
        int windowHeight;
        private Vector3 abs = new Vector3();
        private Vector3 rel = new Vector3();

        public PCMouse(IntPtr mouse, int windowWidth, int windowHeight, EventManager eventManager)
            :base(eventManager)
        {
            this.mouse = mouse;
            oisMouse_setWindowSize(mouse, windowWidth, windowHeight);
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }

        internal void windowResized(OSWindow window)
        {
            this.windowWidth = window.WindowWidth;
            this.windowHeight = window.WindowHeight;
            oisMouse_setWindowSize(mouse, windowWidth, windowHeight);
        }

        public override bool buttonDown(MouseButtonCode button)
        {
            return oisMouse_buttonDown(mouse, button);
        }

        public override void capture()
        {
            oisMouse_capture(mouse, ref abs, ref rel);
        }

        public override Vector3 AbsolutePosition
        {
            get
            {
                return abs;
            }
        }

        public override Vector3 RelativePosition
        {
            get
            {
                return rel;
            }
        }

        public override float AreaWidth
        {
            get
            {
                return windowWidth;
            }
        }

        public override float AreaHeight
        {
            get
            {
                return windowHeight;
            }
        }

        internal IntPtr MouseHandle
        {
            get
            {
                return mouse;
            }
        }

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern void oisMouse_setWindowSize(IntPtr mouse, int width, int height);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool oisMouse_buttonDown(IntPtr mouse, MouseButtonCode button);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern void oisMouse_capture(IntPtr mouse, ref Vector3 absPos, ref Vector3 relPos);
    }
}
