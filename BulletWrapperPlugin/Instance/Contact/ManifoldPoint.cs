using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    public class ManifoldPoint
    {
        IntPtr manifoldPoint;

        internal void setPoint(IntPtr manifoldPoint)
        {
            this.manifoldPoint = manifoldPoint;
        }

        public float getDistance()
        {
            return btManifoldPoint_getDistance(manifoldPoint);
        }

        public int getLifeTime()
        {
            return btManifoldPoint_getLifeTime(manifoldPoint);
        }

        public Vector3 getPositionWorldOnA()
        {
            return btManifoldPoint_getPositionWorldOnA(manifoldPoint);
        }

        public Vector3 getPositionWorldOnB()
        {
            return btManifoldPoint_getPositionWorldOnB(manifoldPoint);
        }

        public float getAppliedImpulse()
        {
            return btManifoldPoint_getAppliedImpulse(manifoldPoint);
        }

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float btManifoldPoint_getDistance(IntPtr point);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int btManifoldPoint_getLifeTime(IntPtr point);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 btManifoldPoint_getPositionWorldOnA(IntPtr point);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 btManifoldPoint_getPositionWorldOnB(IntPtr point);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float btManifoldPoint_getAppliedImpulse(IntPtr point);
    }
}
