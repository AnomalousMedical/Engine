using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngineRayTracing
{
    [StructLayout(LayoutKind.Sequential)]
    struct BoxAttribs
    {
        public float minX, minY, minZ;
        public float maxX, maxY, maxZ;
        public float padding0, padding1;
    };
}
