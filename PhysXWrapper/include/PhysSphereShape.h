#pragma once

#include "PhysShape.h"

class NxSphereShape;

namespace PhysXWrapper{

ref class PhysSphereShapeDesc;

/// <summary>
/// A sphere shaped collision detection primitive. Each shape is owned by an
/// actor that it is attached to.
/// </summary>
public ref class PhysSphereShape : public PhysShape
{
private:
	NxSphereShape* nxSphere;

internal:
	/// <summary>
	/// Returns the native NxSphereShape
	/// </summary>
	NxSphereShape* getNxSphereShape();

	/// <summary>
	/// Constructor
	/// </summary>
	PhysSphereShape(NxSphereShape* nxSphere);

public:
	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysSphereShape();

	/// <summary>
	/// Sets the sphere radius. Call this to initialize or alter the sphere. If
    /// this is not called, then the default settings create a unit sphere at
    /// the origin.
	/// </summary>
	/// <param name="radius">The radius to set.</param>
	void setRadius(float radius);

	/// <summary>
	/// Retrieves the radius of the sphere. 
	/// </summary>
	/// <returns>The radius of the sphere.</returns>
	float getRadius();

	/// <summary>
	/// Save the shape to the description.
	/// </summary>
	/// <param name="desc">The description to save the shape to.</param>
	void saveToDesc(PhysSphereShapeDesc^ desc);
};

}