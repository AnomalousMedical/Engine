using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;

namespace OgrePlugin
{
    class OgreWindowListener : WindowEventListener
    {
        private OgreOSWindow ogreWindow;

        public OgreWindowListener(OgreOSWindow ogreWindow)
        {
            this.ogreWindow = ogreWindow;
        }

        protected override void windowMoved(IntPtr renderWindow)
        {
            ogreWindow._fireWindowMoved();
        }

        protected override void windowResized(IntPtr renderWindow)
        {
            ogreWindow._fireWindowResized();
        }

        protected override bool windowClosing(IntPtr renderWindow)
        {
            ogreWindow._fireWindowClosing();
            return true;
        }

        protected override void windowClosed(IntPtr renderWindow)
        {
            ogreWindow._fireWindowClosed();
        }

        protected override void windowFocusChange(IntPtr renderWindow)
        {
            ogreWindow._fireFocusChanged();
        }
    }
}
