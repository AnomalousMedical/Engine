using BEPUik;
using Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public abstract class ExternalControl
    {
        internal abstract Control IKControl { get; }

        public abstract BEPUikBone TargetBone { get; set; }

        internal String CurrentSolverName { get; set; }

        internal abstract void draw(DebugDrawingSurface drawingSurface, DebugDrawMode drawMode);
    }
}
