using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.ObjectManagement;

namespace PhysXPlugin
{
    class PhysPointOnLineJointElement : PhysJointElementBase<PhysPointOnLineJoint>
    {
        public PhysPointOnLineJointElement(Identifier jointId, PhysPointOnLineJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(jointId, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysPointOnLineJointDefinition(jointId.ElementName, this);
        }
    }
}
