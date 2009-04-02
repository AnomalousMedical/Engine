#include "StdAfx.h"
#include "..\include\PhysSoftBodyMeshDesc.h"
#include "NxPhysics.h"

namespace PhysXWrapper
{

PhysSoftBodyMeshDesc::PhysSoftBodyMeshDesc(void)
:meshDesc(new NxSoftBodyMeshDesc())
{
}

System::UInt32 PhysSoftBodyMeshDesc::NumVertices::get() 
{
	return meshDesc->numVertices;
}

void PhysSoftBodyMeshDesc::NumVertices::set(System::UInt32 value) 
{
	meshDesc->numVertices = value;
}

System::UInt32 PhysSoftBodyMeshDesc::NumTetrahedra::get() 
{
	return meshDesc->numTetrahedra;
}

void PhysSoftBodyMeshDesc::NumTetrahedra::set(System::UInt32 value) 
{
	meshDesc->numTetrahedra = value;
}

System::UInt32 PhysSoftBodyMeshDesc::VertexStrideBytes::get() 
{
	return meshDesc->vertexStrideBytes;
}

void PhysSoftBodyMeshDesc::VertexStrideBytes::set(System::UInt32 value) 
{
	meshDesc->vertexStrideBytes = value;
}

System::UInt32 PhysSoftBodyMeshDesc::TetrahedronStrideBytes::get() 
{
	return meshDesc->tetrahedronStrideBytes;
}

void PhysSoftBodyMeshDesc::TetrahedronStrideBytes::set(System::UInt32 value) 
{
	meshDesc->tetrahedronStrideBytes = value;
}

void* PhysSoftBodyMeshDesc::Vertices::get() 
{
	return (void*)meshDesc->vertices;
}

void PhysSoftBodyMeshDesc::Vertices::set(void* value) 
{
	meshDesc->vertices = value;
}

void* PhysSoftBodyMeshDesc::Tetrahedra::get() 
{
	return (void*)meshDesc->tetrahedra;
}

void PhysSoftBodyMeshDesc::Tetrahedra::set(void* value) 
{
	meshDesc->tetrahedra = value;
}

System::UInt32 PhysSoftBodyMeshDesc::VertexMassStrideBytes::get() 
{
	return meshDesc->vertexMassStrideBytes;
}

void PhysSoftBodyMeshDesc::VertexMassStrideBytes::set(System::UInt32 value) 
{
	meshDesc->vertexMassStrideBytes = value;
}

System::UInt32 PhysSoftBodyMeshDesc::VertexFlagStrideBytes::get() 
{
	return meshDesc->vertexFlagStrideBytes;
}

void PhysSoftBodyMeshDesc::VertexFlagStrideBytes::set(System::UInt32 value) 
{
	meshDesc->vertexFlagStrideBytes = value;
}

void* PhysSoftBodyMeshDesc::VertexMasses::get() 
{
	return (void*)meshDesc->vertexMasses;
}

void PhysSoftBodyMeshDesc::VertexMasses::set(void* value) 
{
	meshDesc->vertexMasses = value;
}

void* PhysSoftBodyMeshDesc::VertexFlags::get() 
{
	return (void*)meshDesc->vertexFlags;
}

void PhysSoftBodyMeshDesc::VertexFlags::set(void* value) 
{
	meshDesc->vertexFlags = value;
}

PhysSoftBodyMeshFlags PhysSoftBodyMeshDesc::Flags::get() 
{
	return static_cast<PhysSoftBodyMeshFlags>(meshDesc->flags);
}

void PhysSoftBodyMeshDesc::Flags::set(PhysSoftBodyMeshFlags value) 
{
	meshDesc->flags = static_cast<NxSoftBodyMeshFlags>(value);
}

}