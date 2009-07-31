#include "StdAfx.h"
#include "..\include\RigidBodyDefinition.h"
#include "BulletScene.h"
#include "BulletFactory.h"
#include "BulletInterface.h"
#include "RigidBody.h"

namespace BulletPlugin
{

#pragma unmanaged

btRigidBody::btRigidBodyConstructionInfo* createConstructionInfo()
{
	return new btRigidBody::btRigidBodyConstructionInfo(0.0f, 0, 0);
}

#pragma managed

RigidBodyDefinition::RigidBodyDefinition(String^ name)
:BulletElementDefinition(name),
editInterface(nullptr)
{
	constructionInfo = createConstructionInfo();
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
	btCollisionShape* shape = new btSphereShape(1.);
	constructionInfo->m_collisionShape = shape;
	shape->calculateLocalInertia(constructionInfo->m_mass, constructionInfo->m_localInertia);
	RigidBody^ rigidBody = gcnew RigidBody(this, scene);
	rigidBody->setWorldTransform(instance->Translation, instance->Rotation);
	instance->addElement(rigidBody);
}

void RigidBodyDefinition::createStaticProduct(SimObjectBase^ instance, BulletScene^ scene)
{

}

//Saving
RigidBodyDefinition::RigidBodyDefinition(LoadInfo^ info)
:BulletElementDefinition(info)
{
	constructionInfo = createConstructionInfo();
	Mass = info->GetFloat("Mass");
}

void RigidBodyDefinition::getInfo(SaveInfo^ info)
{
	BulletElementDefinition::getInfo(info);
	info->AddValue("Mass", Mass);
}
//End saving

}