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

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr BoxShape_Create(ref Vector3 extents, float collisionMargin);
    }
}
