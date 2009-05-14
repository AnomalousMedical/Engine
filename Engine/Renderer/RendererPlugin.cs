using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine.ObjectManagement;

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
        /// first embedded window added to the renderer. The PrimaryWindow will
        /// be managed by the plugin so it does not need to be destroyed.
        /// </summary>
        RendererWindow PrimaryWindow { get; }

        /// <summary>
        /// Create a new RendererWindow from the Renderer. Any windows created
        /// with this function should be destroyed using destroyRendererWindow
        /// when they are no longer needed.
        /// </summary>
        /// <param name="embedWindow">The window to embed the new RendererWindow into.</param>
        /// <returns>A RendererWindow that can be used to access the newly created window.</returns>
        RendererWindow createRendererWindow(OSWindow embedWindow, String name);

        /// <summary>
        /// Destroy a given RendererWindow. This should be called for all
        /// windows created with createRendererWindow.
        /// </summary>
        /// <param name="window">The window to destroy.</param>
        void destroyRendererWindow(RendererWindow window);

        /// <summary>
        /// Create a new DebugDrawingSurface named name in the specified scene
        /// that renders in the specified way.
        /// </summary>
        /// <param name="name">The name of the DrawingSurface. Must be unique.</param>
        /// <param name="scene">The scene to create the DrawingSurface into.</param>
        /// <returns>A new DebugDrawingSurface configured appropriatly.</returns>
        DebugDrawingSurface createDebugDrawingSurface(String name, SimSubScene scene);

        /// <summary>
        /// Destroy a DebugDrawingSurface. This should be called before the
        /// scene it was created in is destroyed.
        /// </summary>
        /// <param name="surface">The DebugDrawingSurface to destroy.</param>
        void destroyDebugDrawingSurface(DebugDrawingSurface surface);
    }
}
