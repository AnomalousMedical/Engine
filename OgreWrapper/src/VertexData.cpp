#include "StdAfx.h"
#include "..\include\VertexData.h"
#include "HardwareBufferManager.h"
#include "VertexDeclaration.h"

namespace Rendering{

VertexData::VertexData(Ogre::VertexData* vertexData)
:vertexData(vertexData)
{
}

VertexData::~VertexData(void)
{
	if(vertexData != 0)
	{
		HardwareBufferManager::getInstance()->destroyObject(vertexData->vertexBufferBinding);
		HardwareBufferManager::getInstance()->destroyObject(vertexData->vertexDeclaration);
		vertexData = 0;
	}
}

VertexData^ VertexData::clone()
{
	throw gcnew System::NotImplementedException();
}

void VertexData::prepareForShadowVolume()
{
	return vertexData->prepareForShadowVolume();
}

void VertexData::reorganizeBuffers(VertexDeclaration^ newDeclaration, BufferUsageList bufferUsage)
{
	Ogre::BufferUsageList ogreBufferUsage;
	for each(HardwareBuffer::Usage usage in bufferUsage)
	{
		ogreBufferUsage.push_back(static_cast<Ogre::HardwareBuffer::Usage>(usage));
	}
	return vertexData->reorganiseBuffers(newDeclaration->getOgreVertexDeclaration(), ogreBufferUsage);
}

void VertexData::reorganizeBuffers(VertexDeclaration^ newDeclaration)
{
	vertexData->reorganiseBuffers(newDeclaration->getOgreVertexDeclaration());
}

void VertexData::closeGapsInBindings()
{
	return vertexData->closeGapsInBindings();
}

void VertexData::removeUnusedBuffers()
{
	return vertexData->removeUnusedBuffers();
}

void VertexData::convertPackedColor(VertexElementType srcType, VertexElementType destType)
{
	return vertexData->convertPackedColour(static_cast<Ogre::VertexElementType>(srcType), static_cast<Ogre::VertexElementType>(destType));
}

void VertexData::allocateHardwareAnimationElements(unsigned short count)
{
	return vertexData->allocateHardwareAnimationElements(count);
}

VertexDeclaration^ VertexData::vertexDeclaration::get()
{
	return HardwareBufferManager::getInstance()->getObject(vertexData->vertexDeclaration);
}

VertexBufferBinding^ VertexData::vertexBufferBinding::get() 
{
	return HardwareBufferManager::getInstance()->getObject(vertexData->vertexBufferBinding);
}

size_t VertexData::vertexStart::get() 
{
	return vertexData->vertexStart;
}

void VertexData::vertexStart::set(size_t value) 
{
	vertexData->vertexStart = value;
}

size_t VertexData::vertexCount::get() 
{
	return vertexData->vertexCount;
}

void VertexData::vertexCount::set(size_t value) 
{
	vertexData->vertexCount = value;
}

}