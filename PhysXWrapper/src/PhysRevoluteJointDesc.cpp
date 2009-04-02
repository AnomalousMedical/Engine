#include "StdAfx.h"
#include "..\include\PhysRevoluteJointDesc.h"
#include "NxPhysics.h"
#include "PhysJointLimitPairDesc.h"
#include "PhysMotorDesc.h"
#include "PhysSpringDesc.h"

namespace Physics
{

PhysRevoluteJointDesc::PhysRevoluteJointDesc()
:joint(new NxRevoluteJointDesc()),
PhysJointDesc(joint.Get())
{
	limit = gcnew PhysJointLimitPairDesc(&joint->limit);
	motor = gcnew PhysMotorDesc(&joint->motor);
	spring = gcnew PhysSpringDesc(&joint->spring);
}

PhysJointLimitPairDesc^ PhysRevoluteJointDesc::Limit::get() 
{
	return limit;
}

PhysMotorDesc^ PhysRevoluteJointDesc::Motor::get() 
{
	return motor;
}

PhysSpringDesc^ PhysRevoluteJointDesc::Spring::get() 
{
	return spring;
}

float PhysRevoluteJointDesc::ProjectionDistance::get() 
{
	return joint->projectionDistance;
}

void PhysRevoluteJointDesc::ProjectionDistance::set(float value) 
{
	joint->projectionDistance = value;
}

float PhysRevoluteJointDesc::ProjectionAngle::get() 
{
	return joint->projectionAngle;
}

void PhysRevoluteJointDesc::ProjectionAngle::set(float value) 
{
	joint->projectionAngle = value;
}

RevoluteJointFlag PhysRevoluteJointDesc::Flags::get() 
{
	return (RevoluteJointFlag)joint->flags;
}

void PhysRevoluteJointDesc::Flags::set(RevoluteJointFlag value) 
{
	joint->flags = (NxU32)value;
}

JointProjectionMode PhysRevoluteJointDesc::ProjectionMode::get() 
{
	return (JointProjectionMode)joint->projectionMode;
}

void PhysRevoluteJointDesc::ProjectionMode::set(JointProjectionMode value) 
{
	joint->projectionMode = (NxJointProjectionMode)value;
}

}