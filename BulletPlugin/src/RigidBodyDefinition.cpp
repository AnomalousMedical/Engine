#include "StdAfx.h"
#include "..\include\RigidBodyDefinition.h"
#include "BulletScene.h"
#include "BulletFactory.h"
#include "BulletInterface.h"
#include "RigidBody.h"
#include "BulletShapeCollection.h"
#include "BulletShapeRepository.h"

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
editInterface(nullptr),
linearVelocity(Vector3::Zero),
angularVelocity(Vector3::Zero),
activationState(ActivationState::ActiveTag),
anisotropicFriction(Vector3(1.0f, 1.0f, 1.0f)),
deactivationTime(0.0f),
flags(static_cast<CollisionFlags>(0)),
hitFraction(1.0f),
maxContactDistance(0.0f),
collisionFilterMask(-1),
collisionFilterGroup(1)
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
		Logging::Log::Default->sendMessage("Cannot add RigidBodyDefinition {0} to SimSubScene {1} because it does not contain a BulletSceneManager.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, subscene->Name);
    }
}

EditInterface^ RigidBodyDefinition::getEditInterface()
{
	if(editInterface == nullptr)
	{
		editInterface = ReflectedEditInterface::createEditInterface(this, memberScanner, this->Name + " - Rigid Body", nullptr);
		editInterface->IconReferenceTag = EngineIcons::RigidBody;
	}
	return editInterface;
}

void RigidBodyDefinition::createProduct(SimObjectBase^ instance, BulletScene^ scene)
{
	BulletShapeRepository^ repository = BulletInterface::Instance->ShapeRepository;
	if(repository->containsValidCollection(shapeName))
	{
		btCollisionShape* shape = repository->getCollection(shapeName)->CollisionShape;
		constructionInfo->m_collisionShape = shape;
		if(constructionInfo->m_mass != 0.0f)
		{
			shape->calculateLocalInertia(constructionInfo->m_mass, constructionInfo->m_localInertia);
		}
		RigidBody^ rigidBody = gcnew RigidBody(this, scene, instance->Translation, instance->Rotation);
		instance->addElement(rigidBody);
	}
	else
	{
		Logging::Log::Default->sendMessage("Could not find collision shape named {0}.", Logging::LogLevel::Warning, "BulletPlugin", shapeName);
	}
}

void RigidBodyDefinition::createStaticProduct(SimObjectBase^ instance, BulletScene^ scene)
{
	//This code allows the rigid bodies to be seen, however it will still attempt to move them it is left as a reference but breaks things badly.
	/*RigidBodyDefinition^ staticBody = gcnew RigidBodyDefinition(this->Name);
	staticBody->ShapeName = this->ShapeName;
	staticBody->Mass = 10;
	staticBody->CollisionFilterMask = 0;
	staticBody->Flags = CollisionFlags::NoContactResponse | CollisionFlags::StaticObject;

	BulletShapeRepository^ repository = BulletInterface::Instance->ShapeRepository;
	if(repository->containsValidCollection(shapeName))
	{
		btCollisionShape* shape = repository->getCollection(shapeName)->CollisionShape;
		staticBody->constructionInfo->m_collisionShape = shape;
		if(staticBody->constructionInfo->m_mass != 0.0f)
		{
			shape->calculateLocalInertia(staticBody->constructionInfo->m_mass, constructionInfo->m_localInertia);
		}
		RigidBody^ rigidBody = gcnew RigidBody(staticBody, scene, instance->Translation, instance->Rotation);
		instance->addElement(rigidBody);
	}
	else
	{
		Logging::Log::Default->sendMessage("Could not find collision shape named {0}.", Logging::LogLevel::Warning, "BulletPlugin", shapeName);
	}*/
}

//Saving
RigidBodyDefinition::RigidBodyDefinition(LoadInfo^ info)
:BulletElementDefinition(info)
{
	constructionInfo = createConstructionInfo();
	AngularDamping = info->GetFloat("AngularDamping");
	AngularSleepingThreshold = info->GetFloat("AngularSleepingThreshold");
	AngularVelocity = info->GetVector3("AngularVelocity");
	Friction = info->GetFloat("Friction");
	LinearDamping = info->GetFloat("LinearDamping");
	LinearSleepingThreshold = info->GetFloat("LinearSleepingThreshold");
	LinearVelocity = info->GetVector3("LinearVelocity");
	Mass = info->GetFloat("Mass");
	Restitution = info->GetFloat("Restitution");
	CurrentActivationState = info->GetValue<ActivationState>("CurrentActivationState");
	AnisotropicFriction = info->GetVector3("AnisotropicFriction");
	DeactivationTime = info->GetFloat("DeactivationTime");
	Flags = info->GetValue<CollisionFlags>("Flags");
	HitFraction = info->GetFloat("HitFraction");
	ShapeName = info->GetString("ShapeName");
	MaxContactDistance = info->GetFloat("MaxContactDistance", 0.0f);
	CollisionFilterMask = info->GetInt16("CollisionFilterMask", -1);
	CollisionFilterGroup = info->GetInt16("CollisionFilterGroup", 1);
}

void RigidBodyDefinition::getInfo(SaveInfo^ info)
{
	BulletElementDefinition::getInfo(info);
	info->AddValue("AngularDamping", AngularDamping);
	info->AddValue("AngularSleepingThreshold", AngularSleepingThreshold);
	info->AddValue("AngularVelocity", AngularVelocity);
	info->AddValue("Friction", Friction);
	info->AddValue("LinearDamping", LinearDamping);
	info->AddValue("LinearSleepingThreshold", LinearSleepingThreshold);
	info->AddValue("LinearVelocity", LinearVelocity);
	info->AddValue("Mass", Mass);
	info->AddValue("Restitution", Restitution);
	info->AddValue("CurrentActivationState", CurrentActivationState);
	info->AddValue("AnisotropicFriction", AnisotropicFriction);
	info->AddValue("DeactivationTime", DeactivationTime);
	info->AddValue("Flags", Flags);
	info->AddValue("HitFraction", HitFraction);
	info->AddValue("ShapeName", ShapeName);
	info->AddValue("MaxContactDistance", MaxContactDistance);
	info->AddValue("CollisionFilterMask", CollisionFilterMask);
	info->AddValue("CollisionFilterGroup", CollisionFilterGroup);
}
//End saving

}