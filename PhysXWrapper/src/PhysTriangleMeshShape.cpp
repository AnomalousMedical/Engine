#include "StdAfx.h"
#include "..\include\PhysTriangleMeshShape.h"

#include "NxTriangleMeshShape.h"

namespace Engine
{

namespace Physics
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

}