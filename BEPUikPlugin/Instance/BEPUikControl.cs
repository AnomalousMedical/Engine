﻿using BEPUik;
using Engine;
using Engine.ObjectManagement;
using Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public abstract class BEPUikControl : SimElement
    {
        private BEPUikScene scene;

        public BEPUikControl(BEPUikScene scene, String name)
            :base(name)
        {
            this.scene = scene;
            MovedThisTick = false;
        }

        protected override void Dispose()
        {
            scene.removeControl(this);
        }

        protected override void updatePositionImpl(ref Vector3 translation, ref Quaternion rotation)
        {
            MovedThisTick = true;
        }

        protected override void updateTranslationImpl(ref Vector3 translation)
        {
            MovedThisTick = true;
        }

        protected override void updateRotationImpl(ref Quaternion rotation)
        {
            MovedThisTick = true;
        }

        protected override void updateScaleImpl(ref Vector3 scale)
        {
            MovedThisTick = true;
        }

        protected override void setEnabled(bool enabled)
        {
            if (enabled)
            {
                scene.addControl(this);
            }
            else
            {
                scene.removeControl(this);
            }
        }

        internal abstract void syncPosition();

        internal bool MovedThisTick { get; set; }

        internal abstract Control IKControl { get; }

        /// <summary>
        /// Don't allow this bone to be changed, we rely on it staying the same in the scene to find
        /// which solver to add/remove this bone from.
        /// </summary>
        public abstract BEPUikBone Bone { get; }

        internal abstract void draw(DebugDrawingSurface drawingSurface, DebugDrawMode drawMode);
    }
}
