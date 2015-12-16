using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BulletPlugin
{
    public class btSphereShape : btCollisionShape
    {
        public btSphereShape(float radius, float collisionMargin)
            : base(SphereShape_Create(radius, collisionMargin))
        {

        }

        internal btSphereShape(IntPtr btShape)
           : base(btShape)
        {

        }

        public override btCollisionShape createClone()
        {
            return new btSphereShape(CollisionShape_Clone(btShape));
        }

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SphereShape_Create(float radius, float collisionMargin);
    }
}
