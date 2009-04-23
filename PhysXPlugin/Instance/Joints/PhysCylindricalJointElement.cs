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
        internal PhysCylindricalJointElement(Identifier jointId, PhysCylindricalJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(jointId, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysCylindricalJointDefinition(jointId.ElementName, this);
        }
    }
}
