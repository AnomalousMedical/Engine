/// <file>Viewport.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\Viewport.h"

#include "Ogre.h"

namespace OgreWrapper
{

Viewport::Viewport(Ogre::Viewport* viewport, System::String^ name)
:viewport( viewport ), name( name )
{

}

Viewport::~Viewport()
{
	
}

Ogre::Viewport* Viewport::getViewport()
{
	return viewport;
}

System::String^ Viewport::getName()
{
	return name;
}

void Viewport::setVisibilityMask(unsigned int mask)
{
	viewport->setVisibilityMask(mask);
}

unsigned int Viewport::getVisibilityMask()
{
	return viewport->getVisibilityMask();
}

}