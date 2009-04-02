#include "StdAfx.h"
#include "..\include\PhysJointLimitDesc.h"

namespace Engine
{

namespace Physics
{

PhysJointLimitDesc::PhysJointLimitDesc(NxJointLimitDesc* limitDesc)
:limitDesc(limitDesc)
{

}

void PhysJointLimitDesc::setToDefault()
{
	limitDesc->setToDefault();
}

bool PhysJointLimitDesc::isValid()
{
	return limitDesc->isValid();
}

float PhysJointLimitDesc::Value::get() 
{
	return limitDesc->value;
}

void PhysJointLimitDesc::Value::set(float value) 
{
	limitDesc->value = value;
}

float PhysJointLimitDesc::Restitution::get() 
{
	return limitDesc->restitution;
}

void PhysJointLimitDesc::Restitution::set(float value) 
{
	limitDesc->restitution = value;
}

float PhysJointLimitDesc::Hardness::get() 
{
	return limitDesc->hardness;
}

void PhysJointLimitDesc::Hardness::set(float value) 
{
	limitDesc->hardness = value;
}

}

}