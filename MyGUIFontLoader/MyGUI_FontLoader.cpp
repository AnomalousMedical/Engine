#include "Stdafx.h"
#include "MyGUI_ResourceTrueTypeFont.h"

extern "C" _AnomalousExport MyGUI::ResourceTrueTypeFont* MyGUIFontLoader_LoadFont(MyGUI::uint8 * fontBuffer, size_t fontBufferSize)
{
	MyGUI::ResourceTrueTypeFont* font = new MyGUI::ResourceTrueTypeFont();
	font->setSize(16);
	font->setResolution(50);
	font->setAntialias(false);
	font->setTabWidth(8);
	font->setOffsetHeight(0);
	font->addCodePointRange(33, 126);
	font->addCodePointRange(161, 255);
	font->addCodePointRange(1025, 1105);
	font->addCodePointRange(8470, 8470);
	font->removeCodePointRange(128, 128);
	font->removeCodePointRange(1026, 1039);
	font->removeCodePointRange(1104, 1104);

	font->initialise(fontBuffer, fontBufferSize);

	return font;
}

extern "C" _AnomalousExport void MyGUIFontLoader_DestoryFont(MyGUI::ResourceTrueTypeFont* font)
{
	delete font;
}