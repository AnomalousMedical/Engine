#include "StdAfx.h"
#include "..\include\RigidBodyDefinition.h"
#include "BulletScene.h"
#include "BulletFactory.h"
#include "BulletInterface.h"

namespace BulletPlugin
{

RigidBodyDefinition::RigidBodyDefinition(String^ name)
:BulletElementDefinition(name),
editInterface(nullptr)
{

}

void RigidBodyDefinition::registerScene(SimSubScene^ subscene, SimObjectBase^ instance)
{
	if (subscene->hasSimElementManagerType(BulletScene::typeid))
    {
        BulletScene^ sceneManager = subscene->getSimElementManager<BulletScene^>();
        sceneManager->getBulletFactory()->addRigidBody(this, instance);
    }
    else
    {
		Logging::Log::Default->sendMessage("Cannot add PhysActorDefinition {0} to SimSubScene {1} because it does not contain a PhysXSceneManager.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, subscene->Name);
    }
}

EditInterface^ RigidBodyDefinition::getEditInterface()
{
	if(editInterface == nullptr)
	{
		editInterface = ReflectedEditInterface::createEditInterface(this, memberScanner, this->Name + " - Rigid Body", nullptr);
	}
	return editInterface;
}

void RigidBodyDefinition::createProduct(SimObjectBase^ instance, BulletScene^ scene)
{

}

void RigidBodyDefinition::createStaticProduct(SimObjectBase^ instance, BulletScene^ scene)
{

}

}