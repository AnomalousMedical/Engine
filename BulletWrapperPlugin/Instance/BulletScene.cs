using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Engine.ObjectManagement;
using Engine.Renderer;
using Engine;

namespace BulletPlugin
{
    public partial class BulletScene : SimElementManager, BackgroundUpdateListener
    {
        public delegate void TickCallback(float timeSpan);

        /// <summary>
        /// Called when the physics scene ticks. You should use this to apply forces and other operations, if these are not
        /// done when the physics scene ticks you could get weird jitter or other strange behavior.
        /// 
        /// Note that this event will come on whatever thread the physics is running on, which may not be the main thread
        /// and may be during another operation such as rendering. You should only update physics related classes or store the
        /// state until a behavior's update function to ensure that you do not update resources being used by another thread.
        /// </summary>
        public event TickCallback Tick;

        ManagedTickCallback managedTickCallback;
        private String name;
        private UpdateTimer timer;
        protected BulletFactory factory;
        private IntPtr bulletScene;
        private BulletDebugDraw debugDraw;
        private String performanceName;

        protected List<RigidBody> rigidBodies = new List<RigidBody>();

        public unsafe BulletScene(BulletSceneDefinition definition, UpdateTimer timer)
        {
            managedTickCallback = new ManagedTickCallback(managedTickCallbackFunc);
            fixed (BulletSceneInfo* info = &definition.sceneInfo)
            {
                bulletScene = BulletScene_CreateBulletScene(info, managedTickCallback);
            }
            this.timer = timer;
            this.name = definition.Name;
            timer.addBackgroundUpdateListener("Rendering", this);
            factory = new BulletFactory(this);
            debugDraw = new BulletDebugDraw();
            performanceName = String.Format("BulletScene {0} Background", name);
            Active = true;
        }

        public void Dispose()
        {
            timer.removeBackgroundUpdateListener("Rendering", this);
            BulletScene_DestroyBulletScene(bulletScene);
            debugDraw.Dispose();
            Active = false;
            managedTickCallback = null;
        }

        internal void addRigidBody(RigidBody rigidBody, short collisionFilterGroup, short collisionFilterMask)
        {
            rigidBodies.Add(rigidBody);
            BulletScene_addRigidBody(bulletScene, rigidBody.NativeRigidBody, collisionFilterGroup, collisionFilterMask);
        }

        internal void removeRigidBody(RigidBody rigidBody)
        {
            BulletScene_removeRigidBody(bulletScene, rigidBody.NativeRigidBody);
            rigidBodies.Remove(rigidBody);
        }

        internal void addConstraint(TypedConstraintElement constraint, bool disableCollisionsBetweenLinkedBodies)
        {
            BulletScene_addConstraint(bulletScene, constraint.constraint, disableCollisionsBetweenLinkedBodies);
        }

        internal void removeConstraint(TypedConstraintElement constraint)
        {
            BulletScene_removeConstraint(bulletScene, constraint.constraint);
        }

        internal virtual MotionState createMotionState(RigidBody rigidBody, float maxContactDistance, ref Vector3 initialTrans, ref Quaternion initialRot)
        {
            return new MotionState(rigidBody, maxContactDistance, ref initialTrans, ref initialRot);
        }

        private void managedTickCallbackFunc(float timeSpan)
        {
            if (Tick != null)
            {
                Tick.Invoke(timeSpan);
            }
        }

        public bool Active { get; set; }

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

        public void doBackgroundWork(Clock clock)
        {
            PerformanceMonitor.start(performanceName);
            if (Active)
            {
                BulletScene_update(bulletScene, clock.fSeconds);
            }
            PerformanceMonitor.stop(performanceName);
        }

        public void synchronizeResults()
        {
            foreach (RigidBody rigidBody in rigidBodies)
            {
                rigidBody.syncObjectPosition();
            }
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ManagedTickCallback(float timeStep);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr BulletScene_CreateBulletScene(BulletSceneInfo* sceneInfo, ManagedTickCallback managedTickCallback);

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
