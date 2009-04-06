#include "StdAfx.h"
#include "..\include\PhysCapsuleShapeDesc.h"
#include "NxPhysics.h"

namespace PhysXWrapper
{

PhysCapsuleShapeDesc::PhysCapsuleShapeDesc()
:capsuleShape(new NxCapsuleShapeDesc()), 
PhysShapeDesc(capsuleShape.Get())
{
}

float PhysCapsuleShapeDesc::Radius::get() 
{
	 return capsuleShape->radius;
}

void PhysCapsuleShapeDesc::Radius::set(float value) 
{
	capsuleShape->radius = value;
}

float PhysCapsuleShapeDesc::Height::get() 
{
	 return capsuleShape->height;
}

void PhysCapsuleShapeDesc::Height::set(float value) 
{
	capsuleShape->height = value;
}

}