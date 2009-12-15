#include "StdAfx.h"
#include "..\include\SoftBodyDefinition.h"
#include "BulletScene.h"
#include "BulletFactory.h"
#include "BulletInterface.h"
#include "SoftBody.h"

namespace BulletPlugin
{


SoftBodyDefinition::SoftBodyDefinition(String^ name)
:BulletElementDefinition(name),
editInterface(nullptr)
{
}

SoftBodyDefinition::~SoftBodyDefinition(void)
{
}

void SoftBodyDefinition::registerScene(SimSubScene^ subscene, SimObjectBase^ instance)
{
	if (subscene->hasSimElementManagerType(BulletScene::typeid))
    {
        BulletScene^ sceneManager = subscene->getSimElementManager<BulletScene^>();
        sceneManager->getBulletFactory()->addSoftBody(this, instance);
    }
    else
    {
		Logging::Log::Default->sendMessage("Cannot add SoftBodyDefinition {0} to SimSubScene {1} because it does not contain a BulletSceneManager.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, subscene->Name);
    }
}

EditInterface^ SoftBodyDefinition::getEditInterface()
{
	if(editInterface == nullptr)
	{
		editInterface = ReflectedEditInterface::createEditInterface(this, memberScanner, this->Name + " - Soft Body", nullptr);
	}
	return editInterface;
}

void SoftBodyDefinition::createProduct(SimObjectBase^ instance, BulletScene^ scene)
{
	SoftBody^ softBody = gcnew SoftBody(this, scene);
	instance->addElement(softBody);
}

void SoftBodyDefinition::createStaticProduct(SimObjectBase^ instance, BulletScene^ scene)
{

}

//Saving
SoftBodyDefinition::SoftBodyDefinition(LoadInfo^ info)
:BulletElementDefinition(info)
{
	
}

void SoftBodyDefinition::getInfo(SaveInfo^ info)
{
	BulletElementDefinition::getInfo(info);
}
//End saving

}