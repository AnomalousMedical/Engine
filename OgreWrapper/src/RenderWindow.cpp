/// <file>RenderWindow.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\RenderWindow.h"

#include "Ogre.h"

namespace OgreWrapper
{

RenderWindow::RenderWindow(Ogre::RenderWindow* ogreRenderWindow)
:RenderTarget( ogreRenderWindow ), 
ogreRenderWindow( ogreRenderWindow )
{

}

RenderWindow::~RenderWindow()
{

}

Ogre::RenderWindow* RenderWindow::getRenderWindow()
{
	return ogreRenderWindow;
}

void RenderWindow::windowMovedOrResized()
{
	ogreRenderWindow->windowMovedOrResized();
}

}