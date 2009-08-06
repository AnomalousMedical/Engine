#pragma once

#include "TypedConstraintDefinition.h"

using namespace System;
using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace Engine::Saving;

namespace BulletPlugin
{


ref class RotationalLimitMotorDefinition;
ref class TranslationalLimitMotorDefinition;

ref class Generic6DofConstraintDefinition : public TypedConstraintDefinition
{
private:
	[Editable] TranslationalLimitMotorDefinition^ translationMotor;
	[Editable] RotationalLimitMotorDefinition^ xRotMotor;
	[Editable] RotationalLimitMotorDefinition^ yRotMotor;
	[Editable] RotationalLimitMotorDefinition^ zRotMotor;


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

	property TranslationalLimitMotorDefinition^ TranslationMotor
	{
		TranslationalLimitMotorDefinition^ get()
		{
			return translationMotor;
		}
	}

	property RotationalLimitMotorDefinition^ XRotMotor
	{
		RotationalLimitMotorDefinition^ get()
		{
			return xRotMotor;
		}
	}

	property RotationalLimitMotorDefinition^ YRotMotor
	{
		RotationalLimitMotorDefinition^ get()
		{
			return yRotMotor;
		}
	}

	property RotationalLimitMotorDefinition^ ZRotMotor
	{
		RotationalLimitMotorDefinition^ get()
		{
			return zRotMotor;
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