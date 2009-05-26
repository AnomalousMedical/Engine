using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using PhysXWrapper;

namespace PhysXPlugin
{
    public class PhysFixedJointElement : PhysJointElementBase<PhysFixedJoint>
    {
        internal PhysFixedJointElement(String name, PhysFixedJoint joint, PhysXSceneManager scene, Subscription subscription)
            :base(name, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysFixedJointDefinition(Name, this);
        }
    }
}
