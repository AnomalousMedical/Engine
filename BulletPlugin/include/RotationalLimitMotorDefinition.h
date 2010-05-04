#pragma once

#include "AutoPtr.h"

using namespace Engine;
using namespace Engine::Editing;
using namespace Engine::Saving;

namespace BulletPlugin
{

[Engine::Attributes::NativeSubsystemType]
public ref class RotationalLimitMotorDefinition : Saveable
{
private:
	AutoPtr<btRotationalLimitMotor> motor;

internal:
	property btRotationalLimitMotor* Motor
	{
		btRotationalLimitMotor* get()
		{
			return motor.Get();
		}
	}

public:
	RotationalLimitMotorDefinition(void);

	[Editable]
	property float LoLimit
	{
		float get()
		{
			return motor->m_loLimit;
		}
		void set(float value)
		{
			motor->m_loLimit = value;
		}
	}

	[Editable]
	property float HiLimit
	{
		float get()
		{
			return motor->m_hiLimit;
		}
		void set(float value)
		{
			motor->m_hiLimit = value;
		}
	}

	[Editable]
	property float TargetVelocity
	{
		float get()
		{
			return motor->m_targetVelocity;
		}
		void set(float value)
		{
			motor->m_targetVelocity = value;
		}
	}

	[Editable]
	property float MaxMotorForce
	{
		float get()
		{
			return motor->m_maxMotorForce;
		}
		void set(float value)
		{
			motor->m_maxMotorForce = value;
		}
	}

	[Editable]
	property float MaxLimitForce
	{
		float get()
		{
			return motor->m_maxLimitForce;
		}
		void set(float value)
		{
			motor->m_maxLimitForce = value;
		}
	}

	[Editable]
	property float Damping
	{
		float get()
		{
			return motor->m_damping;
		}
		void set(float value)
		{
			motor->m_damping = value;
		}
	}

	[Editable]
	property float LimitSoftness
	{
		float get()
		{
			return motor->m_limitSoftness;
		}
		void set(float value)
		{
			motor->m_limitSoftness = value;
		}
	}

	/*[Editable]
	property float ERP
	{
		float get()
		{
			return motor->m_ERP;
		}
		void set(float value)
		{
			motor->m_ERP = value;
		}
	}*/

	[Editable]
	property float Bounce
	{
		float get()
		{
			return motor->m_bounce;
		}
		void set(float value)
		{
			motor->m_bounce = value;
		}
	}

	[Editable]
	property bool EnableMotor
	{
		bool get()
		{
			return motor->m_enableMotor;
		}
		void set(bool value)
		{
			motor->m_enableMotor = value;
		}
	}

	[Editable]
	property float CurrentLimitError
	{
		float get()
		{
			return motor->m_currentLimitError;
		}
		void set(float value)
		{
			motor->m_currentLimitError = value;
		}
	}

	[Editable]
	property int CurrentLimit
	{
		int get()
		{
			return motor->m_currentLimit;
		}
		void set(int value)
		{
			motor->m_currentLimit = value;
		}
	}

	[Editable]
	property float AccumulatedImpulse
	{
		float get()
		{
			return motor->m_accumulatedImpulse;
		}
		void set(float value)
		{
			motor->m_accumulatedImpulse = value;
		}
	}
	//Saving

protected:
RotationalLimitMotorDefinition(LoadInfo^ info);

public:
virtual void getInfo(SaveInfo^ info);

//End Saving
};

}