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
        internal PhysPointOnLineJointElement(Identifier jointId, PhysPointOnLineJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(jointId, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysPointOnLineJointDefinition(jointId.ElementName, this);
        }
    }
}
