using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using PhysXWrapper;

namespace PhysXPlugin
{
    public class PhysCylindricalJointElement : PhysJointElementBase<PhysCylindricalJoint>
    {
        internal PhysCylindricalJointElement(String name, PhysCylindricalJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(name, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysCylindricalJointDefinition(Name, this);
        }
    }
}
