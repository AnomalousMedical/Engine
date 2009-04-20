using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;

namespace OgrePlugin
{
    /// <summary>
    /// This is an OgreWindow for windows that Ogre creates.
    /// </summary>
    class AutomaticWindow : OgreWindow
    {
        private OgreOSWindow ogreWindow;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ogreWindow">The OgreOSWindow that Ogre created.</param>
        public AutomaticWindow(OgreOSWindow ogreWindow)
            : base(ogreWindow)
        {
            this.ogreWindow = ogreWindow;
        }

        /// <summary>
        /// Dispose function.
        /// </summary>
        public override void Dispose()
        {
            ogreWindow.Dispose();
        }

        /// <summary>
        /// Get the RenderWindow for this window.
        /// </summary>
        public override RenderWindow OgreRenderWindow
        {
            get
            {
                return ogreWindow.RenderWindow;
            }
        }
    }
}
