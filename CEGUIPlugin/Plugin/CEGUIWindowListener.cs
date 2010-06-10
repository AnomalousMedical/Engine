using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Logging;

namespace CEGUIPlugin
{
    class CEGUIWindowListener : OSWindowListener
    {
        private CEGUISystem system;

        public CEGUIWindowListener(CEGUISystem system)
        {
            this.system = system;
        }

        public void moved(OSWindow window)
        {
            
        }

        public void resized(OSWindow window)
        {
            system.notifyDisplaySizeChanged(window.WindowWidth, window.WindowHeight);
        }

        public void closing(OSWindow window)
        {
            
        }
    }
}
