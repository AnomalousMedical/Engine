using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace Engine.Renderer
{
    public interface RendererWindow
    {
        OSWindow Handle { get; }
    }
}
