using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.ObjectManagement;

namespace PhysXPlugin
{
    class PhysRevoluteJointElement : PhysJointElementBase<PhysRevoluteJoint>
    {
        public PhysRevoluteJointElement(Identifier jointId, PhysRevoluteJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(jointId, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysRevoluteJointDefinition(jointId.ElementName, this);
        }
    }
}
