using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpGui
{
    class ScreenPositioner : IScreenPositioner
    {
        private OSWindow window;

        public ScreenPositioner(OSWindow window)
        {
            this.window = window;
        }

        public IntRect GetBottomRightRect(in IntSize2 size)
        {
            return new IntRect(window.WindowWidth - size.Width, window.WindowHeight - size.Height, size.Width, size.Height);
        }
    }
}
