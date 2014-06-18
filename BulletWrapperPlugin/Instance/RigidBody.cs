using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using Engine.ObjectManagement;

namespace BulletPlugin
{
    /// <summary>
    /// This is the callback for a collision event. The contact info has information
    /// about the contact, sourceBody is the body that fired the event, otherBody is
    /// the other rigid body in the collision and isBodyA indicates if the RigidBody
    /// that fired the event is rigidBodyA in the contact info.
    /// </summary>
    public delegate void CollisionCallback(ContactInfo contact, RigidBody sourceBody, RigidBody otherBody, bool isBodyA);

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
        private IntPtr rigidBody;
        private BulletScene scene;
        String shapeName;
        short collisionFilterMask;
        short collisionFilterGroup;
        private MotionState motionState;

        public unsafe RigidBody(RigidBodyDefinition description, BulletScene scene, IntPtr collisionShape, Vector3 initialTrans, Quaternion initialRot)
            :base(description.Name, description.Subscription)
        {
            this.scene = scene;
            shapeName = description.ShapeName;
            collisionFilterMask = description.CollisionFilterMask;
            collisionFilterGroup = description.CollisionFilterGroup;
            StayLocalTransform = description.StayLocalTransform;
            motionState = scene.createMotionState(this, description.MaxContactDistance, ref initialTrans, ref initialRot);
	
            rigidBody = btRigidBody_Create(ref description.constructionInfo, motionState.motionState, collisionShape);

            setLinearVelocity(description.LinearVelocity);
	        setAngularVelocity(description.AngularVelocity);
	        forceActivationState(description.CurrentActivationState);
	        setAnisotropicFriction(description.AnisotropicFriction);
	        setDeactivationTime(description.DeactivationTime);
	        setCollisionFlags(description.Flags);
	        setHitFraction(description.HitFraction);

            RigidBodyManager.add(rigidBody, this);
        }

        protected override void Dispose()
        {
            if(rigidBody != IntPtr.Zero)
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

                if (Owner.Enabled)
                {
                    scene.removeRigidBody(this);
                }

                RigidBodyManager.remove(rigidBody);

                btRigidBody_Delete(rigidBody);
                rigidBody = IntPtr.Zero;

                motionState.Dispose();
            }
        }

        /// <summary>
        /// Get an alert that contact started on this rigid body.
        /// 
        /// Note that this event will come on whatever thread the physics is running on, which may not be the main thread
        /// and may be during another operation such as rendering. You should only update physics related classes or store the
        /// state until a behavior's update function to ensure that you do not update resources being used by another thread.
        /// </summary>
        public event CollisionCallback ContactStarted
        {
            add
            {
                motionState.ContactStarted += value;
            }
            remove
            {
                motionState.ContactStarted -= value;
            }
        }

        /// <summary>
        /// Get an alert that contact ended on this rigid body.
        /// 
        /// Note that this event will come on whatever thread the physics is running on, which may not be the main thread
        /// and may be during another operation such as rendering. You should only update physics related classes or store the
        /// state until a behavior's update function to ensure that you do not update resources being used by another thread.
        /// </summary>
        public event CollisionCallback ContactEnded
        {
            add
            {
                motionState.ContactEnded += value;
            }
            remove
            {
                motionState.ContactEnded -= value;
            }
        }

        /// <summary>
        /// Get an alert that contact is occuring and continues on this rigid body.
        /// 
        /// Note that this event will come on whatever thread the physics is running on, which may not be the main thread
        /// and may be during another operation such as rendering. You should only update physics related classes or store the
        /// state until a behavior's update function to ensure that you do not update resources being used by another thread.
        /// </summary>
        public event CollisionCallback ContactContinues
        {
            add
            {
                motionState.ContactContinues += value;
            }
            remove
            {
                motionState.ContactContinues -= value;
            }
        }

        public bool IsNull
        {
            get
            {
                return rigidBody == IntPtr.Zero;
            }
        }

        internal IntPtr NativeRigidBody
        {
            get
            {
                return rigidBody;
            }
        }

        internal void syncObjectPosition()
        {
            if (motionState.PositionUpdated)
            {
                Vector3 updatedTranslation = motionState.UpdatedTranslation;
                Quaternion updatedRotation = motionState.UpdatedRotation;
                updatePosition(ref updatedTranslation, ref updatedRotation);
                motionState.positionSynched();
            }
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
            definition.MaxContactDistance = motionState.MaxContactDistance;
            definition.CollisionFilterGroup = collisionFilterGroup;
            definition.CollisionFilterMask = collisionFilterMask;
            definition.StayLocalTransform = StayLocalTransform;
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

        public float MaxContactDistance
        {
            get
            {
                return motionState.MaxContactDistance;
            }
            set
            {
                motionState.MaxContactDistance = value;
            }
        }

        public BulletScene Scene
        {
            get
            {
                return scene;
            }
        }

        public bool StayLocalTransform { get; private set; }
    }

    //Dll Imports
    unsafe partial class RigidBody
    {
        //btRigidBody

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr btRigidBody_Create(ref RigidBodyConstructionInfo constructionInfo, IntPtr motionState, IntPtr collisionShape);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_Delete(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setDamping(IntPtr instance, float linearDamping, float angularDamping);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float btRigidBody_getLinearDamping(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float btRigidBody_getAngularDamping(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float btRigidBody_getLinearSleepingThreshold(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float btRigidBody_getAngularSleepingThreshold(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setMassProps(IntPtr instance, float mass);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setMassPropsInertia(IntPtr instance, float mass, ref Vector3 inertia);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float btRigidBody_getInvMass(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_applyCentralForce(IntPtr instance, ref Vector3 force);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 btRigidBody_getTotalForce(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 btRigidBody_getTotalTorque(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setSleepingThresholds(IntPtr instance, float linear, float angular);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_applyTorque(IntPtr instance, ref Vector3 torque);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_applyForce(IntPtr instance, ref Vector3 force, ref Vector3 rel_pos);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_applyCentralImpulse(IntPtr instance, ref Vector3 impulse);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_applyTorqueImpulse(IntPtr instance, ref Vector3 torque);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_clearForces(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 btRigidBody_getCenterOfMassPosition(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 btRigidBody_getLinearVelocity(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 btRigidBody_getAngularVelocity(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setLinearVelocity(IntPtr instance, ref Vector3 lin_vel);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setAngularVelocity(IntPtr instance, ref Vector3 ang_vel);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 btRigidBody_getVelocityInLocalPoint(IntPtr instance, ref Vector3 rel_pos);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_translate(IntPtr instance, ref Vector3 v);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_getAabb(IntPtr instance, out Vector3 aabbMin, out Vector3 aabbMax);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float btRigidBody_computeImpulseDenominator(IntPtr instance, ref Vector3 pos, ref Vector3 normal);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float btRigidBody_computeAngularImpulseDenominator(IntPtr instance, ref Vector3 axis);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool btRigidBody_wantsSleeping(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool btRigidBody_isInWorld(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setAnisotropicFriction(IntPtr instance, ref Vector3 anisotropicFriction);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 btRigidBody_getAnisotropicFriction(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool btRigidBody_hasAnisotropicFriction(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool btRigidBody_isStaticObject(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool btRigidBody_isKinematicObject(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool btRigidBody_isStaticOrKinematicObject(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int btRigidBody_getActivationState(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setActivationState(IntPtr instance, int state);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setDeactivationTime(IntPtr instance, float time);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float btRigidBody_getDeactivationTime(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_forceActivationState(IntPtr instance, int state);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_activate(IntPtr instance, bool forceActivation);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool btRigidBody_isActive(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setRestitution(IntPtr instance, float restitution);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float btRigidBody_getRestitution(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setFriction(IntPtr instance, float friction);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float btRigidBody_getFriction(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setHitFraction(IntPtr instance, float fraction);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float btRigidBody_getHitFraction(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int btRigidBody_getCollisionFlags(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setCollisionFlags(IntPtr instance, int flags);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setLocalScaling(IntPtr instance, ref Vector3 scaling);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 btRigidBody_getLocalScaling(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setWorldTranslation(IntPtr rigidBody, ref Vector3 trans);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setWorldRotation(IntPtr rigidBody, ref Quaternion rot);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void btRigidBody_setWorldTransform(IntPtr rigidBody, ref Vector3 trans, ref Quaternion rot);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int btRigidBody_getNumConstraintRefs(IntPtr rigidBody);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr btRigidBody_getConstraintRef(IntPtr rigidBody, int num);
    }
}
