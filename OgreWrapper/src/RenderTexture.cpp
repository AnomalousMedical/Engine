#include "StdAfx.h"
#include "..\include\RenderTexture.h"

#include "OgreRenderTexture.h"

namespace OgreWrapper
{

RenderTexture::RenderTexture(Ogre::RenderTexture* renderTexture)
:RenderTarget(renderTexture),
renderTexture( renderTexture )
{

}

RenderTexture::~RenderTexture()
{
	renderTexture = 0;
}

Ogre::RenderTexture* RenderTexture::getRenderTexture()
{
	return renderTexture;
}

}