using Engine;
using Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BulletPlugin
{
    [NativeSubsystemType]
    public class RayResultCallback : IDisposable
    {
        internal IntPtr ptr;

        internal RayResultCallback(IntPtr ptr)
        {
            this.ptr = ptr;
        }

        public void Dispose()
        {
            RayResultCallback_Delete(ptr);
            ptr = IntPtr.Zero;
        }

        public void reset()
        {
            ManagedRayResultCallback_reset(ptr);
        }

        public Vector3 RayFromWorld
        {
            get
            {
                return ManagedRayResultCallback_getRayFromWorld(ptr);
            }
            set
            {
                ManagedRayResultCallback_setRayFromWorld(ptr, value);
            }
        }

        public Vector3 RayToWorld
        {
            get
            {
                return ManagedRayResultCallback_getRayToWorld(ptr);
            }
            set
            {
                ManagedRayResultCallback_setRayToWorld(ptr, value);
            }
        }

        public RigidBody CollisionObject
        {
            get
            {
                return RigidBodyManager.get(RayResultCallback_getCollisionObject(ptr));
            }
        }

        public bool HasHit
        {
            get
            {
                return ManagedRayResultCallback_hasHit(ptr);
            }
        }

#region PInvoke

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RayResultCallback_Delete(IntPtr cb);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedRayResultCallback_setRayFromWorld(IntPtr cb, Vector3 rayFromWorld);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector3 ManagedRayResultCallback_getRayFromWorld(IntPtr cb);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedRayResultCallback_setRayToWorld(IntPtr cb, Vector3 rayToWorld);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector3 ManagedRayResultCallback_getRayToWorld(IntPtr cb);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr RayResultCallback_getCollisionObject(IntPtr cb);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedRayResultCallback_reset(IntPtr cb);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ManagedRayResultCallback_hasHit(IntPtr cb);

#endregion
    }
}
