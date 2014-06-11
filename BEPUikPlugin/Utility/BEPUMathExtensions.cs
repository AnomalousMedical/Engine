using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BEPUikPlugin
{
    public static class BEPUMathExtensions
    {
        public static BEPUutilities.Vector3 toBepuVec3(this Vector3 conv)
        {
            return new BEPUutilities.Vector3(conv.x, conv.y, conv.z);
        }

        public static BEPUutilities.Quaternion toBepuQuat(this Quaternion conv)
        {
            return new BEPUutilities.Quaternion(conv.x, conv.y, conv.z, conv.w);
        }

        public static Vector3 toEngineVec3(this BEPUutilities.Vector3 conv)
        {
            return new Vector3(conv.X, conv.Y, conv.Z);
        }

        public static Quaternion toEngineQuat(this BEPUutilities.Quaternion conv)
        {
            return new Quaternion(conv.X, conv.Y, conv.Z, conv.W);
        }
    }
}
