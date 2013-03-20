#include "Stdafx.h"

extern "C" _AnomalousExport void Viewport_setVisibilityMask(Ogre::Viewport* viewport, uint mask)
{
	viewport->setVisibilityMask(mask);
}

extern "C" _AnomalousExport uint Viewport_getVisibilityMask(Ogre::Viewport* viewport)
{
	return viewport->getVisibilityMask();
}

extern "C" _AnomalousExport void Viewport_setBackgroundColor(Ogre::Viewport* viewport, Color color)
{
	viewport->setBackgroundColour(color.toOgre());
}

extern "C" _AnomalousExport Color Viewport_getBackgroundColor(Ogre::Viewport* viewport)
{
	return viewport->getBackgroundColour();
}

extern "C" _AnomalousExport float Viewport_getLeft(Ogre::Viewport* viewport)
{
	return viewport->getLeft();
}

extern "C" _AnomalousExport float Viewport_getTop(Ogre::Viewport* viewport)
{
	return viewport->getTop();
}

extern "C" _AnomalousExport float Viewport_getWidth(Ogre::Viewport* viewport)
{
	return viewport->getWidth();
}

extern "C" _AnomalousExport float Viewport_getHeight(Ogre::Viewport* viewport)
{
	return viewport->getHeight();
}

extern "C" _AnomalousExport int Viewport_getActualLeft(Ogre::Viewport* viewport)
{
	return viewport->getActualLeft();
}

extern "C" _AnomalousExport int Viewport_getActualTop(Ogre::Viewport* viewport)
{
	return viewport->getActualTop();
}

extern "C" _AnomalousExport int Viewport_getActualWidth(Ogre::Viewport* viewport)
{
	return viewport->getActualWidth();
}

extern "C" _AnomalousExport int Viewport_getActualHeight(Ogre::Viewport* viewport)
{
	return viewport->getActualHeight();
}

extern "C" _AnomalousExport void Viewport_setDimensions(Ogre::Viewport* viewport, float left, float top, float width, float height)
{
	viewport->setDimensions(left, top, width, height);
}

extern "C" _AnomalousExport void Viewport_setClearEveryFrame(Ogre::Viewport* viewport, bool clear, uint buffers)
{
	viewport->setClearEveryFrame(clear, buffers);
}

extern "C" _AnomalousExport bool Viewport_getClearEveryFrame(Ogre::Viewport* viewport)
{
	return viewport->getClearEveryFrame();
}

extern "C" _AnomalousExport void Viewport_setMaterialScheme(Ogre::Viewport* viewport, String schemeName)
{
	viewport->setMaterialScheme(schemeName);
}

extern "C" _AnomalousExport String Viewport_getMaterialScheme(Ogre::Viewport* viewport)
{
	return viewport->getMaterialScheme().c_str();
}

extern "C" _AnomalousExport void Viewport_setOverlaysEnabled(Ogre::Viewport* viewport, bool enabled)
{
	viewport->setOverlaysEnabled(enabled);
}

extern "C" _AnomalousExport bool Viewport_getOverlaysEnabled(Ogre::Viewport* viewport)
{
	return viewport->getOverlaysEnabled();
}

extern "C" _AnomalousExport void Viewport_setSkiesEnabled(Ogre::Viewport* viewport, bool enabled)
{
	viewport->setSkiesEnabled(enabled);
}

extern "C" _AnomalousExport bool Viewport_getSkiesEnabled(Ogre::Viewport* viewport)
{
	return viewport->getSkiesEnabled();
}

extern "C" _AnomalousExport void Viewport_setShadowsEnabled(Ogre::Viewport* viewport, bool enabled)
{
	viewport->setShadowsEnabled(enabled);
}

extern "C" _AnomalousExport bool Viewport_getShadowsEnabled(Ogre::Viewport* viewport)
{
	return viewport->getShadowsEnabled();
}

extern "C" _AnomalousExport void Viewport_setRenderQueueInvocationSequenceName(Ogre::Viewport* viewport, String sequenceName)
{
	viewport->setRenderQueueInvocationSequenceName(sequenceName);
}

extern "C" _AnomalousExport String Viewport_getRenderQueueInvocationSequenceName(Ogre::Viewport* viewport)
{
	return viewport->getRenderQueueInvocationSequenceName().c_str();
}

extern "C" _AnomalousExport Ogre::Camera* Viewport_getCamera(Ogre::Viewport* viewport)
{
	return viewport->getCamera();
}

extern "C" _AnomalousExport void Viewport_clear1(Ogre::Viewport* viewport)
{
	viewport->clear();
}

extern "C" _AnomalousExport void Viewport_clear2(Ogre::Viewport* viewport, Ogre::FrameBufferType buffers)
{
	viewport->clear(buffers);
}

extern "C" _AnomalousExport void Viewport_clear3(Ogre::Viewport* viewport, Ogre::FrameBufferType buffers, Color color)
{
	viewport->clear(buffers, color.toOgre());
}

extern "C" _AnomalousExport void Viewport_clear4(Ogre::Viewport* viewport, Ogre::FrameBufferType buffers, Color color, float depth)
{
	viewport->clear(buffers, color.toOgre(), depth);
}

extern "C" _AnomalousExport void Viewport_clear5(Ogre::Viewport* viewport, Ogre::FrameBufferType buffers, Color color, float depth, ushort stencil)
{
	viewport->clear(buffers, color.toOgre(), depth, stencil);
}