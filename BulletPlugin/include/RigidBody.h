#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;

namespace BulletPlugin
{

class MotionState;
ref class RigidBodyDefinition;
ref class BulletScene;

public ref class RigidBody : public SimElement
{
private:
	MotionState* motionState;
	BulletScene^ scene;
	btRigidBody* rigidBody;

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

public:
	RigidBody(RigidBodyDefinition^ description, BulletScene^ scene);

	virtual ~RigidBody(void);

	virtual SimElementDefinition^ saveToDefinition() override;

	void setWorldTransform(Vector3 translation, Quaternion rotation);

	float getLinearDamping();

	float getAngularDamping();

	float getLinearSleepingThreshold();

	float getAngularSleepingThreshold();

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
};

}