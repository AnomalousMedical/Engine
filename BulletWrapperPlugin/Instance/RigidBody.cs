using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using Engine.ObjectManagement;

namespace BulletPlugin
{
    [Engine.Attributes.SingleEnum]
    public enum ActivationState : int
    {
        ActiveTag = 1,
	    IslandSleeping = 2,
	    WantsDeactivation = 3,
	    DisableDeactivation = 4,
	    DisableSimulation = 5,
    }

    [Engine.Attributes.MultiEnum]
    public enum CollisionFlags : int
    {
	    StaticObject = 1,
	    KinematicObject = 2,
	    NoContactResponse = 4,
	    CustomMaterialCallback = 8,
	    CharacterCallback = 16,
        DisableVisualizeObject = 32,
        DisableSPUCollisionProcessing = 64,
    };

    public partial class RigidBody : SimElement
    {
        private static IntPtr nullIntPtr = new IntPtr(0);
        private static HandleRef deletedRef = new HandleRef(null, nullIntPtr);

        private HandleRef rigidBody;
        private BulletScene scene;
        String shapeName;
        float maxContactDistance;
        short collisionFilterMask;
        short collisionFilterGroup;
        private SetXformCallback xformCallback;
        private IntPtr motionState;

        public unsafe RigidBody(RigidBodyDefinition description, BulletScene scene, IntPtr collisionShape, Vector3 initialTrans, Quaternion initialRot)
            :base(description.Name, description.Subscription)
        {
            this.scene = scene;
            shapeName = description.ShapeName;
            maxContactDistance = description.MaxContactDistance;
            collisionFilterMask = description.CollisionFilterMask;
            collisionFilterGroup = description.CollisionFilterGroup;
            xformCallback = new SetXformCallback(motionStateCallback);
            motionState = MotionState_Create(xformCallback, ref initialTrans, ref initialRot);
	
            rigidBody = new HandleRef(this, btRigidBody_Create(ref description.constructionInfo, motionState, collisionShape));

            setLinearVelocity(description.LinearVelocity);
	        setAngularVelocity(description.AngularVelocity);
	        forceActivationState(description.CurrentActivationState);
	        setAnisotropicFriction(description.AnisotropicFriction);
	        setDeactivationTime(description.DeactivationTime);
	        setCollisionFlags(description.Flags);
	        setHitFraction(description.HitFraction);
        }

        protected override void Dispose()
        {
            if(rigidBody.Handle != nullIntPtr)
            {
                int numRefs = btRigidBody_getNumConstraintRefs(rigidBody);
                List<TypedConstraintElement> constraints = new List<TypedConstraintElement>(numRefs);
                //Gather up all constraints
                for(int i = 0; i < numRefs; ++i)
                {
                    IntPtr typedConstraint = btRigidBody_getConstraintRef(rigidBody, i);
                    TypedConstraintElement element = TypedConstraintManager.getElement(typedConstraint);
                    if(element != null)
                    {
                        constraints.Add(element);
                    }
                }
                //Set all constraints to inactive
                foreach(TypedConstraintElement constraint in constraints)
                {
                    constraint.setInactive();
                }

                MotionState_Delete(motionState);
                motionState = nullIntPtr;
                xformCallback = null;

                if (Owner.Enabled)
                {
                    scene.removeRigidBody(this);
                }

                btRigidBody_Delete(rigidBody);
                rigidBody = deletedRef;
            }
        }

        internal HandleRef NativeRigidBody
        {
            get
            {
                return rigidBody;
            }
        }

        protected void motionStateCallback(Vector3 trans, Quaternion rot)
        {
            this.updatePosition(ref trans, ref rot);
        }

        protected override void updatePositionImpl(ref Vector3 translation, ref Quaternion rotation)
        {
            btRigidBody_setWorldTransform(rigidBody, ref translation, ref rotation);
        }

        protected override void updateTranslationImpl(ref Vector3 translation)
        {
            btRigidBody_setWorldTranslation(rigidBody, ref translation);
        }

        protected override void updateRotationImpl(ref Quaternion rotation)
        {
            btRigidBody_setWorldRotation(rigidBody, ref rotation);
        }

        protected override void updateScaleImpl(ref Vector3 scale)
        {
            btRigidBody_setLocalScaling(rigidBody, ref scale);
        }

        protected override void setEnabled(bool enabled)
        {
            if (enabled)
            {
                scene.addRigidBody(this, collisionFilterGroup, collisionFilterMask);
            }
            else
            {
                scene.removeRigidBody(this);
            }
        }

        public override SimElementDefinition saveToDefinition()
        {
            RigidBodyDefinition definition = new RigidBodyDefinition(Name);
            fillOutDefinition(definition);
            return definition;
        }

        void fillOutDefinition(RigidBodyDefinition definition)
        {
            float mass = getInvMass();
            if(mass > 0.0f)
            {
                definition.Mass = 1.0f / mass;
            }
            else
            {
                definition.Mass = 0.0f;
            }
            definition.AngularDamping = getAngularDamping();
            definition.AngularSleepingThreshold = getAngularSleepingThreshold();
            definition.Friction = getFriction();
            definition.LinearDamping = getLinearDamping();
            definition.LinearSleepingThreshold = getLinearSleepingThreshold();
            definition.Restitution = getRestitution();
            definition.LinearVelocity = getLinearVelocity();
            definition.AngularVelocity = getAngularVelocity();
            definition.CurrentActivationState = getActivationState();
            definition.AnisotropicFriction = getAnisotropicFriction();
            definition.DeactivationTime = getDeactivationTime();
            definition.Flags = getCollisionFlags();
            definition.HitFraction = getHitFraction();
            definition.ShapeName = shapeName;
            definition.MaxContactDistance = maxContactDistance;
            definition.CollisionFilterGroup = collisionFilterGroup;
            definition.CollisionFilterMask = collisionFilterMask;
        }

        public void setDamping(float linearDamping, float angularDamping)
        {
	        btRigidBody_setDamping(rigidBody, linearDamping, angularDamping);
        }

        public float getLinearDamping()
        {
	        return btRigidBody_getLinearDamping(rigidBody);
        }

        public float getAngularDamping()
        {
	        return btRigidBody_getAngularDamping(rigidBody);
        }

        public float getLinearSleepingThreshold()
        {
	        return btRigidBody_getLinearSleepingThreshold(rigidBody);
        }

        public float getAngularSleepingThreshold()
        {
	        return btRigidBody_getAngularSleepingThreshold(rigidBody);
        }

        public void setMassProps(float mass)
        {
            btRigidBody_setMassProps(rigidBody, mass);
        }

        public void setMassProps(float mass, Vector3 inertia)
        {
	        btRigidBody_setMassPropsInertia(rigidBody, mass, ref inertia);
        }

        public float getInvMass()
        {
	        return btRigidBody_getInvMass(rigidBody);
        }

        public void applyCentralForce(Vector3 force)
        {
	        btRigidBody_applyCentralForce(rigidBody, ref force);
        }

        public Vector3 getTotalForce()
        {
	        return btRigidBody_getTotalForce(rigidBody);
        }

        public Vector3 getTotalTorque()
        {
	        return btRigidBody_getTotalTorque(rigidBody);
        }

        public void setSleepingThresholds(float linear, float angular)
        {
            btRigidBody_setSleepingThresholds(rigidBody, linear, angular);
        }

        public void applyTorque(Vector3 torque)
        {
            btRigidBody_applyTorque(rigidBody, ref torque);
        }

        public void applyForce(Vector3 force, Vector3 rel_pos)
        {
	        btRigidBody_applyForce(rigidBody, ref force, ref rel_pos);
        }

        public void applyCentralImpulse(Vector3 impulse)
        {
	        btRigidBody_applyCentralImpulse(rigidBody, ref impulse);
        }

        public void applyTorqueImpulse(Vector3 torque)
        {
	        btRigidBody_applyTorqueImpulse(rigidBody, ref torque);
        }

        public void clearForces()
        {
	        btRigidBody_clearForces(rigidBody);
        }

        public Vector3 getCenterOfMassPosition()
        {
	        return btRigidBody_getCenterOfMassPosition(rigidBody);
        }

        public Vector3 getLinearVelocity()
        {
	        return btRigidBody_getLinearVelocity(rigidBody);
        }

        public Vector3 getAngularVelocity()
        {
	        return btRigidBody_getAngularVelocity(rigidBody);
        }

        public void setLinearVelocity(Vector3 lin_vel)
        {
	        btRigidBody_setLinearVelocity(rigidBody, ref lin_vel);
        }

        public void setAngularVelocity(Vector3 ang_vel)
        {
	        btRigidBody_setAngularVelocity(rigidBody, ref ang_vel);
        }

        public Vector3 getVelocityInLocalPoint(Vector3 rel_pos)
        {
	        return btRigidBody_getVelocityInLocalPoint(rigidBody, ref rel_pos);
        }

        public void translate(Vector3 v)
        {
	        btRigidBody_translate(rigidBody, ref v);
        }

        public void getAabb(out Vector3 aabbMin, out Vector3 aabbMax)
        {
	        btRigidBody_getAabb(rigidBody, out aabbMin, out aabbMax);
        }

        public float computeImpulseDenominator(Vector3 pos, Vector3 normal)
        {
	        return btRigidBody_computeImpulseDenominator(rigidBody, ref pos, ref normal);
        }

        public float computeAngularImpulseDenominator(Vector3 axis)
        {
	        return btRigidBody_computeAngularImpulseDenominator(rigidBody, ref axis);
        }

        public bool wantsSleeping()
        {
            return btRigidBody_wantsSleeping(rigidBody);
        }

        public bool isInWorld()
        {
            return btRigidBody_isInWorld(rigidBody);
        }

        public void removeFromWorld()
        {
	        scene.removeRigidBody(this);
        }

        public void addToWorld()
        {
            scene.addRigidBody(this, collisionFilterGroup, collisionFilterMask);
        }

        public void setAnisotropicFriction(Vector3 anisotropicFriction)
        {
	        btRigidBody_setAnisotropicFriction(rigidBody, ref anisotropicFriction);
        }

        public Vector3 getAnisotropicFriction()
        {
            return btRigidBody_getAnisotropicFriction(rigidBody);
        }

        public bool hasAnisotropicFriction()
        {
            return btRigidBody_hasAnisotropicFriction(rigidBody);
        }

        public bool isStaticObject()
        {
            return btRigidBody_isStaticObject(rigidBody);
        }

        public bool isKinematicObject()
        {
            return btRigidBody_isKinematicObject(rigidBody);
        }

        public bool isStaticOrKinematicObject()
        {
            return btRigidBody_isStaticOrKinematicObject(rigidBody);
        }

        public ActivationState getActivationState()
        {
            return (ActivationState)(btRigidBody_getActivationState(rigidBody));
        }

        public void setActivationState(ActivationState state)
        {
	        btRigidBody_setActivationState(rigidBody, (int)state);
        }

        public void setDeactivationTime(float time)
        {
            btRigidBody_setDeactivationTime(rigidBody, time);
        }

        public float getDeactivationTime()
        {
	        return btRigidBody_getDeactivationTime(rigidBody);
        }

        public void forceActivationState(ActivationState state)
        {
            btRigidBody_forceActivationState(rigidBody, (int)state);
        }

        public void activate(bool forceActivation)
        {
	        btRigidBody_activate(rigidBody, forceActivation);
        }

        public bool isActive()
        {
	        return btRigidBody_isActive(rigidBody);
        }

        public void setRestitution(float restitution)
        {
	        btRigidBody_setRestitution(rigidBody, restitution);
        }

        public float getRestitution()
        {
	        return btRigidBody_getRestitution(rigidBody);
        }

        public void setFriction(float friction)
        {
	        btRigidBody_setFriction(rigidBody, friction);
        }

        public float getFriction()
        {
	        return btRigidBody_getFriction(rigidBody);
        }

        public void setHitFraction(float fraction)
        {
	        btRigidBody_setHitFraction(rigidBody, fraction);
        }

        public float getHitFraction()
        {
	        return btRigidBody_getHitFraction(rigidBody);
        }

        public CollisionFlags getCollisionFlags()
        {
	        return (CollisionFlags)(btRigidBody_getCollisionFlags(rigidBody));
        }

        public void setCollisionFlags(CollisionFlags flags)
        {
	        btRigidBody_setCollisionFlags(rigidBody, (int)flags);
        }

        public void raiseCollisionFlag(CollisionFlags flag)
        {	
	        btRigidBody_setCollisionFlags(rigidBody, btRigidBody_getCollisionFlags(rigidBody) | (int)flag);
        }

        public void clearCollisionFlag(CollisionFlags flag)
        {
	        int collisionFlags = btRigidBody_getCollisionFlags(rigidBody);
	        int clear = ~(int)flag;
	        btRigidBody_setCollisionFlags(rigidBody, collisionFlags & clear);
        }

        public void setLocalScaling(Vector3 scaling)
        {
	        btRigidBody_setLocalScaling(rigidBody, ref scaling);
        }

        public Vector3 getLocalScaling()
        {
	        return btRigidBody_getLocalScaling(rigidBody);
        }
    }

    //Dll Imports
    unsafe partial class RigidBody
    {
        //btRigidBody

        [DllImport("BulletWrapper")]
        private static extern IntPtr btRigidBody_Create(ref RigidBodyConstructionInfo constructionInfo, IntPtr motionState, IntPtr collisionShape);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_Delete(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setDamping(HandleRef instance, float linearDamping, float angularDamping);

        [DllImport("BulletWrapper")]
        private static extern float btRigidBody_getLinearDamping(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern float btRigidBody_getAngularDamping(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern float btRigidBody_getLinearSleepingThreshold(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern float btRigidBody_getAngularSleepingThreshold(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setMassProps(HandleRef instance, float mass);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setMassPropsInertia(HandleRef instance, float mass, ref Vector3 inertia);

        [DllImport("BulletWrapper")]
        private static extern float btRigidBody_getInvMass(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_applyCentralForce(HandleRef instance, ref Vector3 force);

        [DllImport("BulletWrapper")]
        private static extern Vector3 btRigidBody_getTotalForce(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern Vector3 btRigidBody_getTotalTorque(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setSleepingThresholds(HandleRef instance, float linear, float angular);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_applyTorque(HandleRef instance, ref Vector3 torque);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_applyForce(HandleRef instance, ref Vector3 force, ref Vector3 rel_pos);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_applyCentralImpulse(HandleRef instance, ref Vector3 impulse);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_applyTorqueImpulse(HandleRef instance, ref Vector3 torque);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_clearForces(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern Vector3 btRigidBody_getCenterOfMassPosition(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern Vector3 btRigidBody_getLinearVelocity(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern Vector3 btRigidBody_getAngularVelocity(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setLinearVelocity(HandleRef instance, ref Vector3 lin_vel);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setAngularVelocity(HandleRef instance, ref Vector3 ang_vel);

        [DllImport("BulletWrapper")]
        private static extern Vector3 btRigidBody_getVelocityInLocalPoint(HandleRef instance, ref Vector3 rel_pos);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_translate(HandleRef instance, ref Vector3 v);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_getAabb(HandleRef instance, out Vector3 aabbMin, out Vector3 aabbMax);

        [DllImport("BulletWrapper")]
        private static extern float btRigidBody_computeImpulseDenominator(HandleRef instance, ref Vector3 pos, ref Vector3 normal);

        [DllImport("BulletWrapper")]
        private static extern float btRigidBody_computeAngularImpulseDenominator(HandleRef instance, ref Vector3 axis);

        [DllImport("BulletWrapper")]
        private static extern bool btRigidBody_wantsSleeping(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern bool btRigidBody_isInWorld(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setAnisotropicFriction(HandleRef instance, ref Vector3 anisotropicFriction);

        [DllImport("BulletWrapper")]
        private static extern Vector3 btRigidBody_getAnisotropicFriction(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern bool btRigidBody_hasAnisotropicFriction(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern bool btRigidBody_isStaticObject(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern bool btRigidBody_isKinematicObject(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern bool btRigidBody_isStaticOrKinematicObject(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern int btRigidBody_getActivationState(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setActivationState(HandleRef instance, int state);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setDeactivationTime(HandleRef instance, float time);

        [DllImport("BulletWrapper")]
        private static extern float btRigidBody_getDeactivationTime(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_forceActivationState(HandleRef instance, int state);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_activate(HandleRef instance, bool forceActivation);

        [DllImport("BulletWrapper")]
        private static extern bool btRigidBody_isActive(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setRestitution(HandleRef instance, float restitution);

        [DllImport("BulletWrapper")]
        private static extern float btRigidBody_getRestitution(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setFriction(HandleRef instance, float friction);

        [DllImport("BulletWrapper")]
        private static extern float btRigidBody_getFriction(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setHitFraction(HandleRef instance, float fraction);

        [DllImport("BulletWrapper")]
        private static extern float btRigidBody_getHitFraction(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern int btRigidBody_getCollisionFlags(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setCollisionFlags(HandleRef instance, int flags);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setLocalScaling(HandleRef instance, ref Vector3 scaling);

        [DllImport("BulletWrapper")]
        private static extern Vector3 btRigidBody_getLocalScaling(HandleRef instance);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setWorldTranslation(HandleRef rigidBody, ref Vector3 trans);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setWorldRotation(HandleRef rigidBody, ref Quaternion rot);

        [DllImport("BulletWrapper")]
        private static extern void btRigidBody_setWorldTransform(HandleRef rigidBody, ref Vector3 trans, ref Quaternion rot);

        [DllImport("BulletWrapper")]
        private static extern int btRigidBody_getNumConstraintRefs(HandleRef rigidBody);

        [DllImport("BulletWrapper")]
        private static extern IntPtr btRigidBody_getConstraintRef(HandleRef rigidBody, int num);

        //MotionState
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void SetXformCallback(Vector3 trans, Quaternion rot);

        [DllImport("BulletWrapper")]
        private static extern IntPtr MotionState_Create(SetXformCallback xformCallback, ref Vector3 initialTrans, ref Quaternion initialRot);

        [DllImport("BulletWrapper")]
        private static extern void MotionState_Delete(IntPtr instance);
    }
}
