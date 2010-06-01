#include "Stdafx.h"

extern "C" __declspec(dllexport) void VertexData_prepareForShadowVolume(Ogre::VertexData* vertexData)
{
	vertexData->prepareForShadowVolume();
}

extern "C" __declspec(dllexport) void VertexData_reorganizeBuffers(Ogre::VertexData* vertexData, Ogre::VertexDeclaration* newDeclaration)
{
	vertexData->reorganiseBuffers(newDeclaration);
}

extern "C" __declspec(dllexport) void VertexData_closeGapsInBindings(Ogre::VertexData* vertexData)
{
	vertexData->closeGapsInBindings();
}

extern "C" __declspec(dllexport) void VertexData_removeUnusedBuffers(Ogre::VertexData* vertexData)
{
	vertexData->removeUnusedBuffers();
}

extern "C" __declspec(dllexport) void VertexData_convertPackedColor(Ogre::VertexData* vertexData, Ogre::VertexElementType srcType, Ogre::VertexElementType destType)
{
	vertexData->convertPackedColour(srcType, destType);
}

extern "C" __declspec(dllexport) void VertexData_allocateHardwareAnimationElements(Ogre::VertexData* vertexData, ushort count)
{
	vertexData->allocateHardwareAnimationElements(count);
}

extern "C" __declspec(dllexport) Ogre::VertexBufferBinding* VertexData_getVertexBufferBinding(Ogre::VertexData* vertexData)
{
	return vertexData->vertexBufferBinding;
}

extern "C" __declspec(dllexport) Ogre::VertexDeclaration* VertexData_getVertexDeclaration(Ogre::VertexData* vertexData)
{
	return vertexData->vertexDeclaration;
}

extern "C" __declspec(dllexport) void VertexData_setVertexStart(Ogre::VertexData* vertexData, size_t vertexStart)
{
	vertexData->vertexStart = vertexStart;
}

extern "C" __declspec(dllexport) size_t VertexData_getVertexStart(Ogre::VertexData* vertexData)
{
	return vertexData->vertexStart;
}

extern "C" __declspec(dllexport) void VertexData_setVertexCount(Ogre::VertexData* vertexData, size_t vertexCount)
{
	vertexData->vertexCount = vertexCount;
}

extern "C" __declspec(dllexport) size_t VertexData_getVertexCount(Ogre::VertexData* vertexData)
{
	return vertexData->vertexCount;
}