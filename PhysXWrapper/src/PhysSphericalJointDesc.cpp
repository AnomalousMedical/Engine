#include "StdAfx.h"
#include "..\include\PhysSphericalJointDesc.h"
#include "NxPhysics.h"
#include "PhysJointLimitPairDesc.h"
#include "PhysJointLimitDesc.h"
#include "PhysSpringDesc.h"

namespace Physics
{

PhysSphericalJointDesc::PhysSphericalJointDesc()
:joint(new NxSphericalJointDesc()), 
PhysJointDesc(joint.Get())
{
	twistLimit = gcnew PhysJointLimitPairDesc(&joint->twistLimit);
	swingLimit = gcnew PhysJointLimitDesc(&joint->swingLimit);
	twistSpring = gcnew PhysSpringDesc(&joint->twistSpring);
	swingSpring = gcnew PhysSpringDesc(&joint->swingSpring);
	jointSpring = gcnew PhysSpringDesc(&joint->jointSpring);
}

EngineMath::Vector3 PhysSphericalJointDesc::SwingAxis::get() 
{
	NxVec3 v = joint->swingAxis;
	return EngineMath::Vector3(v.x, v.y, v.z);
}

void PhysSphericalJointDesc::SwingAxis::set(EngineMath::Vector3 value) 
{
	joint->swingAxis = NxVec3(value.x, value.y, value.z);
}

float PhysSphericalJointDesc::ProjectionDistance::get() 
{
	return joint->projectionDistance;
}

void PhysSphericalJointDesc::ProjectionDistance::set(float value) 
{
	joint->projectionDistance = value;
}

PhysJointLimitPairDesc^ PhysSphericalJointDesc::TwistLimit::get() 
{
	return twistLimit;
}

PhysJointLimitDesc^ PhysSphericalJointDesc::SwingLimit::get() 
{
	return swingLimit;
}

PhysSpringDesc^ PhysSphericalJointDesc::TwistSpring::get() 
{
	return twistSpring;
}

PhysSpringDesc^ PhysSphericalJointDesc::SwingSpring::get() 
{
	return swingSpring;
}

PhysSpringDesc^ PhysSphericalJointDesc::JointSpring::get() 
{
	return jointSpring;
}

SphericalJointFlag PhysSphericalJointDesc::Flags::get() 
{
	return (SphericalJointFlag)joint->flags;
}

void PhysSphericalJointDesc::Flags::set(SphericalJointFlag value) 
{
	joint->flags = (NxU32)value;
}

JointProjectionMode PhysSphericalJointDesc::ProjectionMode::get() 
{
	return (JointProjectionMode)joint->projectionMode;
}

void PhysSphericalJointDesc::ProjectionMode::set(JointProjectionMode value) 
{
	joint->projectionMode = (NxJointProjectionMode)value;
}

}