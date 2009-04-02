#include "StdAfx.h"
#include "..\include\PhysPlaneShapeDesc.h"
#include "NxPhysics.h"

namespace PhysXWrapper
{

PhysPlaneShapeDesc::PhysPlaneShapeDesc(System::String^ name)
:planeShape(new NxPlaneShapeDesc()),
PhysShapeDesc(planeShape.Get(), name)
{
}

EngineMath::Vector3 PhysPlaneShapeDesc::Normal::get() 
{
	NxVec3 v = planeShape->normal;
	return EngineMath::Vector3(v.x, v.y, v.z);
}

void PhysPlaneShapeDesc::Normal::set(EngineMath::Vector3 value) 
{
	planeShape->normal = NxVec3(value.x, value.y, value.z);
}

float PhysPlaneShapeDesc::D::get() 
{
	return planeShape->d;
}

void PhysPlaneShapeDesc::D::set(float value) 
{
	planeShape->d = value;
}

}