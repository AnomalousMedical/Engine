#pragma once

#include "AutoPtr.h"
#include "Enums.h"

class NxBodyDesc;

namespace Engine
{

namespace Physics
{

/// <summary>
/// Wrapper for the NxBodyDesc class.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysBodyDesc
{
internal:
	AutoPtr<NxBodyDesc> bodyDesc;

public:
	PhysBodyDesc(void);

	//property NxMat34 massLocalPose

	/// <summary>
	/// Diagonal mass space inertia tensor in bodies mass frame.
	/// </summary>
	property EngineMath::Vector3 MassSpaceInertia
	{
		EngineMath::Vector3 get();
		void set(EngineMath::Vector3 inertia);
	}

	/// <summary>
	/// Mass of body.
	/// </summary>
	property float Mass
	{
		float get();
		void set(float mass);
	}

	/// <summary>
	/// Linear Velocity of the body.
	/// </summary>
	property EngineMath::Vector3 LinearVelocity
	{
		EngineMath::Vector3 get();
		void set(EngineMath::Vector3 linearVelocity);
	}

	/// <summary>
	/// Angular velocity of the body.
	/// </summary>
	property EngineMath::Vector3 AngularVelocity
	{
		EngineMath::Vector3 get();
		void set(EngineMath::Vector3 angularVelocity);
	}

	/// <summary>
	/// The body's initial wake up counter.
	/// </summary>
	property float WakeUpCounter
	{
		float get();
		void set(float wakeUpCounter);
	}

	/// <summary>
	/// Linear damping applied to the body.
	/// </summary>
	property float LinearDamping
	{
		float get();
		void set(float linearDamping);
	}

	/// <summary>
	/// Angular damping applied to the body.
	/// </summary>
	property float AngularDamping
	{
		float get();
		void set(float angularDamping);
	}

	/// <summary>
	/// Maximum allowed angular velocity.
	/// </summary>
	property float MaxAngularVelocity
	{
		float get();
		void set(float maxAngularVelocity);
	}

	/// <summary>
	/// When CCD is globally enabled, it is still not performed if the motion 
	/// distance of all points on the body is below this threshold.
	/// </summary>
	property float CCDMotionThreshold
	{
		float get();
		void set(float motionThreshold);
	}

	/// <summary>
	/// Combination of BodyFlag flags
	/// </summary>
	property BodyFlag Flags
	{
		BodyFlag get();
		void set(BodyFlag flags);
	}

	/// <summary>
	/// Maximum linear velocity at which body can go to sleep.
	/// </summary>
	property float SleepLinearVelocity
	{
		float get();
		void set(float sleepLinearVelocity);
	}

	/// <summary>
	/// Maximum angular velocity at which body can go to sleep.
	/// </summary>
	property float SleepAngularVelocity
	{
		float get();
		void set(float sleepAngularVelocity);
	}

	/// <summary>
	/// Number of solver iterations performed when processing 
	/// joint/contacts connected to this body.
	/// </summary>
	property unsigned int SolverIterationCount
	{
		unsigned int get();
		void set(unsigned int solverIterationCount);
	}

	/// <summary>
	/// Threshold for the energy-based sleeping algorithm. 
	/// Only used when the NX_BF_ENERGY_SLEEP_TEST flag is set.
	/// </summary>
	property float SleepEnergyThreshold
	{
		float get();
		void set(float sleepEnergyThreshold);
	}

	/// <summary>
	/// Damping factor for bodies that are about to sleep.
	/// </summary>
	property float SleepDamping
	{
		float get();
		void set(float sleepDamping);
	}

	/// <summary>
	/// The force threshold for contact reports.
	/// </summary>
	property float ContactReportThreshold
	{
		float get();
		void set(float contactReportThreshold);
	}
};

}

}