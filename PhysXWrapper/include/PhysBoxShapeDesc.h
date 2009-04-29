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
	property Engine::Vector3 Dimensions 
	{ 
		Engine::Vector3 get();
		void set(Engine::Vector3 dimensions);
	}
};

}