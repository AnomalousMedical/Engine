using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.ObjectManagement;

namespace PhysXPlugin
{
    public class PhysSphericalJointElement : PhysJointElementBase<PhysSphericalJoint>
    {
        internal PhysSphericalJointElement(String name, PhysSphericalJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(name, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysSphericalJointDefinition(Name, this);
        }
    }
}
