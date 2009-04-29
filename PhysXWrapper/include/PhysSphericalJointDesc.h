#pragma once

#include "AutoPtr.h"
#include "PhysJointDesc.h"
#include "NxSphericalJointDesc.h"

namespace PhysXWrapper
{

ref class PhysJointLimitPairDesc;
ref class PhysJointLimitDesc;
ref class PhysSpringDesc;

/// <summary>
/// Wrapper for NxSphericalJointDesc.
/// Describes SphericalJoint.
/// </summary>
public ref class PhysSphericalJointDesc : public PhysJointDesc
{
private:
	PhysJointLimitPairDesc^ twistLimit;
	PhysJointLimitDesc^ swingLimit;
	PhysSpringDesc^ twistSpring;
	PhysSpringDesc^ swingSpring;
	PhysSpringDesc^ jointSpring;

internal:
	AutoPtr<NxSphericalJointDesc> joint;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysSphericalJointDesc();

	/// <summary>
	/// Swing limit axis defined in the joint space of actor 0.
	/// </summary>
	property Engine::Vector3 SwingAxis 
	{
		Engine::Vector3 get();
		void set(Engine::Vector3 value);
	}

	/// <summary>
	/// Distance above which to project joint.
	/// </summary>
	property float ProjectionDistance 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Limits rotation around twist axis.
	/// </summary>
	property PhysJointLimitPairDesc^ TwistLimit 
	{
		PhysJointLimitPairDesc^ get();
	}

	/// <summary>
	/// Limits swing of twist axis.
	/// </summary>
	property PhysJointLimitDesc^ SwingLimit 
	{
		PhysJointLimitDesc^ get();
	}

	/// <summary>
	/// Spring that works against twisting.
	/// </summary>
	property PhysSpringDesc^ TwistSpring 
	{
		PhysSpringDesc^ get();
	}

	/// <summary>
	/// Spring that works against swinging.
	/// </summary>
	property PhysSpringDesc^ SwingSpring 
	{
		PhysSpringDesc^ get();
	}

	/// <summary>
	/// Spring that lets the joint get pulled apart.
	/// </summary>
	property PhysSpringDesc^ JointSpring 
	{
		PhysSpringDesc^ get();
	}

	/// <summary>
	/// This is a combination of the bits defined by SphericalJointFlag.
	/// </summary>
	property SphericalJointFlag Flags 
	{
		SphericalJointFlag get();
		void set(SphericalJointFlag value);
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