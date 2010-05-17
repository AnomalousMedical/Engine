using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Engine.ObjectManagement;

namespace BulletPlugin
{
    public partial class BulletScene : SimElementManager, UpdateListener
    {
        private String name;
        private UpdateTimer timer;
        private BulletFactory factory;
        private HandleRef bulletScene;

        public unsafe BulletScene(BulletSceneDefinition definition, UpdateTimer timer)
        {
            fixed (BulletSceneInfo* info = &definition.sceneInfo)
            {
                bulletScene = new HandleRef(this, BulletScene_CreateBulletScene(info));
            }
            this.timer = timer;
            this.name = definition.Name;
            timer.addFixedUpdateListener(this);
            factory = new BulletFactory(this);
        }

        public void Dispose()
        {
            timer.removeFixedUpdateListener(this);
            BulletScene_DestroyBulletScene(bulletScene);
        }

        public void addRigidBody(RigidBody rigidBody, short collisionFilterGroup, short collisionFilterMask)
        {
            BulletScene_addRigidBody(bulletScene, rigidBody.NativeRigidBody, collisionFilterGroup, collisionFilterMask);
        }

        public void removeRigidBody(RigidBody rigidBody)
        {
            BulletScene_removeRigidBody(bulletScene, rigidBody.NativeRigidBody);
        }

        internal void addConstraint(TypedConstraintElement constraint, bool disableCollisionsBetweenLinkedBodies)
        {
            BulletScene_addConstraint(bulletScene, constraint.constraint, disableCollisionsBetweenLinkedBodies);
        }

        internal void removeConstraint(TypedConstraintElement constraint)
        {
            BulletScene_removeConstraint(bulletScene, constraint.constraint);
        }

        #region SimElementManager Members

        public SimElementFactory getFactory()
        {
            return factory;
        }

        internal BulletFactory getBulletFactory()
        {
            return factory;
        }

        public Type getSimElementManagerType()
        {
            return typeof(BulletScene);
        }

        public string getName()
        {
            return name;
        }

        public unsafe SimElementManagerDefinition createDefinition()
        {
            BulletSceneDefinition definition = new BulletSceneDefinition(name);
            fixed (BulletSceneInfo* info = &definition.sceneInfo)
            {
                BulletScene_fillOutInfo(bulletScene, info);
            }
            return definition;
        }

        #endregion

        #region UpdateListener Members

        public void sendUpdate(Clock clock)
        {
            BulletScene_update(bulletScene, (float)clock.Seconds);
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }

        #endregion
    }

    //Dll Imports
    unsafe partial class BulletScene
    {
        [DllImport("BulletWrapper")]
        private static extern IntPtr BulletScene_CreateBulletScene(BulletSceneInfo* sceneInfo);

        [DllImport("BulletWrapper")]
        private static extern void BulletScene_DestroyBulletScene(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern void BulletScene_fillOutInfo(HandleRef instance, BulletSceneInfo* sceneInfo);

        [DllImport("BulletWrapper")]
        private static extern void BulletScene_update(HandleRef instance, float seconds);

        [DllImport("BulletWrapper")]
        private static extern void BulletScene_addRigidBody(HandleRef instance, HandleRef rigidBody, short group, short mask);

        [DllImport("BulletWrapper")]
        private static extern void BulletScene_removeRigidBody(HandleRef instance, HandleRef rigidBody);

        [DllImport("BulletWrapper")]
        private static extern void BulletScene_addConstraint(HandleRef instance, IntPtr constraint, bool disableCollisionsBetweenLinkedBodies);

        [DllImport("BulletWrapper")]
        private static extern void BulletScene_removeConstraint(HandleRef instance, IntPtr constraint);
    }
}
