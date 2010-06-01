#include "Stdafx.h"

extern "C" __declspec(dllexport) void IndexData_optimizeVertexCacheTriList(Ogre::IndexData* indexData)
{
	indexData->optimiseVertexCacheTriList();
}

extern "C" __declspec(dllexport) Ogre::HardwareIndexBuffer* IndexData_getIndexBuffer(Ogre::IndexData* indexData, ProcessWrapperObjectDelegate processIndexBuffer)
{
	processIndexBuffer(indexData->indexBuffer.getPointer(), &indexData->indexBuffer);
	return indexData->indexBuffer.getPointer();
}

extern "C" __declspec(dllexport) void IndexData_setIndexStart(Ogre::IndexData* indexData, size_t indexStart)
{
	indexData->indexStart = indexStart;
}

extern "C" __declspec(dllexport) size_t IndexData_getIndexStart(Ogre::IndexData* indexData)
{
	return indexData->indexStart;
}

extern "C" __declspec(dllexport) void IndexData_setIndexCount(Ogre::IndexData* indexData, size_t indexCount)
{
	indexData->indexCount = indexCount;
}

extern "C" __declspec(dllexport) size_t IndexData_getIndexCount(Ogre::IndexData* indexData)
{
	return indexData->indexCount;
}