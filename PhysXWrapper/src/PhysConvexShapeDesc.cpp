#include "StdAfx.h"
#include "..\include\PhysConvexShapeDesc.h"
#include "PhysConvexMesh.h"
#include "NxPhysics.h"

namespace PhysXWrapper
{

PhysConvexShapeDesc::PhysConvexShapeDesc(System::String^ name)
:convexShape(new NxConvexShapeDesc()),
PhysShapeDesc(convexShape.Get(), name)
{

}

PhysConvexMesh^ PhysConvexShapeDesc::MeshData::get() 
{
	return mesh;
}

void PhysConvexShapeDesc::MeshData::set(PhysConvexMesh^ value) 
{
	mesh = value;
	convexShape->meshData = mesh->convexMesh;
}

MeshShapeFlag PhysConvexShapeDesc::MeshFlags::get() 
{
	return (MeshShapeFlag)convexShape->meshFlags;
}

void PhysConvexShapeDesc::MeshFlags::set(MeshShapeFlag value) 
{
	convexShape->meshFlags = (NxU32)value;
}

}