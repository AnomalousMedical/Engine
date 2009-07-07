#include "StdAfx.h"
#include "..\include\TextureManager.h"

#include "OgreTextureManager.h"
#include "MarshalUtils.h"

namespace OgreWrapper
{

TextureManager::TextureManager()
:textureManager( Ogre::TextureManager::getSingletonPtr() )
{

}

TextureManager::~TextureManager()
{
	
}

Ogre::TextureManager* TextureManager::getTextureManager()
{
	return textureManager;
}

TextureManager^ TextureManager::getInstance()
{
	if(instance == nullptr)
	{
		instance = gcnew TextureManager();
	}
	return instance;
}

TexturePtr^ TextureManager::getObject(const Ogre::TexturePtr& ogrePtr)
{
	return texturePtrCollection.getObject(ogrePtr);
}

TexturePtr^ TextureManager::createManual(System::String^ name, System::String^ group, TextureType texType, unsigned int width, unsigned int height, unsigned int depth, int num_mips, PixelFormat format, TextureUsage usage, bool hwGammaCorrection, unsigned int fsaa)
{
	return getObject(textureManager->createManual(MarshalUtils::convertString(name), MarshalUtils::convertString(group), static_cast<Ogre::TextureType>(texType), width, height, depth, num_mips, static_cast<Ogre::PixelFormat>(format), static_cast<Ogre::TextureUsage>(usage), 0, hwGammaCorrection, fsaa));
}

}