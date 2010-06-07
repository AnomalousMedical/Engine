#include "Stdafx.h"

extern "C" _AnomalousExport void VertexBufferBinding_setBinding(Ogre::VertexBufferBinding* vertexBinding, ushort index, Ogre::HardwareVertexBufferSharedPtr* buffer)
{
	vertexBinding->setBinding(index, *buffer);
}

extern "C" _AnomalousExport void VertexBufferBinding_unsetBinding(Ogre::VertexBufferBinding* vertexBinding, ushort index)
{
	vertexBinding->unsetBinding(index);
}

extern "C" _AnomalousExport void VertexBufferBinding_unsetAllBindings(Ogre::VertexBufferBinding* vertexBinding)
{
	vertexBinding->unsetAllBindings();
}

extern "C" _AnomalousExport Ogre::HardwareVertexBuffer* VertexBufferBinding_getBuffer(Ogre::VertexBufferBinding* vertexBinding, ushort index, ProcessWrapperObjectDelegate processCallback)
{
	const Ogre::HardwareVertexBufferSharedPtr& sharedPtr = vertexBinding->getBuffer(index);
	processCallback(sharedPtr.getPointer(), &sharedPtr);
	return sharedPtr.getPointer();
}

extern "C" _AnomalousExport bool VertexBufferBinding_isBufferBound(Ogre::VertexBufferBinding* vertexBinding, ushort index)
{
	return vertexBinding->isBufferBound(index);
}

extern "C" _AnomalousExport size_t VertexBufferBinding_getBufferCount(Ogre::VertexBufferBinding* vertexBinding)
{
	return vertexBinding->getBufferCount();
}

extern "C" _AnomalousExport ushort VertexBufferBinding_getNextIndex(Ogre::VertexBufferBinding* vertexBinding)
{
	return vertexBinding->getNextIndex();
}

extern "C" _AnomalousExport ushort VertexBufferBinding_getLastBoundIndex(Ogre::VertexBufferBinding* vertexBinding)
{
	return vertexBinding->getLastBoundIndex();
}

extern "C" _AnomalousExport bool VertexBufferBinding_hasGaps(Ogre::VertexBufferBinding* vertexBinding)
{
	return vertexBinding->hasGaps();
}