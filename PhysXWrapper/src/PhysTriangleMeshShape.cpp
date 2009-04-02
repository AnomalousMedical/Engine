#include "StdAfx.h"
#include "..\include\PhysTriangleMeshShape.h"

#include "NxTriangleMeshShape.h"

namespace PhysXWrapper
{

PhysTriangleMeshShape::PhysTriangleMeshShape(NxTriangleMeshShape* nxTriangleMesh)
:PhysShape(nxTriangleMesh), nxTriangleMesh( nxTriangleMesh )
{

}

PhysTriangleMeshShape::~PhysTriangleMeshShape()
{
	
}

NxTriangleMeshShape* PhysTriangleMeshShape::getNxTriangleMeshShape()
{
	return nxTriangleMesh;
}

}