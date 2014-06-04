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
    public class BEPUikTwistJoint : BEPUikJoint
    {
        private IKTwistJoint joint;

        public BEPUikTwistJoint(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikTwistJointDefinition definition, String name, Subscription subscription)
            :base(connectionA, connectionB, name, subscription)
        {
            joint = new IKTwistJoint(connectionA.IkBone, connectionB.IkBone, definition.WorldAxisA.toBepuVec3(), definition.WorldAxisB.toBepuVec3());
            setupJoint(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikTwistJointDefinition(Name)
            {
                WorldAxisA = joint.AxisA.toEngineVec3(),
                WorldAxisB = joint.AxisB.toEngineVec3()
            };
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
