#include "Stdafx.h"

extern "C" __declspec(dllexport) void VertexBufferBinding_setBinding(Ogre::VertexBufferBinding* vertexBinding, ushort index, Ogre::HardwareVertexBufferSharedPtr* buffer)
{
	vertexBinding->setBinding(index, *buffer);
}

extern "C" __declspec(dllexport) void VertexBufferBinding_unsetBinding(Ogre::VertexBufferBinding* vertexBinding, ushort index)
{
	vertexBinding->unsetBinding(index);
}

extern "C" __declspec(dllexport) void VertexBufferBinding_unsetAllBindings(Ogre::VertexBufferBinding* vertexBinding)
{
	vertexBinding->unsetAllBindings();
}

extern "C" __declspec(dllexport) Ogre::HardwareVertexBuffer* VertexBufferBinding_getBuffer(Ogre::VertexBufferBinding* vertexBinding, ushort index, ProcessWrapperObjectDelegate processCallback)
{
	const Ogre::HardwareVertexBufferSharedPtr& sharedPtr = vertexBinding->getBuffer(index);
	processCallback(sharedPtr.getPointer(), &sharedPtr);
	return sharedPtr.getPointer();
}

extern "C" __declspec(dllexport) bool VertexBufferBinding_isBufferBound(Ogre::VertexBufferBinding* vertexBinding, ushort index)
{
	return vertexBinding->isBufferBound(index);
}

extern "C" __declspec(dllexport) size_t VertexBufferBinding_getBufferCount(Ogre::VertexBufferBinding* vertexBinding)
{
	return vertexBinding->getBufferCount();
}

extern "C" __declspec(dllexport) ushort VertexBufferBinding_getNextIndex(Ogre::VertexBufferBinding* vertexBinding)
{
	return vertexBinding->getNextIndex();
}

extern "C" __declspec(dllexport) ushort VertexBufferBinding_getLastBoundIndex(Ogre::VertexBufferBinding* vertexBinding)
{
	return vertexBinding->getLastBoundIndex();
}

extern "C" __declspec(dllexport) bool VertexBufferBinding_hasGaps(Ogre::VertexBufferBinding* vertexBinding)
{
	return vertexBinding->hasGaps();
}