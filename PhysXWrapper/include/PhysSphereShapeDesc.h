#pragma once

#include "AutoPtr.h"
#include "PhysShapeDesc.h"

class NxSphereShapeDesc;

namespace Physics
{

/// <summary>
/// A shape description for a sphere.
/// </summary>
public ref class PhysSphereShapeDesc : public PhysShapeDesc
{
internal:
	AutoPtr<NxSphereShapeDesc> sphereShape;

public:
	/// <summary>
	/// Constructor.
	/// </summary>
	PhysSphereShapeDesc(System::String^ name);

	/// <summary>
	/// The radius of the sphere.
	/// </summary>
	property float Radius 
	{
		float get();
		void set(float value);
	}
};

}