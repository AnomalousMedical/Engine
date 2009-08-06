#pragma once

#include "TypedConstraintDefinition.h"

using namespace System;
using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace Engine::Saving;

namespace BulletPlugin
{

ref class Generic6DofConstraintDefinition : public TypedConstraintDefinition
{
protected:
	virtual TypedConstraintElement^ createConstraint(RigidBody^ rbA, RigidBody^ rbB, SimObjectBase^ instance, BulletScene^ scene) override;

internal:
	static SimElementDefinition^ Create(String^ name, EditUICallback^ callback)
	{
		return gcnew Generic6DofConstraintDefinition(name);
	}

public:
	Generic6DofConstraintDefinition(String^ name);

	virtual property String^ JointType
	{
		virtual String^ get() override
		{
			return "Generic 6 Dof Constraint";
		}
	}

//Saving

protected:
	Generic6DofConstraintDefinition(LoadInfo^ info);

public:
	virtual void getInfo(SaveInfo^ info) override;

//End Saving
};

}