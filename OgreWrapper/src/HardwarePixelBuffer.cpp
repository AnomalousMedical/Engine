#include "StdAfx.h"
#include "..\include\HardwarePixelBuffer.h"
#include "RenderTexture.h"

namespace OgreWrapper
{

HardwarePixelBuffer::HardwarePixelBuffer(const Ogre::HardwarePixelBufferSharedPtr& pixelBuffer)
:pixelBufferAutoPtr(new Ogre::HardwarePixelBufferSharedPtr(pixelBuffer)),
pixelBuffer(pixelBuffer.get()),
renderTexture(nullptr)
{

}

HardwarePixelBuffer::~HardwarePixelBuffer()
{
	pixelBuffer = 0;
}

Ogre::HardwarePixelBuffer* HardwarePixelBuffer::getHardwarePixelBuffer()
{
	return pixelBuffer;
}

RenderTexture^ HardwarePixelBuffer::getRenderTarget()
{
	if(renderTexture == nullptr)
	{
		renderTexture = gcnew RenderTexture(pixelBuffer->getRenderTarget());
	}
	return renderTexture;
}

}
