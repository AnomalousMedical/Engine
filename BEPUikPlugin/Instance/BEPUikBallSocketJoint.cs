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
    public class BEPUikBallSocketJoint : BEPUikJoint
    {
        private BEPUikScene scene;
        private IKBallSocketJoint joint;

        public BEPUikBallSocketJoint(BEPUikBone connectionA, BEPUikBone connectionB, Vector3 anchor, BEPUikBallSocketJointDefinition definition, BEPUikScene scene, String name, Subscription subscription)
            :base(connectionA, connectionB, name, subscription)
        {
            this.scene = scene;
            joint = new IKBallSocketJoint(connectionA.IkBone, connectionB.IkBone, anchor.toBepuVec3());
            setupJoint(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikBallSocketJointDefinition(Name);
            setupJointDefinition(definition);
            return definition;
        }

        public override IKJoint IKJoint
        {
            get
            {
                return joint;
            }
        }
    }
}
