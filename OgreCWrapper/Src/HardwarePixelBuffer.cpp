#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::RenderTexture* HardwarePixelBuffer_getRenderTarget(Ogre::HardwarePixelBuffer* hardwarePixelBuffer)
{
	return hardwarePixelBuffer->getRenderTarget();
}

extern "C" _AnomalousExport void HardwarePixelBuffer_blit(Ogre::HardwarePixelBuffer* hardwarePixelBuffer, Ogre::HardwarePixelBufferSharedPtr* src, int srcLeft, int srcTop, int srcRight, int srcBottom, int dstLeft, int dstTop, int dstRight, int dstBottom)
{
	hardwarePixelBuffer->blit(*src, Ogre::Image::Box(srcLeft, srcTop, srcRight, srcBottom), Ogre::Image::Box(dstLeft, dstTop, dstRight, dstBottom));
}

extern "C" _AnomalousExport void HardwarePixelBuffer_blitFromMemory(Ogre::HardwarePixelBuffer* hardwarePixelBuffer, Ogre::PixelBox* src, int left, int top, int right, int bottom)
{
	hardwarePixelBuffer->blitFromMemory(*src, Ogre::Image::Box(left, top, right, bottom));
}

extern "C" _AnomalousExport void HardwarePixelBuffer_blitFromMemoryFill(Ogre::HardwarePixelBuffer* hardwarePixelBuffer, Ogre::PixelBox* src)
{
	hardwarePixelBuffer->blitFromMemory(*src);
}

extern "C" _AnomalousExport void HardwarePixelBuffer_blitToMemoryFill(Ogre::HardwarePixelBuffer* hardwarePixelBuffer, Ogre::PixelBox* dst)
{
	hardwarePixelBuffer->blitToMemory(*dst);
}

extern "C" _AnomalousExport void HardwarePixelBuffer_blitToMemory(Ogre::HardwarePixelBuffer* hardwarePixelBuffer, Ogre::PixelBox* dst, int left, int top, int right, int bottom)
{
	hardwarePixelBuffer->blitToMemory(Ogre::Image::Box(left, top, right, bottom), *dst);
}

extern "C" _AnomalousExport void HardwarePixelBuffer_lock(Ogre::HardwarePixelBuffer* hardwarePixelBuffer, int left, int top, int right, int bottom, Ogre::HardwareBuffer::LockOptions options)
{
	hardwarePixelBuffer->lock(Ogre::Box(left, top, right, bottom), options);
}

extern "C" _AnomalousExport const Ogre::PixelBox* HardwarePixelBuffer_getCurrentLock(Ogre::HardwarePixelBuffer* hardwarePixelBuffer)
{
	return &hardwarePixelBuffer->getCurrentLock();
}

extern "C" _AnomalousExport void HardwarePixelBuffer_setOptimizedReadbackEnabled(Ogre::HardwarePixelBuffer* hardwarePixelBuffer, bool enabled)
{
	hardwarePixelBuffer->setOptimizedReadbackEnabled(enabled);
}

/// Returns true if optimized readback is enabled, false if disabled.
extern "C" _AnomalousExport bool HardwarePixelBuffer_isOptimizedReadbackEnabled(Ogre::HardwarePixelBuffer* hardwarePixelBuffer)
{
	return hardwarePixelBuffer->isOptimizedReadbackEnabled();
}

extern "C" _AnomalousExport void HardwarePixelBuffer_blitToStaging(Ogre::HardwarePixelBuffer* hardwarePixelBuffer)
{
	hardwarePixelBuffer->blitToStaging();
}

extern "C" _AnomalousExport void HardwarePixelBuffer_blitStagingToMemory(Ogre::HardwarePixelBuffer* hardwarePixelBuffer, Ogre::PixelBox* dst)
{
	hardwarePixelBuffer->blitStagingToMemory(*dst);
}