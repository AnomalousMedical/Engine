#include "StdAfx.h"
#include "..\include\PhysD6JointDesc.h"
#include "NxPhysics.h"
#include "MathUtil.h"

namespace PhysXWrapper
{

PhysD6JointDesc::PhysD6JointDesc()
:joint(new NxD6JointDesc()), 
PhysJointDesc(joint.Get())
{
	linearLimit = gcnew PhysJointLimitSoftDesc(&joint->linearLimit);
	swing1Limit = gcnew PhysJointLimitSoftDesc(&joint->swing1Limit);
	swing2Limit = gcnew PhysJointLimitSoftDesc(&joint->swing2Limit);
	twistLimit = gcnew PhysJointLimitSoftPairDesc(&joint->twistLimit);
	xDrive = gcnew PhysJointDriveDesc(&joint->xDrive);
	yDrive = gcnew PhysJointDriveDesc(&joint->yDrive);
	zDrive = gcnew PhysJointDriveDesc(&joint->zDrive);
	swingDrive = gcnew PhysJointDriveDesc(&joint->swingDrive);
	twistDrive = gcnew PhysJointDriveDesc(&joint->twistDrive);
	slerpDrive = gcnew PhysJointDriveDesc(&joint->slerpDrive);
}

D6JointMotion PhysD6JointDesc::XMotion::get() 
{
	return (D6JointMotion)joint->xMotion;
}

void PhysD6JointDesc::XMotion::set(D6JointMotion value) 
{
	joint->xMotion = (NxD6JointMotion)value;
}

D6JointMotion PhysD6JointDesc::YMotion::get() 
{
	return (D6JointMotion)joint->yMotion;
}

void PhysD6JointDesc::YMotion::set(D6JointMotion value) 
{
	joint->yMotion = (NxD6JointMotion)value;
}

D6JointMotion PhysD6JointDesc::ZMotion::get() 
{
	return (D6JointMotion)joint->zMotion;
}

void PhysD6JointDesc::ZMotion::set(D6JointMotion value) 
{
	joint->zMotion = (NxD6JointMotion)value;
}

D6JointMotion PhysD6JointDesc::Swing1Motion::get() 
{
	return (D6JointMotion)joint->swing1Motion;
}

void PhysD6JointDesc::Swing1Motion::set(D6JointMotion value) 
{
	joint->swing1Motion = (NxD6JointMotion)value;
}

D6JointMotion PhysD6JointDesc::Swing2Motion::get() 
{
	return (D6JointMotion)joint->swing2Motion;
}

void PhysD6JointDesc::Swing2Motion::set(D6JointMotion value) 
{
	joint->swing2Motion = (NxD6JointMotion)value;
}

D6JointMotion PhysD6JointDesc::TwistMotion::get() 
{
	return (D6JointMotion)joint->twistMotion;
}

void PhysD6JointDesc::TwistMotion::set(D6JointMotion value) 
{
	joint->twistMotion = (NxD6JointMotion)value;
}

PhysJointLimitSoftDesc^ PhysD6JointDesc::LinearLimit::get() 
{
	return linearLimit;
}

PhysJointLimitSoftDesc^ PhysD6JointDesc::Swing1Limit::get() 
{
	return swing1Limit;
}

PhysJointLimitSoftDesc^ PhysD6JointDesc::Swing2Limit::get() 
{
	return swing2Limit;
}

PhysJointLimitSoftPairDesc^ PhysD6JointDesc::TwistLimit::get() 
{
	return twistLimit;
}

PhysJointDriveDesc^ PhysD6JointDesc::XDrive::get() 
{
	return xDrive;
}

PhysJointDriveDesc^ PhysD6JointDesc::YDrive::get() 
{
	return yDrive;
}

PhysJointDriveDesc^ PhysD6JointDesc::ZDrive::get() 
{
	return zDrive;
}

PhysJointDriveDesc^ PhysD6JointDesc::SwingDrive::get() 
{
	return swingDrive;
}

PhysJointDriveDesc^ PhysD6JointDesc::TwistDrive::get() 
{
	return twistDrive;
}

PhysJointDriveDesc^ PhysD6JointDesc::SlerpDrive::get() 
{
	return slerpDrive;
}

Engine::Vector3 PhysD6JointDesc::DrivePosition::get() 
{
	NxVec3 v = joint->drivePosition;
	return Engine::Vector3(v.x, v.y, v.z);
}

void PhysD6JointDesc::DrivePosition::set(Engine::Vector3 value) 
{
	joint->drivePosition = NxVec3(value.x, value.y, value.z);
}

Engine::Quaternion PhysD6JointDesc::DriveOrientation::get() 
{
	NxQuat q = joint->driveOrientation;
	return Engine::Quaternion(q.x, q.y, q.z, q.w);
}

void PhysD6JointDesc::DriveOrientation::set(Engine::Quaternion value) 
{
	joint->driveOrientation = MathUtil::convertNxQuaternion(value);
}

Engine::Vector3 PhysD6JointDesc::DriveLinearVelocity::get() 
{
	NxVec3 v = joint->driveLinearVelocity;
	return Engine::Vector3(v.x, v.y, v.z);
}

void PhysD6JointDesc::DriveLinearVelocity::set(Engine::Vector3 value) 
{
	joint->driveLinearVelocity = NxVec3(value.x, value.y, value.z);
}

Engine::Vector3 PhysD6JointDesc::DriveAngularVelocity::get() 
{
	NxVec3 v = joint->driveAngularVelocity;
	return Engine::Vector3(v.x, v.y, v.z);
}

void PhysD6JointDesc::DriveAngularVelocity::set(Engine::Vector3 value) 
{
	joint->driveAngularVelocity = NxVec3(value.x, value.y, value.z);
}

JointProjectionMode PhysD6JointDesc::ProjectionMode::get() 
{
	return (JointProjectionMode)joint->projectionMode;
}

void PhysD6JointDesc::ProjectionMode::set(JointProjectionMode value) 
{
	joint->projectionMode = (NxJointProjectionMode)value;
}

float PhysD6JointDesc::ProjectionDistance::get() 
{
	return joint->projectionDistance;
}

void PhysD6JointDesc::ProjectionDistance::set(float value) 
{
	joint->projectionDistance = value;
}

float PhysD6JointDesc::ProjectionAngle::get() 
{
	return joint->projectionAngle;
}

void PhysD6JointDesc::ProjectionAngle::set(float value) 
{
	joint->projectionAngle = value;
}

float PhysD6JointDesc::GearRatio::get() 
{
	return joint->gearRatio;
}

void PhysD6JointDesc::GearRatio::set(float value) 
{
	joint->gearRatio = value;
}

D6JointFlag PhysD6JointDesc::Flags::get() 
{
	return (D6JointFlag)joint->flags;
}

void PhysD6JointDesc::Flags::set(D6JointFlag value) 
{
	joint->flags = (NxU32)value;
}

}