using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_99_Pbo
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EnvMapRenderAttribs
    {
        ToneMappingAttribs TMAttribs;

        float AverageLogLum;
        float MipLevel;
        float Unusued1;
        float Unusued2;
    };
}
