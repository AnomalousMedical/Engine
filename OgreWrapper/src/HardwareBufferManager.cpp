#include "StdAfx.h"
#include "..\include\HardwareBufferManager.h"
#include "HardwareVertexBuffer.h"
#include "HardwareIndexBuffer.h"
#include "VertexDeclaration.h"
#include "OgreHardwareBufferManager.h"
#include "VertexBufferBinding.h"

namespace OgreWrapper
{

HardwareBufferManager::HardwareBufferManager()
:hbManager(Ogre::HardwareBufferManager::getSingletonPtr())
{

}

HardwareBufferManager::~HardwareBufferManager()
{
	vertexBuffers.clearObjects();
	indexBuffers.clearObjects();
	vertexDeclarations.clearObjects();
	vertexBufferBindings.clearObjects();
}

HardwareVertexBufferSharedPtr^ HardwareBufferManager::getObject(const Ogre::HardwareVertexBufferSharedPtr& vertexBuffer)
{
	return vertexBuffers.getObject(vertexBuffer);
}

HardwareIndexBufferSharedPtr^ HardwareBufferManager::getObject(const Ogre::HardwareIndexBufferSharedPtr& indexBuffer)
{
	return indexBuffers.getObject(indexBuffer);
}

HardwarePixelBufferSharedPtr^ HardwareBufferManager::getObject(const Ogre::HardwarePixelBufferSharedPtr& pixelBuffer)
{
	return pixelBuffers.getObject(pixelBuffer);
}

HardwareBufferManager^ HardwareBufferManager::getInstance()
{
	return instance;
}

VertexDeclaration^ HardwareBufferManager::getObject(Ogre::VertexDeclaration* ogreObject)
{
	if(ogreObject == NULL)
	{
		return nullptr;
	}
	return vertexDeclarations.getObject(ogreObject);
}

void HardwareBufferManager::destroyObject(Ogre::VertexDeclaration* ogreObject)
{
	vertexDeclarations.destroyObject(ogreObject);
}

VertexBufferBinding^ HardwareBufferManager::getObject(Ogre::VertexBufferBinding* ogreObject)
{
	if(ogreObject == NULL)
	{
		return nullptr;
	}
	return vertexBufferBindings.getObject(ogreObject);
}

void HardwareBufferManager::destroyObject(Ogre::VertexBufferBinding* ogreObject)
{
	vertexBufferBindings.destroyObject(ogreObject);
}

HardwareVertexBufferSharedPtr^ HardwareBufferManager::createVertexBuffer(size_t vertexSize, size_t numVerts, HardwareBuffer::Usage usage)
{
	return getObject(hbManager->createVertexBuffer(vertexSize, numVerts, static_cast<Ogre::HardwareBuffer::Usage>(usage)));
}

HardwareVertexBufferSharedPtr^ HardwareBufferManager::createVertexBuffer(size_t vertexSize, size_t numVerts, HardwareBuffer::Usage usage, bool useShadowBuffer)
{
	return getObject(hbManager->createVertexBuffer(vertexSize, numVerts, static_cast<Ogre::HardwareBuffer::Usage>(usage), useShadowBuffer));
}

HardwareIndexBufferSharedPtr^ HardwareBufferManager::createIndexBuffer(HardwareIndexBuffer::IndexType itype, size_t numIndexes, HardwareBuffer::Usage usage)
{
	return getObject(hbManager->createIndexBuffer(static_cast<Ogre::HardwareIndexBuffer::IndexType>(itype), numIndexes, static_cast<Ogre::HardwareBuffer::Usage>(usage)));
}

HardwareIndexBufferSharedPtr^ HardwareBufferManager::createIndexBuffer(HardwareIndexBuffer::IndexType itype, size_t numIndexes, HardwareBuffer::Usage usage, bool useShadowBuffer)
{
	return getObject(hbManager->createIndexBuffer(static_cast<Ogre::HardwareIndexBuffer::IndexType>(itype), numIndexes, static_cast<Ogre::HardwareBuffer::Usage>(usage), useShadowBuffer));
}

VertexDeclaration^ HardwareBufferManager::createVertexDeclaration()
{
	return getObject(hbManager->createVertexDeclaration());
}

void HardwareBufferManager::destroyVertexDeclaration(VertexDeclaration^ decl)
{
	Ogre::VertexDeclaration* ogreDecl = decl->getOgreVertexDeclaration();
	vertexDeclarations.destroyObject(ogreDecl);
	hbManager->destroyVertexDeclaration(ogreDecl);
}

VertexBufferBinding^ HardwareBufferManager::createVertexBufferBinding()
{
	return getObject(hbManager->createVertexBufferBinding());
}

void HardwareBufferManager::destroyVertexBufferBinding(VertexBufferBinding^ binding)
{
	Ogre::VertexBufferBinding* ogreBinding = binding->getVertexBufferBinding();
	vertexBufferBindings.destroyObject(ogreBinding);
	hbManager->destroyVertexBufferBinding(ogreBinding);
}

}