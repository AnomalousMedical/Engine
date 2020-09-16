#include "Stdafx.h"

extern "C" _AnomalousExport void VertexBufferBinding_setBinding(Ogre::v1::VertexBufferBinding* vertexBinding, ushort index, Ogre::v1::HardwareVertexBufferSharedPtr* buffer)
{
	vertexBinding->setBinding(index, *buffer);
}

extern "C" _AnomalousExport void VertexBufferBinding_unsetBinding(Ogre::v1::VertexBufferBinding* vertexBinding, ushort index)
{
	vertexBinding->unsetBinding(index);
}

extern "C" _AnomalousExport void VertexBufferBinding_unsetAllBindings(Ogre::v1::VertexBufferBinding* vertexBinding)
{
	vertexBinding->unsetAllBindings();
}

extern "C" _AnomalousExport Ogre::v1::HardwareVertexBuffer* VertexBufferBinding_getBuffer(Ogre::v1::VertexBufferBinding* vertexBinding, ushort index, ProcessWrapperObjectDelegate processCallback)
{
	const Ogre::v1::HardwareVertexBufferSharedPtr& sharedPtr = vertexBinding->getBuffer(index);
	processCallback(sharedPtr.getPointer(), &sharedPtr);
	return sharedPtr.getPointer();
}

extern "C" _AnomalousExport bool VertexBufferBinding_isBufferBound(Ogre::v1::VertexBufferBinding* vertexBinding, ushort index)
{
	return vertexBinding->isBufferBound(index);
}

extern "C" _AnomalousExport size_t VertexBufferBinding_getBufferCount(Ogre::v1::VertexBufferBinding* vertexBinding)
{
	return vertexBinding->getBufferCount();
}

extern "C" _AnomalousExport ushort VertexBufferBinding_getNextIndex(Ogre::v1::VertexBufferBinding* vertexBinding)
{
	return vertexBinding->getNextIndex();
}

extern "C" _AnomalousExport ushort VertexBufferBinding_getLastBoundIndex(Ogre::v1::VertexBufferBinding* vertexBinding)
{
	return vertexBinding->getLastBoundIndex();
}

extern "C" _AnomalousExport bool VertexBufferBinding_hasGaps(Ogre::v1::VertexBufferBinding* vertexBinding)
{
	return vertexBinding->hasGaps();
}