#include "StdAfx.h"
#include "..\include\BulletInterface.h"
#include "BulletSceneDefinition.h"
#include "BulletScene.h"

using namespace Engine::Command;

namespace BulletPlugin
{

BulletInterface::BulletInterface(void)
{
	if (instance == nullptr)
    {
        instance = this;
    }
    else
    {
        throw gcnew Exception("Cannot create the BulletInterface more than once. Only call the constructor one time.");
    }
}

BulletInterface::~BulletInterface(void)
{

}

void BulletInterface::initialize(PluginManager^ pluginManager)
{
	pluginManager->addCreateSimElementManagerCommand(gcnew AddSimElementManagerCommand("Create Bullet Scene Definition", gcnew CreateSimElementManager(BulletSceneDefinition::Create)));
}

void BulletInterface::setPlatformInfo(Platform::UpdateTimer^ mainTimer, Platform::EventManager^ eventManager)
{
	this->timer = mainTimer;
}

String^ BulletInterface::getName()
{
	return PluginName;
}

DebugInterface^ BulletInterface::getDebugInterface()
{
	return nullptr;
}

void BulletInterface::createDebugCommands(System::Collections::Generic::List<CommandManager^>^ commandList)
{

}

BulletScene^ BulletInterface::createScene(BulletSceneDefinition^ definition)
{
	return gcnew BulletScene(definition, timer);
}

}