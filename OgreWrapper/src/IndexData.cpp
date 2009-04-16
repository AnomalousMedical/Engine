#include "StdAfx.h"
#include "..\include\IndexData.h"
#include "HardwareBufferManager.h"

namespace Rendering{

IndexData::IndexData(Ogre::IndexData* indexData)
:indexData(indexData)
{
}

IndexData::~IndexData(void)
{
	indexData = 0;
}

IndexData^ IndexData::clone()
{
	throw gcnew System::NotImplementedException();
}

void IndexData::optimizeVertexCacheTriList()
{
	indexData->optimiseVertexCacheTriList();
}

HardwareIndexBufferSharedPtr^ IndexData::IndexBuffer::get() 
{
	return HardwareBufferManager::getInstance()->getObject(indexData->indexBuffer);
}

size_t IndexData::IndexStart::get() 
{
	return indexData->indexStart;
}

void IndexData::IndexStart::set(size_t value) 
{
	indexData->indexStart = value;
}

size_t IndexData::IndexCount::get() 
{
	return indexData->indexCount;
}

void IndexData::IndexCount::set(size_t value) 
{
	indexData->indexCount = value;
}

}