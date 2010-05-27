#include "Stdafx.h"

extern "C" __declspec(dllexport) void Viewport_setVisibilityMask(Ogre::Viewport* viewport, uint mask)
{
	viewport->setVisibilityMask(mask);
}

extern "C" __declspec(dllexport) uint Viewport_getVisibilityMask(Ogre::Viewport* viewport)
{
	return viewport->getVisibilityMask();
}

extern "C" __declspec(dllexport) void Viewport_setBackgroundColor(Ogre::Viewport* viewport, Color color)
{
	viewport->setBackgroundColour(color.toOgre());
}

extern "C" __declspec(dllexport) Color Viewport_getBackgroundColor(Ogre::Viewport* viewport)
{
	return viewport->getBackgroundColour();
}

extern "C" __declspec(dllexport) float Viewport_getLeft(Ogre::Viewport* viewport)
{
	return viewport->getLeft();
}

extern "C" __declspec(dllexport) float Viewport_getTop(Ogre::Viewport* viewport)
{
	return viewport->getTop();
}

extern "C" __declspec(dllexport) float Viewport_getWidth(Ogre::Viewport* viewport)
{
	return viewport->getWidth();
}

extern "C" __declspec(dllexport) float Viewport_getHeight(Ogre::Viewport* viewport)
{
	return viewport->getHeight();
}

extern "C" __declspec(dllexport) int Viewport_getActualLeft(Ogre::Viewport* viewport)
{
	return viewport->getActualLeft();
}

extern "C" __declspec(dllexport) int Viewport_getActualTop(Ogre::Viewport* viewport)
{
	return viewport->getActualTop();
}

extern "C" __declspec(dllexport) int Viewport_getActualWidth(Ogre::Viewport* viewport)
{
	return viewport->getActualWidth();
}

extern "C" __declspec(dllexport) int Viewport_getActualHeight(Ogre::Viewport* viewport)
{
	return viewport->getActualHeight();
}

extern "C" __declspec(dllexport) void Viewport_setDimensions(Ogre::Viewport* viewport, float left, float top, float width, float height)
{
	viewport->setDimensions(left, top, width, height);
}

extern "C" __declspec(dllexport) void Viewport_setClearEveryFrame(Ogre::Viewport* viewport, bool clear)
{
	viewport->setClearEveryFrame(clear);
}

extern "C" __declspec(dllexport) bool Viewport_getClearEveryFrame(Ogre::Viewport* viewport)
{
	return viewport->getClearEveryFrame();
}

extern "C" __declspec(dllexport) void Viewport_setMaterialScheme(Ogre::Viewport* viewport, String schemeName)
{
	viewport->setMaterialScheme(schemeName);
}

extern "C" __declspec(dllexport) String Viewport_getMaterialScheme(Ogre::Viewport* viewport)
{
	return viewport->getMaterialScheme().c_str();
}

extern "C" __declspec(dllexport) void Viewport_setOverlaysEnabled(Ogre::Viewport* viewport, bool enabled)
{
	viewport->setOverlaysEnabled(enabled);
}

extern "C" __declspec(dllexport) bool Viewport_getOverlaysEnabled(Ogre::Viewport* viewport)
{
	return viewport->getOverlaysEnabled();
}

extern "C" __declspec(dllexport) void Viewport_setSkiesEnabled(Ogre::Viewport* viewport, bool enabled)
{
	viewport->setSkiesEnabled(enabled);
}

extern "C" __declspec(dllexport) bool Viewport_getSkiesEnabled(Ogre::Viewport* viewport)
{
	return viewport->getSkiesEnabled();
}

extern "C" __declspec(dllexport) void Viewport_setShadowsEnabled(Ogre::Viewport* viewport, bool enabled)
{
	viewport->setShadowsEnabled(enabled);
}

extern "C" __declspec(dllexport) bool Viewport_getShadowsEnabled(Ogre::Viewport* viewport)
{
	return viewport->getShadowsEnabled();
}

extern "C" __declspec(dllexport) void Viewport_setRenderQueueInvocationSequenceName(Ogre::Viewport* viewport, String sequenceName)
{
	viewport->setRenderQueueInvocationSequenceName(sequenceName);
}

extern "C" __declspec(dllexport) String Viewport_getRenderQueueInvocationSequenceName(Ogre::Viewport* viewport)
{
	return viewport->getRenderQueueInvocationSequenceName().c_str();
}