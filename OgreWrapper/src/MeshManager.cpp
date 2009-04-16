#include "StdAfx.h"
#include "..\include\MeshManager.h"
#include "Ogre.h"
#include "Mesh.h"
#include "MarshalUtils.h"
#include "Mesh.h"

namespace Rendering{

MeshManager::MeshManager()
:meshManager(Ogre::MeshManager::getSingletonPtr())
{
}

MeshManager^ MeshManager::getInstance()
{
	return instance;
}

MeshPtr^ MeshManager::getObject(const Ogre::MeshPtr& ogrePtr)
{
	return meshPtrs.getObject(ogrePtr);
}

MeshManager::~MeshManager()
{
	meshPtrs.clearObjects();
}

MeshPtr^ MeshManager::prepare(System::String^ filename, System::String^ group)
{
	Ogre::MeshPtr ogrePtr = static_cast<Ogre::MeshPtr>(meshManager->prepare(MarshalUtils::convertString(filename), MarshalUtils::convertString(group)));
	if(ogrePtr.isNull())
	{
		return nullptr;
	}
	return getObject(ogrePtr);
}

MeshPtr^ MeshManager::prepare(System::String^ filename, System::String^ group, HardwareBuffer::Usage vertexBufferUsage, HardwareBuffer::Usage indexBufferUsage)
{
	Ogre::MeshPtr ogrePtr = static_cast<Ogre::MeshPtr>(meshManager->prepare(MarshalUtils::convertString(filename), MarshalUtils::convertString(group), static_cast<Ogre::HardwareBuffer::Usage>(vertexBufferUsage), static_cast<Ogre::HardwareBuffer::Usage>(indexBufferUsage)));
	if(ogrePtr.isNull())
	{
		return nullptr;
	}
	return getObject(ogrePtr);
}

MeshPtr^ MeshManager::prepare(System::String^ filename, System::String^ group, HardwareBuffer::Usage vertexBufferUsage, HardwareBuffer::Usage indexBufferUsage, bool vertexBufferShadowed, bool indexBufferShadowed)
{
	Ogre::MeshPtr ogrePtr = static_cast<Ogre::MeshPtr>(meshManager->prepare(MarshalUtils::convertString(filename), MarshalUtils::convertString(group), static_cast<Ogre::HardwareBuffer::Usage>(vertexBufferUsage), static_cast<Ogre::HardwareBuffer::Usage>(indexBufferUsage), vertexBufferShadowed, indexBufferShadowed));
	if(ogrePtr.isNull())
	{
		return nullptr;
	}
	return getObject(ogrePtr);
}

MeshPtr^ MeshManager::load(System::String^ filename, System::String^ group)
{
	Ogre::MeshPtr ogrePtr = static_cast<Ogre::MeshPtr>(meshManager->load(MarshalUtils::convertString(filename), MarshalUtils::convertString(group)));
	if(ogrePtr.isNull())
	{
		return nullptr;
	}
	return getObject(ogrePtr);
}

MeshPtr^ MeshManager::load(System::String^ filename, System::String^ group, HardwareBuffer::Usage vertexBufferUsage, HardwareBuffer::Usage indexBufferUsage)
{
	Ogre::MeshPtr ogrePtr = static_cast<Ogre::MeshPtr>(meshManager->load(MarshalUtils::convertString(filename), MarshalUtils::convertString(group), static_cast<Ogre::HardwareBuffer::Usage>(vertexBufferUsage), static_cast<Ogre::HardwareBuffer::Usage>(indexBufferUsage)));
	if(ogrePtr.isNull())
	{
		return nullptr;
	}
	return getObject(ogrePtr);
}

MeshPtr^ MeshManager::load(System::String^ filename, System::String^ group, HardwareBuffer::Usage vertexBufferUsage, HardwareBuffer::Usage indexBufferUsage, bool vertexBufferShadowed, bool indexBufferShadowed)
{
	Ogre::MeshPtr ogrePtr = static_cast<Ogre::MeshPtr>(meshManager->load(MarshalUtils::convertString(filename), MarshalUtils::convertString(group), static_cast<Ogre::HardwareBuffer::Usage>(vertexBufferUsage), static_cast<Ogre::HardwareBuffer::Usage>(indexBufferUsage), vertexBufferShadowed, indexBufferShadowed));
	if(ogrePtr.isNull())
	{
		return nullptr;
	}
	return getObject(ogrePtr);
}

MeshPtr^ MeshManager::getByName(System::String^ name)
{
	Ogre::MeshPtr ogrePtr = static_cast<Ogre::MeshPtr>(meshManager->getByName(MarshalUtils::convertString(name)));
	if(ogrePtr.isNull())
	{
		return nullptr;
	}
	return getObject(ogrePtr);
}

MeshPtr^ MeshManager::getByHandle(unsigned long handle)
{
	Ogre::MeshPtr ogrePtr = static_cast<Ogre::MeshPtr>(meshManager->getByHandle(handle));
	if(ogrePtr.isNull())
	{
		return nullptr;
	}
	return getObject(ogrePtr);
}

void MeshManager::setPrepareAllMeshesForShadowVolumes(bool enable)
{
	return meshManager->setPrepareAllMeshesForShadowVolumes(enable);
}

bool MeshManager::getPrepareAllMeshesForShadowVolumes()
{
	return meshManager->getPrepareAllMeshesForShadowVolumes();
}

float MeshManager::getBoundsPaddingFactor()
{
	return meshManager->getBoundsPaddingFactor();
}

void MeshManager::setBoundsPaddingFactor(float paddingFactor)
{
	return meshManager->setBoundsPaddingFactor(paddingFactor);
}

void MeshManager::setMemoryBudget(size_t bytes)
{
	return meshManager->setMemoryBudget(bytes);
}

size_t MeshManager::getMemoryBudget()
{
	return meshManager->getMemoryBudget();
}

size_t MeshManager::getMemoryUsage()
{
	return meshManager->getMemoryUsage();
}

void MeshManager::unload(System::String^ name)
{
	return meshManager->unload(MarshalUtils::convertString(name));
}

void MeshManager::unload(unsigned long handle)
{
	return meshManager->unload(handle);
}

void MeshManager::unloadAll()
{
	return meshManager->unloadAll();
}

void MeshManager::unloadAll(bool reloadableOnly)
{
	return meshManager->unloadAll(reloadableOnly);
}

void MeshManager::reloadAll()
{
	return meshManager->reloadAll();
}

void MeshManager::reloadAll(bool reloadableOnly)
{
	return meshManager->reloadAll(reloadableOnly);
}

void MeshManager::unloadUnreferencedResources()
{
	return meshManager->unloadUnreferencedResources();
}

void MeshManager::unloadUnreferencedResources(bool reloadableOnly)
{
	return meshManager->unloadUnreferencedResources(reloadableOnly);
}

void MeshManager::reloadUnreferencedResources()
{
	return meshManager->reloadUnreferencedResources();
}

void MeshManager::reloadUnreferencedResources(bool reloadableOnly)
{
	return meshManager->reloadUnreferencedResources(reloadableOnly);
}

void MeshManager::remove(MeshPtr^ r)
{
	Ogre::ResourcePtr ptr = r->Value->getOgreMeshPtr();
	return meshManager->remove(ptr);
}

void MeshManager::remove(System::String^ name)
{
	return meshManager->remove(MarshalUtils::convertString(name));
}

void MeshManager::remove(unsigned long handle)
{
	return meshManager->remove(handle);
}

void MeshManager::removeAll()
{
	return meshManager->removeAll();
}

bool MeshManager::resourceExists(System::String^ name)
{
	return meshManager->resourceExists(MarshalUtils::convertString(name));
}

bool MeshManager::resourceExists(unsigned long handle)
{
	return meshManager->resourceExists(handle);
}

void MeshManager::setVerbose(bool v)
{
	return meshManager->setVerbose(v);
}

bool MeshManager::getVerbose()
{
	return meshManager->getVerbose();
}

}