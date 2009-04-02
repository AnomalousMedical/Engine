#pragma once

#include "AutoPtr.h"
#include "PhysJointDesc.h"
#include "NxRevoluteJointDesc.h"

namespace Engine
{

namespace Physics
{

ref class PhysJointLimitPairDesc;
ref class PhysMotorDesc;
ref class PhysSpringDesc;

/// <summary>
/// Wrapper for NxRevoluteJointDesc.
/// Describes RevoluteJoint.
/// </summary>
public ref class PhysRevoluteJointDesc : public PhysJointDesc
{
private:
	PhysJointLimitPairDesc^ limit;
	PhysMotorDesc^ motor;
	PhysSpringDesc^ spring;

internal:
	AutoPtr<NxRevoluteJointDesc> joint;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysRevoluteJointDesc();

	/// <summary>
	/// Optional limits for the angular motion of the joint.
	/// </summary>
	property PhysJointLimitPairDesc^ Limit 
	{
		PhysJointLimitPairDesc^ get();
	}

	/// <summary>
	/// Optional motor.
	/// </summary>
	property PhysMotorDesc^ Motor 
	{
		PhysMotorDesc^ get();
	}

	/// <summary>
	/// Optional spring.
	/// </summary>
	property PhysSpringDesc^ Spring 
	{
		PhysSpringDesc^ get();
	}

	/// <summary>
	/// The distance beyond which the joint is projected.
	/// </summary>
	property float ProjectionDistance 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// The angle beyond which the joint is projected.
	/// </summary>
	property float ProjectionAngle 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// This is a combination of the bits defined by RevoluteJointFlag.
	/// </summary>
	property RevoluteJointFlag Flags 
	{
		RevoluteJointFlag get();
		void set(RevoluteJointFlag value);
	}

	/// <summary>
	/// Use this to enable joint projection.
	/// </summary>
	property JointProjectionMode ProjectionMode 
	{
		JointProjectionMode get();
		void set(JointProjectionMode value);
	}
};

}

}