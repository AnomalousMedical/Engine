#include "Stdafx.h"

extern "C" _AnomalousExport void StaticImage_setItemResource(MyGUI::StaticImage* staticImage, String value)
{
	staticImage->setItemResource(value);
}

extern "C" _AnomalousExport void StaticImage_setItemGroup(MyGUI::StaticImage* staticImage, String value)
{
	staticImage->setItemGroup(value);
}

extern "C" _AnomalousExport void StaticImage_setItemName(MyGUI::StaticImage* staticImage, String value)
{
	staticImage->setItemName(value);
}