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
        internal PhysPointInPlaneJointElement(String name, PhysPointInPlaneJoint joint, PhysXSceneManager scene, Subscription subscription)
            : base(name, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysPointInPlaneJointDefinition(Name, this);
        }
    }
}
