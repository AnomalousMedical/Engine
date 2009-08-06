#include "StdAfx.h"
#include "..\include\Generic6DofConstraintElement.h"
#include "Generic6DofConstraintDefinition.h"
#include "RigidBody.h"
#include "RotationalLimitMotorDefinition.h"
#include "TranslationalLimitMotorDefinition.h"

namespace BulletPlugin
{

#pragma unmanaged

btGeneric6DofConstraint* createConstraint(btRigidBody* rbA, btRigidBody* rbB, float* jointPos, float* jointRot, btTranslationalLimitMotor* transMotor)
{
	btTransform jointTf;
	jointTf.setIdentity();
	jointTf.setOrigin(btVector3(jointPos[0], jointPos[1], jointPos[2]));
	jointTf.getBasis().setRotation(btQuaternion(jointRot[0], jointRot[1], jointRot[2], jointRot[3]));
	btGeneric6DofConstraint* dof = new btGeneric6DofConstraint(*rbA, *rbB, rbA->getCenterOfMassTransform().inverse() * jointTf, rbB->getCenterOfMassTransform().inverse() * jointTf, true);
	*dof->getTranslationalLimitMotor() = *transMotor;
	return dof;
}

void copyMotor(btTranslationalLimitMotor* src, btTranslationalLimitMotor* dest)
{
	*dest = *src;
}

#pragma managed

Generic6DofConstraintElement::Generic6DofConstraintElement(Generic6DofConstraintDefinition^ definition, SimObjectBase^ instance, RigidBody^ rbA, RigidBody^ rbB, BulletScene^ scene)
:TypedConstraintElement(definition->Name, definition->Subscription, scene, rbA, rbB)
{
	dof = createConstraint(rbA->Body, rbB->Body, &instance->Translation.x, &instance->Rotation.x, definition->TranslationMotor->Motor);
	this->setConstraint(dof);
	*dof->getRotationalLimitMotor(0) = *definition->XRotMotor->Motor;
	*dof->getRotationalLimitMotor(1) = *definition->YRotMotor->Motor;
	*dof->getRotationalLimitMotor(2) = *definition->ZRotMotor->Motor;
}

Generic6DofConstraintElement::~Generic6DofConstraintElement(void)
{
	if(dof != 0)
	{
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
	*definition->XRotMotor->Motor = *dof->getRotationalLimitMotor(0);
	*definition->YRotMotor->Motor = *dof->getRotationalLimitMotor(1);
	*definition->ZRotMotor->Motor = *dof->getRotationalLimitMotor(2);
	copyMotor(dof->getTranslationalLimitMotor(), definition->TranslationMotor->Motor);
	return definition;
}

}