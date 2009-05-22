#pragma once

#include "NxPhysics.h"

namespace PhysXWrapper
{

/// <summary>
/// Describes a joint limit.
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class PhysJointLimitSoftDesc
{
internal:
	NxJointLimitSoftDesc* limitDesc;

	/// <summary>
	/// Constructor
	/// </summary>
	PhysJointLimitSoftDesc(NxJointLimitSoftDesc* limitDesc);
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
	/// the angle / position beyond which the limit is active. 
	/// 
	/// Which side the limit restricts depends on whether this is a high or low limit.
	/// </summary>
	property float Value 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Controls the amount of bounce when the joint hits a limit. 
	/// 
	/// A restitution value of 1.0 causes the joint to bounce back with the velocity 
	/// which it hit the limit. A value of zero causes the joint to stop dead.
	/// 
	/// In situations where the joint has many locked DOFs (e.g. 5) the restitution may 
	/// not be applied correctly. This is due to a limitation in the solver which causes 
	/// the restitution velocity to become zero as the solver enforces constraints on the 
	/// other DOFs.
	/// 
	/// This limitation applies to both angular and linear limits, however it is generally 
	/// most apparent with limited angular DOFs.
	/// 
	/// Disabling joint projection and increasing the solver iteration count may improve 
	/// this behavior to some extent.
	/// 
	/// Also, combining soft joint limits with joint motors driving against those limits 
	/// may affect stability.
	/// </summary>
	property float Restitution 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// If greater than zero, the limit is soft, i.e. a spring pulls the joint back to the limit.
	/// </summary>
	property float Spring 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// If spring is greater than zero, this is the damping of the spring.
	/// </summary>
	property float Damping 
	{
		float get();
		void set(float value);
	}
};

}