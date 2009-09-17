/// <file>Viewport.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\Viewport.h"
#include "MathUtils.h"
#include "MarshalUtils.h"

#include "Ogre.h"

namespace OgreWrapper
{

Viewport::Viewport(Ogre::Viewport* viewport)
:viewport( viewport )
{

}

Viewport::~Viewport()
{
	viewport = 0;
}

Ogre::Viewport* Viewport::getViewport()
{
	return viewport;
}

void Viewport::setVisibilityMask(unsigned int mask)
{
	viewport->setVisibilityMask(mask);
}

unsigned int Viewport::getVisibilityMask()
{
	return viewport->getVisibilityMask();
}

void Viewport::setBackgroundColor(Engine::Color color)
{
	return viewport->setBackgroundColour(MathUtils::copyColor(color));
}

Engine::Color Viewport::getBackgroundColor()
{
	return MathUtils::copyColor(viewport->getBackgroundColour());
}

float Viewport::getLeft()
{
	return viewport->getLeft();
}

float Viewport::getTop()
{
	return viewport->getTop();
}

float Viewport::getWidth()
{
	return viewport->getWidth();
}

float Viewport::getHeight()
{
	return viewport->getHeight();
}

int Viewport::getActualLeft()
{
	return viewport->getActualLeft();
}

int Viewport::getActualTop()
{
	return viewport->getActualTop();
}

int Viewport::getActualWidth()
{
	return viewport->getActualWidth();
}

int Viewport::getActualHeight()
{
	return viewport->getActualHeight();
}

void Viewport::setDimensions(float left, float top, float width, float height)
{
	return viewport->setDimensions(left, top, width, height);
}

void Viewport::setClearEveryFrame(bool clear)
{
	return viewport->setClearEveryFrame(clear);
}

bool Viewport::getClearEveryFrame()
{
	return viewport->getClearEveryFrame();
}

void Viewport::setMaterialScheme(System::String^ schemeName)
{
	return viewport->setMaterialScheme(MarshalUtils::convertString(schemeName));
}

System::String^ Viewport::getMaterialScheme()
{
	return MarshalUtils::convertString(viewport->getMaterialScheme());
}

void Viewport::setOverlaysEnabled(bool enabled)
{
	return viewport->setOverlaysEnabled(enabled);
}

bool Viewport::getOverlaysEnabled()
{
	return viewport->getOverlaysEnabled();
}

void Viewport::setSkiesEnabled(bool enabled)
{
	return viewport->setSkiesEnabled(enabled);
}

bool Viewport::getSkiesEnabled()
{
	return viewport->getSkiesEnabled();
}

void Viewport::setShadowsEnabled(bool enabled)
{
	return viewport->setShadowsEnabled(enabled);
}

bool Viewport::getShadowsEnabled()
{
	return viewport->getShadowsEnabled();
}

void Viewport::setRenderQueueInvocationSequenceName(System::String^ sequenceName)
{
	return viewport->setRenderQueueInvocationSequenceName(MarshalUtils::convertString(sequenceName));
}

System::String^ Viewport::getRenderQueueInvocationSequenceName()
{
	return MarshalUtils::convertString(viewport->getRenderQueueInvocationSequenceName());
}

}