using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletPlugin
{
    public class ReshapeableRigidBodySection : IDisposable
    {
        btCollisionShape collisionShape;
        Vector3 translation;
        Quaternion rotation;

        public ReshapeableRigidBodySection(btCollisionShape collisionShape, Vector3 translation, Quaternion rotation)
        {
            this.collisionShape = collisionShape;
            this.translation = translation;
            this.rotation = rotation;
        }

        public void Dispose()
        {
            collisionShape.Dispose();
        }

        public void addToShape(btCompoundShape compoundShape)
        {
            compoundShape.addChildShape(collisionShape, translation, rotation);
        }

        public void removeFromShape(btCompoundShape compoundShape)
        {
            compoundShape.removeChildShape(collisionShape);
        }

        public void setScaling(Vector3 scaling)
        {
            collisionShape.setLocalScaling(scaling);
        }
    }
}
