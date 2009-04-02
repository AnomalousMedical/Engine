#include "StdAfx.h"
#include "..\include\PhysPlaneShape.h"

#include "NxPlaneShape.h"
#include "MathUtil.h"
#include "PhysPlaneShapeDesc.h"

namespace Engine
{

namespace Physics
{

PhysPlaneShape::PhysPlaneShape(NxPlaneShape* nxPlane)
:PhysShape(nxPlane), nxPlane( nxPlane )
{

}

PhysPlaneShape::~PhysPlaneShape()
{
	
}

NxPlaneShape* PhysPlaneShape::getNxPlaneShape()
{
	return nxPlane;
}

void PhysPlaneShape::setPlane(EngineMath::Vector3 normal, float d)
{
	return nxPlane->setPlane(MathUtil::copyVector3(normal), d);
}

void PhysPlaneShape::setPlane(EngineMath::Vector3% normal, float d)
{
	return nxPlane->setPlane(MathUtil::copyVector3(normal), d);
}

void PhysPlaneShape::saveToDesc(PhysPlaneShapeDesc^ desc)
{
	return nxPlane->saveToDesc(*desc->planeShape.Get());
}

}

}