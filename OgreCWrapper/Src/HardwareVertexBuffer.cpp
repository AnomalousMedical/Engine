#include "Stdafx.h"

extern "C" __declspec(dllexport) size_t HardwareVertexBuffer_getVertexSize(Ogre::HardwareVertexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getVertexSize();
}

extern "C" __declspec(dllexport) size_t HardwareVertexBuffer_getNumVertices(Ogre::HardwareVertexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getNumVertices();
}