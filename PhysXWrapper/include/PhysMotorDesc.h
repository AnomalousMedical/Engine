#pragma once

#include "AutoPtr.h"

class NxMotorDesc;

namespace Physics
{

/// <summary>
/// Describes a joint motor. 
/// Some joints can be motorized, this allows them to apply a force to cause attached 
/// actors to move.
///
/// Joints which can be motorized:
///		PhysPulleyJoint 
///		PhysRevoluteJoint 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysMotorDesc
{
private:
	AutoPtr<NxMotorDesc> autoMotorDesc;  //Auto pointer used if this is created directly.

internal:
	/// <summary>
	/// Constructor, used to wrap motors that are part of other descriptors.
	/// </summary>
	PhysMotorDesc(NxMotorDesc* motorDesc);

	NxMotorDesc* motorDesc; //Pointer to the wrapped motor.

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysMotorDesc();

	/// <summary>
	/// Sets members to default values.
	/// </summary>
	void setToDefault();

	/// <summary>
	/// Returns true if the descriptor is valid.
	/// </summary>
	/// <returns>True if valid.</returns>
	bool isValid();

	/// <summary>
	/// The relative velocity the motor is trying to achieve.
	/// </summary>
	property float VelTarget 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// The maximum force (or torque) the motor can exert.
	/// </summary>
	property float MaxForce 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// If true, motor will not brake when it spins faster than velTarget.
	/// </summary>
	property bool FreeSpin 
	{
		bool get();
		void set(bool value);
	}
};

}