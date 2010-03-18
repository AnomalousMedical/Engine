#include "StdAfx.h"
#include "..\include\SoftBodyLinearJointDefinition.h"
#include "BulletScene.h"
#include "BulletFactory.h"
#include "BulletInterface.h"
#include "RigidBody.h"
#include "SoftBody.h"
#include "SoftBodyAnchor.h"

namespace BulletPlugin
{

#pragma unmanaged

void addLinearJoint(btSoftBody* softBody, btRigidBody* rigidBody, float* position)
{
	btSoftBody::LJoint::Specs joint;
	joint.position	= btVector3(position[0], position[1], position[2]);
	softBody->appendLinearJoint(joint, rigidBody);
}

#pragma managed

SoftBodyLinearJointDefinition::SoftBodyLinearJointDefinition(System::String^ name)
:BulletElementDefinition(name)
{
}

SoftBodyLinearJointDefinition::~SoftBodyLinearJointDefinition(void)
{
}

void SoftBodyLinearJointDefinition::registerScene(SimSubScene^ subscene, SimObjectBase^ instance)
{
	if (subscene->hasSimElementManagerType(BulletScene::typeid))
    {
        BulletScene^ sceneManager = subscene->getSimElementManager<BulletScene^>();
        sceneManager->getBulletFactory()->addSoftBodyAnchorOrJointDefinition(this, instance);
    }
    else
    {
		Logging::Log::Default->sendMessage("Cannot add SoftBodyLinearJointDefinition {0} to SimSubScene {1} because it does not contain a BulletSceneManager.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, subscene->Name);
    }
}

EditInterface^ SoftBodyLinearJointDefinition::getEditInterface()
{
	if(editInterface == nullptr)
	{
		editInterface = ReflectedEditInterface::createEditInterface(this, memberScanner, this->Name + " - Soft Body", nullptr);
	}
	return editInterface;
}

void SoftBodyLinearJointDefinition::createProduct(SimObjectBase^ instance, BulletScene^ scene)
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
						Vector3 trans = instance->Translation;
						addLinearJoint(sb->Body, rb->Body, &trans.x);
						//incomplete needs sim element
					}
					else
					{
						Logging::Log::Default->sendMessage("Cannot create SoftBodyLinearJoint {0} because SoftBodyElement {1} in SimObject {2} does not have any clusters.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, softBodyElement, softBodySimObject);
					}
				}
				else
				{
					Logging::Log::Default->sendMessage("Cannot create SoftBodyLinearJoint {0} because SoftBodyElement {1} in SimObject {2} cannot be found.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, softBodyElement, softBodySimObject);
				}
			}
			else
			{
				Logging::Log::Default->sendMessage("Cannot create SoftBodyLinearJoint {0} because SimObject for the soft body {1} cannot be found.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, softBodySimObject);
			}
		}
		else
		{
			Logging::Log::Default->sendMessage("Cannot create SoftBodyLinearJoint {0} because RigidBodyElement {1} in SimObject {2} cannot be found.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, rigidBodyElement, rigidBodySimObject);
		}
	}
	else
	{
		Logging::Log::Default->sendMessage("Cannot create SoftBodyLinearJoint {0} because SimObject for the rigid body {1} cannot be found.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, rigidBodySimObject);
	}
}

void SoftBodyLinearJointDefinition::createStaticProduct(SimObjectBase^ instance, BulletScene^ scene)
{

}

//Saving
SoftBodyLinearJointDefinition::SoftBodyLinearJointDefinition(LoadInfo^ info)
:BulletElementDefinition(info)
{
	rigidBodySimObject = info->GetString("rigidBodySimObject");
	rigidBodyElement = info->GetString("rigidBodyElement");
	softBodySimObject = info->GetString("softBodySimObject");
	softBodyElement = info->GetString("softBodyElement");
}

void SoftBodyLinearJointDefinition::getInfo(SaveInfo^ info)
{
	BulletElementDefinition::getInfo(info);

	info->AddValue("rigidBodySimObject", rigidBodySimObject);
	info->AddValue("rigidBodyElement", rigidBodyElement);
	info->AddValue("softBodySimObject", softBodySimObject);
	info->AddValue("softBodyElement", softBodyElement);
}
//End saving

}