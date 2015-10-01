#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::HardwarePixelBuffer* Texture_getBuffer(Ogre::Texture* texture, size_t face, size_t mipmap, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::HardwarePixelBufferSharedPtr& pixelBufPtr = texture->getBuffer(face, mipmap);
	processWrapper(pixelBufPtr.getPointer(), &pixelBufPtr);
	return pixelBufPtr.getPointer();
}
						    
extern "C" _AnomalousExport Ogre::TextureType Texture_getTextureType(Ogre::Texture* texture)
{
	return texture->getTextureType();
}
						    
extern "C" _AnomalousExport Ogre::uint32 Texture_getHeight(Ogre::Texture* texture)
{
	return texture->getHeight();
}
						    
extern "C" _AnomalousExport Ogre::uint32 Texture_getWidth(Ogre::Texture* texture)
{
	return texture->getWidth();
}
						    
extern "C" _AnomalousExport Ogre::uint32 Texture_getDepth(Ogre::Texture* texture)
{
	return texture->getDepth();
}

extern "C" _AnomalousExport Ogre::uint8 Texture_getNumMipmaps(Ogre::Texture* texture)
{
	return texture->getNumMipmaps();
}
						    
extern "C" _AnomalousExport Ogre::PixelFormat Texture_getFormat(Ogre::Texture* texture)
{
	return texture->getFormat();
}

extern "C" _AnomalousExport bool Texture_getMipmapsHardwareGenerated(Ogre::Texture* texture)
{
	return texture->getMipmapsHardwareGenerated();
}

extern "C" _AnomalousExport bool Texture_getAllowMipmapGeneration(Ogre::Texture* texture)
{
	return texture->getAllowMipmapGeneration();
}

/** Set the height of the texture; can only do this before load();
*/
extern "C" _AnomalousExport void Texture_setAllowMipmapGeneration(Ogre::Texture* texture, bool v)
{
	texture->setAllowMipmapGeneration(v);
}

extern "C" _AnomalousExport int Texture_getUsage(Ogre::Texture* texture)
{
	return texture->getUsage();
}

extern "C" _AnomalousExport void Texture_loadImage(Ogre::Texture* texture, Ogre::Image* image)
{
	return texture->loadImage(*image);
}

extern "C" _AnomalousExport void Texture_blitFromImage(Ogre::Texture* texture, Ogre::Image* image)
{
	int width = texture->getWidth();
	int height = texture->getHeight();
	int depth = texture->getDepth();
	int numSlices = texture->getNumMipmaps() + 1;
	int imageIndex = image->getNumMipmaps() + 1 - numSlices;

	for (int slice = 0; slice < numSlices; ++slice)
	{
		Ogre::PixelBox srcBox = image->getPixelBox(0, imageIndex++);
		Ogre::Box dstBox = Ogre::Box(0, 0, 0, width, height, depth);

		Ogre::HardwarePixelBufferSharedPtr hardwarePixelBuffer = texture->getBuffer(0, slice);

		const Ogre::PixelBox &locked = hardwarePixelBuffer->lock(dstBox, Ogre::HardwarePixelBuffer::HBL_DISCARD);

		Ogre::PixelUtil::bulkPixelConversion(srcBox, locked);

		hardwarePixelBuffer->unlock();

		width >>= 1;
		height >>= 1;
	}
}