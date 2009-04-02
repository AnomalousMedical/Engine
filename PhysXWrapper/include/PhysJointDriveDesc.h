#pragma once

#include "NxPhysics.h"
#include "Enums.h"

namespace Engine
{

namespace Physics
{

/// <summary>
/// Class used to describe drive properties for a NxD6Joint.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysJointDriveDesc
{
internal:
	NxJointDriveDesc* driveDesc;

public:
	PhysJointDriveDesc(NxJointDriveDesc* driveDesc);

	property D6JointDriveType DriveType
	{
		D6JointDriveType get();
		void set(D6JointDriveType value);
	}

	property float Spring 
	{
		float get();
		void set(float value);
	}

	property float Damping 
	{
		float get();
		void set(float value);
	}

	property float ForceLimit 
	{
		float get();
		void set(float value);
	}
};

}

}