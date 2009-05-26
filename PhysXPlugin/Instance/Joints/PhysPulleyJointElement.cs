using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using PhysXWrapper;

namespace PhysXPlugin
{
    public class PhysPulleyJointElement : PhysJointElementBase<PhysPulleyJoint>
    {
        internal PhysPulleyJointElement(String name, PhysPulleyJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(name, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysPulleyJointDefinition(Name, this);
        }
    }
}
