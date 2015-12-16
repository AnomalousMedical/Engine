using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BulletPlugin
{
    public unsafe class btHullShape : btCollisionShape
    {
        public btHullShape(float* vertices, int numPoints, int stride, float collisionMargin)
            : base(ConvexHullShape_Create(vertices, numPoints, stride, collisionMargin))
        {

        }

        internal btHullShape(IntPtr btShape)
           : base(btShape)
        {

        }

        public override btCollisionShape createClone()
        {
            return new btHullShape(CollisionShape_Clone(btShape));
        }

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe IntPtr ConvexHullShape_Create(float* vertices, int numPoints, int stride, float collisionMargin);
    }
}
