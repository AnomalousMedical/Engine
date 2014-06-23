using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    public partial class FixedConstraintElement : TypedConstraintElement
    {
        public FixedConstraintElement(FixedConstraintDefinition definition, SimObjectBase instance, RigidBody rbA, RigidBody rbB, BulletScene scene)
            :base(definition.Name, definition.Subscription, scene, rbA, rbB)
        {
            setConstraint(btFixedConstraint_Create(rbA.NativeRigidBody, rbB.NativeRigidBody, instance.Translation, instance.Rotation));
        }

        public override SimElementDefinition saveToDefinition()
        {
            FixedConstraintDefinition definition = new FixedConstraintDefinition(Name);
	        definition.RigidBodyAElement = RigidBodyA.Name;
	        definition.RigidBodyASimObject = RigidBodyA.Owner.Name;
	        definition.RigidBodyBElement = RigidBodyB.Name;
	        definition.RigidBodyBSimObject = RigidBodyB.Owner.Name;
	        return definition;
        }
    
        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr btFixedConstraint_Create(IntPtr rbA, IntPtr rbB, Vector3 jointPos, Quaternion jointRot);
    }
}
