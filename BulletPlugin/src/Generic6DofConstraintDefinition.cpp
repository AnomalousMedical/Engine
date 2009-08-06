#include "StdAfx.h"
#include "..\include\Generic6DofConstraintDefinition.h"
#include "Generic6DofConstraintElement.h"
#include "RotationalLimitMotorDefinition.h"
#include "TranslationalLimitMotorDefinition.h"

namespace BulletPlugin
{

Generic6DofConstraintDefinition::Generic6DofConstraintDefinition(String^ name)
:TypedConstraintDefinition(name),
translationMotor(gcnew TranslationalLimitMotorDefinition()),
xRotMotor(gcnew RotationalLimitMotorDefinition()),
yRotMotor(gcnew RotationalLimitMotorDefinition()),
zRotMotor(gcnew RotationalLimitMotorDefinition())
{

}

TypedConstraintElement^ Generic6DofConstraintDefinition::createConstraint(RigidBody^ rbA, RigidBody^ rbB, SimObjectBase^ instance, BulletScene^ scene)
{
	if(rbA != nullptr && rbB != nullptr)
	{
		return gcnew Generic6DofConstraintElement(this, instance, rbA, rbB, scene);
	}
	return nullptr;
}

Generic6DofConstraintDefinition::Generic6DofConstraintDefinition(LoadInfo^ info)
:TypedConstraintDefinition(info)
{
	translationMotor = info->GetValue<TranslationalLimitMotorDefinition^>("TranslationMotor");
	xRotMotor = info->GetValue<RotationalLimitMotorDefinition^>("XRotMotor");
	yRotMotor = info->GetValue<RotationalLimitMotorDefinition^>("YRotMotor");
	zRotMotor = info->GetValue<RotationalLimitMotorDefinition^>("ZRotMotor");
}

void Generic6DofConstraintDefinition::getInfo(SaveInfo^ info)
{
	TypedConstraintDefinition::getInfo(info);
	info->AddValue("TranslationMotor", translationMotor);
	info->AddValue("XRotMotor", xRotMotor);
	info->AddValue("YRotMotor", yRotMotor);
	info->AddValue("ZRotMotor", zRotMotor);
}

}