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

	void addHullShape(float* vertices, int numPoints, int stride, float collisionMargin, const Vector3& translation, const Quaternion& rotation, btCompoundShape* compoundShape);

	void cloneAndAddShape(btCollisionShape* toClone, const Vector3& translation, const Quaternion& rotation, btCompoundShape* compoundShape);

	/// <summary>
	/// Move the origin of this section. Will not take effect until the shapes are removed and readded.
	/// </summary>
	/// <param name="translation">The translation of the origin.</param>
	/// <param name="orientation">The orientation of the origin.</param>
	void moveOrigin(const Vector3& translation, const Quaternion& orientation);

	void setLocalScaling(const Vector3& scale);

	/// <summary>
	/// Callback from the convex decomposition algorithm. Will build the actual
    /// convex hulls for this section.
	/// </summary>
	/// <param name="result">The ConvexResult.</param>
	virtual void ConvexDecompResult(ConvexDecomposition::ConvexResult &result);

	btCollisionShape* convertCollisionShape(btCollisionShapeData* shapeData);

	btCollisionShape* createPlaneShape(const btVector3& planeNormal, btScalar planeConstant);

	btCollisionShape* createBoxShape(const btVector3& halfExtents);

	btCollisionShape* createSphereShape(btScalar radius);

	btCollisionShape* createCapsuleShapeX(btScalar radius, btScalar height);

	btCollisionShape* createCapsuleShapeY(btScalar radius, btScalar height);

	btCollisionShape* createCapsuleShapeZ(btScalar radius, btScalar height);

	btCollisionShape* createCylinderShapeX(btScalar radius, btScalar height);

	btCollisionShape* createCylinderShapeY(btScalar radius, btScalar height);

	btCollisionShape* createCylinderShapeZ(btScalar radius, btScalar height);

	btCollisionShape* createConeShapeX(btScalar radius, btScalar height);

	btCollisionShape* createConeShapeY(btScalar radius, btScalar height);

	btCollisionShape* createConeShapeZ(btScalar radius, btScalar height);

	btTriangleIndexVertexArray*	createTriangleMeshContainer();

	btOptimizedBvh*	createOptimizedBvh();

	btTriangleInfoMap* createTriangleInfoMap();

	btBvhTriangleMeshShape* createBvhTriangleMeshShape(btStridingMeshInterface* trimesh, btOptimizedBvh* bvh);

	btCollisionShape* createConvexTriangleMeshShape(btStridingMeshInterface* trimesh);

	btConvexHullShape* createConvexHullShape();

	btCompoundShape* createCompoundShape();

	btScaledBvhTriangleMeshShape* createScaledTrangleMeshShape(btBvhTriangleMeshShape* meshShape, const btVector3& localScaling);

	btMultiSphereShape* createMultiSphereShape(const btVector3* positions, const btScalar* radi, int numSpheres);
};
