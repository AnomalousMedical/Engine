#include "StdAfx.h"
#include "..\include\Generic6DofConstraintElement.h"
#include "Generic6DofConstraintDefinition.h"
#include "RigidBody.h"

namespace BulletPlugin
{

#pragma unmanaged

btGeneric6DofConstraint* createConstraint(btRigidBody* rbA, btRigidBody* rbB)
{
	btTransform tfA;
	btTransform tfB;
	tfA.setIdentity();
	tfB.setIdentity();
	tfB.getOrigin().setX(10.0f);
	return new btGeneric6DofConstraint(*rbA, *rbB, tfA, tfB, true);
}

#pragma managed

Generic6DofConstraintElement::Generic6DofConstraintElement(Generic6DofConstraintDefinition^ definition, RigidBody^ rbA, RigidBody^ rbB, BulletScene^ scene)
:TypedConstraintElement(definition->Name, definition->Subscription, scene, rbA, rbB)
{
	btGeneric6DofConstraint* dof = createConstraint(rbA->Body, rbB->Body);
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