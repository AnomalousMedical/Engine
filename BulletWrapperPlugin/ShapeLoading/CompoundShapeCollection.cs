using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    class CompoundShapeCollection : BulletShapeCollection
    {
        private int count;

        public CompoundShapeCollection(btCompoundShape collisionShape, String name)
            :base(collisionShape, name)
        {
            this.count = collisionShape.ChildCount;
        }

        public override int Count
        {
            get
            {
                return count;
            }
        }
    }
}
