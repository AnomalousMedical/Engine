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

        public IntRect GetTopLeftRect(in IntSize2 size)
        {
            return new IntRect(0, 0, size.Width, size.Height);
        }

        public IntRect GetTopRightRect(in IntSize2 size)
        {
            return new IntRect(window.WindowWidth - size.Width, 0, size.Width, size.Height);
        }

        public IntRect GetBottomRightRect(in IntSize2 size)
        {
            return new IntRect(window.WindowWidth - size.Width, window.WindowHeight - size.Height, size.Width, size.Height);
        }

        public IntRect GetBottomLeftRect(in IntSize2 size)
        {
            return new IntRect(0, window.WindowHeight - size.Height, size.Width, size.Height);
        }
    }
}
