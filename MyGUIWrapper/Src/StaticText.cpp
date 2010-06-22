#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport MyGUI::IntCoord StaticText_getTextRegion(MyGUI::StaticText* staticText)
{
	return staticText->getTextRegion();
}

extern "C" _AnomalousExport ThreeIntHack StaticText_getTextSize(MyGUI::StaticText* staticText)
{
	return staticText->getTextSize();
}

extern "C" _AnomalousExport void StaticText_setFontName(MyGUI::StaticText* staticText, String font)
{
	staticText->setFontName(font);
}

extern "C" _AnomalousExport String StaticText_getFontName(MyGUI::StaticText* staticText)
{
	return staticText->getFontName().c_str();
}

extern "C" _AnomalousExport void StaticText_setFontHeight(MyGUI::StaticText* staticText, int height)
{
	staticText->setFontHeight(height);
}

extern "C" _AnomalousExport int StaticText_getFontHeight(MyGUI::StaticText* staticText)
{
	return staticText->getFontHeight();
}

extern "C" _AnomalousExport void StaticText_setTextAlign(MyGUI::StaticText* staticText, MyGUI::Align::Enum align)
{
	staticText->setTextAlign(align);
}

extern "C" _AnomalousExport MyGUI::Align::Enum StaticText_getTextAlign(MyGUI::StaticText* staticText)
{
	return getAlignEnumVal(staticText->getTextAlign());
}

extern "C" _AnomalousExport void StaticText_setTextColour(MyGUI::StaticText* staticText, Color colour)
{
	staticText->setTextColour(colour.toMyGUI());
}

extern "C" _AnomalousExport Color StaticText_getTextColour(MyGUI::StaticText* staticText)
{
	return staticText->getTextColour();
}

#pragma warning(pop)