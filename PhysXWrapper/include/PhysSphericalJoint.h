#pragma once

#include "PhysJoint.h"
#include "Enums.h"

class NxSphericalJoint;
class NxJointDesc;
class NxSphericalJointDesc;

namespace PhysXWrapper
{

ref class PhysSphericalJointDesc;

/// <summary>
/// Wrapper for NxSphericalJoint.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
public ref class PhysSphericalJoint : public PhysJoint
{
internal:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysSphericalJoint(NxSphericalJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene);

	//A pointer to the actual joint subclass.
	NxSphericalJoint* typedJoint;

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysSphericalJoint(){}
public:
	/// <summary>
	/// Writes all of the object's attributes to the desc struct
	/// </summary>
	/// <param name="desc">The descriptor used to retrieve the state of the object.</param>
	void saveToDesc(PhysSphericalJointDesc^ desc);

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
	void loadFromDesc(PhysSphericalJointDesc^ desc);

	/// <summary>
	/// Sets the flags to enable/disable the spring/motor/limit.
	/// </summary>
	/// <param name="flags">A combination of SphericalJointFlag flags to set for this joint.</param>
	void setFlags(SphericalJointFlag flags);

	/// <summary>
	/// Returns the current flag settings.
	/// </summary>
	/// <returns>The current flag settings.</returns>
	SphericalJointFlag getFlags();

	/// <summary>
	/// Sets the joint projection mode.
	/// </summary>
	/// <param name="projectionMode">The new projection mode.</param>
	void setProjectionMode(JointProjectionMode projectionMode);

	/// <summary>
	/// Returns the current flag settings.
	/// </summary>
	/// <returns>The current flag settings.</returns>
	JointProjectionMode getProjectionMode();
};

}