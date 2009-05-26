using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.ObjectManagement;

namespace PhysXPlugin
{
    public class PhysDistanceJointElement : PhysJointElementBase<PhysDistanceJoint>
    {
        internal PhysDistanceJointElement(String name, PhysDistanceJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(name, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysDistanceJointDefinition(Name, this);
        }
    }
}
