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
    class EmbeddedWindow : OgreWindow
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
            osWindow.Resized += windowChanged;
            osWindow.Moved += windowChanged;
            osWindow.Closing += windowClosing;
        }

        /// <summary>
        /// Dispose function.
        /// </summary>
        public override void Dispose()
        {
            osWindow.Resized -= windowChanged;
            osWindow.Moved -= windowChanged;
            osWindow.Closing -= windowClosing;
        }

        /// <summary>
        /// Callback for when the window changes to alert ogre.
        /// </summary>
        /// <param name="window"></param>
        void windowChanged(OSWindow window)
        {
            renderWindow.windowMovedOrResized();
        }

        /// <summary>
        /// Callback for when the window is closing. This will disable the
        /// RenderWindow so it will not throw an exception.
        /// </summary>
        /// <param name="window"></param>
        void windowClosing(OSWindow window)
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
    }
}
