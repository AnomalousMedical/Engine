using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Saving;

namespace BulletPlugin
{
    public class FixedConstraintDefinition : TypedConstraintDefinition
    {
        public FixedConstraintDefinition(String name)
            :base(name)
        {
            
        }

        protected override TypedConstraintElement createConstraint(RigidBody rbA, RigidBody rbB, SimObjectBase instance, BulletScene scene)
        {
            if(rbA != null && rbB != null)
	        {
                return new FixedConstraintElement(this, instance, rbA, rbB, scene);
	        }
	        return null;
        }

        public override string JointType
        {
            get
            {
                return "Fixed Constraint";
            }
        }

        internal static void Create(String name, EditUICallback callback, CompositeSimObjectDefinition simObjectDef)
        {
            simObjectDef.addElement(new FixedConstraintDefinition(name));
        }

        protected FixedConstraintDefinition(LoadInfo info)
            :base(info)
        {
	        
        }

        public override void getInfo(SaveInfo info)
        {
	        base.getInfo(info);
        }
    }
}
