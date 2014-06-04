using BEPUik;
using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public abstract class BEPUikJoint : BEPUikConstraint
    {
        public BEPUikJoint(String name, Subscription subscription)
            :base(name, subscription)
        {
            
        }

        protected void setupJoint(BEPUikJointDefinition definition)
        {
            setupConstraint(definition);
        }

        protected void setupJointDefinition(BEPUikJointDefinition definition)
        {
            setupConstraintDefinition(definition);
        }

        public override IKConstraint IKConstraint
        {
            get
            {
                return IKJoint;
            }
        }

        public abstract IKJoint IKJoint { get; }
    }
}
