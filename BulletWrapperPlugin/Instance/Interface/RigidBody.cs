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

    public interface RigidBody : ISimElement
    {
        event CollisionCallback ContactStarted;

        event CollisionCallback ContactEnded;

        event CollisionCallback ContactContinues;

        bool IsNull { get; }

        SimElementDefinition saveToDefinition();

        void setDamping(float linearDamping, float angularDamping);

        float getLinearDamping();

        float getAngularDamping();

        float getLinearSleepingThreshold();

        float getAngularSleepingThreshold();

        void setMassProps(float mass);

        void setMassProps(float mass, Vector3 inertia);

        float getInvMass();

        void applyCentralForce(Vector3 force);

        Vector3 getTotalForce();

        Vector3 getTotalTorque();

        void setSleepingThresholds(float linear, float angular);

        void applyTorque(Vector3 torque);

        void applyForce(Vector3 force, Vector3 rel_pos);

        void applyCentralImpulse(Vector3 impulse);

        void applyTorqueImpulse(Vector3 torque);

        void clearForces();

        Vector3 getCenterOfMassPosition();

        Vector3 getLinearVelocity();

        Vector3 getAngularVelocity();

        void setLinearVelocity(Vector3 lin_vel);

        void setAngularVelocity(Vector3 ang_vel);

        Vector3 getVelocityInLocalPoint(Vector3 rel_pos);

        void translate(Vector3 v);

        void getAabb(out Vector3 aabbMin, out Vector3 aabbMax);

        float computeImpulseDenominator(Vector3 pos, Vector3 normal);

        float computeAngularImpulseDenominator(Vector3 axis);

        bool wantsSleeping();

        bool isInWorld();

        void removeFromWorld();

        void addToWorld();

        void setAnisotropicFriction(Vector3 anisotropicFriction);

        Vector3 getAnisotropicFriction();

        bool hasAnisotropicFriction();

        bool isStaticObject();

        bool isKinematicObject();

        bool isStaticOrKinematicObject();

        ActivationState getActivationState();

        void setActivationState(ActivationState state);

        void setDeactivationTime(float time);

        float getDeactivationTime();

        void forceActivationState(ActivationState state);

        void activate(bool forceActivation);

        bool isActive();

        void setRestitution(float restitution);

        float getRestitution();

        void setFriction(float friction);

        float getFriction();

        void setHitFraction(float fraction);

        float getHitFraction();

        CollisionFlags getCollisionFlags();

        void setCollisionFlags(CollisionFlags flags);

        void raiseCollisionFlag(CollisionFlags flag);

        void clearCollisionFlag(CollisionFlags flag);

        void setLocalScaling(Vector3 scaling);

        Vector3 getLocalScaling();

        float MaxContactDistance { get; set; }
    }
}
