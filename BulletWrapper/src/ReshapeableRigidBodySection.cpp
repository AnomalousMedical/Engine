#include "StdAfx.h"
#include "../Include/ReshapeableRigidBodySection.h"

ReshapeableRigidBodySection::ReshapeableRigidBodySection(void)
{
	transform.setIdentity();
}

ReshapeableRigidBodySection::ReshapeableRigidBodySection(const Vector3& translation, const Quaternion& orientation)
{
	transform.setOrigin(translation.toBullet());
	transform.setRotation(orientation.toBullet());
}

ReshapeableRigidBodySection::~ReshapeableRigidBodySection(void)
{
	deleteShapes();
}

void ReshapeableRigidBodySection::addShapes(btCompoundShape* compoundShape)
{
	btTransform trans;
	for (int i=0; i < m_convexShapes.size(); ++i)
	{
		trans.setIdentity();
		btVector3 centroid = m_convexCentroids[i];
		trans.setOrigin(centroid);
		trans = transform * trans;
		btCollisionShape* convexShape = m_convexShapes[i];
		compoundShape->addChildShape(trans, convexShape);
	}
}

void ReshapeableRigidBodySection::removeShapes(btCompoundShape* compoundShape)
{
	for (int i=0; i < m_convexShapes.size(); ++i)
	{
		btCollisionShape* convexShape = m_convexShapes[i];
		compoundShape->removeChildShape(convexShape);
	}
}

void ReshapeableRigidBodySection::deleteShapes()
{
	for (int i=0; i < m_convexShapes.size(); ++i)
	{
		btCollisionShape* convexShape = m_convexShapes[i];
		delete convexShape;
	}
	m_convexShapes.clear();
	m_convexCentroids.clear();
}

void ReshapeableRigidBodySection::addSphere(float radius, const Vector3& translation, btCompoundShape* compoundShape)
{
	btSphereShape* sphereShape = new btSphereShape(radius);
	btVector3 centroid = translation.toBullet();
	m_convexShapes.push_back(sphereShape);
	m_convexCentroids.push_back(centroid);
	if(compoundShape != 0)
	{
		btTransform transform;
		transform.setIdentity();
		transform.setOrigin(centroid);
		compoundShape->addChildShape(transform, sphereShape);
	}
}

void ReshapeableRigidBodySection::moveOrigin(const Vector3& translation, const Quaternion& orientation)
{
	transform.setOrigin(translation.toBullet());
	transform.setRotation(orientation.toBullet());
}

void ReshapeableRigidBodySection::ConvexDecompResult(ConvexDecomposition::ConvexResult &result)
{
	btVector3 localScaling(1.f,1.f,1.f);

	//calc centroid, to shift vertices around center of mass
	btVector3 centroid(0,0,0);
	btAlignedObjectArray<btVector3> vertices;
	for (unsigned int i=0; i<result.mHullVcount; i++)
	{
		btVector3 vertex(result.mHullVertices[i*3],result.mHullVertices[i*3+1],result.mHullVertices[i*3+2]);
		vertex *= localScaling;
		centroid += vertex;
	}

	centroid *= 1.f/(float(result.mHullVcount) );

	//Shift vertices by centroid.
	for (unsigned int i=0; i<result.mHullVcount; i++)
	{
		btVector3 vertex(result.mHullVertices[i*3],result.mHullVertices[i*3+1],result.mHullVertices[i*3+2]);
		vertex *= localScaling;
		vertex -= centroid ;
		vertices.push_back(vertex);
	}

	//Create convex hull
	btConvexHullShape* convexShape = new btConvexHullShape(&(vertices[0].getX()),vertices.size());

	convexShape->setMargin(0.0f);
	m_convexShapes.push_back(convexShape);
	m_convexCentroids.push_back(centroid);
}