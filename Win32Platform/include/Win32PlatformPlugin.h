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

	virtual DebugInterface^ getDebugInterface(){return nullptr;}

	virtual SystemTimer^ createTimer();

	virtual void destroyTimer(SystemTimer^ timer);

	virtual InputHandler^ createInputHandler(OSWindow^ window, bool foreground, bool exclusive, bool noWinKey);

	virtual void destroyInputHandler(InputHandler^ inputHandler);

	virtual void setPlatformInfo(UpdateTimer^ mainTimer, EventManager^ eventManager){}

	/// <summary>
    /// This function will create any debug commands for the plugin and add them to the commands list.
    /// </summary>
    /// <param name="commands">A list of CommandManagers to add debug commands to.</param>
	virtual void createDebugCommands(System::Collections::Generic::List<CommandManager^>^ commands){}
};

}

}