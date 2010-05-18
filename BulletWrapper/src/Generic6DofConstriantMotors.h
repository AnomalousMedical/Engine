#pragma once

//! Rotation Limit structure for generic joints
class RotationalLimitMotorDefinition
{
public:
    //! limit_parameters
    //!@{
    float m_loLimit;//!< joint limit
    float m_hiLimit;//!< joint limit
    float m_targetVelocity;//!< target motor velocity
    float m_maxMotorForce;//!< max force on motor
    float m_maxLimitForce;//!< max force on limit
    float m_damping;//!< Damping.
    float m_limitSoftness;//! Relaxation factor
    float m_normalCFM;//!< Constraint force mixing factor
    float m_stopERP;//!< Error tolerance factor when joint is at limit
    float m_stopCFM;//!< Constraint force mixing factor when joint is at limit
    float m_bounce;//!< restitution factor
    bool m_enableMotor;

    //!@}

    //! temp_variables
    //!@{
    float m_currentLimitError;//!  How much is violated this limit
    float m_currentPosition;     //!  current value of angle 
    int m_currentLimit;//!< 0=free, 1=at lo limit, 2=at hi limit
    float m_accumulatedImpulse;
    //!@}

	void fromBullet(btRotationalLimitMotor* btMotor)
	{
		m_loLimit = btMotor->m_loLimit;
		m_hiLimit = btMotor->m_hiLimit;
		m_targetVelocity = btMotor->m_targetVelocity;
		m_maxMotorForce = btMotor->m_maxMotorForce;
		m_maxLimitForce = btMotor->m_maxLimitForce;
		m_damping = btMotor->m_damping;
		m_limitSoftness = btMotor->m_limitSoftness;
		m_normalCFM = btMotor->m_normalCFM;
		m_stopERP = btMotor->m_stopERP;
		m_stopCFM = btMotor->m_stopCFM;
		m_bounce = btMotor->m_bounce;
		m_enableMotor = btMotor->m_enableMotor;
		m_currentLimitError = btMotor->m_currentLimitError;
		m_currentPosition = btMotor->m_currentPosition;
		m_currentLimit = btMotor->m_currentLimit;
		m_accumulatedImpulse = btMotor->m_accumulatedImpulse;
	}

	void toBullet(btRotationalLimitMotor* btMotor)
	{
		btMotor->m_loLimit = m_loLimit;
		btMotor->m_hiLimit = m_hiLimit;
		btMotor->m_targetVelocity = m_targetVelocity;
		btMotor->m_maxMotorForce = m_maxMotorForce;
		btMotor->m_maxLimitForce = m_maxLimitForce;
		btMotor->m_damping = m_damping;
		btMotor->m_limitSoftness = m_limitSoftness;
		btMotor->m_normalCFM = m_normalCFM;
		btMotor->m_stopERP = m_stopERP;
		btMotor->m_stopCFM = m_stopCFM;
		btMotor->m_bounce = m_bounce;
		btMotor->m_enableMotor = m_enableMotor;
		btMotor->m_currentLimitError = m_currentLimitError;
		btMotor->m_currentPosition = m_currentPosition;
		btMotor->m_currentLimit = m_currentLimit;
		btMotor->m_accumulatedImpulse = m_accumulatedImpulse;
	}
};



class TranslationalLimitMotorDefinition
{
public:
	Vector3 m_lowerLimit;//!< the constraint lower limits
    Vector3 m_upperLimit;//!< the constraint upper limits
    Vector3 m_accumulatedImpulse;
    //! Linear_Limit_parameters
    //!@{
    float	m_limitSoftness;//!< Softness for linear limit
    float	m_damping;//!< Damping for linear limit
    float	m_restitution;//! Bounce parameter for linear limit
	Vector3	m_normalCFM;//!< Constraint force mixing factor
    Vector3	m_stopERP;//!< Error tolerance factor when joint is at limit
	Vector3	m_stopCFM;//!< Constraint force mixing factor when joint is at limit
    //!@}
	//Replaced array with three bools
	bool m_enableMotorX; //must replace the bool array with 3 elements
	bool m_enableMotorY;
	bool m_enableMotorZ;
    Vector3	m_targetVelocity;//!< target motor velocity
    Vector3	m_maxMotorForce;//!< max force on motor
    Vector3	m_currentLimitError;//!  How much is violated this limit
    Vector3	m_currentLinearDiff;//!  Current relative offset of constraint frames
	//Replaced array with three ints
	int m_currentLimitX;//!< 0=free, 1=at lower limit, 2=at upper limit
	int m_currentLimitY;//!< 0=free, 1=at lower limit, 2=at upper limit
	int m_currentLimitZ;//!< 0=free, 1=at lower limit, 2=at upper limit

	void fromBullet(btTranslationalLimitMotor* motor)
	{
		m_lowerLimit = motor->m_lowerLimit;
		m_upperLimit = motor->m_upperLimit;
		m_accumulatedImpulse = motor->m_accumulatedImpulse;
		m_limitSoftness = motor->m_limitSoftness;
		m_damping = motor->m_damping;
		m_restitution = motor->m_restitution;
		m_normalCFM = motor->m_normalCFM;
		m_stopERP = motor->m_stopERP;
		m_stopCFM = motor->m_stopCFM;
		m_enableMotorX = motor->m_enableMotor[0];
		m_enableMotorY = motor->m_enableMotor[1];
		m_enableMotorZ = motor->m_enableMotor[2];
		m_targetVelocity = motor->m_targetVelocity;
		m_maxMotorForce = motor->m_maxMotorForce;
		m_currentLimitError = motor->m_currentLimitError;
		m_currentLinearDiff = motor->m_currentLinearDiff;
		m_currentLimitX = motor->m_currentLimit[0];
		m_currentLimitY = motor->m_currentLimit[1];
		m_currentLimitZ = motor->m_currentLimit[2];
	}

	void toBullet(btTranslationalLimitMotor* motor)
	{
		motor->m_lowerLimit = m_lowerLimit.toBullet();
		motor->m_upperLimit = m_upperLimit.toBullet();
		motor->m_accumulatedImpulse = m_accumulatedImpulse.toBullet();
		motor->m_limitSoftness = m_limitSoftness;
		motor->m_damping = m_damping;
		motor->m_restitution = m_restitution;
		motor->m_normalCFM = m_normalCFM.toBullet();
		motor->m_stopERP = m_stopERP.toBullet();
		motor->m_stopCFM = m_stopCFM.toBullet();
		motor->m_enableMotor[0] = m_enableMotorX;
		motor->m_enableMotor[1] = m_enableMotorY;
		motor->m_enableMotor[2] = m_enableMotorZ;
		motor->m_targetVelocity = m_targetVelocity.toBullet();
		motor->m_maxMotorForce = m_maxMotorForce.toBullet();
		motor->m_currentLimitError = m_currentLimitError.toBullet();
		motor->m_currentLinearDiff = m_currentLinearDiff.toBullet();
		motor->m_currentLimit[0] = m_currentLimitX;
		motor->m_currentLimit[1] = m_currentLimitY;
		motor->m_currentLimit[2] = m_currentLimitZ;
	}
};