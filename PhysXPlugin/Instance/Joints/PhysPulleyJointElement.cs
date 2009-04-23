using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using PhysXWrapper;

namespace PhysXPlugin
{
    class PhysPulleyJointElement : PhysJointElementBase<PhysPulleyJoint>
    {
        public PhysPulleyJointElement(Identifier jointId, PhysPulleyJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(jointId, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysPulleyJointDefinition(jointId.ElementName, this);
        }
    }
}
