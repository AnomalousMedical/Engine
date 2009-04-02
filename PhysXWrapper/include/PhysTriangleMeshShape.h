#pragma once

#include "PhysShape.h"

class NxTriangleMeshShape;

namespace Engine{

namespace Physics{

/// <summary>
/// This class is a shape instance of a triangle mesh object of type
/// NxTriangleMesh. Each shape is owned by an actor that it is attached to.
/// </summary>
public ref class PhysTriangleMeshShape : public PhysShape
{
private:
	NxTriangleMeshShape* nxTriangleMesh;

internal:
	/// <summary>
	/// Returns the native NxTriangleMeshShape
	/// </summary>
	NxTriangleMeshShape* getNxTriangleMeshShape();

	/// <summary>
	/// Constructor
	/// </summary>
	PhysTriangleMeshShape(NxTriangleMeshShape* nxTriangleMesh);

public:
	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysTriangleMeshShape();
};

}

}
