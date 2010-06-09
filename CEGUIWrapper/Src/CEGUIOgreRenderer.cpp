#include "Stdafx.h"
#include "RendererModules/Ogre/CEGUIOgreRenderer.h"

extern "C" _AnomalousExport CEGUI::OgreRenderer* CEGUIOgreRenderer_create(Ogre::RenderTarget* ogreRenderTarget)
{
	return &CEGUI::OgreRenderer::create(*ogreRenderTarget);
}

extern "C" _AnomalousExport CEGUI::OgreResourceProvider* CEGUIOgreRenderer_createOgreResourceProvider()
{
	return &CEGUI::OgreRenderer::createOgreResourceProvider();
}

extern "C" _AnomalousExport CEGUI::OgreImageCodec* CEGUIOgreRenderer_createOgreImageCodec()
{
	return &CEGUI::OgreRenderer::createOgreImageCodec();
}

extern "C" _AnomalousExport void CEGUIOgreRenderer_destroy(CEGUI::OgreRenderer* ogreRenderer)
{
	CEGUI::OgreRenderer::destroy(*ogreRenderer);
}

extern "C" _AnomalousExport void CEGUIOgreRenderer_destroyOgreResourceProvider(CEGUI::OgreResourceProvider* ogreResourceProvider)
{
	CEGUI::OgreRenderer::destroyOgreResourceProvider(*ogreResourceProvider);
}

extern "C" _AnomalousExport void CEGUIOgreRenderer_destroyOgreImageCodec(CEGUI::OgreImageCodec* ogreImageCodec)
{
	CEGUI::OgreRenderer::destroyOgreImageCodec(*ogreImageCodec);
}