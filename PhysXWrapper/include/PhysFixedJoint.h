#pragma once

#include "PhysJoint.h"

class NxFixedJoint;
class NxJointDesc;
class NxFixedJointDesc;

namespace Physics
{

ref class PhysFixedJointDesc;

/// <summary>
/// Wrapper for NxFixedJoint.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
public ref class PhysFixedJoint : public PhysJoint
{
internal:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysFixedJoint(NxFixedJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene);

	//A pointer to the actual joint subclass.
	NxFixedJoint* typedJoint;
	virtual NxJointDesc& getDesc() override;

	virtual void reloadFromDesc(NxJointDesc& desc) override
	{
		typedJoint->loadFromDesc((NxFixedJointDesc&)desc);
	}

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysFixedJoint(){}
public:
	/// <summary>
	/// Writes all of the object's attributes to the desc struct
	/// </summary>
	/// <param name="desc">The descriptor used to retrieve the state of the object.</param>
	void saveToDesc(PhysFixedJointDesc^ desc);

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
	void loadFromDesc(PhysFixedJointDesc^ desc);
};

}