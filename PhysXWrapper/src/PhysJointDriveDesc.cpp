#include "StdAfx.h"
#include "..\include\PhysJointDriveDesc.h"

namespace Physics
{

PhysJointDriveDesc::PhysJointDriveDesc(NxJointDriveDesc* driveDesc)
:driveDesc(driveDesc)
{
}

D6JointDriveType PhysJointDriveDesc::DriveType::get() 
{
	return (D6JointDriveType)driveDesc->driveType.bitField;
}

void PhysJointDriveDesc::DriveType::set(D6JointDriveType value) 
{
	driveDesc->driveType.bitField = (NxU32)value;
}

float PhysJointDriveDesc::Spring::get() 
{
	return driveDesc->spring;
}

void PhysJointDriveDesc::Spring::set(float value) 
{
	driveDesc->spring = value;
}

float PhysJointDriveDesc::Damping::get() 
{
	return driveDesc->damping;
}

void PhysJointDriveDesc::Damping::set(float value) 
{
	driveDesc->damping = value;
}

float PhysJointDriveDesc::ForceLimit::get() 
{
	return driveDesc->forceLimit;
}

void PhysJointDriveDesc::ForceLimit::set(float value) 
{
	driveDesc->forceLimit = value;
}

}