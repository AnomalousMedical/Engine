#include "StdAfx.h"
#include "..\include\PhysTriangleMesh.h"
#include "NxPhysics.h"
#include "PhysTriangleMeshDesc.h"

namespace PhysXWrapper
{

PhysTriangleMesh::PhysTriangleMesh(NxTriangleMesh* triangleMesh)
:triangleMesh(triangleMesh)
{

}

bool PhysTriangleMesh::saveToDesc(PhysTriangleMeshDesc^ desc)
{
	return triangleMesh->saveToDesc(*desc->meshDesc.Get());
}

unsigned int PhysTriangleMesh::getReferenceCount()
{
	return triangleMesh->getReferenceCount();
}

}