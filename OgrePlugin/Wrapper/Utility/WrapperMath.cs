using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    public class WrapperMath
    {
        public static Pair<bool, float> intersects(Ray3 ray, Vector3 a, Vector3 b, Vector3 c, bool positiveSide, bool negativeSide)
        {
            float distance;
            bool intersect = Math_intersectsRayPoly(ray, a, b, c, positiveSide, negativeSide, out distance);
            return new Pair<bool, float>(intersect, distance);
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern bool Math_intersectsRayPoly(Ray3 ray, Vector3 a, Vector3 b, Vector3 c, bool positiveSide, bool negativeSide, out float dist);

#endregion
    }
}
