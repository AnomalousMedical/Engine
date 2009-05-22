#pragma once

#include "NxPhysics.h"

namespace PhysXWrapper
{

ref class PhysJointLimitSoftDesc;

/// <summary>
/// Describes a pair of joint limits.
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class PhysJointLimitSoftPairDesc
{
private:
	PhysJointLimitSoftDesc^ low;
	PhysJointLimitSoftDesc^ high;

internal:
	NxJointLimitSoftPairDesc* limitDesc;

	/// <summary>
	/// Constructor
	/// </summary>
	PhysJointLimitSoftPairDesc(NxJointLimitSoftPairDesc* limitDesc);

public:

	/// <summary>
	/// Returns true if the descriptor is valid.
	/// </summary>
	bool isValid();

	/// <summary>
	/// The low limit (smaller value).
	/// </summary>
	property PhysJointLimitSoftDesc^ Low 
	{
		PhysJointLimitSoftDesc^ get();
	}

	/// <summary>
	/// the high limit (larger value).
	/// </summary>
	property PhysJointLimitSoftDesc^ High 
	{
		PhysJointLimitSoftDesc^ get();
	}
};

}