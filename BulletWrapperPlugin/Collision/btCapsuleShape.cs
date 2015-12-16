using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BulletPlugin
{
    class btCapsuleShape : btCollisionShape
    {
        public btCapsuleShape(float radius, float height, float collisionMargin)
            : base(CapsuleShape_Create(radius, height, collisionMargin))
        {

        }

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CapsuleShape_Create(float radius, float height, float collisionMargin);
    }
}
