using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    public abstract partial class TypedConstraintElement : SimElement
    {
        public enum Param : int
        {
            CONSTRAINT_ERP = 1,
            CONSTRAINT_STOP_ERP,
            CONSTRAINT_STOP_CFM,
            CONSTRAINT_CFM,
        };

        internal IntPtr constraint;
	    BulletScene scene;
	    RigidBody rbA;
	    RigidBody rbB;
	    bool active; //true while both rigid bodies exist. If one is deleted setInactive will be called removing the joint and setting this to false.
	    bool enabled;

        public TypedConstraintElement(String name, Subscription subscription, BulletScene scene, RigidBody rbA, RigidBody rbB)
            :base(name, subscription)
        {
            constraint = IntPtr.Zero;
            this.rbA = rbA;
            this.rbB = rbB;
            this.scene = scene;
            active = true;
            enabled = false;
        }

        protected override void Dispose()
        {
            if (constraint != IntPtr.Zero)
            {
                TypedConstraintManager.removeConstraint(constraint);

                if (active && Owner.Enabled)
                {
                    scene.removeConstraint(this);
                }

                btTypedConstraint_Delete(constraint);
                constraint = IntPtr.Zero;
            }
        }

        protected void setConstraint(IntPtr constraint)
        {
            this.constraint = constraint;
            TypedConstraintManager.addConstraint(constraint, this);
        }

        protected override void updatePositionImpl(ref Engine.Vector3 translation, ref Engine.Quaternion rotation)
        {
            
        }

        protected override void updateTranslationImpl(ref Engine.Vector3 translation)
        {
            
        }

        protected override void updateRotationImpl(ref Engine.Quaternion rotation)
        {
            
        }

        protected override void updateScaleImpl(ref Engine.Vector3 scale)
        {
            
        }

        protected override void setEnabled(bool enabled)
        {
            if (this.enabled != enabled)
            {
                if (enabled)
                {
                    scene.addConstraint(this, true);
                }
                else
                {
                    scene.removeConstraint(this);
                }
                this.enabled = enabled;
            }
        }

        internal void setInactive()
        {
            if (active && Owner.Enabled)
            {
                scene.removeConstraint(this);
            }
            active = false;
        }

        public RigidBody RigidBodyA
        {
            get
            {
                return rbA;
            }
        }

        public RigidBody RigidBodyB
        {
            get
            {
                return rbB;
            }
        }

        public int OverrideNumIterations
        {
            get
            {
                return btTypedConstraint_getOverrideNumSolverIterations(constraint);
            }
            set
            {
                btTypedConstraint_setOverrideNumSolverIterations(constraint, value);
            }
        }
    }

    partial class TypedConstraintElement
    {
        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btTypedConstraint_Delete(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btTypedConstraint_setOverrideNumSolverIterations(IntPtr instance, int overrideNumIterations);

        [DllImport("BulletWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern int btTypedConstraint_getOverrideNumSolverIterations(IntPtr instance);
    }
}
