#include "StdAfx.h"
#include "..\include\RigidBodyDefinition.h"

namespace BulletPlugin
{

RigidBodyDefinition::RigidBodyDefinition(String^ name)
:SimElementDefinition(name),
editInterface(nullptr)
{

}

void RigidBodyDefinition::registerScene(SimSubScene^ subscene, SimObjectBase^ instance)
{

}

EditInterface^ RigidBodyDefinition::getEditInterface()
{
	if(editInterface == nullptr)
	{
		editInterface = ReflectedEditInterface::createEditInterface(this, memberScanner, this->Name + " - Rigid Body", nullptr);
	}
	return editInterface;
}

}