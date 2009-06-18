#include "StdAfx.h"
#include "..\include\Win32PlatformPlugin.h"
#include "Win32SystemTimer.h"
#include "InputHandler.h"

namespace Engine
{

namespace Platform
{

Win32PlatformPlugin::Win32PlatformPlugin(void)
{

}

Win32PlatformPlugin::~Win32PlatformPlugin()
{

}

void Win32PlatformPlugin::initialize(PluginManager^ pluginManager)
{
	pluginManager->setPlatformPlugin(this);
}

System::String^ Win32PlatformPlugin::getName()
{
	return "Win32Platform";
}

SystemTimer^ Win32PlatformPlugin::createTimer()
{
	return gcnew Win32SystemTimer();
}

void Win32PlatformPlugin::destroyTimer(SystemTimer^ timer)
{
	delete static_cast<Win32SystemTimer^>(timer);
}

InputHandler^ Win32PlatformPlugin::createInputHandler(OSWindow^ window, bool foreground, bool exclusive, bool noWinKey)
{
	return gcnew OISInputHandler(window, foreground, exclusive, noWinKey);
}

void Win32PlatformPlugin::destroyInputHandler(InputHandler^ inputHandler)
{
	delete static_cast<OISInputHandler^>(inputHandler);
}

}

}