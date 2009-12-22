#pragma once

using namespace System;
using namespace Engine;

namespace BulletOgrePlugin
{

public ref class BulletOgreInterface : public PluginInterface
{
private:
	static BulletOgreInterface^ instance;
	static String^ PluginName = "BulletOgrePlugin";

public:
	BulletOgreInterface(void);

	virtual ~BulletOgreInterface(void);

	virtual void initialize(PluginManager^ pluginManager);

	virtual void setPlatformInfo(Platform::UpdateTimer^ mainTimer, Platform::EventManager^ eventManager);

	virtual String^ getName();

	virtual DebugInterface^ getDebugInterface();

	virtual void createDebugCommands(System::Collections::Generic::List<CommandManager^>^ commandList);

	static property BulletOgreInterface^ Instance
	{
		BulletOgreInterface^ get()
		{
			return instance;
		}
	}
};

}