using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace Engine.Renderer
{
    /// <summary>
    /// This interface provides a way for the Engine to communicate with a Renderer plugin.
    /// </summary>
    public interface RendererPlugin : PluginInterface
    {
        /// <summary>
        /// Get the RendererWindow of the primary window. If the window was
        /// auto-created this will be that window, otherwise it will be the
        /// first embedded window added to the renderer.
        /// </summary>
        RendererWindow PrimaryWindow { get; }

        /// <summary>
        /// Create a new RendererWindow from the Renderer.
        /// </summary>
        /// <param name="embedWindow">The window to embed the new RendererWindow into.</param>
        /// <returns>A RendererWindow that can be used to access the newly created window.</returns>
        RendererWindow createRendererWindow(OSWindow embedWindow, String name);

        /// <summary>
        /// Destroy a given RendererWindow.
        /// </summary>
        /// <param name="window">The window to destroy.</param>
        void destroyRendererWindow(RendererWindow window);
    }
}
