#pragma once

#include "PhysShape.h"

class NxBoxShape;

namespace PhysXWrapper{

ref class PhysBoxShapeDesc;

/// <summary>
/// A box shaped collision detection primitive. 
/// Each shape is owned by the actor which it is attached to.
/// </summary>
public ref class PhysBoxShape : public PhysShape
{
private:
	NxBoxShape* nxBox;

internal:
	/// <summary>
	/// Returns the native NxBoxShape
	/// </summary>
	NxBoxShape* getNxBoxShape();

	/// <summary>
	/// Constructor
	/// </summary>
	PhysBoxShape(NxBoxShape* nxBox);

public:
	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysBoxShape();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="dimen"></param>
	void setDimensions(Engine::Vector3 dimen);

	/// <summary>
	/// Sets the box dimensions. 
	/// <para>
	/// The dimensions are the 'radii' of the box, meaning 1/2 extents in x
    /// dimension, 1/2 extents in y dimension, 1/2 extents in z dimension.
	/// </para>
	/// </summary>
	/// <param name="dimen">The new 'radii' of the box. Range: direction vector</param>
	void setDimensions(Engine::Vector3% dimen);

	/// <summary>
	/// Retrieves the dimensions of the box.
	/// <para>
	/// The dimensions are the 'radii' of the box, meaning 1/2 extents in x
    /// dimension, 1/2 extents in y dimension, 1/2 extents in z dimension.
	/// </para>
	/// </summary>
	/// <returns>The 'radii' of the box.</returns>
	Engine::Vector3 getDimensions();

	/// <summary>
	/// Saves the state of the shape object to a descriptor.
	/// </summary>
	/// <param name="desc">Descriptor to save to.</param>
	void saveToDesc(PhysBoxShapeDesc^ desc);
};

}