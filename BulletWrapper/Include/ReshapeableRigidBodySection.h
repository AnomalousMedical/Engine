#pragma once

#include "ConvexDecomposition.h"

/// <summary>
/// This class represents a section of a reshapeable rigid body. It will
/// maintian the shapes for a given region.
/// </summary>
class ReshapeableRigidBodySection : public ConvexDecomposition::ConvexDecompInterface
{
private:
	btAlignedObjectArray<btCollisionShape*> m_convexShapes;
	btAlignedObjectArray<btVector3> m_convexCentroids;
	btTransform transform;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	ReshapeableRigidBodySection(void);

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="translation">The translation of the origin.</param>
	/// <param name="orientation">The orientation of the origin.</param>
	ReshapeableRigidBodySection(const Vector3& translation, const Quaternion& orientation);

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

	/// <summary>
	/// Delete all the shapes in this section. Make sure the shapes are not part
    /// of any other collision object first.
	/// </summary>
	void deleteShapes();

	/// <summary>
	/// Add a sphere to this RigidBodySection.
	/// </summary>
	/// <param name="radius">The radius.</param>
	/// <param name="translation">The translation of the sphere.</param>
	/// <param name="compoundShape">The compoundShape to add the new sphere shape to. Can be 0 to not add the sphere to anything yet.</param>
	void addSphere(float radius, const Vector3& translation, btCompoundShape* compoundShape = 0);

	/// <summary>
	/// Move the origin of this section. Will not take effect until the shapes are removed and readded.
	/// </summary>
	/// <param name="translation">The translation of the origin.</param>
	/// <param name="orientation">The orientation of the origin.</param>
	void moveOrigin(const Vector3& translation, const Quaternion& orientation);

	/// <summary>
	/// Callback from the convex decomposition algorithm. Will build the actual
    /// convex hulls for this section.
	/// </summary>
	/// <param name="result">The ConvexResult.</param>
	virtual void ConvexDecompResult(ConvexDecomposition::ConvexResult &result);
};
