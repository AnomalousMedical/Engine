#include "StdAfx.h"
#include "..\include\ReshapeableRigidBodySection.h"

namespace BulletPlugin
{


#pragma unmanaged

ReshapeableRigidBodySection::ReshapeableRigidBodySection(void)
{
}

ReshapeableRigidBodySection::~ReshapeableRigidBodySection(void)
{
}

void ReshapeableRigidBodySection::addShapes(btCompoundShape* compoundShape)
{
	btTransform trans;
	trans.setIdentity();
	for (int i=0; i < m_convexShapes.size(); ++i)
	{
		btVector3 centroid = m_convexCentroids[i];
		trans.setOrigin(centroid);
		btConvexHullShape* convexShape = m_convexShapes[i];
		compoundShape->addChildShape(trans, convexShape);
	}
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

	convexShape->setMargin(0.01f);
	m_convexShapes.push_back(convexShape);
	m_convexCentroids.push_back(centroid);
}

#pragma managed

}