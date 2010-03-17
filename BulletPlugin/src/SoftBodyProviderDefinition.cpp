#include "StdAfx.h"
#include "..\include\SoftBodyProviderDefinition.h"
#include "BulletScene.h"
#include "BulletFactory.h"
#include "BulletInterface.h"
#include "SoftBodyProvider.h"

namespace BulletPlugin
{

SoftBodyProviderDefinition::SoftBodyProviderDefinition(String^ elementName)
:SimElementDefinition(elementName)
{
}

SoftBodyProviderDefinition::~SoftBodyProviderDefinition(void)
{
}

void SoftBodyProviderDefinition::registerScene(SimSubScene^ subscene, SimObjectBase^ instance)
{
	if (subscene->hasSimElementManagerType(BulletScene::typeid))
    {
        BulletScene^ sceneManager = subscene->getSimElementManager<BulletScene^>();
        sceneManager->getBulletFactory()->addSoftBodyProviderDefinition(this, instance, subscene);
    }
    else
    {
		Logging::Log::Default->sendMessage("Cannot add SoftBodyProviderDefinition {0} to SimSubScene {1} because it does not contain a BulletSceneManager.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, subscene->Name);
    }
}

EditInterface^ SoftBodyProviderDefinition::getEditInterface()
{
	if(editInterface == nullptr)
	{
		editInterface = ReflectedEditInterface::createEditInterface(this, memberScanner, this->Name + " - Soft Body Provider", nullptr);
	}
	return editInterface;
}

void SoftBodyProviderDefinition::createProduct(SimObjectBase^ instance, BulletScene^ bulletScene, SimSubScene^ subScene)
{
	SoftBodyProvider^ provider = createProductImpl(instance, bulletScene, subScene);
	if(provider != nullptr)
	{
		provider->setInitialPosition(instance->Translation, instance->Rotation);
		instance->addElement(provider);
	}
	else
	{
		Logging::Log::Default->sendMessage("Error creating SoftBodyProvider {0}.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name);
	}
}

void SoftBodyProviderDefinition::createStaticProduct(SimObjectBase^ instance, BulletScene^ bulletScene, SimSubScene^ subScene)
{
	SoftBodyProvider^ provider = createProductImpl(instance, bulletScene, subScene);
	if(provider != nullptr)
	{
		provider->createStaticRepresentation();
		provider->setInitialPosition(instance->Translation, instance->Rotation);
		instance->addElement(provider);
	}
	else
	{
		Logging::Log::Default->sendMessage("Error creating SoftBodyProvider {0}.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name);
	}
}

SoftBodyProviderDefinition::SoftBodyProviderDefinition(LoadInfo^ info)
:SimElementDefinition(info)
{

}

void SoftBodyProviderDefinition::getInfo(SaveInfo^ info)
{
	SimElementDefinition::getInfo(info);
}

}