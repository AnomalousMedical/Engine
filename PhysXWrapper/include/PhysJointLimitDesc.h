#pragma once

#include "NxPhysics.h"

namespace Physics
{

/// <summary>
/// Describes a joint limit.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysJointLimitDesc
{
internal:
	NxJointLimitDesc* limitDesc;

	/// <summary>
	/// Constructor
	/// </summary>
	PhysJointLimitDesc(NxJointLimitDesc* limitDesc);
public:

	/// <summary>
	/// Sets members to default values.
	/// </summary>
	void setToDefault();

	/// <summary>
	/// Returns true if the descriptor is valid.
	/// </summary>
	bool isValid();

	/// <summary>
	/// The angle / position beyond which the limit is active.
	/// </summary>
	property float Value 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Limit bounce.
	/// </summary>
	property float Restitution 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// [not yet implemented!] limit can be made softer by setting this to less than 1.
	/// </summary>
	property float Hardness 
	{
		float get();
		void set(float value);
	}
};

}