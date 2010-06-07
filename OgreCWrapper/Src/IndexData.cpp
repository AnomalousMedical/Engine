#include "Stdafx.h"

extern "C" _AnomalousExport void IndexData_optimizeVertexCacheTriList(Ogre::IndexData* indexData)
{
	indexData->optimiseVertexCacheTriList();
}

extern "C" _AnomalousExport Ogre::HardwareIndexBuffer* IndexData_getIndexBuffer(Ogre::IndexData* indexData, ProcessWrapperObjectDelegate processIndexBuffer)
{
	processIndexBuffer(indexData->indexBuffer.getPointer(), &indexData->indexBuffer);
	return indexData->indexBuffer.getPointer();
}

extern "C" _AnomalousExport void IndexData_setIndexStart(Ogre::IndexData* indexData, size_t indexStart)
{
	indexData->indexStart = indexStart;
}

extern "C" _AnomalousExport size_t IndexData_getIndexStart(Ogre::IndexData* indexData)
{
	return indexData->indexStart;
}

extern "C" _AnomalousExport void IndexData_setIndexCount(Ogre::IndexData* indexData, size_t indexCount)
{
	indexData->indexCount = indexCount;
}

extern "C" _AnomalousExport size_t IndexData_getIndexCount(Ogre::IndexData* indexData)
{
	return indexData->indexCount;
}