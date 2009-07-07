#include "StdAfx.h"
#include "..\include\Texture.h"

#include "OgreTexture.h"
#include "HardwareBufferManager.h"

namespace OgreWrapper
{

Texture::Texture(const Ogre::TexturePtr& texture)
:Resource(texture.get()),
textureAutoPtr(new Ogre::TexturePtr(texture)),
texture(texture.get())
{

}

Texture::~Texture()
{
	texture = 0;
}

Ogre::Texture* Texture::getTexture()
{
	return texture;
}

HardwarePixelBufferSharedPtr^ Texture::getBuffer()
{
	return HardwareBufferManager::getInstance()->getObject(texture->getBuffer());
}

}