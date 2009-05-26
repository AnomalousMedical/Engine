using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using PhysXWrapper;

namespace PhysXPlugin
{
    public abstract class PhysJointElement : SimElement
    {
        internal PhysJointElement(String name, Subscription subscription)
            :base(name, subscription)
        {

        }

        public abstract PhysJoint Joint
        {
            get;
        }

        public PhysActorElement Actor0 { get; set; }

        public PhysActorElement Actor1 { get; set; }
    }
}
