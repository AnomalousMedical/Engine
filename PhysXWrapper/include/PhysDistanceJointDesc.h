#pragma once

#include "AutoPtr.h"
#include "PhysJointDesc.h"
#include "NxDistanceJointDesc.h"

namespace PhysXWrapper
{

ref class PhysSpringDesc;

/// <summary>
/// Wrapper for NxDistanceJointDesc.
/// </summary>
public ref class PhysDistanceJointDesc : public PhysJointDesc
{
private:
	PhysSpringDesc^ spring;

internal:
	AutoPtr<NxDistanceJointDesc> joint;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysDistanceJointDesc();

	/// <summary>
	/// The maximum rest length of the rope or rod between the two anchor points.
	/// </summary>
	property float MaxDistance 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// The minimum rest length of the rope or rod between the two anchor points.
	/// </summary>
	property float MinDistance 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Makes the joint springy. The spring.targetValue field is not used.
	/// </summary>
	property PhysSpringDesc^ Spring 
	{
		PhysSpringDesc^ get();
	}

	/// <summary>
	/// This is a combination of the bits defined by DistanceJointFlag.
	/// </summary>
	property DistanceJointFlag Flags 
	{
		DistanceJointFlag get();
		void set(DistanceJointFlag value);
	}
};

}