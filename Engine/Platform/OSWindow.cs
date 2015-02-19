using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    public delegate void OSWindowEvent(OSWindow window);

    public interface OSWindow
    {
        /// <summary>
        /// An int pointer to the handle of the window.
        /// </summary>
        IntPtr WindowHandle { get; }

        /// <summary>
        /// The current width of the window.
        /// </summary>
        int WindowWidth { get; }

        /// <summary>
        /// The current height of the window.
        /// </summary>
        int WindowHeight { get; }

        /// <summary>
        /// The OS scaling factor of the window.
        /// </summary>
        float WindowScaling { get; }

        /// <summary>
        /// True if the window has focus.
        /// </summary>
        bool Focused { get; }

        /// <summary>
        /// Called when the window is moved.
        /// </summary>
        event OSWindowEvent Moved;

        /// <summary>
        /// Called when the window is resized.
        /// </summary>
        event OSWindowEvent Resized;

        /// <summary>
        /// Called when the window is closing. This happens before the window is
        /// gone and its handle is no longer valid.
        /// </summary>
        event OSWindowEvent Closing;

        /// <summary>
        /// Called whenthe window has closed.
        /// </summary>
        event OSWindowEvent Closed;

        /// <summary>
        /// Called when the focus has changed on the window.
        /// </summary>
        event OSWindowEvent FocusChanged;
    }
}
