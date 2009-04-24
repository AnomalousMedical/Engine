#pragma once

namespace Engine
{

namespace Platform
{

public ref class Win32PlatformPlugin : public PlatformPlugin
{
public:
	Win32PlatformPlugin(void);

	virtual ~Win32PlatformPlugin();

	virtual void initialize(PluginManager^ pluginManager);

	virtual System::String^ getName();

	virtual UpdateTimer^ createTimer();

	virtual void destroyTimer(UpdateTimer^ timer);

	virtual InputHandler^ createInputHandler(OSWindow^ window, bool foreground, bool exclusive, bool noWinKey);

	virtual void destroyInputHandler(InputHandler^ inputHandler);

	virtual void setPlatformInfo(UpdateTimer^ mainTimer, EventManager^ eventManager){}
};

}

}