#pragma once

#include "NxPhysics.h"
#include "AutoPtr.h"

namespace PhysXWrapper
{

ref class PhysJointLimitDesc;

/// <summary>
/// Describes a pair of joint limits.
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class PhysJointLimitPairDesc
{
private:
	PhysJointLimitDesc^ low;
	PhysJointLimitDesc^ high;
	AutoPtr<NxJointLimitPairDesc> autoLimitDesc; //auto pointer for directly created descs.

internal:
	NxJointLimitPairDesc* limitDesc;

	/// <summary>
	/// Constructor, wraps an exisiting NxJointLimitPairDesc.
	/// </summary>
	PhysJointLimitPairDesc(NxJointLimitPairDesc* limitDesc);

public:
	PhysJointLimitPairDesc();

	/// <summary>
	/// Returns true if the descriptor is valid.
	/// </summary>
	bool isValid();

	/// <summary>
	/// The low limit (smaller value).
	/// </summary>
	property PhysJointLimitDesc^ Low 
	{
		PhysJointLimitDesc^ get();
	}

	/// <summary>
	/// the high limit (larger value).
	/// </summary>
	property PhysJointLimitDesc^ High 
	{
		PhysJointLimitDesc^ get();
	}
};

}