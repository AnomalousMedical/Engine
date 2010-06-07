#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::RenderTexture* HardwarePixelBuffer_getRenderTarget(Ogre::HardwarePixelBuffer* hardwarePixelBuffer)
{
	return hardwarePixelBuffer->getRenderTarget();
}