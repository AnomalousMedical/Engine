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
        protected btCollisionShape collisionShape;

        public BulletShapeCollection(btCollisionShape collisionShape, String name)
        {
            this.collisionShape = collisionShape;
            this.Name = name;
        }

        public virtual void Dispose()
        {
            if (collisionShape != null)
            {
                collisionShape.Dispose();
                collisionShape = null;
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

        internal btCollisionShape CollisionShape
        {
            get
            {
                return collisionShape;
            }
        }
    }
}
