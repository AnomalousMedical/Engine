#pragma once

#include "ConvexDecomposition.h"

namespace BulletPlugin
{

#pragma unmanaged

class ReshapeableRigidBodySection : public ConvexDecomposition::ConvexDecompInterface
{
private:
	btAlignedObjectArray<btConvexHullShape*> m_convexShapes;
	btAlignedObjectArray<btVector3> m_convexCentroids;

public:
	ReshapeableRigidBodySection(void);

	virtual ~ReshapeableRigidBodySection(void);

	void addShapes(btCompoundShape* compoundShape);

	virtual void ConvexDecompResult(ConvexDecomposition::ConvexResult &result);
};

#pragma managed

}