#pragma once

#include "PhysShapeDesc.h"
#include "AutoPtr.h"
#include "Enums.h"

class NxTriangleMeshShapeDesc;

namespace PhysXWrapper
{

ref class PhysTriangleMesh;

/// <summary>
/// Descriptor for triangle mesh shapes.
/// </summary>
public ref class PhysTriangleMeshShapeDesc : PhysShapeDesc
{
private:
	PhysTriangleMesh^ triangleMesh;

internal:
	AutoPtr<NxTriangleMeshShapeDesc> triangleShape;

public:
	PhysTriangleMeshShapeDesc();

	/// <summary>
	/// The triangle mesh for the shape.
	/// </summary>
	property PhysTriangleMesh^ MeshData 
	{
		PhysTriangleMesh^ get();
		void set(PhysTriangleMesh^ value);
	}

	/// <summary>
	/// A combination of MeshFlags.
	/// </summary>
	property MeshFlags Flags 
	{
		MeshFlags get();
		void set(MeshFlags value);
	}
};

}