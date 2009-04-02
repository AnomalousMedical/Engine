#include "StdAfx.h"
#include "..\include\PhysMaterialDesc.h"

namespace Engine
{

namespace Physics
{

PhysMaterialDesc::PhysMaterialDesc(System::String^ name)
:desc(new NxMaterialDesc()), name(name)
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

System::String^ PhysMaterialDesc::Name::get() 
{
	return name;
}

}

}