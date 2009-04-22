using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using PhysXWrapper;

namespace PhysXPlugin.Instance.Joints
{
    public class PhysFixedJointElement : PhysJointElementBase<PhysFixedJoint>
    {
        public PhysFixedJointElement(Identifier jointId, PhysFixedJoint joint, PhysXSceneManager scene, Subscription subscription)
            :base(jointId, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            throw new NotImplementedException();
        }
    }
}
