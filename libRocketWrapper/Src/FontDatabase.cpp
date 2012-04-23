#include "StdAfx.h"

extern "C" _AnomalousExport bool FontDatabase_LoadFontFace(String file_name)
{
	return Rocket::Core::FontDatabase::LoadFontFace(file_name);
}

extern "C" _AnomalousExport bool FontDatabase_LoadFontFace_Opt(String file_name, String family, Rocket::Core::Font::Style style, Rocket::Core::Font::Weight weight)
{
	return Rocket::Core::FontDatabase::LoadFontFace(file_name, family, style, weight);
}

//static bool FontDatabase_LoadFontFace(const byte* data, int data_length, const String& family, Rocket::Core::Font::Style style, Rocket::Core::Font::Weight weight)
//{
//
//}