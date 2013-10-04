using Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletPlugin
{
    interface BulletSceneInternal : BulletScene
    {
        void addRigidBody(MTRigidBody rigidBody, short collisionFilterGroup, short collisionFilterMask);

        void removeRigidBody(MTRigidBody rigidBody);

        void addConstraint(MTTypedConstraintElement constraint, bool disableCollisionsBetweenLinkedBodies);

        void removeConstraint(MTTypedConstraintElement constraint);

        BulletFactory getBulletFactory();

        void drawDebug(DebugDrawingSurface drawingSurface);

        void clearDebug(DebugDrawingSurface drawingSurface);
    }
}
