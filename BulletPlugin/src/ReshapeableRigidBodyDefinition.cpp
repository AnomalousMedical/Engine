#include "StdAfx.h"
#include "..\include\ReshapeableRigidBodyDefinition.h"
#include "ReshapeableRigidBody.h"

namespace BulletPlugin
{

ReshapeableRigidBodyDefinition::ReshapeableRigidBodyDefinition(String^ name)
:RigidBodyDefinition(name)
{
}

ReshapeableRigidBodyDefinition::~ReshapeableRigidBodyDefinition(void)
{
}

void ReshapeableRigidBodyDefinition::createProduct(SimObjectBase^ instance, BulletScene^ scene)
{
	btCollisionShape* shape = new btCompoundShape(false);
	constructionInfo->m_collisionShape = shape;
	/*if(constructionInfo->m_mass != 0.0f)
	{
		shape->calculateLocalInertia(constructionInfo->m_mass, constructionInfo->m_localInertia);
	}*/
	ReshapeableRigidBody^ rigidBody = gcnew ReshapeableRigidBody(this, scene, instance->Translation, instance->Rotation);
	instance->addElement(rigidBody);
}

EditInterface^ ReshapeableRigidBodyDefinition::getEditInterface()
{
	if(editInterface == nullptr)
	{
		editInterface = ReflectedEditInterface::createEditInterface(this, memberScanner, this->Name + " - Reshapeable Rigid Body", nullptr);
		editInterface->IconReferenceTag = EngineIcons::RigidBody;
	}
	return editInterface;
}

ReshapeableRigidBodyDefinition::ReshapeableRigidBodyDefinition(LoadInfo^ info)
:RigidBodyDefinition(info)
{

}

void ReshapeableRigidBodyDefinition::getInfo(SaveInfo^ info)
{
	RigidBodyDefinition::getInfo(info);
}

}