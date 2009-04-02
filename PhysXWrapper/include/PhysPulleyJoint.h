#pragma once

#include "PhysJoint.h"

class NxPulleyJoint;
class NxJointDesc;
class NxPulleyJointDesc;

namespace PhysXWrapper
{

ref class PhysPulleyJointDesc;
ref class PhysMotorDesc;

/// <summary>
/// Wrapper for NxPulleyJoint.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
public ref class PhysPulleyJoint : public PhysJoint
{
internal:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysPulleyJoint(NxPulleyJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene);

	//A pointer to the actual joint subclass.
	NxPulleyJoint* typedJoint;
	virtual NxJointDesc& getDesc() override;

	virtual void reloadFromDesc(NxJointDesc& desc) override
	{
		typedJoint->loadFromDesc((NxPulleyJointDesc&)desc);
	}

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysPulleyJoint(){}
public:
	/// <summary>
	/// Writes all of the object's attributes to the desc struct
	/// </summary>
	/// <param name="desc">The descriptor used to retrieve the state of the object.</param>
	void saveToDesc(PhysPulleyJointDesc^ desc);

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
	void loadFromDesc(PhysPulleyJointDesc^ desc);

	void setMotor(PhysMotorDesc^ motor);

	void getMotor(PhysMotorDesc^ motor);

	void setFlags(PulleyJointFlag flags);

	PulleyJointFlag getFlags();
};

}