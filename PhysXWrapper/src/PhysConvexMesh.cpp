#include "StdAfx.h"
#include "..\include\PhysConvexMesh.h"
#include "NxPhysics.h"
#include "PhysConvexMeshDesc.h"

namespace PhysXWrapper
{

PhysConvexMesh::PhysConvexMesh(NxConvexMesh* convexMesh)
:convexMesh(convexMesh)
{

}

bool PhysConvexMesh::saveToDesc(PhysConvexMeshDesc^ desc)
{
	return convexMesh->saveToDesc(*(desc->meshDesc.Get()));
}

unsigned int PhysConvexMesh::getReferenceCount()
{
	return convexMesh->getReferenceCount();
}

}