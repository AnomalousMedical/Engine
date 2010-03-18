#include "StdAfx.h"
#include "..\include\SoftBodyAngularJointDefinition.h"
#include "BulletScene.h"
#include "BulletFactory.h"
#include "BulletInterface.h"
#include "RigidBody.h"
#include "SoftBody.h"
#include "SoftBodyAnchor.h"

namespace BulletPlugin
{

SoftBodyAngularJointDefinition::SoftBodyAngularJointDefinition(System::String^ name)
:BulletElementDefinition(name)
{
}

SoftBodyAngularJointDefinition::~SoftBodyAngularJointDefinition(void)
{
}

void SoftBodyAngularJointDefinition::registerScene(SimSubScene^ subscene, SimObjectBase^ instance)
{
	if (subscene->hasSimElementManagerType(BulletScene::typeid))
    {
        BulletScene^ sceneManager = subscene->getSimElementManager<BulletScene^>();
        sceneManager->getBulletFactory()->addSoftBodyAnchorOrJointDefinition(this, instance);
    }
    else
    {
		Logging::Log::Default->sendMessage("Cannot add SoftBodyAngularJointDefinition {0} to SimSubScene {1} because it does not contain a BulletSceneManager.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, subscene->Name);
    }
}

EditInterface^ SoftBodyAngularJointDefinition::getEditInterface()
{
	if(editInterface == nullptr)
	{
		editInterface = ReflectedEditInterface::createEditInterface(this, memberScanner, this->Name + " - Soft Body", nullptr);
	}
	return editInterface;
}

void SoftBodyAngularJointDefinition::createProduct(SimObjectBase^ instance, BulletScene^ scene)
{
	RigidBody^ rb = nullptr;
	SoftBody^ sb = nullptr;

	SimObject^ other = instance->getOtherSimObject(rigidBodySimObject);
	if(other != nullptr)
	{
		rb = dynamic_cast<RigidBody^>(other->getElement(rigidBodyElement));
		if(rb != nullptr)
		{
			other = instance->getOtherSimObject(softBodySimObject);
			if(other != nullptr)
			{
				sb = dynamic_cast<SoftBody^>(other->getElement(softBodyElement));
				if(sb != nullptr)
				{
					if(sb->Body->m_clusters.size() > 0)
					{
						//incomplete, build joint here
					}
					else
					{
						Logging::Log::Default->sendMessage("Cannot create SoftBodyAngularJoint {0} because SoftBodyElement {1} in SimObject {2} does not have any clusters.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, softBodyElement, softBodySimObject);
					}
				}
				else
				{
					Logging::Log::Default->sendMessage("Cannot create SoftBodyAngularJoint {0} because SoftBodyElement {1} in SimObject {2} cannot be found.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, softBodyElement, softBodySimObject);
				}
			}
			else
			{
				Logging::Log::Default->sendMessage("Cannot create SoftBodyAngularJoint {0} because SimObject for the soft body {1} cannot be found.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, softBodySimObject);
			}
		}
		else
		{
			Logging::Log::Default->sendMessage("Cannot create SoftBodyAngularJoint {0} because RigidBodyElement {1} in SimObject {2} cannot be found.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, rigidBodyElement, rigidBodySimObject);
		}
	}
	else
	{
		Logging::Log::Default->sendMessage("Cannot create SoftBodyAngularJoint {0} because SimObject for the rigid body {1} cannot be found.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, rigidBodySimObject);
	}
}

void SoftBodyAngularJointDefinition::createStaticProduct(SimObjectBase^ instance, BulletScene^ scene)
{

}

//Saving
SoftBodyAngularJointDefinition::SoftBodyAngularJointDefinition(LoadInfo^ info)
:BulletElementDefinition(info)
{
	rigidBodySimObject = info->GetString("rigidBodySimObject");
	rigidBodyElement = info->GetString("rigidBodyElement");
	softBodySimObject = info->GetString("softBodySimObject");
	softBodyElement = info->GetString("softBodyElement");
}

void SoftBodyAngularJointDefinition::getInfo(SaveInfo^ info)
{
	BulletElementDefinition::getInfo(info);

	info->AddValue("rigidBodySimObject", rigidBodySimObject);
	info->AddValue("rigidBodyElement", rigidBodyElement);
	info->AddValue("softBodySimObject", softBodySimObject);
	info->AddValue("softBodyElement", softBodyElement);
}
//End saving

}