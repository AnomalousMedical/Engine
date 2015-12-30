using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;

namespace Anomalous.GuiFramework
{
    public delegate void ScreenSizeChangedDelegate(int width, int height);

    public class ScreenLayoutManager
    {
        public event ScreenSizeChangedDelegate ScreenSizeChanging;
        public event ScreenSizeChangedDelegate ScreenSizeChanged;

        private LayoutChain layoutChain;
        private OSWindow window;

        public ScreenLayoutManager(OSWindow window)
        {
            this.window = window;
            window.Resized += resized;
        }

        public void changeOSWindow(OSWindow newWindow)
        {
            if (window != null)
            {
                window.Resized -= resized;
            }
            this.window = newWindow;
            window.Resized += resized;
            resized(window);
        }

        public LayoutChain LayoutChain
        {
            get
            {
                return layoutChain;
            }
            set
            {
                layoutChain = value;
                layoutChain.Location = new IntVector2(0, 0);
                layoutChain.WorkingSize = new IntSize2(window.WindowWidth, window.WindowHeight);
            }
        }

        void resized(OSWindow window)
        {
            if (ScreenSizeChanging != null)
            {
                ScreenSizeChanging.Invoke(window.WindowWidth, window.WindowHeight);
            }
            layoutChain.WorkingSize = new IntSize2(window.WindowWidth, window.WindowHeight);
            layoutChain.layout();
            if (ScreenSizeChanged != null)
            {
                ScreenSizeChanged.Invoke(window.WindowWidth, window.WindowHeight);
            }
        }
    }
}
