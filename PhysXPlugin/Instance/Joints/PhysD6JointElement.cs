using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.ObjectManagement;

namespace PhysXPlugin
{
    class PhysD6JointElement : PhysJointElementBase<PhysD6Joint>
    {
        public PhysD6JointElement(Identifier jointId, PhysD6Joint joint, PhysXSceneManager scene, Subscription subscription)
            : base(jointId, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysD6JointDefinition(jointId.ElementName, this);
        }
    }
}
