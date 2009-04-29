/// <file>OISInputHandler.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\InputHandler.h"
#include "OIS.h"
#include "MarshalUtils.h"
#include <sstream>
#include "Keyboard.h"
#include "Mouse.h"

namespace Engine
{

namespace Platform
{

OISInputHandler::OISInputHandler(OSWindow^ windowHandle, bool foreground, bool exclusive, bool noWinKey)
:nInputManager(0), 
numKeyboards(0), 
numMice(0), 
numJoysticks(0), 
createdKeyboard(nullptr),
window(windowHandle)
{
	OIS::ParamList pl;

	std::stringstream ss;
	ss << windowHandle->Handle.ToInt32();
	pl.insert(std::make_pair( std::string("WINDOW"), ss.str() ));

	std::string foregroundMode = "DISCL_BACKGROUND";
	if( foreground )
	{
		foregroundMode = "DISCL_FOREGROUND";
	}
	std::string exclusiveMode = "DISCL_NONEXCLUSIVE";
	if( exclusive )
	{
		exclusiveMode = "DISCL_EXCLUSIVE";
	}
	pl.insert(std::make_pair(std::string("w32_mouse"), foregroundMode));
	pl.insert(std::make_pair(std::string("w32_mouse"), exclusiveMode));
	if( noWinKey )
	{
		pl.insert(std::make_pair(std::string("w32_mouse"), std::string("DISCL_NOWINKEY")));
	}

	nInputManager = OIS::InputManager::createInputSystem(pl);

	numKeyboards = nInputManager->getNumberOfDevices(OIS::OISKeyboard);
	numMice = nInputManager->getNumberOfDevices(OIS::OISMouse);
	numJoysticks = nInputManager->getNumberOfDevices(OIS::OISJoyStick);

	//Log some info
	unsigned int v = nInputManager->getVersionNumber();
	Logging::Log::Default->sendMessage("Using OIS Version: " + (v>>16) + "." + ((v>>8) & 0x000000FF) + "." + (v & 0x000000FF) + " \"" + MarshalUtils::convertString( nInputManager->getVersionName() ) + "\"", Logging::LogLevel::Info, "Input");
	Logging::Log::Default->sendMessage("Manager: " + MarshalUtils::convertString( nInputManager->inputSystemName() ), Logging::LogLevel::Info, "Input");
	Logging::Log::Default->sendMessage("Total Keyboards: " + numKeyboards, Logging::LogLevel::Info, "Input");
	Logging::Log::Default->sendMessage("Total Mice: " + numMice, Logging::LogLevel::Info, "Input");
	Logging::Log::Default->sendMessage("Total JoySticks: " + numJoysticks, Logging::LogLevel::Info, "Input");

}

OISInputHandler::~OISInputHandler()
{
	if( createdMouse != nullptr )
	{
		destroyMouse(createdMouse);
	}
	if( createdKeyboard != nullptr )
	{
		destroyKeyboard(createdKeyboard);
	}
	OIS::InputManager::destroyInputSystem(nInputManager);
	nInputManager = 0;
}

Keyboard^ OISInputHandler::createKeyboard(bool buffered)
{
	if( createdKeyboard == nullptr )
	{
		Logging::Log::Default->sendMessage("Creating keyboard.", Logging::LogLevel::Info, "Input");
		createdKeyboard = gcnew OISKeyboard((OIS::Keyboard*)nInputManager->createInputObject(OIS::OISKeyboard, buffered));
	}
	return createdKeyboard;
}

void OISInputHandler::destroyKeyboard(Keyboard^ keyboard)
{
	if( createdKeyboard == keyboard )
	{
		Logging::Log::Default->sendMessage("Destroying keyboard.", Logging::LogLevel::Info, "Input");
		nInputManager->destroyInputObject(static_cast<OISKeyboard^>(keyboard)->getKeyboard());
		delete keyboard;
		createdKeyboard = nullptr;
	}
	else
	{
		if( createdKeyboard == nullptr )
		{
			Logging::Log::Default->sendMessage("OISKeyboard has already been destroyed.", Logging::LogLevel::Error, "Input");
		}
		else
		{
			Logging::Log::Default->sendMessage("Attempted to erase keyboard that does not belong to this input manager.  OISKeyboard not destroyed.", Logging::LogLevel::Error, "Input");
		}
	}
}

Mouse^ OISInputHandler::createMouse(bool buffered)
{
	if( createdMouse == nullptr )
	{
		Logging::Log::Default->sendMessage("Creating mouse.", Logging::LogLevel::Info, "Input");
		createdMouse = gcnew OISMouse((OIS::Mouse*)nInputManager->createInputObject(OIS::OISMouse, buffered), window->Width, window->Height);
		window->addListener(this);
	}
	return createdMouse;
}

void OISInputHandler::destroyMouse(Mouse^ mouse)
{
	if( createdMouse == mouse )
	{
		window->removeListener(this);
		Logging::Log::Default->sendMessage("Destroying mouse.", Logging::LogLevel::Info, "Input");
		nInputManager->destroyInputObject(static_cast<OISMouse^>(mouse)->getMouse());
		delete mouse;
		createdMouse = nullptr;
	}
	else
	{
		if( createdMouse == nullptr )
		{
			Logging::Log::Default->sendMessage("OISMouse has already been destroyed.", Logging::LogLevel::Error, "Input");
		}
		else
		{
			Logging::Log::Default->sendMessage("Attempted to erase mouse that does not belong to this input manager.  OISMouse not destroyed.", Logging::LogLevel::Error, "Input");
		}
	}
}

void OISInputHandler::moved(Engine::Platform::OSWindow^ window)
{

}

void OISInputHandler::resized(Engine::Platform::OSWindow^ window)
{
	if(createdMouse != nullptr)
	{
		createdMouse->windowResized(window);
	}
}

void OISInputHandler::closing(Engine::Platform::OSWindow^ window)
{

}

}

}