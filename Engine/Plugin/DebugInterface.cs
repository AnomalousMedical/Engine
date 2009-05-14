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
        void renderDebug(DebugDrawingSurface drawingSurface, SimSubScene subScene);

        /// <summary>
        /// Enable or disable the rendering for this entire interface.
        /// </summary>
        /// <param name="enabled">True to enable drawing, false to disable.</param>
        void setEnabled(bool enabled);
    }
}
