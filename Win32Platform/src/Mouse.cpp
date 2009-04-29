/// <file>OISMouse.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\Mouse.h"
#include "OIS.h"

namespace Engine{

namespace Platform{

OISMouse::OISMouse(OIS::Mouse* oisMouse, int width, int height)
:oisMouse( oisMouse ), mouseListener(0), sensitivity(1.0f)
{
	const OIS::MouseState& state = oisMouse->getMouseState();
	state.width = width;
	state.height = height;
	mouseListener.Reset(new NMouseListener(this));
	oisMouse->setEventCallback(mouseListener.Get());
}

OISMouse::~OISMouse()
{
	
}

void OISMouse::moved()
{
	this->MouseMoved(this);
}

void OISMouse::buttonPressed(OIS::MouseButtonID id)
{
	this->MousePressed(this, (MouseButtonCode)id);
}

void OISMouse::buttonReleased(OIS::MouseButtonID id)
{
	this->MouseReleased(this, (MouseButtonCode)id);
}

OIS::Mouse* OISMouse::getMouse()
{
	return oisMouse;
}

void OISMouse::windowResized(OSWindow^ window)
{
	const OIS::MouseState& state = oisMouse->getMouseState();
	state.width = window->Width;
	state.height = window->Height;
}

Engine::Vector3 OISMouse::getAbsMouse()
{
	const OIS::MouseState& state = oisMouse->getMouseState();
	return Engine::Vector3(
	(float)state.X.abs * sensitivity,
	(float)state.Y.abs * sensitivity,
	(float)state.Z.abs * sensitivity);
}

Engine::Vector3 OISMouse::getRelMouse()
{
	const OIS::MouseState& state = oisMouse->getMouseState();
	return Engine::Vector3(
	(float)state.X.rel * sensitivity,
	(float)state.Y.rel * sensitivity,
	(float)state.Z.rel * sensitivity);
}

bool OISMouse::buttonDown( MouseButtonCode button )
{
	return oisMouse->getMouseState().buttonDown( (OIS::MouseButtonID)button );
}

void OISMouse::capture()
{
	oisMouse->capture();
}

void OISMouse::setSensitivity(float sensitivity)
{
	this->sensitivity = sensitivity;
}

float OISMouse::getMouseAreaWidth()
{
	return static_cast<float>(oisMouse->getMouseState().width);
}

float OISMouse::getMouseAreaHeight()
{
	return static_cast<float>(oisMouse->getMouseState().height);
}

}

}