using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.GltfPbr
{
    public enum DebugViewType : int
    {
        None = 0,
        BaseColor = 1,
        Transparency = 2,
        NormalMap = 3,
        Occlusion = 4,
        Emissive = 5,
        Metallic = 6,
        Roughness = 7,
        DiffuseColor = 8,
        SpecularColor = 9,
        Reflectance90 = 10,
        MeshNormal = 11,
        PerturbedNormal = 12,
        NdotV = 13,
        DiffuseIBL = 14,
        SpecularIBL = 15,
        NumDebugViews
    }
}
