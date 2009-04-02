#pragma once

class NxJointDesc;

#include "Enums.h"

namespace Engine
{

namespace Physics
{

ref class PhysActor;
typedef array<PhysActor^> ActorArray;

/// <summary>
/// Wrapper for NxJointDesc.
/// Joint descriptors for all the different joint types are derived from this class.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysJointDesc
{
protected:
	ActorArray^ actors;
	Engine::Identifier^ name;

internal:
	NxJointDesc* jointDesc;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysJointDesc(NxJointDesc* jointDesc, Engine::Identifier^ name);

	/// <summary>
	/// (re)sets the structure to the default.
	/// </summary>
	void setToDefault();

	/// <summary>
	/// Returns true if the descriptor is valid.
	/// </summary>
	bool isValid();

	/// <summary>
	/// Set the localAnchor[] members using a world space point. 
	/// Sets the members localAnchor[0,1] by transforming the passed world space 
	/// vector into actor1 resp. actor2's local space. The actor pointers must already be set!
	/// </summary>
	/// <param name="wsAnchor">wsAnchor  Global frame anchor point. Range: position vector</param>
	void setGlobalAnchor(EngineMath::Vector3 wsAnchor);

	/// <summary>
	/// Set the local axis/normal using a world space axis. 
	/// Sets the members localAxis[0,1] by transforming the passed world space vector into 
	/// actor1 resp. actor2's local space, and finding arbitrary orthogonals for 
	/// localNormal[0,1]. The actor pointers must already be set!
	/// </summary>
	/// <param name="wsAxis">wsAxis  Global frame axis. Range: direction vector</param>
	void setGlobalAxis(EngineMath::Vector3 wsAxis);

	/// <summary>
	/// The two actors connected by the joint.
	/// The actors must be in the same scene as this joint.
	/// At least one of the two pointers must be a dynamic actor.
	/// One of the two may be NULL to indicate the world frame. Neither may 
	/// be a static actor!
	/// </summary>
	property PhysActor^ Actor[int]
	{
		PhysActor^ get(int index);
		void set(int index, PhysActor^ value);
	}

	/// <summary>
	/// X axis of joint space, in actor[i]'s space, orthogonal to localAxis[i]. 
	/// LocalAxis and LocalNormal should be unit length and at right angles to 
	/// each other, i.e. dot(localNormal[0],localAxis[0])==0 and dot(localNormal[1],
	/// localAxis[1])==0.
	/// </summary>
	property EngineMath::Vector3 LocalNormal[int]
	{
		EngineMath::Vector3 get(int index);
		void set(int index, EngineMath::Vector3 value);
	}

	/// <summary>
	/// Z axis of joint space, in actor[i]'s space. This is the primary axis of the joint. 
	/// LocalAxis and LocalNormal should be unit length and at right angles to each other, 
	/// i.e. dot(localNormal[0],localAxis[0])==0 and dot(localNormal[1],localAxis[1])==0.
	/// </summary>
	property EngineMath::Vector3 LocalAxis[int]
	{
		EngineMath::Vector3 get(int index);
		void set(int index, EngineMath::Vector3 value);
	}

	/// <summary>
	/// Attachment point of joint in actor[i]'s space.
	/// </summary>
	property EngineMath::Vector3 LocalAnchor[int]
	{
		EngineMath::Vector3 get(int index);
		void set(int index, EngineMath::Vector3 value);
	}

	/// <summary>
	/// Maximum linear force that the joint can withstand before breaking, must be positive.
	/// </summary>
	property float MaxForce 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Maximum angular force (torque) that the joint can withstand before breaking, 
	/// must be positive.
	/// </summary>
	property float MaxTorque 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Extrapolation factor for solving joint constraints. 
	/// This parameter can be used to build stronger joints and increase the solver 
	/// convergence. Higher values lead to stronger joints.
	/// </summary>
	/// <remarks>
	/// Setting the value too high can decrease the joint stability.
	/// Currently, this feature is supported for D6, Revolute and Spherical Joints only.
	/// </remarks>
	property float SolverExtrapolationFactor 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Switch to acceleration based spring. 
	/// This parameter can be used to switch between acceleration and force based 
	/// spring. Acceleration based springs do not take the mass of the attached objects 
	/// into account, i.e., the spring/damping behaviour will be independent of the load.
	/// </summary>
	property unsigned int UseAccelerationSpring 
	{
		unsigned int get();
		void set(unsigned int value);
	}

	/// <summary>
	/// This is a combination of the bits defined by JointFlag.
	/// </summary>
	property JointFlag JointFlags 
	{
		JointFlag get();
		void set(JointFlag value);
	}

	/// <summary>
	/// Debug name for the joint.
	/// </summary>
	property Engine::Identifier^ Name 
	{
		Engine::Identifier^ get();
		void set(Engine::Identifier^ value);
	}
};

}

}