#include "StdAfx.h"
#include "..\include\PhysMotorDesc.h"
#include "NxPhysics.h"

namespace PhysXWrapper
{

PhysMotorDesc::PhysMotorDesc(NxMotorDesc* motorDesc)
:motorDesc(motorDesc)
{

}

PhysMotorDesc::PhysMotorDesc()
:autoMotorDesc(new NxMotorDesc())
{
	motorDesc = autoMotorDesc.Get();
}

void PhysMotorDesc::setToDefault()
{
	motorDesc->setToDefault();
}

bool PhysMotorDesc::isValid()
{
	return motorDesc->isValid();
}

float PhysMotorDesc::VelTarget::get() 
{
	return motorDesc->velTarget;
}

void PhysMotorDesc::VelTarget::set(float value) 
{
	motorDesc->velTarget = value;
}

float PhysMotorDesc::MaxForce::get() 
{
	return motorDesc->maxForce;
}

void PhysMotorDesc::MaxForce::set(float value) 
{
	motorDesc->maxForce = value;
}

bool PhysMotorDesc::FreeSpin::get() 
{
	return motorDesc->freeSpin;
}

void PhysMotorDesc::FreeSpin::set(bool value) 
{
	motorDesc->freeSpin = value;
}

}