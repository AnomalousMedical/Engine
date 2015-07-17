#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::Image* Image_create()
{
	return new Ogre::Image();
}

extern "C" _AnomalousExport Ogre::Image* Image_create1(uint32 uWidth, uint32 uHeight, uint32 depth, Ogre::PixelFormat eFormat, size_t numFaces, uint8 numMipMaps)
{
	Ogre::Image *image = new Ogre::Image();

	size_t bufSize = Ogre::Image::calculateSize(numMipMaps, numFaces, uWidth, uHeight, depth, eFormat);
	uchar* buffer = OGRE_ALLOC_T(uchar, bufSize, Ogre::MEMCATEGORY_GENERAL);

	image->loadDynamicImage(buffer, uWidth, uHeight, depth, eFormat, true, numFaces, numMipMaps);
	return image;
}

extern "C" _AnomalousExport void Image_delete(Ogre::Image* image)
{
	delete image;
}

extern "C" _AnomalousExport void Image_flipAroundY(Ogre::Image* image)
{
	image->flipAroundY();
}

extern "C" _AnomalousExport void Image_flipAroundX(Ogre::Image* image)
{
	image->flipAroundX();
}

extern "C" _AnomalousExport void Image_load(Ogre::Image* image, Ogre::DataStream* stream, String type)
{
	image->load(Ogre::DataStreamPtr(stream), type);
}

extern "C" _AnomalousExport size_t Image_getSize(Ogre::Image* image)
{
	return image->getSize();
}

extern "C" _AnomalousExport uint8 Image_getNumMipmaps(Ogre::Image* image)
{
	return image->getNumMipmaps();
}

extern "C" _AnomalousExport bool Image_hasFlag(Ogre::Image* image, const Ogre::ImageFlags imgFlag)
{
	return image->hasFlag(imgFlag);
}

extern "C" _AnomalousExport uint32 Image_getWidth(Ogre::Image* image)
{
	return image->getWidth();
}

extern "C" _AnomalousExport uint32 Image_getHeight(Ogre::Image* image)
{
	return image->getHeight();
}

extern "C" _AnomalousExport uint32 Image_getDepth(Ogre::Image* image)
{
	return image->getDepth();
}

extern "C" _AnomalousExport size_t Image_getNumFaces(Ogre::Image* image)
{
	return image->getNumFaces();
}

extern "C" _AnomalousExport size_t Image_getRowSpan(Ogre::Image* image)
{
	return image->getRowSpan();
}

extern "C" _AnomalousExport Ogre::PixelFormat Image_getFormat(Ogre::Image* image)
{
	return image->getFormat();
}

extern "C" _AnomalousExport uchar Image_getBPP(Ogre::Image* image)
{
	return image->getBPP();
}

extern "C" _AnomalousExport void Image_save(Ogre::Image* image, String filename)
{
	return image->save(filename);
}

extern "C" _AnomalousExport bool Image_getHasAlpha(Ogre::Image* image)
{
	return image->getHasAlpha();
}

extern "C" _AnomalousExport Ogre::PixelBox* Image_getPixelBox(Ogre::Image* image, size_t face, size_t mipmap)
{
	return new Ogre::PixelBox(image->getPixelBox(face, mipmap));
}

extern "C" _AnomalousExport void Image_resize(Ogre::Image* image, ushort width, ushort height, Ogre::Image::Filter filter)
{
	image->resize(width, height, filter);
}

extern "C" _AnomalousExport void Image_scale(Ogre::PixelBox* src, Ogre::PixelBox* dst, Ogre::Image::Filter filter)
{
	Ogre::Image::scale(*src, *dst, filter);
}

extern "C" _AnomalousExport size_t Image_calculateSize(size_t mipmaps, size_t faces, uint32 width, uint32 height, uint32 depth, Ogre::PixelFormat format)
{
	return Ogre::Image::calculateSize(mipmaps, faces, width, height, depth, format);
}