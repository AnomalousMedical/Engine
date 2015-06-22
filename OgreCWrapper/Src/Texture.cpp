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