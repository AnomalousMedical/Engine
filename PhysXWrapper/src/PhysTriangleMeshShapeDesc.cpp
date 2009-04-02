#include "StdAfx.h"
#include "..\include\PhysTriangleMeshShapeDesc.h"
#include "NxPhysics.h"
#include "PhysTriangleMesh.h"

namespace Physics
{

PhysTriangleMeshShapeDesc::PhysTriangleMeshShapeDesc(System::String^ name)
:triangleShape(new NxTriangleMeshShapeDesc()),
PhysShapeDesc(triangleShape.Get(), name)
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