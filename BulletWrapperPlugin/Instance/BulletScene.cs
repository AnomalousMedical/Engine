using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Engine.ObjectManagement;
using Engine.Renderer;

namespace BulletPlugin
{
    public partial class BulletScene : SimElementManager, UpdateListener
    {
        private String name;
        private UpdateTimer timer;
        private BulletFactory factory;
        private IntPtr bulletScene;
        private BulletDebugDraw debugDraw;

        public unsafe BulletScene(BulletSceneDefinition definition, UpdateTimer timer)
        {
            fixed (BulletSceneInfo* info = &definition.sceneInfo)
            {
                bulletScene = BulletScene_CreateBulletScene(info);
            }
            this.timer = timer;
            this.name = definition.Name;
            timer.addFixedUpdateListener(this);
            factory = new BulletFactory(this);
            debugDraw = new BulletDebugDraw();
        }

        public void Dispose()
        {
            timer.removeFixedUpdateListener(this);
            BulletScene_DestroyBulletScene(bulletScene);
            debugDraw.Dispose();
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

        internal void drawDebug(DebugDrawingSurface drawingSurface)
        {
            drawingSurface.begin(name + "BulletDebug", DrawingType.LineList);
            debugDraw.setDrawingSurface(drawingSurface);
            BulletScene_debugDrawWorld(bulletScene, debugDraw.nativeDraw);
            drawingSurface.end();
        }

        internal void clearDebug(DebugDrawingSurface drawingSurface)
        {
            drawingSurface.begin(name + "BulletDebug", DrawingType.LineList);
            drawingSurface.end();
        }
    }

    //Dll Imports
    unsafe partial class BulletScene
    {
        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr BulletScene_CreateBulletScene(BulletSceneInfo* sceneInfo);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void BulletScene_DestroyBulletScene(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void BulletScene_fillOutInfo(IntPtr instance, BulletSceneInfo* sceneInfo);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void BulletScene_update(IntPtr instance, float seconds);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void BulletScene_addRigidBody(IntPtr instance, IntPtr rigidBody, short group, short mask);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void BulletScene_removeRigidBody(IntPtr instance, IntPtr rigidBody);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void BulletScene_addConstraint(IntPtr instance, IntPtr constraint, bool disableCollisionsBetweenLinkedBodies);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void BulletScene_removeConstraint(IntPtr instance, IntPtr constraint);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void BulletScene_debugDrawWorld(IntPtr instance, IntPtr debugDrawer);
    }
}
