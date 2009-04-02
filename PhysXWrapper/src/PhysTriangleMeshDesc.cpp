#include "StdAfx.h"
#include "..\include\PhysTriangleMeshDesc.h"

#include "NxPhysics.h"

namespace Physics
{

PhysTriangleMeshDesc::PhysTriangleMeshDesc(void)
:meshDesc(new NxTriangleMeshDesc())
{

}

unsigned int PhysTriangleMeshDesc::NumVertices::get() 
{
	return meshDesc->numVertices;
}

void PhysTriangleMeshDesc::NumVertices::set(unsigned int value) 
{
	meshDesc->numVertices = value;
}

unsigned int PhysTriangleMeshDesc::NumTriangles::get() 
{
	return meshDesc->numTriangles;
}

void PhysTriangleMeshDesc::NumTriangles::set(unsigned int value) 
{
	meshDesc->numTriangles = value;
}

unsigned int PhysTriangleMeshDesc::PointStrideBytes::get() 
{
	return meshDesc->pointStrideBytes;
}

void PhysTriangleMeshDesc::PointStrideBytes::set(unsigned int value) 
{
	meshDesc->pointStrideBytes = value;
}

unsigned int PhysTriangleMeshDesc::TriangleStrideBytes::get() 
{
	return meshDesc->triangleStrideBytes;
}

void PhysTriangleMeshDesc::TriangleStrideBytes::set(unsigned int value) 
{
	meshDesc->triangleStrideBytes = value;
}

void* PhysTriangleMeshDesc::Points::get() 
{
	return (void*)meshDesc->points;
}

void PhysTriangleMeshDesc::Points::set(void* value) 
{
	meshDesc->points = value;
}

void* PhysTriangleMeshDesc::Triangles::get() 
{
	return (void*)meshDesc->triangles;
}

void PhysTriangleMeshDesc::Triangles::set(void* value) 
{
	meshDesc->triangles = value;
}

MeshFlags PhysTriangleMeshDesc::Flags::get() 
{
	return (MeshFlags)meshDesc->flags;
}

void PhysTriangleMeshDesc::Flags::set(MeshFlags value) 
{
	meshDesc->flags = (NxU32)value;
}

unsigned int PhysTriangleMeshDesc::MaterialIndexStride::get() 
{
	return meshDesc->materialIndexStride;
}

void PhysTriangleMeshDesc::MaterialIndexStride::set(unsigned int value) 
{
	meshDesc->materialIndexStride = value;
}

void* PhysTriangleMeshDesc::MaterialIndices::get() 
{
	return (void*)meshDesc->materialIndices;
}

void PhysTriangleMeshDesc::MaterialIndices::set(void* value) 
{
	meshDesc->materialIndices = value;
}

float PhysTriangleMeshDesc::ConvexEdgeThreshold::get() 
{
	return meshDesc->convexEdgeThreshold;
}

void PhysTriangleMeshDesc::ConvexEdgeThreshold::set(float value) 
{
	meshDesc->convexEdgeThreshold = value;
}

}