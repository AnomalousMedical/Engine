using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.GltfPbr
{

    [StructLayout(LayoutKind.Sequential)]
    struct GLTFRendererShaderParameters
    {
        public float AverageLogLum;
        public float MiddleGray;
        public float WhitePoint;
        public float PrefilteredCubeMipLevels;

        public float IBLScale;
        public int DebugViewType;
        public float OcclusionStrength;
        public float EmissionScale;
    };
}
