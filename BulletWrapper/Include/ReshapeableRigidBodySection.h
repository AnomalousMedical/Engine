#pragma once

#include "ConvexDecomposition.h"

/// <summary>
/// This class represents a section of a reshapeable rigid body. It will
/// maintian the shapes for a given region.
/// </summary>
class ReshapeableRigidBodySection
{
private:
	btCollisionShape* shape;
	btTransform transform;
	btVector3 scale;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	ReshapeableRigidBodySection(void);

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~ReshapeableRigidBodySection(void);

	/// <summary>
	/// Add the shapes in this section to the given compoundShape.
	/// </summary>
	/// <param name="compoundShape">The compoundShape to add this section's shapes to.</param>
	void addShapes(btCompoundShape* compoundShape);

	/// <summary>
	/// Remove the shapes in this section from a given compoundShape.
	/// </summary>
	/// <param name="compoundShape">The compoundShape to remove this section's shapes from.</param>
	void removeShapes(btCompoundShape* compoundShape);
	
	void cloneAndSetShape(btCollisionShape* toClone, btCompoundShape* compoundShape);

	/// <summary>
	/// Move the origin of this section. Will not take effect until the shapes are removed and readded.
	/// </summary>
	/// <param name="translation">The translation of the origin.</param>
	/// <param name="orientation">The orientation of the origin.</param>
	void moveOrigin(const Vector3& translation, const Quaternion& orientation);

	void setLocalScaling(const Vector3& scale);
};
