using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace Engine.Renderer
{
    /// <summary>
    /// This class contains information about the windows for an application
    /// using the engine. Renderer plugins will create these as appropriate and
    /// they will be avaliable through the RendererPlugin interface.
    /// </summary>
    public interface WindowInfo
    {
        /// <summary>
        /// The OSWindow for the primary window. This is either the auto-created
        /// window or the OSWindow that was passed as the EmbedWindow in the
        /// DefaultWindowInfo class.
        /// </summary>
        OSWindow PrimaryRenderWindow { get; }
    }
}
