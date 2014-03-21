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
            osWindow.Moved += osWindow_Moved;
            osWindow.Resized += osWindow_Resized;
            osWindow.Closing += osWindow_Closing;
        }

        /// <summary>
        /// Dispose function.
        /// </summary>
        public override void Dispose()
        {
            osWindow.Moved -= osWindow_Moved;
            osWindow.Resized -= osWindow_Resized;
            osWindow.Closing -= osWindow_Closing;
        }

        void osWindow_Moved(OSWindow window)
        {
            renderWindow.windowMovedOrResized();
        }

        void osWindow_Resized(OSWindow window)
        {
            renderWindow.windowMovedOrResized();
        }

        void osWindow_Closing(OSWindow window)
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
