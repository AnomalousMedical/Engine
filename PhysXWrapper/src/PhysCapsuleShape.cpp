#include "StdAfx.h"
#include "..\include\PhysCapsuleShape.h"

#include "NxCapsuleShape.h"
#include "PhysCapsuleShapeDesc.h"

namespace PhysXWrapper
{

PhysCapsuleShape::PhysCapsuleShape(NxCapsuleShape* nxCapsule)
:PhysShape(nxCapsule), nxCapsule( nxCapsule )
{

}

PhysCapsuleShape::~PhysCapsuleShape()
{
	
}

NxCapsuleShape* PhysCapsuleShape::getNxCapsuleShape()
{
	return nxCapsule;
}

void PhysCapsuleShape::setDimensions(float radius, float height)
{
	return nxCapsule->setDimensions(radius, height);
}

void PhysCapsuleShape::setRadius(float radius)
{
	return nxCapsule->setRadius(radius);
}

float PhysCapsuleShape::getRadius()
{
	return nxCapsule->getRadius();
}

void PhysCapsuleShape::setHeight(float height)
{
	return nxCapsule->setHeight(height);
}

float PhysCapsuleShape::getHeight()
{
	return nxCapsule->getHeight();
}

void PhysCapsuleShape::saveToDesc(PhysCapsuleShapeDesc^ desc)
{
	return nxCapsule->saveToDesc(*desc->capsuleShape.Get());
}

}