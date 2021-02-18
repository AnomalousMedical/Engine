#include "Stdafx.h"
#include "MyGUI_ResourceTrueTypeFont.h"

struct MyGUITrueTypeFontDesc
{
	float size; // Size of the font, in points (there are 72 points per inch).
	MyGUI::uint resolution; // Resolution of the font, in pixels per inch.
	bool antialias; // Whether or not to anti-alias the font by copying its alpha channel to its luminance channel.
	float tabWidth; // The width of the "Tab" special character, in pixels.
	int offsetHeight; // How far up to nudge text rendered in this font, in pixels. May be negative to nudge text down.
	MyGUI::uint substituteCodePoint; // The code point to use as a substitute for code points that don't exist in the font.
};

extern "C" _AnomalousExport MyGUI::ResourceTrueTypeFont* MyGUIFontLoader_LoadFont(MyGUITrueTypeFontDesc &fontDesc, MyGUI::uint8 * fontBuffer, size_t fontBufferSize)
{
	MyGUI::ResourceTrueTypeFont* font = new MyGUI::ResourceTrueTypeFont();
	font->setSize(fontDesc.size);
	font->setResolution(fontDesc.resolution);
	font->setAntialias(fontDesc.antialias);
	font->setTabWidth(fontDesc.tabWidth);
	font->setOffsetHeight(fontDesc.offsetHeight);
	font->setSubstituteCode(fontDesc.substituteCodePoint);
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

struct FontInfoPassStruct {
	size_t charMapLength;
	size_t glyphInfoLength;
	MyGUI::Char substituteCodePoint;
	MyGUI::GlyphInfo substituteGlyphInfo;
	MyGUI::uint8* textureBuffer;
	size_t textureBufferSize;
	int textureBufferWidth;
	int textureBufferHeight;
};

extern "C" _AnomalousExport FontInfoPassStruct MyGUIFontLoader_GetFontInfo(MyGUI::ResourceTrueTypeFont * font)
{
	FontInfoPassStruct ret;
	ret.charMapLength = font->mCharMap.size();
	ret.glyphInfoLength = font->mGlyphMap.size();
	ret.substituteCodePoint = font->getSubstituteCodePoint();
	ret.substituteGlyphInfo = *font->getSubstituteGlyphInfo();
	ret.textureBuffer = font->textureBuffer;
	ret.textureBufferSize = font->textureBufferSize;
	ret.textureBufferWidth = font->textureBufferWidth;
	ret.textureBufferHeight = font->textureBufferHeight;
	return ret;
}

struct CharMapPassStruct {
	MyGUI::Char key;
	MyGUI::uint value;
};

struct GlyphInfoPassStruct {
	MyGUI::uint key;
	MyGUI::GlyphInfo value;
};

extern "C" _AnomalousExport void MyGUIFontLoader_GetArrayInfo(MyGUI::ResourceTrueTypeFont * font, CharMapPassStruct* charMap, GlyphInfoPassStruct * glyphInfo)
{
	MyGUI::ResourceTrueTypeFont::CharMap::iterator charMapIter = font->mCharMap.begin();
	size_t i = 0;
	while (charMapIter != font->mCharMap.end()) {

		charMap[i].key = charMapIter->first;
		charMap[i].value = charMapIter->second;
		++charMapIter;
		++i;
	}

	MyGUI::ResourceTrueTypeFont::GlyphMap::iterator glyphMapIter = font->mGlyphMap.begin();
	i = 0;
	while (glyphMapIter != font->mGlyphMap.end()) {

		glyphInfo[i].key = glyphMapIter->first;
		glyphInfo[i].value = glyphMapIter->second;
		++glyphMapIter;
		++i;
	}
}

extern "C" _AnomalousExport void MyGUIFontLoader_DestoryFont(MyGUI::ResourceTrueTypeFont* font)
{
	delete font;
}