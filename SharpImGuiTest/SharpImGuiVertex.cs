using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{

    [StructLayout(LayoutKind.Sequential)]
    struct SharpImGuiVertex
    {
        public Vector3 pos;
        public Color color;
    };
}
