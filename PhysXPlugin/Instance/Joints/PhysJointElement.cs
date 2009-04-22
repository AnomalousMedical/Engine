using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using EngineMath;
using PhysXWrapper;

namespace PhysXPlugin
{
    public abstract class PhysJointElement : SimElement
    {
        public PhysJointElement(String name, Subscription subscription)
            :base(name, subscription)
        {

        }
    }
}
