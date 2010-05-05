#pragma once

#include "AutoPtr.h"

using namespace Engine;
using namespace Engine::Editing;
using namespace Engine::Saving;

namespace BulletPlugin
{

[Engine::Attributes::NativeSubsystemType]
public ref class TranslationalLimitMotorDefinition : Saveable
{
private:
	AutoPtr<btTranslationalLimitMotor> motor;

internal:
	property btTranslationalLimitMotor* Motor
	{
		btTranslationalLimitMotor* get()
		{
			return motor.Get();
		}
	}

public:
	TranslationalLimitMotorDefinition(void);

	[Editable]
	property Vector3 LowerLimit
	{
		Vector3 get()
		{
			return Vector3(motor->m_lowerLimit.x(), motor->m_lowerLimit.y(), motor->m_lowerLimit.z());
		}
		void set(Vector3 value)
		{
			motor->m_lowerLimit.setX(value.x);
			motor->m_lowerLimit.setY(value.y);
			motor->m_lowerLimit.setZ(value.z);
		}
	}
    
	[Editable]
	property Vector3 UpperLimit
	{
		Vector3 get()
		{
			return Vector3(motor->m_upperLimit.x(), motor->m_upperLimit.y(), motor->m_upperLimit.z());
		}
		void set(Vector3 value)
		{
			motor->m_upperLimit.setX(value.x);
			motor->m_upperLimit.setY(value.y);
			motor->m_upperLimit.setZ(value.z);
		}
	}
    
	[Editable]
	property Vector3 AccumulatedImpulse
	{
		Vector3 get()
		{
			return Vector3(motor->m_accumulatedImpulse.x(), motor->m_accumulatedImpulse.y(), motor->m_accumulatedImpulse.z());
		}
		void set(Vector3 value)
		{
			motor->m_accumulatedImpulse.setX(value.x);
			motor->m_accumulatedImpulse.setY(value.y);
			motor->m_accumulatedImpulse.setZ(value.z);
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
	property float Restitution
	{
		float get()
		{
			return motor->m_restitution;
		}
		void set(float value)
		{
			motor->m_restitution = value;
		}
	}
	
	[Editable]
	property bool EnableMotorX
	{
		bool get()
		{
			return motor->m_enableMotor[0];
		}
		void set(bool value)
		{
			motor->m_enableMotor[0] = value;
		}
	}

	[Editable]
	property bool EnableMotorY
	{
		bool get()
		{
			return motor->m_enableMotor[1];
		}
		void set(bool value)
		{
			motor->m_enableMotor[1] = value;
		}
	}

	[Editable]
	property bool EnableMotorZ
	{
		bool get()
		{
			return motor->m_enableMotor[2];
		}
		void set(bool value)
		{
			motor->m_enableMotor[2] = value;
		}
	}
    
	[Editable]
	property Vector3 TargetVelocity
	{
		Vector3 get()
		{
			return Vector3(motor->m_targetVelocity.x(), motor->m_targetVelocity.y(), motor->m_targetVelocity.z());
		}
		void set(Vector3 value)
		{
			motor->m_targetVelocity.setX(value.x);
			motor->m_targetVelocity.setY(value.y);
			motor->m_targetVelocity.setZ(value.z);
		}
	}
    
	[Editable]
	property Vector3 MaxMotorForce
	{
		Vector3 get()
		{
			return Vector3(motor->m_maxMotorForce.x(), motor->m_maxMotorForce.y(), motor->m_maxMotorForce.z());
		}
		void set(Vector3 value)
		{
			motor->m_maxMotorForce.setX(value.x);
			motor->m_maxMotorForce.setY(value.y);
			motor->m_maxMotorForce.setZ(value.z);
		}
	}
    
	[Editable]
	property Vector3 CurrentLimitError
	{
		Vector3 get()
		{
			return Vector3(motor->m_currentLimitError.x(), motor->m_currentLimitError.y(), motor->m_currentLimitError.z());
		}
		void set(Vector3 value)
		{
			motor->m_currentLimitError.setX(value.x);
			motor->m_currentLimitError.setY(value.y);
			motor->m_currentLimitError.setZ(value.z);
		}
	}
    
	[Editable]
	property int CurrentLimitX
	{
		int get()
		{
			return motor->m_currentLimit[0];
		}
		void set(int value)
		{
			motor->m_currentLimit[0] = value;
		}
	}

	[Editable]
	property int CurrentLimitY
	{
		int get()
		{
			return motor->m_currentLimit[1];
		}
		void set(int value)
		{
			motor->m_currentLimit[1] = value;
		}
	}

	[Editable]
	property int CurrentLimitZ
	{
		int get()
		{
			return motor->m_currentLimit[2];
		}
		void set(int value)
		{
			motor->m_currentLimit[2] = value;
		}
	}

	[Editable]
	property Vector3 NormalCFM
	{
		Vector3 get()
		{
			return Vector3(motor->m_normalCFM.x(), motor->m_normalCFM.y(), motor->m_normalCFM.z());
		}
		void set(Vector3 value)
		{
			motor->m_normalCFM.setX(value.x);
			motor->m_normalCFM.setY(value.y);
			motor->m_normalCFM.setZ(value.z);
		}
	}

	[Editable]
	property Vector3 StopERP
	{
		Vector3 get()
		{
			return Vector3(motor->m_stopERP.x(), motor->m_stopERP.y(), motor->m_stopERP.z());
		}
		void set(Vector3 value)
		{
			motor->m_stopERP.setX(value.x);
			motor->m_stopERP.setY(value.y);
			motor->m_stopERP.setZ(value.z);
		}
	}

	[Editable]
	property Vector3 StopCFM
	{
		Vector3 get()
		{
			return Vector3(motor->m_stopCFM.x(), motor->m_stopCFM.y(), motor->m_stopCFM.z());
		}
		void set(Vector3 value)
		{
			motor->m_stopCFM.setX(value.x);
			motor->m_stopCFM.setY(value.y);
			motor->m_stopCFM.setZ(value.z);
		}
	}

//Saving

protected:
	TranslationalLimitMotorDefinition(LoadInfo^ info);

public:
	virtual void getInfo(SaveInfo^ info);

//End Saving
};

}