#pragma once

#include "NxConvexShapeDesc.h"
#include "PhysShapeDesc.h"
#include "AutoPtr.h"
#include "Enums.h"

namespace PhysXWrapper
{

ref class PhysConvexMesh;

/// <summary>
/// Wrapper for NxConvexShapeDesc.
/// Descriptor class for PhysConvexShape.
/// </summary>
public ref class PhysConvexShapeDesc : public PhysShapeDesc
{
private:
	PhysConvexMesh^ mesh;

internal:
	AutoPtr<NxConvexShapeDesc> convexShape;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysConvexShapeDesc();

	/// <summary>
	/// References the triangle mesh that we want to instance.
	/// </summary>
	property PhysConvexMesh^ MeshData 
	{
		PhysConvexMesh^ get();
		void set(PhysConvexMesh^ value);
	}

	/// <summary>
	/// Combination of MeshShapeFlag.
	/// </summary>
	property MeshShapeFlag MeshFlags 
	{
		MeshShapeFlag get();
		void set(MeshShapeFlag value);
	}
};

}