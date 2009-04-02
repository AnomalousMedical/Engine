#include "StdAfx.h"
#include "..\include\PhysSphereShape.h"

#include "NxSphereShape.h"
#include "PhysSphereShapeDesc.h"

namespace Engine
{

namespace Physics
{

PhysSphereShape::PhysSphereShape(NxSphereShape* nxSphere)
:PhysShape(nxSphere), nxSphere( nxSphere )
{

}

PhysSphereShape::~PhysSphereShape()
{
	
}

NxSphereShape* PhysSphereShape::getNxSphereShape()
{
	return nxSphere;
}

void PhysSphereShape::setRadius(float radius)
{
	return nxSphere->setRadius(radius);
}

float PhysSphereShape::getRadius()
{
	return nxSphere->getRadius();
}

void PhysSphereShape::saveToDesc(PhysSphereShapeDesc^ desc)
{
	return nxSphere->saveToDesc(*desc->sphereShape.Get());
}

}

}
