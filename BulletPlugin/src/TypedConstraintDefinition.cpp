#include "StdAfx.h"
#include "..\include\TypedConstraintDefinition.h"
#include "BulletScene.h"
#include "BulletFactory.h"
#include "BulletInterface.h"
#include "RigidBody.h"
#include "TypedConstraintElement.h"

namespace BulletPlugin
{

TypedConstraintDefinition::TypedConstraintDefinition(String^ name)
:BulletElementDefinition(name)
{
}

void TypedConstraintDefinition::createProduct(SimObjectBase^ instance, BulletScene^ scene)
{
	RigidBody^ rbA = nullptr;
	RigidBody^ rbB = nullptr;

	SimObject^ other = instance->getOtherSimObject(rigidBodyASimObject);
	if(other != nullptr)
	{
		 rbA = dynamic_cast<RigidBody^>(other->getElement(rigidBodyAElement));
	}
	
	other = instance->getOtherSimObject(rigidBodyBSimObject);
	if(other != nullptr)
	{
		rbB = dynamic_cast<RigidBody^>(other->getElement(rigidBodyBElement));
	}
	
	TypedConstraintElement^ element = createConstraint(rbA, rbB, instance, scene);
	if(element != nullptr)
	{
		instance->addElement(element);
	}
}

void TypedConstraintDefinition::createStaticProduct(SimObjectBase^ instance, BulletScene^ scene)
{

}

void TypedConstraintDefinition::registerScene(SimSubScene^ subscene, SimObjectBase^ instance)
{
	if (subscene->hasSimElementManagerType(BulletScene::typeid))
    {
        BulletScene^ sceneManager = subscene->getSimElementManager<BulletScene^>();
        sceneManager->getBulletFactory()->addTypedConstraint(this, instance);
    }
    else
    {
		Logging::Log::Default->sendMessage("Cannot add PhysActorDefinition {0} to SimSubScene {1} because it does not contain a PhysXSceneManager.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, subscene->Name);
    }
}

EditInterface^ TypedConstraintDefinition::getEditInterface()
{
	if(editInterface == nullptr)
	{
		editInterface = ReflectedEditInterface::createEditInterface(this, memberScanner, this->Name + " - " + JointType, nullptr);
		editInterface->IconReferenceTag = EngineIcons::Joint;
	}
	return editInterface;
}

//Saving

TypedConstraintDefinition::TypedConstraintDefinition(LoadInfo^ info)
:BulletElementDefinition(info)
{
	rigidBodyASimObject = info->GetString("RigidBodyASimObject");
	rigidBodyAElement = info->GetString("RigidBodyAElement");
	rigidBodyBSimObject = info->GetString("RigidBodyBSimObject");
	rigidBodyBElement = info->GetString("RigidBodyBElement");
}

void TypedConstraintDefinition::getInfo(SaveInfo^ info)
{
	info->AddValue("RigidBodyASimObject", rigidBodyASimObject);
	info->AddValue("RigidBodyAElement", rigidBodyAElement);
	info->AddValue("RigidBodyBSimObject", rigidBodyBSimObject);
	info->AddValue("RigidBodyBElement", rigidBodyBElement);
	SimElementDefinition::getInfo(info);
}

//End Saving

}