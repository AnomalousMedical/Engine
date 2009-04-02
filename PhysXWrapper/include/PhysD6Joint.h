#pragma once

#include "PhysJoint.h"

class NxD6Joint;
class NxJointDesc;
class NxD6JointDesc;

namespace Engine
{

namespace Physics
{

ref class PhysD6JointDesc;

/// <summary>
/// Wrapper for NxD6Joint.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
public ref class PhysD6Joint : public PhysJoint
{
internal:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysD6Joint(NxD6Joint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene);

	//A pointer to the actual joint subclass.
	NxD6Joint* typedJoint;
	virtual NxJointDesc& getDesc() override;

	virtual void reloadFromDesc(NxJointDesc& desc) override
	{
		typedJoint->loadFromDesc((NxD6JointDesc&)desc);
	}

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysD6Joint(){}
public:
	/// <summary>
	/// Writes all of the object's attributes to the desc struct
	/// </summary>
	/// <param name="desc">The descriptor used to retrieve the state of the object.</param>
	void saveToDesc(PhysD6JointDesc^ desc);

	/// <summary>
	/// Use this for changing a significant number of joint parameters at once.
	/// </summary>
	/// <remarks>
	/// Use the set() methods for changing only a single property at once.
	/// 
	/// Please note that you can not change the actor pointers using this function, if you 
	/// do so the joint will be marked as broken and will stop working.
	/// 
	/// Calling the loadFromDesc() method on a broken joint will result in an error message.
	/// 
	/// Sleeping: This call wakes the actor if it is sleeping.
	/// </remarks>
	/// <param name="desc">The descriptor used to set the state of the object.</param>
	void loadFromDesc(PhysD6JointDesc^ desc);

	/// <summary>
	/// Set the drive position goal position when it is being driven. 
	/// 
	/// The goal position is specified relative to the joint frame corresponding to actor[0].
	/// </summary>
	/// <param name="position">The goal position if NX_D6JOINT_DRIVE_POSITION is set for xDrive,yDrive or zDrive. Range: position vector</param>
	void setDrivePosition(EngineMath::Vector3 position);

	/// <summary>
	/// Set the drive goal orientation when it is being driven. 
	///
	/// The goal orientation is specified relative to the joint frame corresponding to actor[0].
	/// </summary>
	/// <param name="orientation">The goal orientation if NX_D6JOINT_DRIVE_POSITION is set for swingDrive or twistDrive. Range: unit quaternion</param>
	void setDriveOrientation(EngineMath::Quaternion orientation);

	/// <summary>
	/// Set the drive goal linear velocity when it is being driven. 
	///
	/// The drive linear velocity is specified relative to the actor[0] joint frame.
	/// </summary>
	/// <param name="linVel">The goal velocity if NX_D6JOINT_DRIVE_VELOCITY is set for xDrive,yDrive or zDrive. See D6JointDesc. Range: velocity vector</param>
	void setDriveLinearVelocity(EngineMath::Vector3 linVel);

	/// <summary>
	/// Set the drive angular velocity goal when it is being driven. 
	/// 
	/// The drive angular velocity is specified relative to the drive orientation 
	/// target in the case of a slerp drive.
	/// 
	/// The drive angular velocity is specified in the actor[0] joint frame in all 
	/// other cases.
	/// </summary>
	/// <param name="angVel">The goal angular velocity if NX_D6JOINT_DRIVE_VELOCITY is set for swingDrive or twistDrive. Range: angular velocity vector.</param>
	void setDriveAngularVelocity(EngineMath::Vector3 angVel);
};

}

}