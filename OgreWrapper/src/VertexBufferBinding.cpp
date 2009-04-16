#include "StdAfx.h"
#include "..\include\VertexBufferBinding.h"
#include "HardwareBufferManager.h"
#include "HardwareVertexBuffer.h"

namespace Engine{

namespace Rendering{

VertexBufferBinding::VertexBufferBinding(Ogre::VertexBufferBinding* vertexBufferBinding)
:vertexBufferBinding(vertexBufferBinding)
{
}

VertexBufferBinding::~VertexBufferBinding(void)
{
}

void VertexBufferBinding::setBinding(unsigned short index, HardwareVertexBufferSharedPtr^ buffer)
{
	return vertexBufferBinding->setBinding(index, buffer->Value->getOgreHardwareVertexBufferPtr());
}

void VertexBufferBinding::unsetBinding(unsigned short index)
{
	return vertexBufferBinding->unsetBinding(index);
}

void VertexBufferBinding::unsetAllBindings()
{
	return vertexBufferBinding->unsetAllBindings();
}

HardwareVertexBufferSharedPtr^ VertexBufferBinding::getBuffer(unsigned short index)
{
	return HardwareBufferManager::getInstance()->getObject(vertexBufferBinding->getBuffer(index));
}

bool VertexBufferBinding::isBufferBound(unsigned short index)
{
	return vertexBufferBinding->isBufferBound(index);
}

size_t VertexBufferBinding::getBufferCount()
{
	return vertexBufferBinding->getBufferCount();
}

unsigned short VertexBufferBinding::getNextIndex()
{
	return vertexBufferBinding->getNextIndex();
}

unsigned short VertexBufferBinding::getLastBoundIndex()
{
	return vertexBufferBinding->getLastBoundIndex();
}

bool VertexBufferBinding::hasGaps()
{
	return vertexBufferBinding->hasGaps();
}

void VertexBufferBinding::closeGaps(BindingIndexMap^ indexMap)
{
	indexMap->Clear();
	Ogre::VertexBufferBinding::BindingIndexMap ogreIndexMap;
	vertexBufferBinding->closeGaps(ogreIndexMap);
	for(Ogre::VertexBufferBinding::BindingIndexMap::iterator iter = ogreIndexMap.begin(); iter != ogreIndexMap.end(); ++iter)
	{
		indexMap->Add((*iter).first, (*iter).second);
	}
}

}

}