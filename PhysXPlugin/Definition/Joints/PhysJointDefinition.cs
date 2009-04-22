using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;
using PhysXWrapper;
using EngineMath;
using Logging;
using Engine.Reflection;
using Engine.Saving;

namespace PhysXPlugin
{
    /// <summary>
    /// Base class for joint definitions.
    /// </summary>
    public abstract class PhysJointDefinition : PhysElementDefinition
    {
        public PhysJointDefinition(String name)
            :base(name)
        {

        }

        protected PhysJointDefinition(LoadInfo info)
            :base(info)
        {

        }
    }
}
