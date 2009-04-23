﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using EngineMath;
using PhysXWrapper;

namespace PhysXPlugin
{
    abstract class PhysJointElement : SimElement
    {
        public PhysJointElement(String name, Subscription subscription)
            :base(name, subscription)
        {

        }

        public abstract PhysJoint Joint
        {
            get;
        }

        internal Identifier Actor0Identifier { get; set; }

        internal Identifier Actor1Identifier { get; set; }
    }
}
