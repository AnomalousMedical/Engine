#include "Stdafx.h"

extern "C" _AnomalousExport const char* RenderTarget_getName(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getName().c_str();
}

extern "C" _AnomalousExport Ogre::Viewport* RenderTarget_addViewport(Ogre::RenderTarget* renderTarget, Ogre::Camera* camera)
{
	return renderTarget->addViewport(camera);
}

extern "C" _AnomalousExport Ogre::Viewport* RenderTarget_addViewportExt(Ogre::RenderTarget* renderTarget, Ogre::Camera* camera, int zOrder, float left, float top, float width, float height)
{
	return renderTarget->addViewport(camera, zOrder, left, top, width, height);
}

extern "C" _AnomalousExport void RenderTarget_destroyViewport(Ogre::RenderTarget* renderTarget, Ogre::Viewport* viewport)
{
	renderTarget->removeViewport(viewport->getZOrder());
}

extern "C" _AnomalousExport uint RenderTarget_getWidth(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getWidth();
}

extern "C" _AnomalousExport uint RenderTarget_getHeight(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getHeight();
}

extern "C" _AnomalousExport uint RenderTarget_getColorDepth(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getColourDepth();
}

extern "C" _AnomalousExport void RenderTarget_update(Ogre::RenderTarget* renderTarget)
{
	renderTarget->update();
}

extern "C" _AnomalousExport void RenderTarget_updateSwap(Ogre::RenderTarget* renderTarget, bool swapBuffers)
{
	renderTarget->update(swapBuffers);
}

extern "C" _AnomalousExport void RenderTarget_swapBuffers(Ogre::RenderTarget* renderTarget)
{
	renderTarget->swapBuffers();
}

extern "C" _AnomalousExport void RenderTarget_swapBuffersVsync(Ogre::RenderTarget* renderTarget, bool waitForVsync)
{
	renderTarget->swapBuffers(waitForVsync);
}

extern "C" _AnomalousExport ushort RenderTarget_getNumViewports(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getNumViewports();
}

extern "C" _AnomalousExport Ogre::Viewport* RenderTarget_getViewport(Ogre::RenderTarget* renderTarget, ushort index)
{
	return renderTarget->getViewport(index);
}

extern "C" _AnomalousExport float RenderTarget_getLastFPS(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getLastFPS();
}

extern "C" _AnomalousExport float RenderTarget_getAverageFPS(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getAverageFPS();
}

extern "C" _AnomalousExport float RenderTarget_getBestFPS(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getBestFPS();
}

extern "C" _AnomalousExport float RenderTarget_getWorstFPS(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getWorstFPS();
}

extern "C" _AnomalousExport float RenderTarget_getBestFrameTime(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getBestFrameTime();
}

extern "C" _AnomalousExport float RenderTarget_getWorstFrameTime(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getWorstFrameTime();
}

extern "C" _AnomalousExport void RenderTarget_resetStatistics(Ogre::RenderTarget* renderTarget)
{
	renderTarget->resetStatistics();
}

extern "C" _AnomalousExport void RenderTarget_getCustomAttribute(Ogre::RenderTarget* renderTarget, const char* name, void* pData)
{
	renderTarget->getCustomAttribute(name, pData);
}

extern "C" _AnomalousExport void RenderTarget_setPriority(Ogre::RenderTarget* renderTarget, byte priority)
{
	renderTarget->setPriority(priority);
}

extern "C" _AnomalousExport byte RenderTarget_getPriority(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getPriority();
}

extern "C" _AnomalousExport bool RenderTarget_isActive(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->isActive();
}

extern "C" _AnomalousExport void RenderTarget_setActive(Ogre::RenderTarget* renderTarget, bool active)
{
	renderTarget->setActive(active);
}

extern "C" _AnomalousExport void RenderTarget_setAutoUpdated(Ogre::RenderTarget* renderTarget, bool autoUpdate)
{
	renderTarget->setAutoUpdated(autoUpdate);
}

extern "C" _AnomalousExport bool RenderTarget_isAutoUpdated(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->isAutoUpdated();
}

extern "C" _AnomalousExport void RenderTarget_copyContentsToMemory(Ogre::RenderTarget* renderTarget, Ogre::PixelBox* dest)
{
	renderTarget->copyContentsToMemory(*dest);
}

extern "C" _AnomalousExport void RenderTarget_copyContentsToMemoryBuffer(Ogre::RenderTarget* renderTarget, Ogre::PixelBox* dest, Ogre::RenderTarget::FrameBuffer buffer)
{
	renderTarget->copyContentsToMemory(*dest, buffer);
}

extern "C" _AnomalousExport Ogre::PixelFormat RenderTarget_suggestPixelFormat(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->suggestPixelFormat();
}

extern "C" _AnomalousExport void RenderTarget_writeContentsToFile(Ogre::RenderTarget* renderTarget, String filename)
{
	renderTarget->writeContentsToFile(filename);
}

extern "C" _AnomalousExport const char* RenderTarget_writeContentsToTimestampedFile(Ogre::RenderTarget* renderTarget, String filenamePrefix, String filenameSuffix)
{
	return renderTarget->writeContentsToTimestampedFile(filenamePrefix, filenameSuffix).c_str();
}

extern "C" _AnomalousExport bool RenderTarget_requiresTextureFlipping(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->requiresTextureFlipping();
}

extern "C" _AnomalousExport uint RenderTarget_getTriangleCount(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getTriangleCount();
}

extern "C" _AnomalousExport uint RenderTarget_getBatchCount(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getBatchCount();
}

extern "C" _AnomalousExport bool RenderTarget_isPrimary(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->isPrimary();
}

extern "C" _AnomalousExport bool RenderTarget_isHardwareGammaEnabled(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->isHardwareGammaEnabled();
}

extern "C" _AnomalousExport uint RenderTarget_getFSAA(Ogre::RenderTarget* renderTarget)
{
	return renderTarget->getFSAA();
}