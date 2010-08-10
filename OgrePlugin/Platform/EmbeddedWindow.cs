using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using OgreWrapper;

namespace OgrePlugin
{
    /// <summary>
    /// A class for RenderWindows from Ogre that have been embedded.
    /// </summary>
    class EmbeddedWindow : OgreWindow, OSWindowListener
    {
        private OSWindow osWindow;
        private RenderWindow renderWindow;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="osWindow">The OSWindow.</param>
        /// <param name="renderWindow">The RenderWindow.</param>
        public EmbeddedWindow(OSWindow osWindow, RenderWindow renderWindow)
            : base(osWindow)
        {
            this.osWindow = osWindow;
            this.renderWindow = renderWindow;
            osWindow.addListener(this);
        }

        /// <summary>
        /// Dispose function.
        /// </summary>
        public override void Dispose()
        {
            osWindow.removeListener(this);
        }

        public void moved(OSWindow window)
        {
            renderWindow.windowMovedOrResized();
        }

        public void resized(OSWindow window)
        {
            renderWindow.windowMovedOrResized();
        }

        public void closing(OSWindow window)
        {
            renderWindow.setActive(false);
        }

        /// <summary>
        /// Get the OgreRenderWindow for this window.
        /// </summary>
        public override RenderWindow OgreRenderWindow
        {
            get
            {
                return renderWindow;
            }
        }

        public void closed(OSWindow window)
        {
            
        }

        public void focusChanged(OSWindow window)
        {
            
        }
    }
}
