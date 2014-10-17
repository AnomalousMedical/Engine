using BEPUik;
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

        public BEPUikControl(BEPUikScene scene, String name, Subscription subscription)
            :base(name, subscription)
        {
            this.scene = scene;
        }

        protected override void Dispose()
        {
            scene.removeControl(this);
        }

        protected override void updatePositionImpl(ref Vector3 translation, ref Quaternion rotation)
        {

        }

        protected override void updateTranslationImpl(ref Vector3 translation)
        {

        }

        protected override void updateRotationImpl(ref Quaternion rotation)
        {

        }

        protected override void updateScaleImpl(ref Vector3 scale)
        {

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

        internal abstract Control IKControl { get; }

        /// <summary>
        /// Don't allow this bone to be changed, we rely on it staying the same in the scene to find
        /// which solver to add/remove this bone from.
        /// </summary>
        internal abstract BEPUikBone Bone { get; }

        internal abstract void draw(DebugDrawingSurface drawingSurface, DebugDrawMode drawMode);
    }
}
