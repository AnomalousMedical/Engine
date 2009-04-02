#include "StdAfx.h"
#include "..\include\PhysJointLimitSoftDesc.h"

namespace Engine
{

namespace Physics
{

PhysJointLimitSoftDesc::PhysJointLimitSoftDesc(NxJointLimitSoftDesc* limitDesc)
:limitDesc(limitDesc)
{

}

void PhysJointLimitSoftDesc::setToDefault()
{
	limitDesc->setToDefault();
}

bool PhysJointLimitSoftDesc::isValid()
{
	return limitDesc->isValid();
}

float PhysJointLimitSoftDesc::Value::get() 
{
	return limitDesc->value;
}

void PhysJointLimitSoftDesc::Value::set(float value) 
{
	limitDesc->value = value;
}

float PhysJointLimitSoftDesc::Restitution::get() 
{
	return limitDesc->restitution;
}

void PhysJointLimitSoftDesc::Restitution::set(float value) 
{
	limitDesc->restitution = value;
}

float PhysJointLimitSoftDesc::Spring::get() 
{
	return limitDesc->spring;
}

void PhysJointLimitSoftDesc::Spring::set(float value) 
{
	limitDesc->spring = value;
}

float PhysJointLimitSoftDesc::Damping::get() 
{
	return limitDesc->damping;
}

void PhysJointLimitSoftDesc::Damping::set(float value) 
{
	limitDesc->damping = value;
}

}

}