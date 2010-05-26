#include "Stdafx.h"

extern "C" __declspec(dllexport) const char* RenderTarget_getName(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getName().c_str();
}

extern "C" __declspec(dllexport) Ogre::Viewport* RenderTarget_addViewport(Ogre::RenderTarget* renderTarget, Ogre::Camera* camera)
{
	return renderTarget->addViewport(camera);
}

extern "C" __declspec(dllexport) Ogre::Viewport* RenderTarget_addViewportExt(Ogre::RenderTarget* renderTarget, Ogre::Camera* camera, int zOrder, float left, float top, float width, float height)
{
	return renderTarget->addViewport(camera, zOrder, left, top, width, height);
}

extern "C" __declspec(dllexport) void RenderTarget_destroyViewport(Ogre::RenderTarget* renderTarget, Ogre::Viewport* viewport)
{
	renderTarget->removeViewport(viewport->getZOrder());
}

extern "C" __declspec(dllexport) uint RenderTarget_getWidth(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getWidth();
}

extern "C" __declspec(dllexport) uint RenderTarget_getHeight(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getHeight();
}

extern "C" __declspec(dllexport) uint RenderTarget_getColorDepth(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getColourDepth();
}

extern "C" __declspec(dllexport) void RenderTarget_update(Ogre::RenderTarget* renderTarget)
{
	renderTarget->update();
}

extern "C" __declspec(dllexport) void RenderTarget_updateSwap(Ogre::RenderTarget* renderTarget, bool swapBuffers)
{
	renderTarget->update(swapBuffers);
}

extern "C" __declspec(dllexport) void RenderTarget_swapBuffers(Ogre::RenderTarget* renderTarget)
{
	renderTarget->swapBuffers();
}

extern "C" __declspec(dllexport) void RenderTarget_swapBuffersVsync(Ogre::RenderTarget* renderTarget, bool waitForVsync)
{
	renderTarget->swapBuffers(waitForVsync);
}

extern "C" __declspec(dllexport) ushort RenderTarget_getNumViewports(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getNumViewports();
}

extern "C" __declspec(dllexport) Ogre::Viewport* RenderTarget_getViewport(Ogre::RenderTarget* renderTarget, ushort index)
{
	return renderTarget->getViewport(index);
}

extern "C" __declspec(dllexport) float RenderTarget_getLastFPS(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getLastFPS();
}

extern "C" __declspec(dllexport) float RenderTarget_getAverageFPS(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getAverageFPS();
}

extern "C" __declspec(dllexport) float RenderTarget_getBestFPS(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getBestFPS();
}

extern "C" __declspec(dllexport) float RenderTarget_getWorstFPS(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getWorstFPS();
}

extern "C" __declspec(dllexport) float RenderTarget_getBestFrameTime(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getBestFrameTime();
}

extern "C" __declspec(dllexport) float RenderTarget_getWorstFrameTime(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getWorstFrameTime();
}

extern "C" __declspec(dllexport) void RenderTarget_resetStatistics(Ogre::RenderTarget* renderTarget)
{
	renderTarget->resetStatistics();
}

extern "C" __declspec(dllexport) void RenderTarget_getCustomAttribute(Ogre::RenderTarget* renderTarget, const char* name, void* pData)
{
	renderTarget->getCustomAttribute(name, pData);
}

extern "C" __declspec(dllexport) void RenderTarget_setPriority(Ogre::RenderTarget* renderTarget, byte priority)
{
	renderTarget->setPriority(priority);
}

extern "C" __declspec(dllexport) byte RenderTarget_getPriority(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getPriority();
}

extern "C" __declspec(dllexport) bool RenderTarget_isActive(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->isActive();
}

extern "C" __declspec(dllexport) void RenderTarget_setActive(Ogre::RenderTarget* renderTarget, bool active)
{
	renderTarget->setActive(active);
}

extern "C" __declspec(dllexport) void RenderTarget_setAutoUpdated(Ogre::RenderTarget* renderTarget, bool autoUpdate)
{
	renderTarget->setAutoUpdated(autoUpdate);
}

extern "C" __declspec(dllexport) bool RenderTarget_isAutoUpdated(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->isAutoUpdated();
}

extern "C" __declspec(dllexport) void RenderTarget_copyContentsToMemory(Ogre::RenderTarget* renderTarget, Ogre::PixelBox* dest)
{
	renderTarget->copyContentsToMemory(*dest);
}

extern "C" __declspec(dllexport) void RenderTarget_copyContentsToMemoryBuffer(Ogre::RenderTarget* renderTarget, Ogre::PixelBox* dest, Ogre::RenderTarget::FrameBuffer buffer)
{
	renderTarget->copyContentsToMemory(*dest, buffer);
}

extern "C" __declspec(dllexport) Ogre::PixelFormat RenderTarget_suggestPixelFormat(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->suggestPixelFormat();
}

extern "C" __declspec(dllexport) void RenderTarget_writeContentsToFile(Ogre::RenderTarget* renderTarget, String filename)
{
	renderTarget->writeContentsToFile(filename);
}

extern "C" __declspec(dllexport) const char* RenderTarget_writeContentsToTimestampedFile(Ogre::RenderTarget* renderTarget, String filenamePrefix, String filenameSuffix)
{
	return renderTarget->writeContentsToTimestampedFile(filenamePrefix, filenameSuffix).c_str();
}

extern "C" __declspec(dllexport) bool RenderTarget_requiresTextureFlipping(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->requiresTextureFlipping();
}

extern "C" __declspec(dllexport) uint RenderTarget_getTriangleCount(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getTriangleCount();
}

extern "C" __declspec(dllexport) uint RenderTarget_getBatchCount(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getBatchCount();
}

extern "C" __declspec(dllexport) bool RenderTarget_isPrimary(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->isPrimary();
}

extern "C" __declspec(dllexport) bool RenderTarget_isHardwareGammaEnabled(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->isHardwareGammaEnabled();
}

extern "C" __declspec(dllexport) uint RenderTarget_getFSAA(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getFSAA();
}