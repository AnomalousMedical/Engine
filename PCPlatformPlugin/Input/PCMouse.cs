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
        public PCMouse(Mouse mouse)
            : base(mouse)
        {

        }

        internal void _fireButtonDown(MouseButtonCode button)
        {
            fireButtonDown(button);
        }

        internal void _fireButtonUp(MouseButtonCode button)
        {
            fireButtonUp(button);
        }

        internal void _fireMoved(int x, int y)
        {
            fireMoved(x, y);
        }

        internal void _fireWheel(int z)
        {
            fireWheel(z);
        }

        internal void windowResized(OSWindow window)
        {
            int windowWidth = window.WindowWidth;
            int windowHeight = window.WindowHeight;
            fireSizeChanged(windowWidth, windowHeight);
        }
    }
}
