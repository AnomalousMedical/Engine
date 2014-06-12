using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Renderer;
using Engine.ObjectManagement;

namespace Engine
{
    /// <summary>
    /// This interface allows a subclass to provide debug rendering for a
    /// plugin.
    /// </summary>
    public interface DebugInterface
    {
        /// <summary>
        /// Get the list of DebugEntries for the plugin.
        /// </summary>
        /// <returns>An enumeration over all DebugEntries.</returns>
        IEnumerable<DebugEntry> getEntries();

        /// <summary>
        /// This will pass a DebugDrawingSurface that can be rendered onto to
        /// provide the debug information.
        /// </summary>
        /// <param name="drawingSurface">The DebugDrawingSurface to render onto.</param>
        /// <param name="subScene">The SubScene contents to render.</param>
        void renderDebug(SimSubScene subScene);

        /// <summary>
        /// Enable or disable the rendering for this entire interface.
        /// </summary>
        /// <param name="enabled">True to enable drawing, false to disable.</param>
        bool Enabled { get; set; }

        /// <summary>
        /// Create a debug interface.
        /// </summary>
        /// <param name="rendererPlugin">The renderer plugin to use.</param>
        /// <param name="subScene">The subScene to put the debug surface into.</param>
        void createDebugInterface(RendererPlugin rendererPlugin, SimSubScene subScene);

        /// <summary>
        /// Destroy the debug interface.
        /// </summary>
        /// <param name="rendererPlugin">The renderer plugin to use.</param>
        /// /// <param name="subScene">The subScene to put the debug surface into.</param>
        void destroyDebugInterface(RendererPlugin rendererPlugin, SimSubScene subScene);

        /// <summary>
        /// Tell the debug interface to draw itself with depth testing enabled or disabled.
        /// True to enable depth checking.
        /// </summary>
        bool DepthTesting { get; set; }

        /// <summary>
        /// The name of this visualizer.
        /// </summary>
        String Name { get; }
    }
}
