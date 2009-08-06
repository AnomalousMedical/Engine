#include "StdAfx.h"
#include "..\include\Generic6DofConstraintElement.h"
#include "Generic6DofConstraintDefinition.h"
#include "RigidBody.h"

namespace BulletPlugin
{

#pragma unmanaged

btGeneric6DofConstraint* createConstraint(btRigidBody* rbA, btRigidBody* rbB, float* jointPos, float* jointRot)
{
	btTransform jointTf;
	jointTf.setIdentity();
	jointTf.setOrigin(btVector3(jointPos[0], jointPos[1], jointPos[2]));
	jointTf.getBasis().setRotation(btQuaternion(jointRot[0], jointRot[1], jointRot[2], jointRot[3]));
	return new btGeneric6DofConstraint(*rbA, *rbB, rbA->getCenterOfMassTransform().inverse() * jointTf, rbB->getCenterOfMassTransform().inverse() * jointTf, true);
}

#pragma managed

Generic6DofConstraintElement::Generic6DofConstraintElement(Generic6DofConstraintDefinition^ definition, SimObjectBase^ instance, RigidBody^ rbA, RigidBody^ rbB, BulletScene^ scene)
:TypedConstraintElement(definition->Name, definition->Subscription, scene, rbA, rbB)
{
	btGeneric6DofConstraint* dof = createConstraint(rbA->Body, rbB->Body, &instance->Translation.x, &instance->Rotation.x);
	this->setConstraint(dof);
	/*dof->setLimit(0, 0.0f, 0.0f);
	dof->setLimit(1, 0.0f, 0.0f);
	dof->setLimit(2, 0.0f, 0.0f);
	dof->setLimit(3, 0.0f, 0.0f);
	dof->setLimit(4, 0.0f, 0.0f);
	dof->setLimit(5, 0.0f, 0.0f);*/
}

Generic6DofConstraintElement::~Generic6DofConstraintElement(void)
{
	if(dof != 0)
	{
		delete dof;
		dof = 0;
	}
}

SimElementDefinition^ Generic6DofConstraintElement::saveToDefinition()
{
	Generic6DofConstraintDefinition^ definition = gcnew Generic6DofConstraintDefinition(Name);
	definition->RigidBodyAElement = RigidBodyA->Name;
	definition->RigidBodyASimObject = RigidBodyA->Owner->Name;
	definition->RigidBodyBElement = RigidBodyB->Name;
	definition->RigidBodyBSimObject = RigidBodyB->Owner->Name;
	return definition;
}

}