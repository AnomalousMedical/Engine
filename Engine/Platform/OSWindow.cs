using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    public interface OSWindow
    {
        /// <summary>
        /// An int pointer to the handle of the window.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// The current width of the window.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// The current height of the window.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Add a listener.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        void addListener(OSWindowListener listener);

        /// <summary>
        /// Remove a listener.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        void removeListener(OSWindowListener listener);
    }
}
