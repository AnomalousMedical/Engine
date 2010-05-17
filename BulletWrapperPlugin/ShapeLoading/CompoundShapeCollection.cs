using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    class CompoundShapeCollection : BulletShapeCollection
    {
        public CompoundShapeCollection(IntPtr collisionShape, String name)
            :base(collisionShape, name)
        {
            
        }

        public override void Dispose()
        {
            if (collisionShape != IntPtr.Zero)
            {
                CollisionShapeInterface.CompoundShape_DeleteChildren(collisionShape);
                collisionShape = IntPtr.Zero;
            }
            base.Dispose();
        }

        public override int Count
        {
            get
            {
                return CollisionShapeInterface.CompoundShape_GetCount(collisionShape);
            }
        }
    }
}
