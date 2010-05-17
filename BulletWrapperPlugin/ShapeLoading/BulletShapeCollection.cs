using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    class BulletShapeCollection : IDisposable
    {
        protected IntPtr collisionShape;

        public BulletShapeCollection(IntPtr collisionShape, String name)
        {
            this.collisionShape = collisionShape;
            this.Name = name;
        }

        public virtual void Dispose()
        {
            if (collisionShape != IntPtr.Zero)
            {
                CollisionShapeInterface.CollisionShape_Delete(collisionShape);
                collisionShape = IntPtr.Zero;
            }
        }

        public virtual int Count
        {
            get
            {
                return 1;
            }
        }

        public String Name { get; private set; }

        public ShapeLocation SourceLocation { get; set; }

        internal IntPtr CollisionShape
        {
            get
            {
                return collisionShape;
            }
        }
    }
}
