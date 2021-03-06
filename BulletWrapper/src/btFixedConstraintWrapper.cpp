#include "Stdafx.h"
#include "Generic6DofConstriantMotors.h"

extern "C" _AnomalousExport btFixedConstraint* btFixedConstraint_Create(btRigidBody* rbA, btRigidBody* rbB, Quaternion jointRot, Vector3 jointPos)
{
	btTransform jointTf;
	jointTf.setIdentity();
	jointTf.setOrigin(jointPos.toBullet());
	jointTf.getBasis().setRotation(jointRot.toBullet());
	btFixedConstraint* dof = new btFixedConstraint(*rbA, *rbB, rbA->getCenterOfMassTransform().inverse() * jointTf, rbB->getCenterOfMassTransform().inverse() * jointTf);
	return dof;
}