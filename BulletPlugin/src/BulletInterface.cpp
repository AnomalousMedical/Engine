#include "StdAfx.h"
#include "..\include\BulletInterface.h"
#include "BulletSceneDefinition.h"
#include "BulletScene.h"
#include "RigidBodyDefinition.h"
#include "Generic6DofConstraintDefinition.h"
#include "BulletShapeFileManager.h"

using namespace Engine::Command;

namespace BulletPlugin
{

BulletInterface::BulletInterface(void)
:fileManager(gcnew BulletShapeFileManager()),
bulletResources(gcnew SubsystemResources("Bullet"))
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

	pluginManager->addCreateSimElementCommand(gcnew AddSimElementCommand("Create Bullet Rigid Body", gcnew CreateSimElement(RigidBodyDefinition::Create)));
	pluginManager->addCreateSimElementCommand(gcnew AddSimElementCommand("Create Bullet Generic 6 Dof Constraint", gcnew CreateSimElement(Generic6DofConstraintDefinition::Create)));

	bulletResources->addResourceListener(fileManager);
	pluginManager->addSubsystemResources(bulletResources);
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


BulletShapeRepository^ BulletInterface::ShapeRepository::get()
{
	return fileManager->ShapeRepository;
}

}