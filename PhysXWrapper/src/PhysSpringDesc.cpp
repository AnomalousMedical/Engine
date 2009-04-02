#include "StdAfx.h"
#include "..\include\PhysSpringDesc.h"
#include "NxPhysics.h"

namespace Engine
{

namespace Physics
{

PhysSpringDesc::PhysSpringDesc(NxSpringDesc* springDesc)
:springDesc(springDesc)
{

}

PhysSpringDesc::PhysSpringDesc()
:autoSpringDesc(new NxSpringDesc)
{
	springDesc = autoSpringDesc.Get();
}

float PhysSpringDesc::Spring::get() 
{
	return springDesc->spring;
}

void PhysSpringDesc::Spring::set(float value) 
{
	springDesc->spring = value;
}

float PhysSpringDesc::Damper::get() 
{
	return springDesc->damper;
}

void PhysSpringDesc::Damper::set(float value) 
{
	springDesc->damper = value;
}

float PhysSpringDesc::TargetValue::get() 
{
	return springDesc->targetValue;
}

void PhysSpringDesc::TargetValue::set(float value) 
{
	springDesc->targetValue = value;
}

}

}