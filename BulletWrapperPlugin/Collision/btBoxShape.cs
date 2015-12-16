using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BulletPlugin
{
    public class btBoxShape : btCollisionShape
    {
        public btBoxShape(ref Vector3 extents, float collisionMargin)
            :base(BoxShape_Create(ref extents, collisionMargin))
        {

        }

        internal btBoxShape(IntPtr btShape)
           : base(btShape)
        {

        }

        public override btCollisionShape createClone()
        {
            return new btBoxShape(CollisionShape_Clone(btShape));
        }

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr BoxShape_Create(ref Vector3 extents, float collisionMargin);
    }
}
