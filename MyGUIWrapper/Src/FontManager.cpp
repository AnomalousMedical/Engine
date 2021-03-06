#include "Stdafx.h"

extern "C" _AnomalousExport MyGUI::FontManager* FontManager_getInstancePtr()
{
	return MyGUI::FontManager::getInstancePtr();
}

extern "C" _AnomalousExport uint FontManager_measureStringWidth(MyGUI::FontManager* fontManager, String fontName, UStringIn measureString, size_t measureStringLength)
{
	MyGUI::IFont* font = fontManager->getByName(fontName);
	uint length = 0;
	if(font != NULL)
	{
		for(size_t i = 0; i < measureStringLength; ++i)
		{
			MyGUI::GlyphInfo* glyphInfo = font->getGlyphInfo(measureString[i]);
			length += glyphInfo->width;
		}
	}
	return length;
}

extern "C" _AnomalousExport void FontManager_setDefaultFont(MyGUI::FontManager* fontManager, String value)
{
	fontManager->setDefaultFont(value);
}

extern "C" _AnomalousExport String FontManager_getDefaultFont(MyGUI::FontManager* fontManager)
{
	return fontManager->getDefaultFont().c_str();
}