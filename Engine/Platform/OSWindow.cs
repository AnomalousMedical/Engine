using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    /// <summary>
    /// Callback for window resizes.
    /// </summary>
    /// <param name="window">The window that fired the event</param>
    public delegate void ResizeEvent(OSWindow window);

    /// <summary>
    /// Callback for window movement.
    /// </summary>
    /// <param name="window">The window that fired the event</param>
    public delegate void MoveEvent(OSWindow window);

    public abstract class OSWindow
    {
        /// <summary>
        /// Called when a window is resized.
        /// </summary>
        public event ResizeEvent Resized;

        /// <summary>
        /// Called when a window is moved.
        /// </summary>
        public event MoveEvent Moved;

        /// <summary>
        /// An int pointer to the handle of the window.
        /// </summary>
        public abstract IntPtr Handle { get; }

        /// <summary>
        /// The current width of the window.
        /// </summary>
        public abstract int Width { get; }

        /// <summary>
        /// The current height of the window.
        /// </summary>
        public abstract int Height { get; }

        /// <summary>
        /// Call when the window is moved.
        /// </summary>
        protected void moved()
        {
            Moved(this);
        }

        /// <summary>
        /// Call when the window is resized.
        /// </summary>
        protected void resized()
        {
            Resized(this);
        }
    }
}
