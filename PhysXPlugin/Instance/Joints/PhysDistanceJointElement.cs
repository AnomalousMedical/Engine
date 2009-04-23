using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.ObjectManagement;

namespace PhysXPlugin
{
    class PhysDistanceJointElement : PhysJointElementBase<PhysDistanceJoint>
    {
        public PhysDistanceJointElement(Identifier jointId, PhysDistanceJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(jointId, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysDistanceJointDefinition(jointId.ElementName, this);
        }
    }
}
