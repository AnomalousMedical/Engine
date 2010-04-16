#pragma once

#include "ConvexDecomposition.h"

namespace BulletPlugin
{

#pragma unmanaged

/// <summary>
/// This class represents a section of a reshapeable rigid body. It will
/// maintian the shapes for a given region.
/// </summary>
class ReshapeableRigidBodySection : public ConvexDecomposition::ConvexDecompInterface
{
private:
	btAlignedObjectArray<btConvexHullShape*> m_convexShapes;
	btAlignedObjectArray<btVector3> m_convexCentroids;

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

	/// <summary>
	/// Delete all the shapes in this section. Make sure the shapes are not part
    /// of any other collision object first.
	/// </summary>
	void deleteShapes();

	/// <summary>
	/// Callback from the convex decomposition algorithm. Will build the actual
    /// convex hulls for this section.
	/// </summary>
	/// <param name="result">The ConvexResult.</param>
	virtual void ConvexDecompResult(ConvexDecomposition::ConvexResult &result);
};

#pragma managed

}