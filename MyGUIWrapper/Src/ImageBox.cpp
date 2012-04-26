#include "Stdafx.h"

extern "C" _AnomalousExport void ImageBox_setItemResource(MyGUI::ImageBox* staticImage, String value)
{
	staticImage->setItemResource(value);
}

extern "C" _AnomalousExport void ImageBox_setItemGroup(MyGUI::ImageBox* staticImage, String value)
{
	staticImage->setItemGroup(value);
}

extern "C" _AnomalousExport void ImageBox_setItemName(MyGUI::ImageBox* staticImage, String value)
{
	staticImage->setItemName(value);
}

extern "C" _AnomalousExport void ImageBox_setImageTexture(MyGUI::ImageBox* staticImage, String value)
{
	staticImage->setImageTexture(value);
}

extern "C" _AnomalousExport void ImageBox_setImageCoord(MyGUI::ImageBox* staticImage, const MyGUI::IntCoord &intCoord)
{
	staticImage->setImageCoord(intCoord);
}

extern "C" _AnomalousExport void ImageBox_setImageTile(MyGUI::ImageBox* staticImage, Size2& _value)
{
	staticImage->setImageTile(_value.toIntSize());
}