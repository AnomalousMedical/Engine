using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    public partial class Generic6DofConstraintElement : TypedConstraintElement
    {
        public Generic6DofConstraintElement(Generic6DofConstraintDefinition definition, SimObjectBase instance, RigidBody rbA, RigidBody rbB, BulletScene scene)
            :base(definition.Name, definition.Subscription, scene, rbA, rbB)
        {
            setConstraint(btGeneric6DofConstraint_Create(rbA.NativeRigidBody, rbB.NativeRigidBody, instance.Translation, instance.Rotation, definition.translationMotor, definition.xRotMotor, definition.yRotMotor, definition.zRotMotor));
        }

        public override SimElementDefinition saveToDefinition()
        {
            Generic6DofConstraintDefinition definition = new Generic6DofConstraintDefinition(Name);
	        definition.RigidBodyAElement = RigidBodyA.Name;
	        definition.RigidBodyASimObject = RigidBodyA.Owner.Name;
	        definition.RigidBodyBElement = RigidBodyB.Name;
	        definition.RigidBodyBSimObject = RigidBodyB.Owner.Name;
            btGeneric6DofConstraint_copyMotors(constraint, definition.translationMotor, definition.xRotMotor, definition.yRotMotor, definition.zRotMotor);
	        return definition;
        }

        public void setFrameOffsetA(Vector3 origin)
        {
            btGeneric6DofConstraint_setFrameOffsetOriginA(constraint, ref origin);
        }

        public void setFrameOffsetA(Quaternion basis)
        {
            btGeneric6DofConstraint_setFrameOffsetBasisA(constraint, ref basis);
        }

        public void setFrameOffsetA(Vector3 origin, Quaternion basis)
        {
            btGeneric6DofConstraint_setFrameOffsetOriginBasisA(constraint, ref origin, ref basis);
        }

        public void setFrameOffsetB(Vector3 origin)
        {
            btGeneric6DofConstraint_setFrameOffsetOriginB(constraint, ref origin);
        }

        public void setFrameOffsetB(Quaternion basis)
        {
            btGeneric6DofConstraint_setFrameOffsetBasisB(constraint, ref basis);
        }

        public void setFrameOffsetB(Vector3 origin, Quaternion basis)
        {
            btGeneric6DofConstraint_setFrameOffsetBasisB(constraint, ref basis);
        }

        public Vector3 getFrameOffsetOriginA()
        {
            return btGeneric6DofConstraint_getFrameOffsetOriginA(constraint);
        }

        public Quaternion getFrameOffsetBasisA()
        {
            return btGeneric6DofConstraint_getFrameOffsetBasisA(constraint);
        }

        public Vector3 getFrameOffsetOriginB()
        {
            return btGeneric6DofConstraint_getFrameOffsetOriginB(constraint);
        }

        public Quaternion getFrameOffsetBasisB()
        {
            return btGeneric6DofConstraint_getFrameOffsetBasisB(constraint);
        }

        public void setLimit(int axis, float lo, float hi)
        {
            btGeneric6DofConstraint_setLimit(constraint, axis, lo, hi);
        }

        public void setLinearLowerLimit(Vector3 linearLower)
        {
            btGeneric6DofConstraint_setLinearLowerLimit(constraint, ref linearLower);
        }

        public void setLinearUpperLimit(Vector3 linearUpper)
        {
            btGeneric6DofConstraint_setLinearUpperLimit(constraint, ref linearUpper);
        }

        public void setAngularLowerLimit(Vector3 angularLower)
        {
            btGeneric6DofConstraint_setAngularLowerLimit(constraint, ref angularLower);
        }

        public void setAngularUpperLimit(Vector3 angularUpper)
        {
            btGeneric6DofConstraint_setAngularUpperLimit(constraint, ref angularUpper);
        }

        public void setParam(Param num, float value, int axis)
        {
            btGeneric6DofConstraint_setParam(constraint, (int)num, value, axis);
        }
    }

    partial class Generic6DofConstraintElement
    {
        [DllImport("BulletWrapper")]
        private static extern IntPtr btGeneric6DofConstraint_Create(IntPtr rbA, IntPtr rbB, Vector3 jointPos, Quaternion jointRot, TranslationalLimitMotorDefinition transMotor, RotationalLimitMotorDefinition xRotMotor, RotationalLimitMotorDefinition yRotMotor, RotationalLimitMotorDefinition zRotMotor);

        [DllImport("BulletWrapper")]
        private static extern void btGeneric6DofConstraint_setFrameOffsetOriginA(IntPtr instance, ref Vector3 origin);

        [DllImport("BulletWrapper")]
        private static extern void btGeneric6DofConstraint_setFrameOffsetBasisA(IntPtr instance, ref Quaternion basis);

        [DllImport("BulletWrapper")]
        private static extern void btGeneric6DofConstraint_setFrameOffsetOriginBasisA(IntPtr instance, ref Vector3 origin, ref Quaternion basis);

        [DllImport("BulletWrapper")]
        private static extern void btGeneric6DofConstraint_setFrameOffsetOriginB(IntPtr instance, ref Vector3 origin);

        [DllImport("BulletWrapper")]
        private static extern void btGeneric6DofConstraint_setFrameOffsetBasisB(IntPtr instance, ref Quaternion basis);

        [DllImport("BulletWrapper")]
        private static extern void btGeneric6DofConstraint_setFrameOffsetOriginBasisB(IntPtr instance, ref Vector3 origin, ref Quaternion basis);

        [DllImport("BulletWrapper")]
        private static extern Vector3 btGeneric6DofConstraint_getFrameOffsetOriginA(IntPtr instance);

        [DllImport("BulletWrapper")]
        private static extern Quaternion btGeneric6DofConstraint_getFrameOffsetBasisA(IntPtr instance);

        [DllImport("BulletWrapper")]
        private static extern Vector3 btGeneric6DofConstraint_getFrameOffsetOriginB(IntPtr instance);

        [DllImport("BulletWrapper")]
        private static extern Quaternion btGeneric6DofConstraint_getFrameOffsetBasisB(IntPtr instance);

        [DllImport("BulletWrapper")]
        private static extern void btGeneric6DofConstraint_setLimit(IntPtr instance, int axis, float lo, float hi);

        [DllImport("BulletWrapper")]
        private static extern void btGeneric6DofConstraint_setLinearLowerLimit(IntPtr instance, ref Vector3 linearLower);

        [DllImport("BulletWrapper")]
        private static extern void btGeneric6DofConstraint_setLinearUpperLimit(IntPtr instance, ref Vector3 linearUpper);

        [DllImport("BulletWrapper")]
        private static extern void btGeneric6DofConstraint_setAngularLowerLimit(IntPtr instance, ref Vector3 angularLower);

        [DllImport("BulletWrapper")]
        private static extern void btGeneric6DofConstraint_setAngularUpperLimit(IntPtr instance, ref Vector3 angularUpper);

        [DllImport("BulletWrapper")]
        private static extern void btGeneric6DofConstraint_setParam(IntPtr instance, int num, float value, int axis);

        [DllImport("BulletWrapper")]
        private static extern void btGeneric6DofConstraint_copyMotors(IntPtr instance, [Out] TranslationalLimitMotorDefinition transMotor, [Out] RotationalLimitMotorDefinition xRotMotor, [Out] RotationalLimitMotorDefinition yRotMotor, [Out] RotationalLimitMotorDefinition zRotMotor);
    }
}
