#include "StdAfx.h"
#include "..\include\TypedConstraintElement.h"
#include "BulletScene.h"

namespace BulletPlugin
{

TypedConstraintElement::TypedConstraintElement(String^ name, Engine::ObjectManagement::Subscription subscription, BulletScene^ scene, RigidBody^ rbA, RigidBody^ rbB)
:SimElement(name, subscription),
constraint(0),
scene(scene),
rbA(rbA),
rbB(rbB)
{
}

TypedConstraintElement::~TypedConstraintElement()
{
	if(constraint != 0)
	{
		if(Owner->Enabled)
		{
			scene->DynamicsWorld->removeConstraint(constraint);
		}
		constraint = 0;
	}
}

void TypedConstraintElement::updatePositionImpl(Vector3% translation, Quaternion% rotation)
{
}

void TypedConstraintElement::updateTranslationImpl(Vector3% translation)
{
}

void TypedConstraintElement::updateRotationImpl(Quaternion% rotation)
{
}

void TypedConstraintElement::updateScaleImpl(Vector3% scale)
{
}

void TypedConstraintElement::setEnabled(bool enabled) 
{
	if(enabled)
	{
		scene->DynamicsWorld->addConstraint(constraint);
	}
	else
	{
		scene->DynamicsWorld->removeConstraint(constraint);
	}
}

}