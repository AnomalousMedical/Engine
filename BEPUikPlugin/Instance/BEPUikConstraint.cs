﻿using BEPUik;
using Engine;
using Engine.ObjectManagement;
using Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public abstract class BEPUikConstraint : SimElement
    {
        public BEPUikConstraint(String name)
            :base(name)
        {
            
        }

        protected void setupConstraint(BEPUikConstraintDefinition definition)
        {
            IKConstraint.MaximumForce = definition.MaximumForce;
            IKConstraint.Rigidity = definition.Rigidity;
            IKConstraint.UserObject = this;
        }

        protected void setupConstraintDefinition(BEPUikConstraintDefinition definition)
        {
            definition.MaximumForce = IKConstraint.MaximumForce;
            definition.Rigidity = IKConstraint.Rigidity;
        }

        internal abstract IKConstraint IKConstraint { get; }

        internal abstract void draw(DebugDrawingSurface drawingSurface, DebugDrawMode drawMode);
    }
}
