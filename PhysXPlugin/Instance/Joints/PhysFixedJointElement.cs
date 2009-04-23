﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using PhysXWrapper;

namespace PhysXPlugin
{
    public class PhysFixedJointElement : PhysJointElementBase<PhysFixedJoint>
    {
        internal PhysFixedJointElement(Identifier jointId, PhysFixedJoint joint, PhysXSceneManager scene, Subscription subscription)
            :base(jointId, joint, scene, subscription)
        {

        }

        public override SimElementDefinition saveToDefinition()
        {
            return new PhysFixedJointDefinition(jointId.ElementName, this);
        }
    }
}
