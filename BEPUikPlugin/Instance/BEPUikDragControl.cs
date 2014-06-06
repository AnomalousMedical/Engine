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
    public class BEPUikDragControl : BEPUikControl
    {
        private DragControl dragControl;
        private BEPUikBone bone;

        public BEPUikDragControl(BEPUikBone bone, BEPUikScene scene, String name, SimObjectBase instance, Subscription subscription)
            :base(scene, name, subscription)
        {
            this.bone = bone;
            dragControl = new DragControl();
            dragControl.TargetBone = bone.IkBone;
            dragControl.LinearMotor.Offset = (instance.Translation - bone.Owner.Translation).toBepuVec3();
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

        internal override void syncPosition()
        {
            dragControl.LinearMotor.TargetPosition = Owner.Translation.toBepuVec3();
        }

        public override Control IKControl
        {
            get
            {
                return dragControl;
            }
        }
    }
}
