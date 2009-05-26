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
        internal PhysPrismaticJointElement(String name, PhysPrismaticJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(name, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysPrismaticJointDefinition(Name, this);
        }
    }
}
