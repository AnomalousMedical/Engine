#include "StdAfx.h"
#include "..\include\PhysTriangleMeshShapeDesc.h"
#include "NxPhysics.h"
#include "PhysTriangleMesh.h"

namespace PhysXWrapper
{

PhysTriangleMeshShapeDesc::PhysTriangleMeshShapeDesc()
:triangleShape(new NxTriangleMeshShapeDesc()),
PhysShapeDesc(triangleShape.Get())
{
}

PhysTriangleMesh^ PhysTriangleMeshShapeDesc::MeshData::get() 
{
	return triangleMesh;
}

void PhysTriangleMeshShapeDesc::MeshData::set(PhysTriangleMesh^ value) 
{
	triangleMesh = value;
	triangleShape->meshData = triangleMesh->triangleMesh;
}

MeshFlags PhysTriangleMeshShapeDesc::Flags::get() 
{
	return (MeshFlags)triangleShape->meshFlags;
}

void PhysTriangleMeshShapeDesc::Flags::set(MeshFlags value) 
{
	triangleShape->meshFlags = (NxU32)triangleShape->meshFlags;
}

}