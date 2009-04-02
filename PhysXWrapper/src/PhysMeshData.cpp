#include "StdAfx.h"
#include "..\include\PhysMeshData.h"

namespace Physics
{

PhysMeshData::PhysMeshData(NxMeshData* meshData)
:meshData(meshData)
{
	meshData->numVerticesPtr = 0;
	meshData->numIndicesPtr = 0;
	meshData->numParentIndicesPtr = 0;
	meshData->dirtyBufferFlagsPtr = 0;
}

PhysMeshData::PhysMeshData(const NxMeshData& meshData)
:autoMeshData(new NxMeshData(meshData))
{
	this->meshData = autoMeshData.Get();
}

PhysMeshData::PhysMeshData()
:autoMeshData(new NxMeshData())
{
	meshData = autoMeshData.Get();
	meshData->numVerticesPtr = 0;
	meshData->numIndicesPtr = 0;
	meshData->numParentIndicesPtr = 0;
	meshData->dirtyBufferFlagsPtr = 0;
}

PhysMeshData::~PhysMeshData()
{
	
}

NxMeshData* PhysMeshData::getNxMeshData()
{
	return meshData;
}

void PhysMeshData::setToDefault()
{
	meshData->setToDefault();
}

bool PhysMeshData::isValid()
{
	return meshData->isValid();
}

void* PhysMeshData::VerticesPosBegin::get() 
{
	return meshData->verticesPosBegin;
}

void PhysMeshData::VerticesPosBegin::set(void* value) 
{
	meshData->verticesPosBegin = value;
}

void* PhysMeshData::VerticesNormalBegin::get() 
{
	return meshData->verticesNormalBegin;
}

void PhysMeshData::VerticesNormalBegin::set(void* value) 
{
	meshData->verticesNormalBegin = value;
}

System::Int32 PhysMeshData::VerticesPosByteStride::get() 
{
	return meshData->verticesPosByteStride;
}

void PhysMeshData::VerticesPosByteStride::set(System::Int32 value) 
{
	meshData->verticesPosByteStride = value;
}

System::Int32 PhysMeshData::VerticesNormalByteStride::get() 
{
	return meshData->verticesNormalByteStride;
}

void PhysMeshData::VerticesNormalByteStride::set(System::Int32 value) 
{
	meshData->verticesNormalByteStride = value;
}

System::UInt32 PhysMeshData::MaxVertices::get() 
{
	return meshData->maxVertices;
}

void PhysMeshData::MaxVertices::set(System::UInt32 value) 
{
	meshData->maxVertices = value;
}

System::UInt32* PhysMeshData::NumVerticesPtr::get() 
{
	return meshData->numVerticesPtr;
}

void PhysMeshData::NumVerticesPtr::set(System::UInt32* value) 
{
	meshData->numVerticesPtr = value;
}

void* PhysMeshData::IndicesBegin::get() 
{
	return meshData->indicesBegin;
}

void PhysMeshData::IndicesBegin::set(void* value) 
{
	meshData->indicesBegin = value;
}

System::Int32 PhysMeshData::IndicesByteStride::get() 
{
	return meshData->indicesByteStride;
}

void PhysMeshData::IndicesByteStride::set(System::Int32 value) 
{
	meshData->indicesByteStride = value;
}

System::UInt32 PhysMeshData::MaxIndices::get() 
{
	return meshData->maxIndices;
}

void PhysMeshData::MaxIndices::set(System::UInt32 value) 
{
	meshData->maxIndices = value;
}

System::UInt32* PhysMeshData::NumIndicesPtr::get() 
{
	return meshData->numIndicesPtr;
}

void PhysMeshData::NumIndicesPtr::set(System::UInt32* value) 
{
	meshData->numIndicesPtr = value;
}

void* PhysMeshData::ParentIndicesBegin::get() 
{
	return meshData->parentIndicesBegin;
}

void PhysMeshData::ParentIndicesBegin::set(void* value) 
{
	meshData->parentIndicesBegin = value;
}

System::Int32 PhysMeshData::ParentIndicesByteStride::get() 
{
	return meshData->parentIndicesByteStride;
}

void PhysMeshData::ParentIndicesByteStride::set(System::Int32 value) 
{
	meshData->parentIndicesByteStride = value;
}

System::UInt32 PhysMeshData::MaxParentIndices::get() 
{
	return meshData->maxParentIndices;
}

void PhysMeshData::MaxParentIndices::set(System::UInt32 value) 
{
	meshData->maxParentIndices = value;
}

System::UInt32* PhysMeshData::NumParentIndicesPtr::get() 
{
	return meshData->numParentIndicesPtr;
}

void PhysMeshData::NumParentIndicesPtr::set(System::UInt32* value) 
{
	meshData->numParentIndicesPtr = value;
}

System::UInt32* PhysMeshData::DirtyBufferFlagsPtr::get() 
{
	return meshData->dirtyBufferFlagsPtr;
}

void PhysMeshData::DirtyBufferFlagsPtr::set(System::UInt32* value) 
{
	meshData->dirtyBufferFlagsPtr = value;
}

PhysMeshDataFlags PhysMeshData::Flags::get() 
{
	return (PhysMeshDataFlags)meshData->flags;
}

void PhysMeshData::Flags::set(PhysMeshDataFlags value) 
{
	meshData->flags = (NxU32)value;
}

}