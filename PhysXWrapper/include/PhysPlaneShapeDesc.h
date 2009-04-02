#pragma once

#include "AutoPtr.h"
#include "PhysShapeDesc.h"

class NxPlaneShapeDesc;

namespace PhysXWrapper
{

/// <summary>
/// A shape description for a plane.
/// </summary>
public ref class PhysPlaneShapeDesc : public PhysShapeDesc
{
internal:
	AutoPtr<NxPlaneShapeDesc> planeShape;

public:
	/// <summary>
	/// Constructor.
	/// </summary>
	PhysPlaneShapeDesc(System::String^ name);

	/// <summary>
	/// The normal of the plane.
	/// </summary>
	property EngineMath::Vector3 Normal 
	{
		EngineMath::Vector3 get();
		void set(EngineMath::Vector3 value);
	}

	/// <summary>
	/// The offset from the origin of the plane.
	/// </summary>
	property float D 
	{
		float get();
		void set(float value);
	}
};

}