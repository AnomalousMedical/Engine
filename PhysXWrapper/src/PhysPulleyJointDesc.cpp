#include "StdAfx.h"
#include "..\include\PhysPulleyJointDesc.h"
#include "NxPhysics.h"
#include "PhysMotorDesc.h"

namespace Engine
{

namespace Physics
{

PhysPulleyJointDesc::PhysPulleyJointDesc()
:joint(new NxPulleyJointDesc()), 
PhysJointDesc(joint.Get())
{
	motor = gcnew PhysMotorDesc(&joint->motor);
}

EngineMath::Vector3 PhysPulleyJointDesc::Pulley::get(int index)
{
	NxVec3 v = joint->pulley[index];
	return EngineMath::Vector3(v.x, v.y, v.z);
}

void PhysPulleyJointDesc::Pulley::set(int index, EngineMath::Vector3 value)
{
	joint->pulley[index] = NxVec3(value.x, value.y, value.z);
}

float PhysPulleyJointDesc::Distance::get() 
{
	return joint->distance;
}

void PhysPulleyJointDesc::Distance::set(float value) 
{
	joint->distance = value;
}

float PhysPulleyJointDesc::Stiffness::get() 
{
	return joint->stiffness;
}

void PhysPulleyJointDesc::Stiffness::set(float value) 
{
	joint->stiffness = value;
}

float PhysPulleyJointDesc::Ratio::get() 
{
	return joint->ratio;
}

void PhysPulleyJointDesc::Ratio::set(float value) 
{
	joint->ratio = value;
}

PulleyJointFlag PhysPulleyJointDesc::Flags::get() 
{
	return (PulleyJointFlag)joint->flags;
}

void PhysPulleyJointDesc::Flags::set(PulleyJointFlag value) 
{
	joint->flags = (NxU32)value;
}

PhysMotorDesc^ PhysPulleyJointDesc::Motor::get() 
{
	return motor;
}

}

}