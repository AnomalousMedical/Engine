using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.GltfPbr
{
    [StructLayout(LayoutKind.Sequential)]
    struct GLTFAttribs
    {
        public GLTFRendererShaderParameters RenderParameters;
        public GLTFShaderAttribs MaterialInfo;
    }
}
