#include "StdAfx.h"

//static TextureResource* Fetch (const String &source, const String &source_directory)

extern "C" _AnomalousExport void TextureDatabase_ReleaseTextures()
{
	Rocket::Core::ReleaseTextures();
}

//static void RemoveTexture(TextureResource *texture)
//{
//
//}
//
//static void ReleaseTextures(RenderInterface *render_interface)
//{
//
//}