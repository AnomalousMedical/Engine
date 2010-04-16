using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Renderer;

namespace Engine.Editing
{
    public interface EditInterfaceRenderer
    {
        void frameUpdate(DebugDrawingSurface drawingSurface);

        void propertiesChanged(DebugDrawingSurface drawingSurface);

        void interfaceSelected(DebugDrawingSurface drawingSurface);

        void interfaceDeselected(DebugDrawingSurface drawingSurface);
    }
}
