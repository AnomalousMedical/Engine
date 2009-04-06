#pragma once

#include "AutoPtr.h"
#include "PhysShapeDesc.h"

class NxBoxShapeDesc;

namespace PhysXWrapper
{

using namespace System;

/// <summary>
/// A description for a box shape.
/// </summary>
public ref class PhysBoxShapeDesc : public PhysShapeDesc
{
internal:
	AutoPtr<NxBoxShapeDesc> boxShape;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysBoxShapeDesc();

	/// <summary>
	/// Dimensions of the box as half extents.
	/// </summary>
	property EngineMath::Vector3 Dimensions 
	{ 
		EngineMath::Vector3 get();
		void set(EngineMath::Vector3 dimensions);
	}
};

}