#include "Stdafx.h"
#include "Generic6DofConstriantMotors.h"

extern "C" _AnomalousExport btGeneric6DofConstraint* btGeneric6DofConstraint_Create(btRigidBody* rbA, btRigidBody* rbB, Quaternion jointRot, Vector3 jointPos, TranslationalLimitMotorDefinition* transMotor, RotationalLimitMotorDefinition* xRotMotor, RotationalLimitMotorDefinition* yRotMotor, RotationalLimitMotorDefinition* zRotMotor)
{
	btTransform jointTf;
	jointTf.setIdentity();
	jointTf.setOrigin(jointPos.toBullet());
	jointTf.getBasis().setRotation(jointRot.toBullet());
	btGeneric6DofConstraint* dof = new btGeneric6DofConstraint(*rbA, *rbB, rbA->getCenterOfMassTransform().inverse() * jointTf, rbB->getCenterOfMassTransform().inverse() * jointTf, true);
	transMotor->toBullet(dof->getTranslationalLimitMotor());
	xRotMotor->toBullet(dof->getRotationalLimitMotor(0));
	yRotMotor->toBullet(dof->getRotationalLimitMotor(1));
	zRotMotor->toBullet(dof->getRotationalLimitMotor(2));
	return dof;
}

extern "C" _AnomalousExport void btGeneric6DofConstraint_setFrameOffsetOriginA(btGeneric6DofConstraint* instance, Vector3* origin)
{
	instance->getFrameOffsetA().setOrigin(origin->toBullet());
}

extern "C" _AnomalousExport void btGeneric6DofConstraint_setFrameOffsetBasisA(btGeneric6DofConstraint* instance, Quaternion* basis)
{
	instance->getFrameOffsetA().getBasis().setRotation(basis->toBullet());
}

extern "C" _AnomalousExport void btGeneric6DofConstraint_setFrameOffsetOriginBasisA(btGeneric6DofConstraint* instance, Vector3* origin, Quaternion* basis)
{
	btTransform& tf = instance->getFrameOffsetA();
	tf.setOrigin(origin->toBullet());
	tf.getBasis().setRotation(basis->toBullet());
}

extern "C" _AnomalousExport void btGeneric6DofConstraint_setFrameOffsetOriginB(btGeneric6DofConstraint* instance, Vector3* origin)
{
	instance->getFrameOffsetB().setOrigin(origin->toBullet());
}

extern "C" _AnomalousExport void btGeneric6DofConstraint_setFrameOffsetBasisB(btGeneric6DofConstraint* instance, Quaternion* basis)
{
	instance->getFrameOffsetB().getBasis().setRotation(basis->toBullet());
}

extern "C" _AnomalousExport void btGeneric6DofConstraint_setFrameOffsetOriginBasisB(btGeneric6DofConstraint* instance, Vector3* origin, Quaternion* basis)
{
	btTransform& tf = instance->getFrameOffsetB();
	tf.setOrigin(origin->toBullet());
	tf.getBasis().setRotation(basis->toBullet());
}

extern "C" _AnomalousExport Vector3 btGeneric6DofConstraint_getFrameOffsetOriginA(btGeneric6DofConstraint* instance)
{
	return instance->getFrameOffsetA().getOrigin();
}

extern "C" _AnomalousExport Quaternion btGeneric6DofConstraint_getFrameOffsetBasisA(btGeneric6DofConstraint* instance)
{
	return instance->getFrameOffsetA().getRotation();
}

extern "C" _AnomalousExport Vector3 btGeneric6DofConstraint_getFrameOffsetOriginB(btGeneric6DofConstraint* instance)
{
	return instance->getFrameOffsetB().getOrigin();
}

extern "C" _AnomalousExport Quaternion btGeneric6DofConstraint_getFrameOffsetBasisB(btGeneric6DofConstraint* instance)
{
	return instance->getFrameOffsetB().getRotation();
}

extern "C" _AnomalousExport void btGeneric6DofConstraint_setLimit(btGeneric6DofConstraint* instance, int axis, float lo, float hi)
{
	instance->setLimit(axis, lo, hi);
}

extern "C" _AnomalousExport void btGeneric6DofConstraint_setLinearLowerLimit(btGeneric6DofConstraint* instance, Vector3* linearLower)
{
	instance->setLinearLowerLimit(linearLower->toBullet());
}

extern "C" _AnomalousExport void btGeneric6DofConstraint_setLinearUpperLimit(btGeneric6DofConstraint* instance, Vector3* linearUpper)
{
	instance->setLinearUpperLimit(linearUpper->toBullet());
}

extern "C" _AnomalousExport void btGeneric6DofConstraint_setAngularLowerLimit(btGeneric6DofConstraint* instance, Vector3* angularLower)
{
	instance->setAngularLowerLimit(angularLower->toBullet());
}

extern "C" _AnomalousExport void btGeneric6DofConstraint_setAngularUpperLimit(btGeneric6DofConstraint* instance, Vector3* angularUpper)
{
	instance->setAngularUpperLimit(angularUpper->toBullet());
}

extern "C" _AnomalousExport void btGeneric6DofConstraint_setParam(btGeneric6DofConstraint* instance, int num, float value, int axis)
{
	instance->setParam(num, value, axis);
}

extern "C" _AnomalousExport void btGeneric6DofConstraint_copyMotors(btGeneric6DofConstraint* instance, TranslationalLimitMotorDefinition* transMotor, RotationalLimitMotorDefinition* xRotMotor, RotationalLimitMotorDefinition* yRotMotor, RotationalLimitMotorDefinition* zRotMotor)
{
	transMotor->fromBullet(instance->getTranslationalLimitMotor());
	xRotMotor->fromBullet(instance->getRotationalLimitMotor(0));
	yRotMotor->fromBullet(instance->getRotationalLimitMotor(1));
	zRotMotor->fromBullet(instance->getRotationalLimitMotor(2));
}