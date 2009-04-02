#pragma once

#include "PhysJoint.h"

class NxCylindricalJoint;
class NxJointDesc;
class NxCylindricalJointDesc;

namespace Engine
{

namespace Physics
{

ref class PhysCylindricalJointDesc;

/// <summary>
/// Wrapper for NxCylindricalJoint.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
public ref class PhysCylindricalJoint : public PhysJoint
{
internal:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysCylindricalJoint(Engine::Identifier^ name, NxCylindricalJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene);

	//A pointer to the actual joint subclass.
	NxCylindricalJoint* typedJoint;
	virtual NxJointDesc& getDesc() override;

	virtual void reloadFromDesc(NxJointDesc& desc) override
	{
		typedJoint->loadFromDesc((NxCylindricalJointDesc&)desc);
	}

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysCylindricalJoint(){}
public:
	/// <summary>
	/// Writes all of the object's attributes to the desc struct
	/// </summary>
	/// <param name="desc">The descriptor used to retrieve the state of the object.</param>
	void saveToDesc(PhysCylindricalJointDesc^ desc);

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
	void loadFromDesc(PhysCylindricalJointDesc^ desc);
};

}

}