using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Engine;

namespace PCPlatform
{
    class PCMouse : Mouse
    {
        IntPtr mouse;
        float sensitivity = 1.0f;
        int windowWidth;
        int windowHeight;
        private Vector3 abs = new Vector3();
        private Vector3 rel = new Vector3();

        public PCMouse(IntPtr mouse, int windowWidth, int windowHeight)
        {
            this.mouse = mouse;
            oisMouse_setWindowSize(mouse, windowWidth, windowHeight);
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }

        internal void windowResized(OSWindow window)
        {
            oisMouse_setWindowSize(mouse, windowWidth, windowHeight);
            this.windowWidth = window.WindowWidth;
            this.windowHeight = window.WindowHeight;
        }

        public override Vector3 getAbsMouse()
        {
            return abs;
        }

        public override Vector3 getRelMouse()
        {
            return rel;
        }

        public override bool buttonDown(MouseButtonCode button)
        {
            return oisMouse_buttonDown(mouse, button);
        }

        public override void capture()
        {
            oisMouse_capture(mouse, ref abs, ref rel);
        }

        public override void setSensitivity(float sensitivity)
        {
            this.sensitivity = sensitivity;
        }

        public override float getMouseAreaWidth()
        {
            return windowWidth;
        }

        public override float getMouseAreaHeight()
        {
            return windowHeight;
        }

        internal IntPtr MouseHandle
        {
            get
            {
                return mouse;
            }
        }

        [DllImport("PCPlatform")]
        private static extern void oisMouse_setWindowSize(IntPtr mouse, int width, int height);

        [DllImport("PCPlatform")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool oisMouse_buttonDown(IntPtr mouse, MouseButtonCode button);

        [DllImport("PCPlatform")]
        private static extern void oisMouse_capture(IntPtr mouse, ref Vector3 absPos, ref Vector3 relPos);
    }
}
