using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    public interface OSWindowListener
    {
        /// <summary>
        /// Called when the window is moved.
        /// </summary>
        /// <param name="window">The window.</param>
        void moved(OSWindow window);

        /// <summary>
        /// Called when the window is resized.
        /// </summary>
        /// <param name="window">The window.</param>
        void resized(OSWindow window);

        /// <summary>
        /// Called when the window is closing. This happens before the window is
        /// gone and its handle is no longer valid.
        /// </summary>
        /// <param name="window">The window.</param>
        void closing(OSWindow window);

        /// <summary>
        /// Called whenthe window has closed.
        /// </summary>
        /// <param name="window">The window that closed.</param>
        void closed(OSWindow window);

        /// <summary>
        /// Called when the focus has changed on the window.
        /// </summary>
        /// <param name="window"></param>
        void focusChanged(OSWindow window);
    }
}
