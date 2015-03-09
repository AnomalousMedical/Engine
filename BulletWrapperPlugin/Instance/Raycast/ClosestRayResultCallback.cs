using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BulletPlugin
{
    public class ClosestRayResultCallback : RayResultCallback
    {
        public ClosestRayResultCallback(Vector3 rayFromWorld, Vector3 rayToWorld)
            : base(ClosestRayResultCallback_Create(rayFromWorld, rayToWorld))
        {

        }

        public Vector3 HitNormalWorld
        {
            get
            {
                return ClosestRayResultCallback_getHitNormalWorld(ptr);
            }
        }

        public Vector3 HitPointWorld
        {
            get
            {
                return ClosestRayResultCallback_getHitPointWorld(ptr);
            }
        }

        #region PInvoke

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ClosestRayResultCallback_Create(Vector3 rayFromWorld, Vector3 rayToWorld);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector3 ClosestRayResultCallback_getHitNormalWorld(IntPtr cb);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector3 ClosestRayResultCallback_getHitPointWorld(IntPtr cb);

        #endregion
    }
}
