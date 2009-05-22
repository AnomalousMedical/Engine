#pragma once

#include "AutoPtr.h"

class NxSpringDesc;

namespace PhysXWrapper
{

/// <summary>
/// Describes a joint spring. 
/// The spring is implicitly integrated, so even high spring and damper coefficients 
/// should be robust.
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class PhysSpringDesc
{
private:
	AutoPtr<NxSpringDesc> autoSpringDesc; //Auto pointer for directly created descs.

internal:
	PhysSpringDesc(NxSpringDesc* springDesc);
	NxSpringDesc* springDesc;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysSpringDesc();

	/// <summary>
	/// spring coefficient
	/// </summary>
	property float Spring 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// damper coefficient
	/// </summary>
	property float Damper 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// target value (angle/position) of spring where the spring force is zero.
	/// Range: Angular: (-PI,PI]
	/// Range: Positional: (-inf,inf)
	/// </summary>
	property float TargetValue 
	{
		float get();
		void set(float value);
	}
};

}