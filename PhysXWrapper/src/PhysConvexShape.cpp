#include "StdAfx.h"
#include "..\include\PhysConvexShape.h"

#include "NxConvexShape.h"

namespace Physics
{

PhysConvexShape::PhysConvexShape(NxConvexShape* nxConvex)
:PhysShape(nxConvex), nxConvex( nxConvex )
{

}

PhysConvexShape::~PhysConvexShape()
{
	
}

NxConvexShape* PhysConvexShape::getNxConvexShape()
{
	return nxConvex;
}

}