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
	for (int i = 0; i < numShapes; ++i)
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

extern "C" _AnomalousExport void CollisionShape_SetLocalScaling(btCollisionShape* shape, Vector3* scale)
{
	shape->setLocalScaling(scale->toBullet());
}

btCollisionShape* cloneSingleShape(btCollisionShape* source)
{
	btCollisionShape* shape = 0;

	//Note that this doesn't really clone all possible types
	switch (source->getShapeType())
	{
	case CONVEX_HULL_SHAPE_PROXYTYPE:
	{
		btConvexHullShape* sourceHull = static_cast<btConvexHullShape*>(source);
		shape = new btConvexHullShape((btScalar*)sourceHull->getUnscaledPoints(), sourceHull->getNumPoints());
	}
	break;
	case CAPSULE_SHAPE_PROXYTYPE:
	{
		btCapsuleShape* sourceCapsule = static_cast<btCapsuleShape*>(source);
		shape = new btCapsuleShape(sourceCapsule->getRadius(), sourceCapsule->getHalfHeight() * 2.0f);
	}
	break;
	case BOX_SHAPE_PROXYTYPE:
	{
		btBoxShape* sourceBox = static_cast<btBoxShape*>(source);
		shape = new btBoxShape(sourceBox->getHalfExtentsWithoutMargin());
	}
	break;
	case SPHERE_SHAPE_PROXYTYPE:
	{
		btSphereShape* source = static_cast<btSphereShape*>(source);
		shape = new btSphereShape(source->getRadius());
	}
	break;
	default:
		shape = 0;
		break;
	}

	if (shape != 0)
	{
		shape->setMargin(source->getMargin());
	}
	return shape;
}

extern "C" _AnomalousExport btCollisionShape* CollisionShape_Clone(btCollisionShape* source)
{
	if (source->isCompound())
	{
		btCompoundShape* shape = new btCompoundShape();
		shape->setMargin(source->getMargin());

		btCompoundShape* sourceCompound = static_cast<btCompoundShape*>(source);
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