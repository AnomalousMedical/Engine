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
    public class BEPUikAngularJoint : BEPUikJoint
    {
        private IKAngularJoint joint;

        public BEPUikAngularJoint(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikAngularJointDefinition definition, String name, Subscription subscription)
            :base(connectionA, connectionB, name, subscription)
        {
            joint = new IKAngularJoint(connectionA.IkBone, connectionB.IkBone);
            setupJoint(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikAngularJointDefinition(Name);
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
