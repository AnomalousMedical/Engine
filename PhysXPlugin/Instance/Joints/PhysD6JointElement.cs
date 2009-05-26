using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.ObjectManagement;

namespace PhysXPlugin
{
    public class PhysD6JointElement : PhysJointElementBase<PhysD6Joint>
    {
        internal PhysD6JointElement(String name, PhysD6Joint joint, PhysXSceneManager scene, Subscription subscription)
            : base(name, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysD6JointDefinition(Name, this);
        }
    }
}
