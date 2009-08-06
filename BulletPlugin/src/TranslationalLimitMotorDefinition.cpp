#include "StdAfx.h"
#include "..\include\TranslationalLimitMotorDefinition.h"

namespace BulletPlugin
{

TranslationalLimitMotorDefinition::TranslationalLimitMotorDefinition(void)
:motor(new btTranslationalLimitMotor())
{
}

TranslationalLimitMotorDefinition::TranslationalLimitMotorDefinition(LoadInfo^ info)
:motor(new btTranslationalLimitMotor())
{
	LowerLimit = info->GetVector3("LowerLimit");
	UpperLimit = info->GetVector3("UpperLimit");
	AccumulatedImpulse = info->GetVector3("AccumulatedImpulse");
	LimitSoftness = info->GetFloat("LimitSoftness");
	Damping = info->GetFloat("Damping");
	Restitution = info->GetFloat("Restitution");
	EnableMotorX = info->GetBoolean("EnableMotorX");
	EnableMotorY = info->GetBoolean("EnableMotorY");
	EnableMotorZ = info->GetBoolean("EnableMotorZ");
	TargetVelocity = info->GetVector3("TargetVelocity");
	MaxMotorForce = info->GetVector3("MaxMotorForce");
	CurrentLimitError = info->GetVector3("CurrentLimitError");
	CurrentLimitX = info->GetInt32("CurrentLimitX");
	CurrentLimitY = info->GetInt32("CurrentLimitY");
	CurrentLimitZ = info->GetInt32("CurrentLimitZ");
}

void TranslationalLimitMotorDefinition::getInfo(SaveInfo^ info)
{
	info->AddValue("LowerLimit", LowerLimit);
	info->AddValue("UpperLimit", UpperLimit);
	info->AddValue("AccumulatedImpulse", AccumulatedImpulse);
	info->AddValue("LimitSoftness", LimitSoftness);
	info->AddValue("Damping", Damping);
	info->AddValue("Restitution", Restitution);
	info->AddValue("EnableMotorX", EnableMotorX);
	info->AddValue("EnableMotorY", EnableMotorY);
	info->AddValue("EnableMotorZ", EnableMotorZ);
	info->AddValue("TargetVelocity", TargetVelocity);
	info->AddValue("MaxMotorForce", MaxMotorForce);
	info->AddValue("CurrentLimitError", CurrentLimitError);
	info->AddValue("CurrentLimitX", CurrentLimitX);
	info->AddValue("CurrentLimitY", CurrentLimitY);
	info->AddValue("CurrentLimitZ", CurrentLimitZ);
}

}