#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;

namespace BulletPlugin
{

class MotionState;
ref class RigidBodyDefinition;
ref class BulletScene;
ref class ContactInfo;
ref class RigidBody;

/// <summary>
/// This is the callback for a collision event. The contact info has information
/// about the contact, sourceBody is the body that fired the event, otherBody is
/// the other rigid body in the collision and isBodyA indicates if the RigidBody
/// that fired the event is rigidBodyA in the contact info.
/// </summary>
public delegate void CollisionCallback(ContactInfo^ contact, RigidBody^ sourceBody, RigidBody^ otherBody, bool isBodyA);

[Engine::Attributes::NativeSubsystemType]
public ref class RigidBody : public SimElement
{
private:
	MotionState* motionState;
	BulletScene^ scene;
	btRigidBody* rigidBody;
	String^ shapeName;
	float maxContactDistance;
	short collisionFilterMask;
	short collisionFilterGroup;

protected:
	virtual void updatePositionImpl(Vector3% translation, Quaternion% rotation) override;

	virtual void updateTranslationImpl(Vector3% translation) override;

	virtual void updateRotationImpl(Quaternion% rotation) override;

	virtual void updateScaleImpl(Vector3% scale) override;

	virtual void setEnabled(bool enabled) override;

internal:
	property btRigidBody* Body
	{
		btRigidBody* get()
		{
			return rigidBody;
		}
	}

	void fireContactStarted(ContactInfo^ contact, RigidBody^ otherBody, bool isBodyA)
	{
		ContactStarted(contact, this, otherBody, isBodyA);
	}

	void fireContactContinues(ContactInfo^ contact, RigidBody^ otherBody, bool isBodyA)
	{
		ContactContinues(contact, this, otherBody, isBodyA);
	}

	void fireContactEnded(ContactInfo^ contact, RigidBody^ otherBody, bool isBodyA)
	{
		ContactEnded(contact, this, otherBody, isBodyA);
	}

public:
	event CollisionCallback^ ContactStarted;

	event CollisionCallback^ ContactContinues;

	event CollisionCallback^ ContactEnded;

	RigidBody(RigidBodyDefinition^ description, BulletScene^ scene, Vector3 initialTrans, Quaternion initialRot);

	virtual ~RigidBody(void);

	virtual SimElementDefinition^ saveToDefinition() override;

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

	void getAabb(Vector3% aabbMin, Vector3% aabbMax);

	float computeImpulseDenominator(Vector3 pos, Vector3 normal);

	float computeAngularImpulseDenominator(Vector3 axis);

	bool wantsSleeping();

	bool isInWorld();

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

	property float MaxContactDistance
	{
		float get()
		{
			return maxContactDistance;
		}
		void set(float value)
		{
			maxContactDistance = value;
		}
	}
};

}