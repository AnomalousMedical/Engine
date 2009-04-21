/// <file>RenderTarget.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\RenderTarget.h"

#include "Ogre.h"

#include "Camera.h"
#include "Viewport.h"
#include "MarshalUtils.h"
#include "PixelBox.h"

namespace OgreWrapper{

RenderTarget::RenderTarget(Ogre::RenderTarget* renderTarget)
:renderTarget( renderTarget )
{
}

RenderTarget::~RenderTarget()
{
	
}

Ogre::RenderTarget* RenderTarget::getRenderTarget()
{
	return renderTarget;
}

System::String^ RenderTarget::getName()
{
	return MarshalUtils::convertString(renderTarget->getName());
}

Viewport^ RenderTarget::addViewport(Camera^ camera)
{
	return viewports.getObject(renderTarget->addViewport(camera->getCamera()));
}

Viewport^ RenderTarget::addViewport(Camera^ camera, int zOrder, float left, float top, float width, float height)
{
	return viewports.getObject(renderTarget->addViewport(camera->getCamera(), zOrder, left, top, width, height));
}

void RenderTarget::destroyViewport( Viewport^ viewport )
{
	Ogre::Viewport* ogreViewport = viewport->getViewport();
	viewports.destroyObject(ogreViewport);
	renderTarget->removeViewport(ogreViewport->getZOrder());
}

unsigned int RenderTarget::getWidth()
{
	return renderTarget->getWidth();
}

unsigned int RenderTarget::getHeight()
{
	return renderTarget->getHeight();
}

unsigned int RenderTarget::getColorDepth()
{
	return renderTarget->getColourDepth();
}

void RenderTarget::update()
{
	renderTarget->update();
}

void RenderTarget::update(bool swapBuffers)
{
	renderTarget->update(swapBuffers);
}

void RenderTarget::swapBuffers()
{
	renderTarget->swapBuffers();
}

void RenderTarget::swapBuffers(bool waitForVsync)
{
	renderTarget->swapBuffers(waitForVsync);
}

unsigned short RenderTarget::getNumViewports()
{
	return renderTarget->getNumViewports();
}

Viewport^ RenderTarget::getViewport(unsigned short index)
{
	return viewports.getObject(renderTarget->getViewport(index));
}

float RenderTarget::getLastFPS()
{
	return renderTarget->getLastFPS();
}

float RenderTarget::getAverageFPS()
{
	return renderTarget->getAverageFPS();
}

float RenderTarget::getBestFPS()
{
	return renderTarget->getBestFPS();
}

float RenderTarget::getWorstFPS()
{
	return renderTarget->getWorstFPS();
}

float RenderTarget::getBestFrameTime()
{
	return renderTarget->getBestFrameTime();
}

float RenderTarget::getWorstFrameTime()
{
	return renderTarget->getWorstFrameTime();
}

void RenderTarget::resetStatistics()
{
	return renderTarget->resetStatistics();
}

void RenderTarget::getCustomAttribute(System::String^ name, void* pData)
{
	return renderTarget->getCustomAttribute(MarshalUtils::convertString(name), pData);
}

void RenderTarget::setPriority(unsigned char priority)
{
	return renderTarget->setPriority(priority);
}

unsigned char RenderTarget::getPriority()
{
	return renderTarget->getPriority();
}

bool RenderTarget::isActive()
{
	return renderTarget->isActive();
}

void RenderTarget::setActive(bool active)
{
	return renderTarget->setActive(active);
}

void RenderTarget::setAutoUpdated(bool autoUpdate)
{
	return renderTarget->setAutoUpdated(autoUpdate);
}

bool RenderTarget::isAutoUpdated()
{
	return renderTarget->isAutoUpdated();
}

void RenderTarget::copyContentsToMemory(PixelBox^ dest)
{
	return renderTarget->copyContentsToMemory(*dest->getPixelBox());
}

void RenderTarget::copyContentsToMemory(PixelBox^ dest, FrameBuffer buffer)
{
	return renderTarget->copyContentsToMemory(*dest->getPixelBox(), (Ogre::RenderTarget::FrameBuffer)buffer);
}

PixelFormat RenderTarget::suggestPixelFormat()
{
	return (PixelFormat)renderTarget->suggestPixelFormat();
}

void RenderTarget::writeContentsToFile(System::String^ filename)
{
	return renderTarget->writeContentsToFile(MarshalUtils::convertString(filename));
}

System::String^ RenderTarget::writeContentsToTimestampedFile(System::String^ filenamePrefix, System::String^ filenameSuffix)
{
	return MarshalUtils::convertString(renderTarget->writeContentsToTimestampedFile(MarshalUtils::convertString(filenamePrefix), MarshalUtils::convertString(filenameSuffix)));
}

bool RenderTarget::requiresTextureFlipping()
{
	return renderTarget->requiresTextureFlipping();
}

size_t RenderTarget::getTriangleCount()
{
	return renderTarget->getTriangleCount();
}

size_t RenderTarget::getBatchCount()
{
	return renderTarget->getBatchCount();
}

bool RenderTarget::isPrimary()
{
	return renderTarget->isPrimary();
}

bool RenderTarget::isHardwareGammaEnabled()
{
	return renderTarget->isHardwareGammaEnabled();
}

unsigned int RenderTarget::getFSAA()
{
	return renderTarget->getFSAA();
}

}