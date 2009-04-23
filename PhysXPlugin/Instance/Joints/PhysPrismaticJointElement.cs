using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.ObjectManagement;

namespace PhysXPlugin
{
    public class PhysPrismaticJointElement : PhysJointElementBase<PhysPrismaticJoint>
    {
        internal PhysPrismaticJointElement(Identifier jointId, PhysPrismaticJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(jointId, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysPrismaticJointDefinition(jointId.ElementName, this);
        }
    }
}
