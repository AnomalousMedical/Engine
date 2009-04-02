#include "StdAfx.h"
#include "..\include\PhysSphereShapeDesc.h"
#include "NxPhysics.h"

namespace Engine
{

namespace Physics
{

PhysSphereShapeDesc::PhysSphereShapeDesc(System::String^ name)
:sphereShape(new NxSphereShapeDesc()),
PhysShapeDesc(sphereShape.Get(), name)
{
}

float PhysSphereShapeDesc::Radius::get() 
{
	return sphereShape->radius;
}

void PhysSphereShapeDesc::Radius::set(float value) 
{
	sphereShape->radius = value;
}

}

}