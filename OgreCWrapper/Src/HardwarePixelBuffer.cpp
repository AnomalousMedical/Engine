#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::RenderTexture* HardwarePixelBuffer_getRenderTarget(Ogre::HardwarePixelBuffer* hardwarePixelBuffer)
{
	return hardwarePixelBuffer->getRenderTarget();
}

extern "C" _AnomalousExport void HardwarePixelBuffer_blitFromMemory(Ogre::HardwarePixelBuffer* hardwarePixelBuffer, Ogre::PixelBox *src, int left, int top, int right, int bottom)
{
	hardwarePixelBuffer->blitFromMemory(*src, Ogre::Image::Box(left, top, right, bottom));
}

extern "C" _AnomalousExport void HardwarePixelBuffer_blitFromMemoryFill(Ogre::HardwarePixelBuffer* hardwarePixelBuffer, Ogre::PixelBox *src)
{
	hardwarePixelBuffer->blitFromMemory(*src);
}

extern "C" _AnomalousExport void HardwarePixelBuffer_blitToMemoryFill(Ogre::HardwarePixelBuffer* hardwarePixelBuffer, Ogre::PixelBox *dst)
{
	hardwarePixelBuffer->blitToMemory(*dst);
}