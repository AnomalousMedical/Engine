#pragma once

#include "AutoPtr.h"
#include "PhysJointDesc.h"
#include "NxD6JointDesc.h"
#include "PhysJointDriveDesc.h"
#include "PhysJointLimitSoftDesc.h"
#include "PhysJointLimitSoftPairDesc.h"
#include "Enums.h"

namespace PhysXWrapper
{

/// <summary>
/// Wrapper for NxD6JointDesc.
/// Descriptor class for the D6Joint. 
/// In the D6Joint, the axes are assigned as follows: 
/// 
/// x-axis = joint axis 
/// y-axis = joint normal axis 
/// z-axis = x-axis cross y-axis 
/// These are defined relative to the parent body (0) of the joint.
/// Swing is defined as the rotation of the x-axis with respect to the y- and z-axis. 
/// 
/// Twist is defined as the rotation about the x-axis.
/// </summary>
public ref class PhysD6JointDesc : public PhysJointDesc
{
private:
	PhysJointLimitSoftDesc^ linearLimit;
	PhysJointLimitSoftDesc^ swing1Limit;
	PhysJointLimitSoftDesc^ swing2Limit;
	PhysJointLimitSoftPairDesc^ twistLimit;
	PhysJointDriveDesc^ xDrive;
	PhysJointDriveDesc^ yDrive;
	PhysJointDriveDesc^ zDrive;
	PhysJointDriveDesc^ swingDrive;
	PhysJointDriveDesc^ twistDrive;
	PhysJointDriveDesc^ slerpDrive;

internal:
	AutoPtr<NxD6JointDesc> joint;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysD6JointDesc();

	/// <summary>
	/// Define the linear degrees of freedom.
	/// </summary>
	property D6JointMotion XMotion 
	{
		D6JointMotion get();
		void set(D6JointMotion value);
	}

	/// <summary>
	/// Define the linear degrees of freedom.
	/// </summary>
	property D6JointMotion YMotion 
	{
		D6JointMotion get();
		void set(D6JointMotion value);
	}
	
	/// <summary>
	/// Define the linear degrees of freedom.
	/// </summary>
	property D6JointMotion ZMotion 
	{
		D6JointMotion get();
		void set(D6JointMotion value);
	}

	/// <summary>
	/// Define the angular degrees of freedom.
	/// </summary>
	property D6JointMotion Swing1Motion 
	{
		D6JointMotion get();
		void set(D6JointMotion value);
	}

	/// <summary>
	/// Define the angular degrees of freedom.
	/// </summary>
	property D6JointMotion Swing2Motion 
	{
		D6JointMotion get();
		void set(D6JointMotion value);
	}

	/// <summary>
	/// Define the angular degrees of freedom.
	/// </summary>
	property D6JointMotion TwistMotion 
	{
		D6JointMotion get();
		void set(D6JointMotion value);
	}

	/// <summary>
	/// If some linear DOF are limited, linearLimit defines the characteristics of 
	/// these limits.
	/// </summary>
	property PhysJointLimitSoftDesc^ LinearLimit 
	{
		PhysJointLimitSoftDesc^ get();
	}

	/// <summary>
	/// If swing1Motion is NX_D6JOINT_MOTION_LIMITED, swing1Limit defines the characteristics 
	/// of the limit.
	/// </summary>
	property PhysJointLimitSoftDesc^ Swing1Limit 
	{
		PhysJointLimitSoftDesc^ get();
	}

	/// <summary>
	/// If swing2Motion is NX_D6JOINT_MOTION_LIMITED, swing2Limit defines the characteristics 
	/// of the limit.
	/// </summary>
	property PhysJointLimitSoftDesc^ Swing2Limit 
	{
		PhysJointLimitSoftDesc^ get();
	}

	/// <summary>
	/// If twistMotion is NX_D6JOINT_MOTION_LIMITED, twistLimit defines the characteristics 
	/// of the limit.
	/// </summary>
	property PhysJointLimitSoftPairDesc^ TwistLimit 
	{
		PhysJointLimitSoftPairDesc^ get();
	}

	/// <summary>
	/// Drive the three linear DOF.
	/// </summary>
	property PhysJointDriveDesc^ XDrive 
	{
		PhysJointDriveDesc^ get();
	}

	/// <summary>
	/// Drive the three linear DOF.
	/// </summary>
	property PhysJointDriveDesc^ YDrive 
	{
		PhysJointDriveDesc^ get();
	}

	/// <summary>
	/// Drive the three linear DOF.
	/// </summary>
	property PhysJointDriveDesc^ ZDrive 
	{
		PhysJointDriveDesc^ get();
	}

	/// <summary>
	/// These drives are used if the flag NX_D6JOINT_SLERP_DRIVE is not set.
	/// </summary>
	property PhysJointDriveDesc^ SwingDrive 
	{
		PhysJointDriveDesc^ get();
	}

	/// <summary>
	/// These drives are used if the flag NX_D6JOINT_SLERP_DRIVE is not set.
	/// </summary>
	property PhysJointDriveDesc^ TwistDrive 
	{
		PhysJointDriveDesc^ get();
	}

	/// <summary>
	/// This drive is used if the flag NX_D6JOINT_SLERP_DRIVE is set.
	/// </summary>
	property PhysJointDriveDesc^ SlerpDrive 
	{
		PhysJointDriveDesc^ get();
	}

	/// <summary>
	/// If the type of xDrive (yDrive,zDrive) is NX_D6JOINT_DRIVE_POSITION, drivePosition 
	/// defines the goal position.
	/// </summary>
	property Engine::Vector3 DrivePosition 
	{
		Engine::Vector3 get();
		void set(Engine::Vector3 value);
	}

	/// <summary>
	/// If the type of swingDrive or twistDrive is NX_D6JOINT_DRIVE_POSITION, driveOrientation 
	/// defines the goal orientation.
	/// </summary>
	property Engine::Quaternion DriveOrientation 
	{
		Engine::Quaternion get();
		void set(Engine::Quaternion value);
	}

	/// <summary>
	/// If the type of xDrive (yDrive,zDrive) is NX_D6JOINT_DRIVE_VELOCITY, driveLinearVelocity 
	/// defines the goal linear velocity.
	/// </summary>
	property Engine::Vector3 DriveLinearVelocity 
	{
		Engine::Vector3 get();
		void set(Engine::Vector3 value);
	}

	/// <summary>
	/// If the type of swingDrive or twistDrive is NX_D6JOINT_DRIVE_VELOCITY, driveAngularVelocity defines the goal angular velocity. 
	/// driveAngularVelocity.x - goal angular velocity about the twist axis 
	/// driveAngularVelocity.y - goal angular velocity about the swing1 axis 
	/// driveAngularVelocity.z - goal angular velocity about the swing2 axis
	/// </summary>
	property Engine::Vector3 DriveAngularVelocity 
	{
		Engine::Vector3 get();
		void set(Engine::Vector3 value);
	}

	/// <summary>
	/// If projectionMode is NX_JPM_NONE, projection is disabled. If NX_JPM_POINT_MINDIST, 
	/// bodies are projected to limits leaving an linear error of projectionDistance and an
	/// angular error of projectionAngle.
	/// </summary>
	property JointProjectionMode ProjectionMode 
	{
		JointProjectionMode get();
		void set(JointProjectionMode value);
	}

	/// <summary>
	/// The distance above which to project the joint.
	/// </summary>
	property float ProjectionDistance 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// The angle above which to project the joint.
	/// </summary>
	property float ProjectionAngle 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// when the flag NX_D6JOINT_GEAR_ENABLED is set, the angular velocity of the second 
	/// actor is driven towards the angular velocity of the first actor times gearRatio 
	/// (both w.r.t. their primary axis).
	/// </summary>
	property float GearRatio 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// This is a combination of the bits defined by NxD6JointFlag.
	/// </summary>
	property D6JointFlag Flags 
	{
		D6JointFlag get();
		void set(D6JointFlag value);
	}
};

}