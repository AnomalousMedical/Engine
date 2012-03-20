#include "Stdafx.h"

extern "C" _AnomalousExport void VertexData_prepareForShadowVolume(Ogre::VertexData* vertexData)
{
	vertexData->prepareForShadowVolume();
}

extern "C" _AnomalousExport void VertexData_reorganizeBuffers(Ogre::VertexData* vertexData, Ogre::VertexDeclaration* newDeclaration)
{
	vertexData->reorganiseBuffers(newDeclaration);
}

extern "C" _AnomalousExport void VertexData_closeGapsInBindings(Ogre::VertexData* vertexData)
{
	vertexData->closeGapsInBindings();
}

extern "C" _AnomalousExport void VertexData_removeUnusedBuffers(Ogre::VertexData* vertexData)
{
	vertexData->removeUnusedBuffers();
}

extern "C" _AnomalousExport void VertexData_convertPackedColor(Ogre::VertexData* vertexData, Ogre::VertexElementType srcType, Ogre::VertexElementType destType)
{
	vertexData->convertPackedColour(srcType, destType);
}

extern "C" _AnomalousExport void VertexData_allocateHardwareAnimationElements(Ogre::VertexData* vertexData, ushort count, bool animateNormals)
{
	vertexData->allocateHardwareAnimationElements(count, animateNormals);
}

extern "C" _AnomalousExport Ogre::VertexBufferBinding* VertexData_getVertexBufferBinding(Ogre::VertexData* vertexData)
{
	return vertexData->vertexBufferBinding;
}

extern "C" _AnomalousExport Ogre::VertexDeclaration* VertexData_getVertexDeclaration(Ogre::VertexData* vertexData)
{
	return vertexData->vertexDeclaration;
}

extern "C" _AnomalousExport void VertexData_setVertexStart(Ogre::VertexData* vertexData, size_t vertexStart)
{
	vertexData->vertexStart = vertexStart;
}

extern "C" _AnomalousExport size_t VertexData_getVertexStart(Ogre::VertexData* vertexData)
{
	return vertexData->vertexStart;
}

extern "C" _AnomalousExport void VertexData_setVertexCount(Ogre::VertexData* vertexData, size_t vertexCount)
{
	vertexData->vertexCount = vertexCount;
}

extern "C" _AnomalousExport size_t VertexData_getVertexCount(Ogre::VertexData* vertexData)
{
	return vertexData->vertexCount;
}