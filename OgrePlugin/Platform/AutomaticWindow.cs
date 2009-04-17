using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;

namespace OgrePlugin
{
    class AutomaticWindow : OgreWindow
    {
        private OgreOSWindow ogreWindow;

        public AutomaticWindow(OgreOSWindow ogreWindow)
            : base(ogreWindow)
        {
            this.ogreWindow = ogreWindow;
        }

        public override void Dispose()
        {
            ogreWindow.Dispose();
        }

        public override RenderWindow OgreRenderWindow
        {
            get
            {
                return ogreWindow.RenderWindow;
            }
        }
    }
}
