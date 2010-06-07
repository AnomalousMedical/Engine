#include "Stdafx.h"

extern "C" _AnomalousExport size_t HardwareVertexBuffer_getVertexSize(Ogre::HardwareVertexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getVertexSize();
}

extern "C" _AnomalousExport size_t HardwareVertexBuffer_getNumVertices(Ogre::HardwareVertexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getNumVertices();
}