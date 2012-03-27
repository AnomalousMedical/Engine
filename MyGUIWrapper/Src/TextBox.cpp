#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport MyGUI::IntCoord TextBox_getTextRegion(MyGUI::TextBox* staticText)
{
	return staticText->getTextRegion();
}

extern "C" _AnomalousExport ThreeIntHack TextBox_getTextSize(MyGUI::TextBox* staticText)
{
	return staticText->getTextSize();
}

extern "C" _AnomalousExport void TextBox_setFontName(MyGUI::TextBox* staticText, String font)
{
	staticText->setFontName(font);
}

extern "C" _AnomalousExport String TextBox_getFontName(MyGUI::TextBox* staticText)
{
	return staticText->getFontName().c_str();
}

extern "C" _AnomalousExport void TextBox_setFontHeight(MyGUI::TextBox* staticText, int height)
{
	staticText->setFontHeight(height);
}

extern "C" _AnomalousExport int TextBox_getFontHeight(MyGUI::TextBox* staticText)
{
	return staticText->getFontHeight();
}

extern "C" _AnomalousExport void TextBox_setTextAlign(MyGUI::TextBox* staticText, MyGUI::Align::Enum align)
{
	staticText->setTextAlign(align);
}

extern "C" _AnomalousExport MyGUI::Align::Enum TextBox_getTextAlign(MyGUI::TextBox* staticText)
{
	return getAlignEnumVal(staticText->getTextAlign());
}

extern "C" _AnomalousExport void TextBox_setTextColour(MyGUI::TextBox* staticText, Color colour)
{
	staticText->setTextColour(colour.toMyGUI());
}

extern "C" _AnomalousExport Color TextBox_getTextColour(MyGUI::TextBox* staticText)
{
	return staticText->getTextColour();
}

extern "C" _AnomalousExport void TextBox_setCaption(MyGUI::TextBox* staticText, UStringIn value)
{
	staticText->setCaption(value);
}

extern "C" _AnomalousExport const MyGUI::UString::code_point* TextBox_getCaption(MyGUI::TextBox* staticText)
{
	return staticText->getCaption().c_str();
}

extern "C" _AnomalousExport void TextBox_setCaptionWithReplacing(MyGUI::TextBox* staticText, String value)
{
	staticText->setCaptionWithReplacing(value);
}

#pragma warning(pop)