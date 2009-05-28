#pragma once

#include "PhysJoint.h"

class NxDistanceJoint;
class NxJointDesc;
class NxDistanceJointDesc;

namespace PhysXWrapper
{

ref class PhysDistanceJointDesc;

/// <summary>
/// Wrapper for NxDistanceJoint.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
public ref class PhysDistanceJoint : public PhysJoint
{
internal:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysDistanceJoint(NxDistanceJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene);

	//A pointer to the actual joint subclass.
	NxDistanceJoint* typedJoint;

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysDistanceJoint(){}
public:
	/// <summary>
	/// Writes all of the object's attributes to the desc struct
	/// </summary>
	/// <param name="desc">The descriptor used to retrieve the state of the object.</param>
	void saveToDesc(PhysDistanceJointDesc^ desc);

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
	void loadFromDesc(PhysDistanceJointDesc^ desc);
};

}