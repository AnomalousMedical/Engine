#include "StdAfx.h"
#include "..\include\Generic6DofConstraintDefinition.h"
#include "Generic6DofConstraintElement.h"

namespace BulletPlugin
{

Generic6DofConstraintDefinition::Generic6DofConstraintDefinition(String^ name)
:TypedConstraintDefinition(name)
{

}

TypedConstraintElement^ Generic6DofConstraintDefinition::createConstraint(RigidBody^ rbA, RigidBody^ rbB, BulletScene^ scene)
{
	if(rbA != nullptr && rbB != nullptr)
	{
		return gcnew Generic6DofConstraintElement(this, rbA, rbB, scene);
	}
	return nullptr;
}

Generic6DofConstraintDefinition::Generic6DofConstraintDefinition(LoadInfo^ info)
:TypedConstraintDefinition(info)
{

}

void Generic6DofConstraintDefinition::getInfo(SaveInfo^ info)
{
	TypedConstraintDefinition::getInfo(info);
}

}