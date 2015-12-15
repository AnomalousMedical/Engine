#pragma once

#include "ConvexDecomposition.h"

/// <summary>
/// This class represents a section of a reshapeable rigid body. It will
/// maintian the shapes for a given region.
/// </summary>
class ReshapeableRigidBodySection
{
private:
	btAlignedObjectArray<btCollisionShape*> m_convexShapes;
	btAlignedObjectArray<btTransform> m_convexCentroids;
	btTransform transform;
	btVector3 scale;

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

	void cloneAndAddShape(btCollisionShape* toClone, btCompoundShape* compoundShape);

	/// <summary>
	/// Move the origin of this section. Will not take effect until the shapes are removed and readded.
	/// </summary>
	/// <param name="translation">The translation of the origin.</param>
	/// <param name="orientation">The orientation of the origin.</param>
	void moveOrigin(const Vector3& translation, const Quaternion& orientation);

	void setLocalScaling(const Vector3& scale);

private:
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
