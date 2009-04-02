#include "StdAfx.h"
#include "..\include\PhysConvexMeshDesc.h"
#include "NxPhysics.h"

namespace Physics
{

PhysConvexMeshDesc::PhysConvexMeshDesc(void)
:meshDesc(new NxConvexMeshDesc())
{
}

PhysConvexMeshDesc::~PhysConvexMeshDesc()
{

}

unsigned int PhysConvexMeshDesc::NumVertices::get() 
{
	return meshDesc->numVertices;
}

void PhysConvexMeshDesc::NumVertices::set(unsigned int value) 
{
	meshDesc->numVertices = value;
}

unsigned int PhysConvexMeshDesc::NumTriangles::get() 
{
	return meshDesc->numTriangles;
}

void PhysConvexMeshDesc::NumTriangles::set(unsigned int value) 
{
	meshDesc->numTriangles = value;
}

unsigned int PhysConvexMeshDesc::PointStrideBytes::get() 
{
	return meshDesc->pointStrideBytes;
}

void PhysConvexMeshDesc::PointStrideBytes::set(unsigned int value) 
{
	meshDesc->pointStrideBytes = value;
}

unsigned int PhysConvexMeshDesc::TriangleStrideBytes::get() 
{
	return meshDesc->triangleStrideBytes;
}

void PhysConvexMeshDesc::TriangleStrideBytes::set(unsigned int value) 
{
	meshDesc->triangleStrideBytes = value;
}

ConvexFlags PhysConvexMeshDesc::Flags::get() 
{
	return (ConvexFlags)meshDesc->flags;
}

void PhysConvexMeshDesc::Flags::set(ConvexFlags value) 
{
	meshDesc->flags = (NxU32)value;
}

void* PhysConvexMeshDesc::Points::get() 
{
	return (void*)meshDesc->points;
}

void PhysConvexMeshDesc::Points::set(void* value) 
{
	meshDesc->points = value;
}

void* PhysConvexMeshDesc::Triangles::get() 
{
	return (void*)meshDesc->triangles;
}

void PhysConvexMeshDesc::Triangles::set(void* value) 
{
	meshDesc->triangles = value;
}

}