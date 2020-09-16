#include "Stdafx.h"

extern "C" _AnomalousExport void IndexData_optimizeVertexCacheTriList(Ogre::v1::IndexData* indexData)
{
	indexData->optimiseVertexCacheTriList();
}

extern "C" _AnomalousExport Ogre::v1::HardwareIndexBuffer* IndexData_getIndexBuffer(Ogre::v1::IndexData* indexData, ProcessWrapperObjectDelegate processIndexBuffer)
{
	processIndexBuffer(indexData->indexBuffer.getPointer(), &indexData->indexBuffer);
	return indexData->indexBuffer.getPointer();
}

extern "C" _AnomalousExport void IndexData_setIndexStart(Ogre::v1::IndexData* indexData, size_t indexStart)
{
	indexData->indexStart = indexStart;
}

extern "C" _AnomalousExport size_t IndexData_getIndexStart(Ogre::v1::IndexData* indexData)
{
	return indexData->indexStart;
}

extern "C" _AnomalousExport void IndexData_setIndexCount(Ogre::v1::IndexData* indexData, size_t indexCount)
{
	indexData->indexCount = indexCount;
}

extern "C" _AnomalousExport size_t IndexData_getIndexCount(Ogre::v1::IndexData* indexData)
{
	return indexData->indexCount;
}