#pragma once

#include "PhysShape.h"

class NxConvexShape;

namespace PhysXWrapper{

/// <summary>
/// Used to represent an instance of an NxConvexMesh.
/// </summary>
public ref class PhysConvexShape : public PhysShape
{
private:
	NxConvexShape* nxConvex;

internal:
	/// <summary>
	/// Returns the native NxConvexShape
	/// </summary>
	NxConvexShape* getNxConvexShape();

	/// <summary>
	/// Constructor
	/// </summary>
	PhysConvexShape(NxConvexShape* nxConvex);

public:
	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysConvexShape();
};

}