using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace Engine
{
    /// <summary>
    /// This interface provides access to the dynamically loaded platform
    /// classes. There can be only one of these plugins per instance of the
    /// engine.
    /// </summary>
    public interface PlatformPlugin : PluginInterface
    {
        /// <summary>
        /// Create a new timer for this platform.
        /// </summary>
        /// <returns>A new timer.</returns>
        SystemTimer createTimer();

        /// <summary>
        /// Destroy the given timer.
        /// </summary>
        /// <param name="timer">The timer to destroy.</param>
        void destroyTimer(SystemTimer timer);

        /// <summary>
        /// Create an InputHandler that can be used to access input devices.
        /// </summary>
        /// <param name="window">The window to bind the input to.</param>
        /// <param name="foreground">True if the mouse should be in foreground mode.</param>
        /// <param name="exclusive">True if the mouse should be in exclusive mode.</param>
        /// <param name="noWinKey">True to disable the windows key.</param>
        /// <returns>A new InputHandler for the platform.</returns>
        InputHandler createInputHandler(OSWindow window, bool foreground, bool exclusive, bool noWinKey);

        /// <summary>
        /// Destroy the given input handler.
        /// </summary>
        /// <param name="handler">The InputHandler to destroy.</param>
        void destroyInputHandler(InputHandler handler);
    }
}
