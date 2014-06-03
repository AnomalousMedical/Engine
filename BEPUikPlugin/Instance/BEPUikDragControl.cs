using BEPUik;
using Engine;
using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikDragControl : SimElement
    {
        private BEPUikScene scene;
        private DragControl dragControl;
        private BEPUikBone bone;

        public BEPUikDragControl(BEPUikBone bone, BEPUikScene scene, String name, Subscription subscription)
            :base(name, subscription)
        {
            this.scene = scene;
            this.bone = bone;
            dragControl = new DragControl();
            dragControl.TargetBone = bone.IkBone;
        }

        protected override void Dispose()
        {
            
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
            if(enabled)
            {
                scene.addControl(dragControl);
            }
            else
            {
                scene.removeControl(dragControl);
            }
        }

        public override SimElementDefinition saveToDefinition()
        {
            return new BEPUikDragControlDefinition(Name)
            {
                BoneSimObjectName = Owner == bone.Owner ? "this" : Owner.Name,
                BoneName = bone.Name,
                Subscription = this.Subscription
            };
        }
    }
}
