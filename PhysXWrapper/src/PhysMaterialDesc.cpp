#include "StdAfx.h"
#include "..\include\PhysMaterialDesc.h"

namespace PhysXWrapper
{

PhysMaterialDesc::PhysMaterialDesc()
:desc(new NxMaterialDesc())
{
}

PhysMaterialDesc::~PhysMaterialDesc(void)
{
	
}

float PhysMaterialDesc::Restitution::get() 
{
	return desc->restitution;
}

void PhysMaterialDesc::Restitution::set(float value) 
{
	desc->restitution = value;
}

float PhysMaterialDesc::StaticFriction::get() 
{
	return desc->staticFriction;
}

void PhysMaterialDesc::StaticFriction::set(float value) 
{
	desc->staticFriction = value;
}

float PhysMaterialDesc::DynamicFriction::get() 
{
	return desc->dynamicFriction;
}

void PhysMaterialDesc::DynamicFriction::set(float value) 
{
	desc->dynamicFriction = value;
}

}