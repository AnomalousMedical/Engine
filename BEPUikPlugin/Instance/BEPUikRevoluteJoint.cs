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
    public class BEPUikRevoluteJoint : BEPUikJoint
    {
        private IKRevoluteJoint joint;

        public BEPUikRevoluteJoint(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikRevoluteJointDefinition definition, String name, Subscription subscription)
            :base(connectionA, connectionB, name, subscription)
        {
            joint = new IKRevoluteJoint(connectionA.IkBone, connectionB.IkBone, definition.WorldFreeAxis.toBepuVec3());
            setupJoint(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikRevoluteJointDefinition(Name)
            {
                WorldFreeAxis = joint.WorldFreeAxisA.toEngineVec3()
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
