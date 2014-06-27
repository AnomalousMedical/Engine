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
    public abstract class BEPUikLimit : BEPUikJoint
    {
        public BEPUikLimit(BEPUikBone connectionA, BEPUikBone connectionB, String name, Subscription subscription, SimObject instance)
            :base(connectionA, connectionB, name, subscription, instance)
        {
               
        }

        public abstract bool Locked { get; set; }

        protected void setupLimit(BEPUikJointDefinition definition)
        {
            setupJoint(definition);
        }

        protected void setupLimitDefinition(BEPUikJointDefinition definition)
        {
            setupJointDefinition(definition);
        }

        internal override sealed IKJoint IKJoint
        {
            get
            {
                return IKLimit;
            }
        }

        internal abstract IKLimit IKLimit { get; }
    }
}
