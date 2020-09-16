#include "Stdafx.h"

extern "C" _AnomalousExport size_t HardwareVertexBuffer_getVertexSize(Ogre::v1::HardwareVertexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getVertexSize();
}

extern "C" _AnomalousExport size_t HardwareVertexBuffer_getNumVertices(Ogre::v1::HardwareVertexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getNumVertices();
}