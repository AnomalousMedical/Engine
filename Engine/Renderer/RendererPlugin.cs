using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Renderer
{
    /// <summary>
    /// This interface provides a way for the Engine to communicate with a Renderer plugin.
    /// </summary>
    public interface RendererPlugin : PluginInterface
    {
        WindowInfo WindowInfo { get; }
    }
}
