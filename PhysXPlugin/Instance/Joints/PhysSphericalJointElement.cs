using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.ObjectManagement;

namespace PhysXPlugin
{
    class PhysSphericalJointElement : PhysJointElementBase<PhysSphericalJoint>
    {
        public PhysSphericalJointElement(Identifier jointId, PhysSphericalJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(jointId, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysSphericalJointDefinition(jointId.ElementName, this);
        }
    }
}
