#include "StdAfx.h"
#include "..\include\SoftBodyAnchorDefinition.h"
#include "BulletScene.h"
#include "BulletFactory.h"
#include "BulletInterface.h"
#include "RigidBody.h"
#include "SoftBody.h"
#include "SoftBodyAnchor.h"

namespace BulletPlugin
{

SoftBodyAnchorDefinition::SoftBodyAnchorDefinition(System::String^ name)
:BulletElementDefinition(name),
findClosestNode(true),
specificNode(0)
{
}

SoftBodyAnchorDefinition::~SoftBodyAnchorDefinition(void)
{
}

void SoftBodyAnchorDefinition::registerScene(SimSubScene^ subscene, SimObjectBase^ instance)
{
	if (subscene->hasSimElementManagerType(BulletScene::typeid))
    {
        BulletScene^ sceneManager = subscene->getSimElementManager<BulletScene^>();
        sceneManager->getBulletFactory()->addSoftBodyAnchorDefinition(this, instance);
    }
    else
    {
		Logging::Log::Default->sendMessage("Cannot add SoftBodyAnchorDefinition {0} to SimSubScene {1} because it does not contain a BulletSceneManager.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, subscene->Name);
    }
}

EditInterface^ SoftBodyAnchorDefinition::getEditInterface()
{
	if(editInterface == nullptr)
	{
		editInterface = ReflectedEditInterface::createEditInterface(this, memberScanner, this->Name + " - Soft Body", nullptr);
	}
	return editInterface;
}

#pragma unmanaged
int findClosestNodeAt(float posX, float posY, float posZ, btSoftBody* softBody)
{
	btSoftBody::tNodeArray nodeArray = softBody->m_nodes;
	int numNodes = nodeArray.size();
	float closestDistanceSq = FLT_MAX;
	float currentDistanceSq;
	btVector3 anchorPos(posX, posY, posZ);
	int closestIndex = 0;
	for(int i = 0; i < numNodes; ++i)
	{
		currentDistanceSq = (nodeArray[i].m_x - anchorPos).length2();
		if(currentDistanceSq < closestDistanceSq)
		{
			closestIndex = i;
			closestDistanceSq = currentDistanceSq;
		}
	}
	return closestIndex;
}
#pragma managed

void SoftBodyAnchorDefinition::createProduct(SimObjectBase^ instance, BulletScene^ scene)
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
					Vector3 trans = instance->Translation;
					int node = specificNode;
					if(findClosestNode)
					{
						node = findClosestNodeAt(trans.x, trans.y, trans.z, sb->Body);
					}
					sb->Body->appendAnchor(node, rb->Body, disableCollisionBetweenNodes);
					SoftBodyAnchor^ anchor = gcnew SoftBodyAnchor(this);
					instance->addElement(anchor);
				}
				else
				{
					Logging::Log::Default->sendMessage("Cannot create SoftBodyAnchor {0} because SoftBodyElement {1} in SimObject {2} cannot be found.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, softBodyElement, softBodySimObject);
				}
			}
			else
			{
				Logging::Log::Default->sendMessage("Cannot create SoftBodyAnchor {0} because SimObject for the soft body {1} cannot be found.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, softBodySimObject);
			}
		}
		else
		{
			Logging::Log::Default->sendMessage("Cannot create SoftBodyAnchor {0} because RigidBodyElement {1} in SimObject {2} cannot be found.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, rigidBodyElement, rigidBodySimObject);
		}
	}
	else
	{
		Logging::Log::Default->sendMessage("Cannot create SoftBodyAnchor {0} because SimObject for the rigid body {1} cannot be found.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, rigidBodySimObject);
	}
}

void SoftBodyAnchorDefinition::createStaticProduct(SimObjectBase^ instance, BulletScene^ scene)
{

}

//Saving
SoftBodyAnchorDefinition::SoftBodyAnchorDefinition(LoadInfo^ info)
:BulletElementDefinition(info)
{
	rigidBodySimObject = info->GetString("rigidBodySimObject");
	rigidBodyElement = info->GetString("rigidBodyElement");
	softBodySimObject = info->GetString("softBodySimObject");
	softBodyElement = info->GetString("softBodyElement");
	disableCollisionBetweenNodes = info->GetBoolean("disableCollisionBetweenNodes");
	findClosestNode = info->GetBoolean("findClosestNode", true);
	specificNode = info->GetInt32("specificNode", 0);
}

void SoftBodyAnchorDefinition::getInfo(SaveInfo^ info)
{
	BulletElementDefinition::getInfo(info);

	info->AddValue("rigidBodySimObject", rigidBodySimObject);
	info->AddValue("rigidBodyElement", rigidBodyElement);
	info->AddValue("softBodySimObject", softBodySimObject);
	info->AddValue("softBodyElement", softBodyElement);
	info->AddValue("disableCollisionBetweenNodes", disableCollisionBetweenNodes);
	info->AddValue("findClosestNode", findClosestNode);
	info->AddValue("specificNode", specificNode);
}
//End saving

}