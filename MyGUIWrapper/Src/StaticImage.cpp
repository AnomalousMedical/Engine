#include "Stdafx.h"

extern "C" _AnomalousExport void StaticImage_setItemResource(MyGUI::ImageBox* staticImage, String value)
{
	staticImage->setItemResource(value);
}

extern "C" _AnomalousExport void StaticImage_setItemGroup(MyGUI::ImageBox* staticImage, String value)
{
	staticImage->setItemGroup(value);
}

extern "C" _AnomalousExport void StaticImage_setItemName(MyGUI::ImageBox* staticImage, String value)
{
	staticImage->setItemName(value);
}