#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::PixelBox* PixelBox_Create()
{
	return new Ogre::PixelBox();
}

extern "C" _AnomalousExport Ogre::PixelBox* PixelBox_Create1(int left, int top, int right, int bottom, Ogre::PixelFormat pixelFormat)
{
	return new Ogre::PixelBox(Ogre::Box(left, top, right, bottom), pixelFormat);
}

extern "C" _AnomalousExport Ogre::PixelBox* PixelBox_Create2(int left, int top, int right, int bottom, Ogre::PixelFormat pixelFormat, void* pixelData)
{
	return new Ogre::PixelBox(Ogre::Box(left, top, right, bottom), pixelFormat, pixelData);
}

extern "C" _AnomalousExport Ogre::PixelBox* PixelBox_Create3(int width, int height, int depth, Ogre::PixelFormat pixelFormat)
{
	return new Ogre::PixelBox(width, height, depth, pixelFormat);
}

extern "C" _AnomalousExport Ogre::PixelBox* PixelBox_Create4(int width, int height, int depth, Ogre::PixelFormat pixelFormat, void* pixelData)
{
	return new Ogre::PixelBox(width, height, depth, pixelFormat, pixelData);
}

extern "C" _AnomalousExport void PixelBox_Delete(Ogre::PixelBox* pixelBox)
{
	delete pixelBox;
}

extern "C" _AnomalousExport void PixelBox_setConsecutive(Ogre::PixelBox* pixelBox)
{
	pixelBox->setConsecutive();
}

extern "C" _AnomalousExport int PixelBox_getRowSkip(Ogre::PixelBox* pixelBox)
{
	return pixelBox->getRowSkip();
}

extern "C" _AnomalousExport int PixelBox_getSliceSkip(Ogre::PixelBox* pixelBox)
{
	return pixelBox->getSliceSkip();
}

extern "C" _AnomalousExport bool PixelBox_isConsecutive(Ogre::PixelBox* pixelBox)
{
	return pixelBox->isConsecutive();
}

extern "C" _AnomalousExport int PixelBox_getConsecutiveSize(Ogre::PixelBox* pixelBox)
{
	return pixelBox->getConsecutiveSize();
}

extern "C" _AnomalousExport int PixelBox_getWidth(Ogre::PixelBox* pixelBox)
{
	return pixelBox->getWidth();
}

extern "C" _AnomalousExport int PixelBox_getHeight(Ogre::PixelBox* pixelBox)
{
	return pixelBox->getHeight();
}

extern "C" _AnomalousExport int PixelBox_getDepth(Ogre::PixelBox* pixelBox)
{
	return pixelBox->getDepth();
}

extern "C" _AnomalousExport void* PixelBox_getData(Ogre::PixelBox* pixelBox)
{
	return pixelBox->data;
}

extern "C" _AnomalousExport void PixelBox_setData(Ogre::PixelBox* pixelBox, void* data)
{
	pixelBox->data = data;
}

extern "C" _AnomalousExport Ogre::PixelFormat PixelBox_getFormat(Ogre::PixelBox* pixelBox)
{
	return pixelBox->format;
}

extern "C" _AnomalousExport void PixelBox_setFormat(Ogre::PixelBox* pixelBox, Ogre::PixelFormat format)
{
	pixelBox->format = format;
}

extern "C" _AnomalousExport int PixelBox_getRowPitch(Ogre::PixelBox* pixelBox)
{
	return pixelBox->rowPitch;
}

extern "C" _AnomalousExport void PixelBox_setRowPitch(Ogre::PixelBox* pixelBox, int rowPitch)
{
	pixelBox->rowPitch = rowPitch;
}

extern "C" _AnomalousExport int PixelBox_getSlicePitch(Ogre::PixelBox* pixelBox)
{
	return pixelBox->slicePitch;
}

extern "C" _AnomalousExport void PixelBox_setSlicePitch(Ogre::PixelBox* pixelBox, int slicePitch)
{
	pixelBox->slicePitch = slicePitch;
}

extern "C" _AnomalousExport uint32 PixelBox_getLeft(Ogre::PixelBox* pixelBox)
{
	return pixelBox->left;
}

extern "C" _AnomalousExport void PixelBox_setLeft(Ogre::PixelBox* pixelBox, uint32 value)
{
	pixelBox->left = value;
}

extern "C" _AnomalousExport uint32 PixelBox_getTop(Ogre::PixelBox* pixelBox)
{
	return pixelBox->top;
}

extern "C" _AnomalousExport void PixelBox_setTop(Ogre::PixelBox* pixelBox, uint32 value)
{
	pixelBox->top = value;
}

extern "C" _AnomalousExport uint32 PixelBox_getRight(Ogre::PixelBox* pixelBox)
{
	return pixelBox->right;
}

extern "C" _AnomalousExport void PixelBox_setRight(Ogre::PixelBox* pixelBox, uint32 value)
{
	pixelBox->right = value;
}

extern "C" _AnomalousExport uint32 PixelBox_getBottom(Ogre::PixelBox* pixelBox)
{
	return pixelBox->bottom;
}

extern "C" _AnomalousExport void PixelBox_setBottom(Ogre::PixelBox* pixelBox, uint32 value)
{
	pixelBox->bottom = value;
}

extern "C" _AnomalousExport void PixelUtil_bulkPixelConversion(Ogre::PixelBox* src, Ogre::PixelBox* dst)
{
	Ogre::PixelUtil::bulkPixelConversion(*src, *dst);
}