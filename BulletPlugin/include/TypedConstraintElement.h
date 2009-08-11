#pragma once

#include "vcclr.h"

using namespace System;
using namespace Engine;
using namespace Engine::ObjectManagement;

namespace BulletPlugin
{

ref class BulletScene;
ref class RigidBody;
ref class TypedConstraintElement;

typedef gcroot<TypedConstraintElement^> ConstraintGCRoot;

[Engine::Attributes::NativeSubsystemType]
public ref class TypedConstraintElement abstract : public SimElement
{
private:
	btTypedConstraint* constraint;
	BulletScene^ scene;
	RigidBody^ rbA;
	RigidBody^ rbB;
	bool active; //true while both rigid bodies exist. If one is deleted setInactive will be called removing the joint and setting this to false.
	bool enabled;

protected:
	void setConstraint(btTypedConstraint* constraint)
	{
		this->constraint = constraint;
		constraint->setUserData(new ConstraintGCRoot(this));
	}

	virtual void updatePositionImpl(Vector3% translation, Quaternion% rotation) override;

	virtual void updateTranslationImpl(Vector3% translation) override;

	virtual void updateRotationImpl(Quaternion% rotation) override;

	virtual void updateScaleImpl(Vector3% scale) override;

	virtual void setEnabled(bool enabled) override;

internal:
	//Called by RigidBodies when they are deleted if they have this joint. This will 
	//remove it from the scene and set active to false. This does not destroy the joint, 
	//but it will no longer be usable.
	void setInactive();

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