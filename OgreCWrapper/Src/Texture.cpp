#include "Stdafx.h"

extern "C" __declspec(dllexport) Ogre::HardwarePixelBuffer* Texture_getBuffer(Ogre::Texture* texture, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::HardwarePixelBufferSharedPtr& pixelBufPtr = texture->getBuffer();
	processWrapper(pixelBufPtr.getPointer(), &pixelBufPtr);
	return pixelBufPtr.getPointer();
}