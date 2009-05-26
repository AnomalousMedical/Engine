using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.ObjectManagement;

namespace PhysXPlugin
{
    public class PhysRevoluteJointElement : PhysJointElementBase<PhysRevoluteJoint>
    {
        internal PhysRevoluteJointElement(String name, PhysRevoluteJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(name, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysRevoluteJointDefinition(Name, this);
        }
    }
}
