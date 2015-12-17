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
        ReshapeableRigidBody parent;
        btCollisionShape collisionShape;

        public ReshapeableRigidBodySection(ReshapeableRigidBody parent, btCollisionShape collisionShape, Vector3 translation, Quaternion rotation)
        {
            this.parent = parent;
            this.collisionShape = collisionShape;

            parent.CompoundShape.addChildShape(collisionShape, translation, rotation);
        }

        public void Dispose()
        {
            parent.CompoundShape.removeChildShape(collisionShape);
            collisionShape.Dispose();
        }

        public void setScaling(Vector3 scaling)
        {
            collisionShape.setLocalScaling(scaling);
        }

        public void moveOrigin(Vector3 translation, Quaternion rotation)
        {
            parent.CompoundShape.removeChildShape(collisionShape);
            parent.CompoundShape.addChildShape(collisionShape, translation, rotation);
        }
    }
}
