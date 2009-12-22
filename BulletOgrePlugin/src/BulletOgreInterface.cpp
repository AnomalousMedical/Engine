#include "StdAfx.h"
#include "..\include\BulletOgreInterface.h"
#include "BulletOgreSoftBodyProviderDefinition.h"

using namespace Engine::Command;

namespace BulletOgrePlugin
{

BulletOgreInterface::BulletOgreInterface(void)
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

BulletOgreInterface::~BulletOgreInterface(void)
{
}

void BulletOgreInterface::initialize(PluginManager^ pluginManager)
{
	pluginManager->addCreateSimElementCommand(gcnew AddSimElementCommand("Create Bullet Ogre Soft Body Provider", gcnew CreateSimElement(BulletOgreSoftBodyProviderDefinition::Create)));
}

void BulletOgreInterface::setPlatformInfo(Platform::UpdateTimer^ mainTimer, Platform::EventManager^ eventManager)
{
}

String^ BulletOgreInterface::getName()
{
	return PluginName;
}

DebugInterface^ BulletOgreInterface::getDebugInterface()
{
	return nullptr;
}

void BulletOgreInterface::createDebugCommands(System::Collections::Generic::List<CommandManager^>^ commandList)
{
}

}