#pragma once

#include "PhysShape.h"

class NxCapsuleShape;

namespace Engine
{

namespace Physics
{

ref class PhysCapsuleShapeDesc;

/// <summary>
/// A capsule shaped collision detection primitive, also known as a line swept
/// sphere. 
/// <para>
/// 'radius' is the radius of the capsule's hemispherical ends and its trunk.
/// 'height' is the distance between the two hemispherical ends of the capsule.
/// The height is along the capsule's Y axis.
/// </para>
/// <para>
/// Each shape is owned by an actor that it is attached to.
/// </para>
/// </summary>
public ref class PhysCapsuleShape : public PhysShape
{
private:
	NxCapsuleShape* nxCapsule;

internal:
	/// <summary>
	/// Returns the native NxCapsuleShape
	/// </summary>
	NxCapsuleShape* getNxCapsuleShape();

	/// <summary>
	/// Constructor
	/// </summary>
	PhysCapsuleShape(NxCapsuleShape* nxCapsule);

public:
	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysCapsuleShape();

	/// <summary>
	/// Call this to initialize or alter the capsule.
	/// </summary>
	/// <param name="radius">The new radius of the capsule. Range: (0,inf)</param>
	/// <param name="height">The new height of the capsule. Range: (0,inf)</param>
	void setDimensions(float radius, float height);

	/// <summary>
	/// Alters the radius of the capsule.
	/// </summary>
	/// <param name="radius">The new radius.</param>
	void setRadius(float radius);

	/// <summary>
	/// Retrieves the radius of the capsule.
	/// </summary>
	/// <returns>The radius.</returns>
	float getRadius();

	/// <summary>
	/// Alters the height of the capsule.
	/// </summary>
	/// <param name="height">The new height of the capsule.</param>
	void setHeight(float height);

	/// <summary>
	/// Retrieves the height of the capsule.
	/// </summary>
	/// <returns>The height of the capsule measured from end to end.</returns>
	float getHeight();

	/// <summary>
	/// Save this capsule to the given desc.
	/// </summary>
	/// <param name="desc">The desc of the capsule.</param>
	void saveToDesc(PhysCapsuleShapeDesc^ desc);
};

}

}
