#include "Stdafx.h"

extern "C" _AnomalousExport void CollisionShape_Delete(btCollisionShape* shape)
{
	delete shape;
}

extern "C" _AnomalousExport btCompoundShape* CompoundShape_Create(float collisionMargin)
{
	btCompoundShape* shape = new btCompoundShape();
	shape->setMargin(collisionMargin);
	return shape;
}

extern "C" _AnomalousExport void CompoundShape_DeleteChildren(btCompoundShape* compound)
{
	int numShapes = compound->getNumChildShapes();
	for(int i = 0; i < numShapes; ++i)
	{
		btCollisionShape* shape = compound->getChildShape(i);
		delete shape;
	}
}

extern "C" _AnomalousExport int CompoundShape_GetCount(btCompoundShape* shape)
{
	return shape->getNumChildShapes();
}

extern "C" _AnomalousExport void CompoundShape_addChildShape(btCompoundShape* compound, btCollisionShape* child, Vector3* translation, Quaternion* rotation)
{
	btTransform childXform;
	childXform.setIdentity();
	childXform.setOrigin(translation->toBullet());
	childXform.getBasis().setRotation(rotation->toBullet());
	compound->addChildShape(childXform, child);
}

extern "C" _AnomalousExport btSphereShape* SphereShape_Create(float radius, float collisionMargin)
{
	btSphereShape* shape = new btSphereShape(radius);
	shape->setMargin(collisionMargin);
	return shape;
}

extern "C" _AnomalousExport btBoxShape* BoxShape_Create(Vector3* extents, float collisionMargin)
{
	btBoxShape* shape = new btBoxShape(extents->toBullet());
	shape->setMargin(collisionMargin);
	return shape;
}

extern "C" _AnomalousExport btCapsuleShape* CapsuleShape_Create(float radius, float height, float collisionMargin)
{
	btCapsuleShape* shape = new btCapsuleShape(radius, height);
	shape->setMargin(collisionMargin);
	return shape;
}

extern "C" _AnomalousExport btConvexHullShape* ConvexHullShape_Create(float* vertices, int numPoints, int stride, float collisionMargin)
{
	btConvexHullShape* shape = new btConvexHullShape(vertices, numPoints, stride);
	shape->setMargin(collisionMargin);
	return shape;
}

extern "C" _AnomalousExport void CollisionShape_CalculateLocalInertia(btCollisionShape* shape, float mass, Vector3* localInertia)
{
	btVector3 bulletInertia;
	shape->calculateLocalInertia(mass, bulletInertia);
	*localInertia = bulletInertia;
}