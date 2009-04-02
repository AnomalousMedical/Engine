#pragma once

#include "AutoPtr.h"
#include "PhysJointDesc.h"
#include "NxPulleyJointDesc.h"

namespace Engine
{

namespace Physics
{

ref class PhysMotorDesc;

/// <summary>
/// Wrapper for NxPulleyJointDesc.
/// Describes PulleyJoint.
/// </summary>
public ref class PhysPulleyJointDesc : public PhysJointDesc
{
private:
	PhysMotorDesc^ motor;

internal:
	AutoPtr<NxPulleyJointDesc> joint;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysPulleyJointDesc();

	/// <summary>
	/// Suspension points of two bodies in world space.
	/// </summary>
	property EngineMath::Vector3 Pulley[int]
	{
		EngineMath::Vector3 get(int index);
		void set(int index, EngineMath::Vector3 value);
	}

	/// <summary>
	/// The rest length of the rope connecting the two objects.
	/// </summary>
	property float Distance 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// How stiff the constraint is, between 0 and 1 (stiffest).
	/// </summary>
	property float Stiffness 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// transmission ratio
	/// </summary>
	property float Ratio 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// This is a combination of the bits defined by PulleyJointFlag.
	/// </summary>
	property PulleyJointFlag Flags 
	{
		PulleyJointFlag get();
		void set(PulleyJointFlag value);
	}

	/// <summary>
	/// Optional joint motor.
	/// </summary>
	property PhysMotorDesc^ Motor 
	{
		PhysMotorDesc^ get();
	}
};

}

}