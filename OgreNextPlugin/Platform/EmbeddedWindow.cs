using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace OgreNextPlugin
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
        {
            this.osWindow = osWindow;
            this.renderWindow = renderWindow;
            osWindow.Moved += osWindow_Moved;
            osWindow.Resized += osWindow_Resized;
            osWindow.Closing += osWindow_Closing;
            osWindow.CreateInternalResources += osWindow_CreateInternalResources;
            osWindow.DestroyInternalResources += osWindow_DestroyInternalResources;
        }

        /// <summary>
        /// Dispose function.
        /// </summary>
        public override void Dispose()
        {
            osWindow.Moved -= osWindow_Moved;
            osWindow.Resized -= osWindow_Resized;
            osWindow.Closing -= osWindow_Closing;
            osWindow.CreateInternalResources -= osWindow_CreateInternalResources;
            osWindow.DestroyInternalResources -= osWindow_DestroyInternalResources;
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
            //renderWindow.setActive(false);
        }

        void osWindow_CreateInternalResources(OSWindow window, InternalResourceType resourceType)
        {
            if ((resourceType & InternalResourceType.Graphics) == InternalResourceType.Graphics)
            {
                renderWindow.createInternalResources(window.WindowHandle);
            }
        }

        void osWindow_DestroyInternalResources(OSWindow window, InternalResourceType resourceType)
        {
            if ((resourceType & InternalResourceType.Graphics) == InternalResourceType.Graphics)
            {
                renderWindow.destroyInternalResources(window.WindowHandle);
            }
        }

        /// <summary>
        /// Get the OgreRenderWindow for this window.
        /// </summary>
        //public override RenderTarget OgreRenderTarget
        //{
        //    get
        //    {
        //        return renderWindow;
        //    }
        //}

        public override OSWindow OSWindow
        {
            get
            {
                return osWindow;
            }
        }
    }
}
