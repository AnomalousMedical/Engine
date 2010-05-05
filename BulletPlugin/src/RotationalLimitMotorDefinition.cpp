#include "StdAfx.h"
#include "..\include\RotationalLimitMotorDefinition.h"

namespace BulletPlugin
{

RotationalLimitMotorDefinition::RotationalLimitMotorDefinition(void)
:motor(new btRotationalLimitMotor())
{
	LoLimit = 0;
	HiLimit = 0;
}

RotationalLimitMotorDefinition::RotationalLimitMotorDefinition(LoadInfo^ info)
:motor(new btRotationalLimitMotor())
{
	LoLimit = info->GetFloat("LoLimit");
    HiLimit = info->GetFloat("HiLimit");
    TargetVelocity = info->GetFloat("TargetVelocity");
    MaxMotorForce = info->GetFloat("MaxMotorForce");
    MaxLimitForce = info->GetFloat("MaxLimitForce");
    Damping = info->GetFloat("Damping");
    LimitSoftness = info->GetFloat("LimitSoftness");
    NormalCFM = info->GetFloat("NormalCFM", NormalCFM);
	StopERP = info->GetFloat("StopERP", StopERP);
	StopCFM = info->GetFloat("StopCFM", StopCFM);
    Bounce = info->GetFloat("Bounce");
    EnableMotor = info->GetBoolean("EnableMotor");
    CurrentLimitError = info->GetFloat("CurrentLimitError");
    CurrentLimit = info->GetInt32("CurrentLimit");
    AccumulatedImpulse = info->GetFloat("AccumulatedImpulse");
}

void RotationalLimitMotorDefinition::getInfo(SaveInfo^ info)
{
	info->AddValue("LoLimit", LoLimit);
    info->AddValue("HiLimit", HiLimit);
    info->AddValue("TargetVelocity", TargetVelocity);
    info->AddValue("MaxMotorForce", MaxMotorForce);
    info->AddValue("MaxLimitForce", MaxLimitForce);
    info->AddValue("Damping", Damping);
    info->AddValue("LimitSoftness", LimitSoftness);
    info->AddValue("NormalCFM", NormalCFM);
	info->AddValue("StopERP", StopERP);
	info->AddValue("StopCFM", StopCFM);
    info->AddValue("Bounce", Bounce);
    info->AddValue("EnableMotor", EnableMotor);
    info->AddValue("CurrentLimitError", CurrentLimitError);
    info->AddValue("CurrentLimit", CurrentLimit);
    info->AddValue("AccumulatedImpulse", AccumulatedImpulse);
}

}