#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace Engine;
using namespace Engine::Resources;

namespace BulletPlugin
{

ref class BulletScene;
ref class BulletSceneDefinition;
ref class BulletShapeFileManager;
ref class BulletShapeRepository;
ref class BulletDebugInterface;

public ref class BulletInterface : public PluginInterface
{
private:
	static BulletInterface^ instance;
	Platform::UpdateTimer^ timer;
	BulletShapeFileManager^ fileManager;
	SubsystemResources^ bulletResources;
	BulletDebugInterface^ debugInterface;

public:
	static String^ PluginName = "BulletPlugin";

	static property BulletInterface^ Instance
	{
		BulletInterface^ get()
		{
			return instance;
		}
	}

	BulletInterface(void);

	virtual ~BulletInterface(void);

	virtual void initialize(PluginManager^ pluginManager);

	virtual void setPlatformInfo(Platform::UpdateTimer^ mainTimer, Platform::EventManager^ eventManager);

	virtual String^ getName();

	virtual DebugInterface^ getDebugInterface();

	virtual void createDebugCommands(System::Collections::Generic::List<CommandManager^>^ commandList);

	BulletScene^ createScene(BulletSceneDefinition^ definition);

	property BulletShapeRepository^ ShapeRepository
	{
		BulletShapeRepository^ get();
	}
};

}