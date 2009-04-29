#pragma once

#include "PhysShape.h"

class NxPlaneShape;

namespace PhysXWrapper{

ref class PhysPlaneShapeDesc;

/// <summary>
/// A plane collision detection primitive. 
/// <para>
/// By default it is configured to be the y == 0 plane. You can then set a
/// normal and a d to specify an arbitrary plane. d is the distance of the plane
/// from the origin along the normal, assuming the normal is normalized. Thus
/// the plane equation is: normal.x * X + normal.y * Y + normal.z * Z = d
/// </para>
/// <para>
/// Note: The plane equation defines the plane in world space. Any other
/// transformations, like actor global pose and shape local pose are ignored by
/// the plane shape.
/// </para>
/// <para>
/// Note: the plane does not represent an infinitely thin object, but rather a
/// completely solid negative half space (all points p for which normal.dot(p) -
/// d &lt; 0 are inside the solid region.)
/// </para>
/// <para>
/// Each shape is owned by an actor that it is attached to.
/// </para>
/// </summary>
public ref class PhysPlaneShape : public PhysShape
{
private:
	NxPlaneShape* nxPlane;

internal:
	/// <summary>
	/// Returns the native NxPlaneShape
	/// </summary>
	NxPlaneShape* getNxPlaneShape();

	/// <summary>
	/// Constructor
	/// </summary>
	PhysPlaneShape(NxPlaneShape* nxPlane);

public:
	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysPlaneShape();

	/// <summary>
	/// sets the plane equation.
	/// </summary>
	/// <param name="normal">Normal for the plane, in the global frame. Range: direction vector</param>
	/// <param name="d">'d' coefficient of the plane equation. Range: (-inf,inf)</param>
	void setPlane(Engine::Vector3 normal, float d);

	/// <summary>
	/// sets the plane equation.
	/// </summary>
	/// <param name="normal">Normal for the plane, in the global frame. Range: direction vector</param>
	/// <param name="d">'d' coefficient of the plane equation. Range: (-inf,inf)</param>
	void setPlane(Engine::Vector3% normal, float d);

	/// <summary>
	/// Save to the given description.
	/// </summary>
	/// <param name="desc">The description to save to.</param>
	void saveToDesc(PhysPlaneShapeDesc^ desc);
};

}