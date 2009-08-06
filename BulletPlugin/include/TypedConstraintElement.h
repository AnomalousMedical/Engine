#pragma once

using namespace System;
using namespace Engine;
using namespace Engine::ObjectManagement;

namespace BulletPlugin
{

ref class BulletScene;
ref class RigidBody;

[Engine::Attributes::NativeSubsystemType]
public ref class TypedConstraintElement abstract : public SimElement
{
private:
	btTypedConstraint* constraint;
	BulletScene^ scene;
	RigidBody^ rbA;
	RigidBody^ rbB;

protected:
	void setConstraint(btTypedConstraint* constraint)
	{
		this->constraint = constraint;
	}

	virtual void updatePositionImpl(Vector3% translation, Quaternion% rotation) override;

	virtual void updateTranslationImpl(Vector3% translation) override;

	virtual void updateRotationImpl(Quaternion% rotation) override;

	virtual void updateScaleImpl(Vector3% scale) override;

	virtual void setEnabled(bool enabled) override;

public:
	TypedConstraintElement(String^ name, Engine::ObjectManagement::Subscription subscription, BulletScene^ scene, RigidBody^ rbA, RigidBody^ rbB);

	virtual ~TypedConstraintElement();

	property RigidBody^ RigidBodyA
	{
		RigidBody^ get()
		{
			return rbA;
		}
	}

	property RigidBody^ RigidBodyB
	{
		RigidBody^ get()
		{
			return rbB;
		}
	}
};

}