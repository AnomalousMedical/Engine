using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.ObjectManagement;

namespace PhysXPlugin
{
    public class PhysPointInPlaneJointElement : PhysJointElementBase<PhysPointInPlaneJoint>
    {
        internal PhysPointInPlaneJointElement(Identifier jointId, PhysPointInPlaneJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(jointId, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysPointInPlaneJointDefinition(jointId.ElementName, this);
        }
    }
}
