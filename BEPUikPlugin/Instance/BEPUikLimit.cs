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
        /// <summary>
        /// This event is fired when the locked status of a limit is changed.
        /// </summary>
        public event Action<BEPUikLimit> LockedChanged;

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

        protected void fireLockedChanged()
        {
            if(LockedChanged != null)
            {
                LockedChanged.Invoke(this);
            }
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
