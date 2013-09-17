#include "Stdafx.h"

extern "C" _AnomalousExport void ImageBox_setItemResource(MyGUI::ImageBox* staticImage, String value)
{
	staticImage->setItemResource(value);
}

extern "C" _AnomalousExport ThreeIntHack ImageBox_getImageSize(MyGUI::ImageBox* staticImage)
{
	return staticImage->getImageSize();
}

extern "C" _AnomalousExport ThreeIntHack ImageBox_getItemGroupSize(MyGUI::ImageBox* staticImage)
{
	return staticImage->getItemGroupSize();
}

extern "C" _AnomalousExport void ImageBox_setItemGroup(MyGUI::ImageBox* staticImage, String value)
{
	staticImage->setItemGroup(value);
}

extern "C" _AnomalousExport void ImageBox_setItemName(MyGUI::ImageBox* staticImage, String value)
{
	staticImage->setItemName(value);
}

extern "C" _AnomalousExport void ImageBox_setImageInfo(MyGUI::ImageBox* staticImage, String _texture, const MyGUI::IntCoord& _coord, IntSize2& _tile)
{
	staticImage->setImageInfo(_texture, _coord, _tile.toIntSize());
}

extern "C" _AnomalousExport void ImageBox_setImageTexture(MyGUI::ImageBox* staticImage, String value)
{
	staticImage->setImageTexture(value);
}

extern "C" _AnomalousExport void ImageBox_setImageCoord(MyGUI::ImageBox* staticImage, const MyGUI::IntCoord &intCoord)
{
	staticImage->setImageCoord(intCoord);
}

extern "C" _AnomalousExport void ImageBox_setImageTile(MyGUI::ImageBox* staticImage, IntSize2& _value)
{
	staticImage->setImageTile(_value.toIntSize());
}

extern "C" _AnomalousExport void ImageBox_deleteAllItems(MyGUI::ImageBox* staticImage)
{
	staticImage->deleteAllItems();
}