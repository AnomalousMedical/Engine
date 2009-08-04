#pragma once
#include "TypedConstraintElement.h"

using namespace System;
using namespace Engine;
using namespace Engine::ObjectManagement;

namespace BulletPlugin
{

ref class Generic6DofConstraintDefinition;
ref class RigidBody;
ref class BulletScene;

ref class Generic6DofConstraintElement : public TypedConstraintElement
{
private:
	btGeneric6DofConstraint* dof;

public:
	Generic6DofConstraintElement(Generic6DofConstraintDefinition^ definition, RigidBody^ rbA, RigidBody^ rbB, BulletScene^ scene);

	virtual ~Generic6DofConstraintElement(void);

	virtual SimElementDefinition^ saveToDefinition() override;
};

}