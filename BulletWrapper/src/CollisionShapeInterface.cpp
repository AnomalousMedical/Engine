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

extern "C" _AnomalousExport void CompoundShape_removeChildShape(btCompoundShape* compound, btCollisionShape* child)
{
	compound->removeChildShape(child);
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

btCollisionShape* cloneSingleShape(btCollisionShape* source)
{
	btCollisionShape* shape = 0;
	btConvexHullShape* sourceHull;

	switch (source->getShapeType())
	{
		case CONVEX_HULL_SHAPE_PROXYTYPE:
			sourceHull = static_cast<btConvexHullShape*>(shape);
			shape = new btConvexHullShape((btScalar*)sourceHull->getUnscaledPoints(), sourceHull->getNumPoints());
		default:
			shape = 0;
	}

	shape->setMargin(source->getMargin());
	return shape;
}

extern "C" _AnomalousExport btCollisionShape* CollisionShape_Clone(btCollisionShape* source)
{
	if (source->isCompound())
	{
		btCompoundShape* shape = new btCompoundShape();
		shape->setMargin(source->getMargin());

		btCompoundShape* sourceCompound = static_cast<btCompoundShape*>(shape);
		btCompoundShapeChild* childPtr = sourceCompound->getChildList();
		for (int i = 0; i < sourceCompound->getNumChildShapes(); ++i)
		{
			btCollisionShape* childShape = cloneSingleShape(childPtr->m_childShape);
			if (childShape != 0)
			{
				shape->addChildShape(childPtr->m_transform, childShape);
			}
			++childPtr;
		}

		return shape;
	}

	return cloneSingleShape(source);
}