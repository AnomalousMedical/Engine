using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.GltfPbr
{
    public struct CascadeAttribs
    {
        Vector4 f4LightSpaceScale;
        Vector4 f4LightSpaceScaledBias;
        Vector4 f4StartEndZ;

        // Cascade margin in light projection space ([-1, +1] x [-1, +1] x [-1(GL) or 0, +1])
        Vector4 f4MarginProjSpace;
    }
}
