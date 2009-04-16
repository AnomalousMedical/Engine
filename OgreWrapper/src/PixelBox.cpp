#include "StdAfx.h"
#include "..\include\PixelBox.h"

#include "OgrePixelFormat.h"

namespace Rendering
{

PixelBox::PixelBox(Ogre::PixelBox* ogrePixel)
:ogrePixel( ogrePixel )
{

}

PixelBox::~PixelBox()
{
	ogrePixel = 0;
}

Ogre::PixelBox* PixelBox::getPixelBox()
{
	return ogrePixel;
}

PixelBox::PixelBox()
:ogrePixelAuto(new Ogre::PixelBox()),
ogrePixel(ogrePixelAuto.Get())
{

}

PixelBox::PixelBox(size_t left, size_t top, size_t right, size_t bottom, PixelFormat pixelFormat)
:ogrePixelAuto(new Ogre::PixelBox(Ogre::Box(left, top, right, bottom), static_cast<Ogre::PixelFormat>(pixelFormat))),
ogrePixel(ogrePixelAuto.Get())
{

}

PixelBox::PixelBox(size_t left, size_t top, size_t right, size_t bottom, PixelFormat pixelFormat, void* pixelData)
:ogrePixelAuto(new Ogre::PixelBox(Ogre::Box(left, top, right, bottom), 
			   static_cast<Ogre::PixelFormat>(pixelFormat), pixelData)),
ogrePixel(ogrePixelAuto.Get())
{

}

PixelBox::PixelBox(size_t width, size_t height, size_t depth, PixelFormat pixelFormat)
:ogrePixelAuto(new Ogre::PixelBox(width, height, depth, static_cast<Ogre::PixelFormat>(pixelFormat))),
ogrePixel(ogrePixelAuto.Get())
{

}

PixelBox::PixelBox(size_t width, size_t height, size_t depth, PixelFormat pixelFormat, void* pixelData)
:ogrePixelAuto(new Ogre::PixelBox(width, height, depth, static_cast<Ogre::PixelFormat>(pixelFormat), pixelData)),
ogrePixel(ogrePixelAuto.Get())
{

}

void PixelBox::setConsecutive()
{
	return ogrePixel->setConsecutive();
}

size_t PixelBox::getRowSkip()
{
	return ogrePixel->getRowSkip();
}

size_t PixelBox::getSliceSkip()
{
	return ogrePixel->getSliceSkip();
}

bool PixelBox::isConsecutive()
{
	return ogrePixel->isConsecutive();
}

size_t PixelBox::getConsecutiveSize()
{
	return ogrePixel->getConsecutiveSize();
}

size_t PixelBox::getWidth()
{
	return ogrePixel->getWidth();
}

size_t PixelBox::getHeight()
{
	return ogrePixel->getHeight();
}

size_t PixelBox::getDepth()
{
	return ogrePixel->getDepth();
}

void* PixelBox::Data::get() 
{
	return ogrePixel->data;
}

void PixelBox::Data::set(void* value) 
{
	ogrePixel->data = value;
}

PixelFormat PixelBox::Format::get() 
{
	return (PixelFormat)ogrePixel->format;
}

void PixelBox::Format::set(PixelFormat value) 
{
	ogrePixel->format = (Ogre::PixelFormat)value;
}

size_t PixelBox::RowPitch::get() 
{
	return ogrePixel->rowPitch;
}

void PixelBox::RowPitch::set(size_t value) 
{
	ogrePixel->rowPitch = value;
}

size_t PixelBox::SlicePitch::get() 
{
	return ogrePixel->slicePitch;
}

void PixelBox::SlicePitch::set(size_t value) 
{
	ogrePixel->slicePitch = value;
}

}