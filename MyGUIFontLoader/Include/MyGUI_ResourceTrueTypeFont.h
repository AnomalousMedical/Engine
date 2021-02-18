#pragma once
/*
 * This source file is part of MyGUI. For the latest info, see http://mygui.info/
 * Distributed under the MIT License
 * (See accompanying file COPYING.MIT or copy at http://opensource.org/licenses/MIT)
 */

#include "MyGUI_Platform.h"
#include "MyGUI_FontData.h"
#include "MyGUI_Types.h"
//#include "MyGUI_ITexture.h"
//#include "MyGUI_IFont.h"

#include <string>
#include <vector>
#include <map>


#include <ft2build.h>
#include FT_FREETYPE_H

namespace MyGUI
{

	struct MYGUI_EXPORT PixelFormat
	{
		enum Enum
		{
			Unknow,
			L8, // 1 byte pixel format, 1 byte luminance
			L8A8, // 2 byte pixel format, 1 byte luminance, 1 byte alpha
			R8G8B8, // 24-bit pixel format, 8 bits for red, green and blue.
			R8G8B8A8 // 32-bit pixel format, 8 bits for red, green, blue and alpha.
		};
	};

	class MYGUI_EXPORT ResourceTrueTypeFont //:
		//public IFont,
		//public ITextureInvalidateListener
	{
		//MYGUI_RTTI_DERIVED(ResourceTrueTypeFont)

	public:
		ResourceTrueTypeFont();
		virtual ~ResourceTrueTypeFont();

		//virtual void deserialization(xml::ElementPtr _node, Version _version);

		// Returns the glyph info for the specified code point, or the glyph info for a substitute glyph if the code point does not
		// exist in this font. Returns nullptr if there is a problem with the font.
		virtual GlyphInfo* getGlyphInfo(Char _id);

		//virtual ITexture* getTextureFont();

		// the resulting height when generated in pixels
		virtual int getDefaultHeight();

		// update texture after render device lost event
		//virtual void textureInvalidate(ITexture* _texture);

		// Returns a collection of code-point ranges that are supported by this font. Each range is specified as [first, second];
		// for example, a range containing a single code point will have the same value for both first and second.
		std::vector<std::pair<Char, Char> > getCodePointRanges() const;

		// Returns the code point that is used as a substitute for code points that don't exist in the font. The default substitute
		// code point is FontCodeType::NotDefined, but it can be customized in the font definition file.
		Char getSubstituteCodePoint() const;

		GlyphInfo* getSubstituteGlyphInfo() const;

		// creating a resource based on current values
		void initialise(uint8* _fontBuffer, size_t fontBufferSize);

		void setSize(float _value);
		void setResolution(uint _value);
		void setHinting(const std::string& _value);
		void setAntialias(bool _value);
		void setTabWidth(float _value);
		void setOffsetHeight(int _value);
		void setSubstituteCode(int _value);
		void setDistance(int _value);

		void addCodePointRange(Char _first, Char _second);
		void removeCodePointRange(Char _first, Char _second);

#ifdef MYGUI_USE_FREETYPE
	private:
		enum Hinting
		{
			HintingUseNative,
			HintingForceAuto,
			HintingDisableAuto,
			HintingDisableAll
		};

		void addCodePoint(Char _codePoint);
		void removeCodePoint(Char _codePoint);

		void clearCodePoints();

		// The following variables are set directly from values specified by the user.
		float mSize; // Size of the font, in points (there are 72 points per inch).
		uint mResolution; // Resolution of the font, in pixels per inch.
		Hinting mHinting; // What type of hinting to use when rendering the font.
		bool mAntialias; // Whether or not to anti-alias the font by copying its alpha channel to its luminance channel.
		float mSpaceWidth; // The width of a "Space" character, in pixels. If zero, the default width is used.
		int mGlyphSpacing; // How far apart the glyphs are placed from each other in the font texture, in pixels.
		float mTabWidth; // The width of the "Tab" special character, in pixels.
		int mOffsetHeight; // How far up to nudge text rendered in this font, in pixels. May be negative to nudge text down.
		Char mSubstituteCodePoint; // The code point to use as a substitute for code points that don't exist in the font.

		// The following variables are calculated automatically.
		int mDefaultHeight; // The nominal height of the font in pixels.
		GlyphInfo* mSubstituteGlyphInfo; // The glyph info to use as a substitute for code points that don't exist in the font.

		// The following constants used to be mutable, but they no longer need to be. Do not modify their values!
		static const int mDefaultGlyphSpacing; // How far apart the glyphs are placed from each other in the font texture, in pixels.
		static const float mDefaultTabWidth; // Default "Tab" width, used only when tab width is no specified.
		static const float mSelectedWidth; // The width of the "Selected" and "SelectedBack" special characters, in pixels.
		static const float mCursorWidth; // The width of the "Cursor" special character, in pixels.

	public:
		// A map of code points to glyph indices.
		typedef std::map<Char, FT_UInt> CharMap;

		// A map of glyph indices to glyph info objects.
		typedef std::map<FT_UInt, GlyphInfo> GlyphMap;

	private:
		// A map of glyph heights to the set of paired glyph indices and glyph info objects that are of that height.
		typedef std::map<FT_Pos, std::map<FT_UInt, GlyphInfo*> > GlyphHeightMap;

		template<bool LAMode, bool Antialias>
		void initialiseFreeType(uint8* _fontBuffer, size_t fontBufferSize);

		// Loads the font face as specified by mSource, mSize, and mResolution. Automatically adjusts code-point ranges according
		// to the capabilities of the font face.
		// Returns a handle to the FreeType face object for the face, or nullptr if the face could not be loaded.
		// Keeps the font file loaded in memory and stores its location in _fontBuffer. The caller is responsible for freeing this
		// buffer when it is done using the face by calling delete[] on the buffer after calling FT_Done_Face() on the face itself.
		FT_Face loadFace(const FT_Library& _ftLibrary, uint8* _fontBuffer, size_t fontBufferSize);

		// Wraps the current texture coordinates _texX and _texY to the beginning of the next line if the specified glyph width
		// doesn't fit at the end of the current line. Automatically takes the glyph spacing into account.
		void autoWrapGlyphPos(int _glyphWidth, int _texWidth, int _lineHeight, int& _texX, int& _texY);

		// Creates a GlyphInfo object using the specified information.
		GlyphInfo createFaceGlyphInfo(Char _codePoint, int _fontAscent, FT_GlyphSlot _glyph);

		// Creates a glyph with the specified glyph index and assigns it to the specified code point.
		// Automatically updates _glyphHeightMap, mCharMap, and mGlyphMap with data from the new glyph..
		int createGlyph(FT_UInt _glyphIndex, const GlyphInfo& _glyphInfo, GlyphHeightMap& _glyphHeightMap);

		// Creates a glyph with the specified index from the specified font face and assigns it to the specified code point.
		// Automatically updates _glyphHeightMap with data from the newly created glyph.
		int createFaceGlyph(FT_UInt _glyphIndex, Char _codePoint, int _fontAscent, const FT_Face& _ftFace, FT_Int32 _ftLoadFlags, GlyphHeightMap& _glyphHeightMap);

		// Renders all of the glyphs in _glyphHeightMap into the specified texture buffer using data from the specified font face.
		template<bool LAMode, bool Antialias>
		void renderGlyphs(const GlyphHeightMap& _glyphHeightMap, const FT_Library& _ftLibrary, const FT_Face& _ftFace, FT_Int32 _ftLoadFlags, uint8* _texBuffer, int _texWidth, int _texHeight);

		// Renders the glyph described by the specified glyph info according to the specified parameters.
		// Supports two types of rendering, depending on the value of UseBuffer: Texture block transfer and rectangular color fill.
		// The _luminance0 value is used for even-numbered columns (from zero), while _luminance1 is used for odd-numbered ones.
		template<bool LAMode, bool UseBuffer, bool Antialias>
		void renderGlyph(GlyphInfo& _info, uint8 _luminance0, uint8 _luminance1, uint8 _alpha, int _lineHeight, uint8* _texBuffer, int _texWidth, int _texHeight, int& _texX, int& _texY, uint8* _glyphBuffer = nullptr);

public: //Make these available so they can be copied quickly.
		CharMap mCharMap; // A map of code points to glyph indices.
		GlyphMap mGlyphMap; // A map of glyph indices to glyph info objects.

		//The buffer with the created character map
		uint8* textureBuffer;
		size_t textureBufferSize;
		int textureBufferWidth;
		int textureBufferHeight;

#endif // MYGUI_USE_FREETYPE

	};

} // namespace MyGUI

