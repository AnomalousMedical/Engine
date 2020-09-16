#include "Stdafx.h"

extern "C" _AnomalousExport void VertexData_delete(Ogre::v1::VertexData* vertexData)
{
	OGRE_DELETE vertexData;
}

extern "C" _AnomalousExport void VertexData_prepareForShadowVolume(Ogre::v1::VertexData* vertexData)
{
	vertexData->prepareForShadowVolume();
}

extern "C" _AnomalousExport void VertexData_reorganizeBuffers(Ogre::v1::VertexData* vertexData, Ogre::v1::VertexDeclaration* newDeclaration)
{
	vertexData->reorganiseBuffers(newDeclaration);
}

extern "C" _AnomalousExport void VertexData_closeGapsInBindings(Ogre::v1::VertexData* vertexData)
{
	vertexData->closeGapsInBindings();
}

extern "C" _AnomalousExport void VertexData_removeUnusedBuffers(Ogre::v1::VertexData* vertexData)
{
	vertexData->removeUnusedBuffers();
}

extern "C" _AnomalousExport void VertexData_convertPackedColor(Ogre::v1::VertexData* vertexData, Ogre::VertexElementType srcType, Ogre::VertexElementType destType)
{
	vertexData->convertPackedColour(srcType, destType);
}

extern "C" _AnomalousExport void VertexData_allocateHardwareAnimationElements(Ogre::v1::VertexData* vertexData, ushort count, bool animateNormals)
{
	vertexData->allocateHardwareAnimationElements(count, animateNormals);
}

extern "C" _AnomalousExport Ogre::v1::VertexBufferBinding* VertexData_getVertexBufferBinding(Ogre::v1::VertexData* vertexData)
{
	return vertexData->vertexBufferBinding;
}

extern "C" _AnomalousExport Ogre::v1::VertexDeclaration* VertexData_getVertexDeclaration(Ogre::v1::VertexData* vertexData)
{
	return vertexData->vertexDeclaration;
}

extern "C" _AnomalousExport void VertexData_setVertexStart(Ogre::v1::VertexData* vertexData, size_t vertexStart)
{
	vertexData->vertexStart = vertexStart;
}

extern "C" _AnomalousExport size_t VertexData_getVertexStart(Ogre::v1::VertexData* vertexData)
{
	return vertexData->vertexStart;
}

extern "C" _AnomalousExport void VertexData_setVertexCount(Ogre::v1::VertexData* vertexData, size_t vertexCount)
{
	vertexData->vertexCount = vertexCount;
}

extern "C" _AnomalousExport size_t VertexData_getVertexCount(Ogre::v1::VertexData* vertexData)
{
	return vertexData->vertexCount;
}

extern "C" _AnomalousExport Ogre::v1::VertexData* VertexData_clone(Ogre::v1::VertexData* vertexData, bool copyData)
{
	return vertexData->clone(copyData);
}