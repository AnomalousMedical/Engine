using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.ObjectManagement;

namespace PhysXPlugin
{
    public class PhysPointOnLineJointElement : PhysJointElementBase<PhysPointOnLineJoint>
    {
        internal PhysPointOnLineJointElement(String name, PhysPointOnLineJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(name, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysPointOnLineJointDefinition(Name, this);
        }
    }
}
