using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT
{
    internal class RtStructures
    {
        public const int OPAQUE_GEOM_MASK = 0x01;
        public const int TRANSPARENT_GEOM_MASK = 0x02;

        public const int HIT_GROUP_STRIDE  = 2;
        public const int PRIMARY_RAY_INDEX = 0;
        public const int SHADOW_RAY_INDEX = 1;
    }
}
