#pragma once

#include "AutoPtr.h"
#include "PhysShapeDesc.h"

class NxCapsuleShapeDesc;

namespace PhysXWrapper
{
/// <summary>
/// A shape description for capsule shapes.
/// </summary>
public ref class PhysCapsuleShapeDesc : public PhysShapeDesc
{
internal:
	AutoPtr<NxCapsuleShapeDesc> capsuleShape;

public:
	/// <summary>
	/// Constructor.
	/// </summary>
	PhysCapsuleShapeDesc(System::String^ name);

	/// <summary>
	/// The radius of the capsule.
	/// </summary>
	property float Radius 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// The height of the capsule.
	/// </summary>
	property float Height 
	{
		float get();
		void set(float value);
	}
};

}