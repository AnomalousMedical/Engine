#include "Stdafx.h"

extern "C" __declspec(dllexport) Ogre::PixelBox* PixelBox_Create()
{
	return new Ogre::PixelBox();
}

extern "C" __declspec(dllexport) Ogre::PixelBox* PixelBox_Create1(int left, int top, int right, int bottom, Ogre::PixelFormat pixelFormat)
{
	return new Ogre::PixelBox(Ogre::Box(left, top, right, bottom), pixelFormat);
}

extern "C" __declspec(dllexport) Ogre::PixelBox* PixelBox_Create2(int left, int top, int right, int bottom, Ogre::PixelFormat pixelFormat, void* pixelData)
{
	return new Ogre::PixelBox(Ogre::Box(left, top, right, bottom), pixelFormat, pixelData);
}

extern "C" __declspec(dllexport) Ogre::PixelBox* PixelBox_Create3(int width, int height, int depth, Ogre::PixelFormat pixelFormat)
{
	return new Ogre::PixelBox(width, height, depth, pixelFormat);
}

extern "C" __declspec(dllexport) Ogre::PixelBox* PixelBox_Create4(int width, int height, int depth, Ogre::PixelFormat pixelFormat, void* pixelData)
{
	return new Ogre::PixelBox(width, height, depth, pixelFormat, pixelData);
}

extern "C" __declspec(dllexport) void PixelBox_Delete(Ogre::PixelBox* pixelBox)
{
	delete pixelBox;
}

extern "C" __declspec(dllexport) void PixelBox_setConsecutive(Ogre::PixelBox* pixelBox)
{
	pixelBox->setConsecutive();
}

extern "C" __declspec(dllexport) int PixelBox_getRowSkip(Ogre::PixelBox* pixelBox)
{
	return pixelBox->getRowSkip();
}

extern "C" __declspec(dllexport) int PixelBox_getSliceSkip(Ogre::PixelBox* pixelBox)
{
	return pixelBox->getSliceSkip();
}

extern "C" __declspec(dllexport) bool PixelBox_isConsecutive(Ogre::PixelBox* pixelBox)
{
	return pixelBox->isConsecutive();
}

extern "C" __declspec(dllexport) int PixelBox_getConsecutiveSize(Ogre::PixelBox* pixelBox)
{
	return pixelBox->getConsecutiveSize();
}

extern "C" __declspec(dllexport) int PixelBox_getWidth(Ogre::PixelBox* pixelBox)
{
	return pixelBox->getWidth();
}

extern "C" __declspec(dllexport) int PixelBox_getHeight(Ogre::PixelBox* pixelBox)
{
	return pixelBox->getHeight();
}

extern "C" __declspec(dllexport) int PixelBox_getDepth(Ogre::PixelBox* pixelBox)
{
	return pixelBox->getDepth();
}

extern "C" __declspec(dllexport) void* PixelBox_getData(Ogre::PixelBox* pixelBox)
{
	return pixelBox->data;
}

extern "C" __declspec(dllexport) void PixelBox_setData(Ogre::PixelBox* pixelBox, void* data)
{
	pixelBox->data = data;
}

extern "C" __declspec(dllexport) Ogre::PixelFormat PixelBox_getFormat(Ogre::PixelBox* pixelBox)
{
	return pixelBox->format;
}

extern "C" __declspec(dllexport) void PixelBox_setFormat(Ogre::PixelBox* pixelBox, Ogre::PixelFormat format)
{
	pixelBox->format = format;
}

extern "C" __declspec(dllexport) int PixelBox_getRowPitch(Ogre::PixelBox* pixelBox)
{
	return pixelBox->rowPitch;
}

extern "C" __declspec(dllexport) void PixelBox_setRowPitch(Ogre::PixelBox* pixelBox, int rowPitch)
{
	pixelBox->rowPitch = rowPitch;
}

extern "C" __declspec(dllexport) int PixelBox_getSlicePitch(Ogre::PixelBox* pixelBox)
{
	return pixelBox->slicePitch;
}

extern "C" __declspec(dllexport) void PixelBox_setSlicePitch(Ogre::PixelBox* pixelBox, int slicePitch)
{
	pixelBox->slicePitch = slicePitch;
}