#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport MyGUI::IntCoord StaticText_getTextRegion(MyGUI::TextBox* staticText)
{
	return staticText->getTextRegion();
}

extern "C" _AnomalousExport ThreeIntHack StaticText_getTextSize(MyGUI::TextBox* staticText)
{
	return staticText->getTextSize();
}

extern "C" _AnomalousExport void StaticText_setFontName(MyGUI::TextBox* staticText, String font)
{
	staticText->setFontName(font);
}

extern "C" _AnomalousExport String StaticText_getFontName(MyGUI::TextBox* staticText)
{
	return staticText->getFontName().c_str();
}

extern "C" _AnomalousExport void StaticText_setFontHeight(MyGUI::TextBox* staticText, int height)
{
	staticText->setFontHeight(height);
}

extern "C" _AnomalousExport int StaticText_getFontHeight(MyGUI::TextBox* staticText)
{
	return staticText->getFontHeight();
}

extern "C" _AnomalousExport void StaticText_setTextAlign(MyGUI::TextBox* staticText, MyGUI::Align::Enum align)
{
	staticText->setTextAlign(align);
}

extern "C" _AnomalousExport MyGUI::Align::Enum StaticText_getTextAlign(MyGUI::TextBox* staticText)
{
	return getAlignEnumVal(staticText->getTextAlign());
}

extern "C" _AnomalousExport void StaticText_setTextColour(MyGUI::TextBox* staticText, Color colour)
{
	staticText->setTextColour(colour.toMyGUI());
}

extern "C" _AnomalousExport Color StaticText_getTextColour(MyGUI::TextBox* staticText)
{
	return staticText->getTextColour();
}

#pragma warning(pop)