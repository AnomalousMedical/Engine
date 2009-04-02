#include "StdAfx.h"
#include "..\include\PhysDistanceJointDesc.h"
#include "NxPhysics.h"
#include "PhysSpringDesc.h"

namespace Engine
{

namespace Physics
{

PhysDistanceJointDesc::PhysDistanceJointDesc(Engine::Identifier^ name)
:joint(new NxDistanceJointDesc()), 
PhysJointDesc(joint.Get(), name)
{
	spring = gcnew PhysSpringDesc(&joint->spring);
}

float PhysDistanceJointDesc::MaxDistance::get() 
{
	return joint->maxDistance;
}

void PhysDistanceJointDesc::MaxDistance::set(float value) 
{
	joint->maxDistance = value;
}

float PhysDistanceJointDesc::MinDistance::get() 
{
	return joint->minDistance;
}

void PhysDistanceJointDesc::MinDistance::set(float value) 
{
	joint->minDistance = value;
}

PhysSpringDesc^ PhysDistanceJointDesc::Spring::get() 
{
	return spring;
}

DistanceJointFlag PhysDistanceJointDesc::Flags::get() 
{
	return (DistanceJointFlag)joint->flags;
}

void PhysDistanceJointDesc::Flags::set(DistanceJointFlag value) 
{
	joint->flags = (NxU32)value;
}

}

}