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
    public class BEPUikBallSocketJoint : SimElement
    {
        private BEPUikScene scene;
        private IKBallSocketJoint joint;
        private BEPUikBone connectionA;
        private BEPUikBone connectionB;

        public BEPUikBallSocketJoint(BEPUikBone connectionA, BEPUikBone connectionB, Vector3 anchor, BEPUikScene scene, String name, Subscription subscription)
            :base(name, subscription)
        {
            this.scene = scene;
            this.connectionA = connectionA;
            this.connectionB = connectionB;
            joint = new IKBallSocketJoint(connectionA.IkBone, connectionB.IkBone, anchor.toBepuVec3());
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
            
        }

        public override SimElementDefinition saveToDefinition()
        {
            return new BEPUikBallSocketJointDefinition(Name)
            {
                ConnectionABoneName = connectionA.Name,
                ConnectionASimObjectName = connectionA.Owner == Owner ? "this" : connectionA.Owner.Name,
                ConnectionBBoneName = connectionB.Name,
                ConnectionBSimObjectName = connectionB.Owner == Owner ? "this" : connectionB.Owner.Name,
                Subscription = this.Subscription
            };
        }
    }
}
