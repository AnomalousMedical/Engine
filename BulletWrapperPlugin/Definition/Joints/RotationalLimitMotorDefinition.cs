using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;
using Engine.Editing;
using Engine.Saving;

namespace BulletPlugin
{

    //! Rotation Limit structure for generic joints
    [StructLayout(LayoutKind.Sequential)]
    public class RotationalLimitMotorDefinition : Saveable
    {
        //! limit_parameters
        //!@{
        private float m_loLimit;//!< joint limit
        private float m_hiLimit;//!< joint limit
        private float m_targetVelocity;//!< target motor velocity
        private float m_maxMotorForce;//!< max force on motor
        private float m_maxLimitForce;//!< max force on limit
        private float m_damping;//!< Damping.
        private float m_limitSoftness;//! Relaxation factor
        private float m_normalCFM;//!< Constraint force mixing factor
        private float m_stopERP;//!< Error tolerance factor when joint is at limit
        private float m_stopCFM;//!< Constraint force mixing factor when joint is at limit
        private float m_bounce;//!< restitution factor
        private bool m_enableMotor;

        //!@}

        //! temp_variables
        //!@{
        private float m_currentLimitError;//!  How much is violated this limit
        private float m_currentPosition;     //!  current value of angle 
        private int m_currentLimit;//!< 0=free, 1=at lo limit, 2=at hi limit
        private float m_accumulatedImpulse;
        //!@}

        public RotationalLimitMotorDefinition()
        {
    	    m_accumulatedImpulse = 0.0f;
            m_targetVelocity = 0;
            m_maxMotorForce = 0.1f;
            m_maxLimitForce = 300.0f;
            m_loLimit = 1.0f;
            m_hiLimit = -1.0f;
		    m_normalCFM = 0.0f;
		    m_stopERP = 0.2f;
		    m_stopCFM = 0.0f;
            m_bounce = 0.0f;
            m_damping = 1.0f;
            m_limitSoftness = 0.5f;
            m_currentLimit = 0;
            m_currentLimitError = 0;
            m_enableMotor = false;
        }

        [Editable]
        public float LoLimit
        {
            get
            {
                return m_loLimit;
            }
            set
            {
                m_loLimit = value;
            }
        }

        [Editable]
        public float HiLimit
        {
            get
            {
                return m_hiLimit;
            }
            set
            {
                m_hiLimit = value;
            }
        }

        [Editable]
        public float TargetVelocity
        {
            get
            {
                return m_targetVelocity;
            }
            set
            {
                m_targetVelocity = value;
            }
        }

        [Editable]
        public float MaxMotorForce
        {
            get
            {
                return m_maxMotorForce;
            }
            set
            {
                m_maxMotorForce = value;
            }
        }

        [Editable]
        public float MaxLimitForce
        {
            get
            {
                return m_maxLimitForce;
            }
            set
            {
                m_maxLimitForce = value;
            }
        }

        [Editable]
        public float Damping
        {
            get
            {
                return m_damping;
            }
            set
            {
                m_damping = value;
            }
        }

        [Editable]
        public float LimitSoftness
        {
            get
            {
                return m_limitSoftness;
            }
            set
            {
                m_limitSoftness = value;
            }
        }

        [Editable]
        public float NormalCFM
        {
            get
            {
                return m_normalCFM;
            }
            set
            {
                m_normalCFM = value;
            }
        }

        [Editable]
        public float StopERP
        {
            get
            {
                return m_stopERP;
            }
            set
            {
                m_stopERP = value;
            }
        }

        [Editable]
        public float StopCFM
        {
            get
            {
                return m_stopCFM;
            }
            set
            {
                m_stopCFM = value;
            }
        }

        [Editable]
        public float Bounce
        {
            get
            {
                return m_bounce;
            }
            set
            {
                m_bounce = value;
            }
        }

        [Editable]
        public bool EnableMotor
        {
            get
            {
                return m_enableMotor;
            }
            set
            {
                m_enableMotor = value;
            }
        }

        [Editable]
        public float CurrentLimitError
        {
            get
            {
                return m_currentLimitError;
            }
            set
            {
                m_currentLimitError = value;
            }
        }

        [Editable]
        public float CurrentPosition
        {
            get
            {
                return m_currentPosition;
            }
            set
            {
                m_currentPosition = value;
            }
        }

        [Editable]
        public int CurrentLimit
        {
            get
            {
                return m_currentLimit;
            }
            set
            {
                m_currentLimit = value;
            }
        }

        [Editable]
        public float AccumulatedImpulse
        {
            get
            {
                return m_accumulatedImpulse;
            }
            set
            {
                m_accumulatedImpulse = value;
            }
        }

        #region Saving

        protected RotationalLimitMotorDefinition(LoadInfo info)
        {
            LoLimit = info.GetFloat("LoLimit");
            HiLimit = info.GetFloat("HiLimit");
            TargetVelocity = info.GetFloat("TargetVelocity");
            MaxMotorForce = info.GetFloat("MaxMotorForce");
            MaxLimitForce = info.GetFloat("MaxLimitForce");
            Damping = info.GetFloat("Damping");
            LimitSoftness = info.GetFloat("LimitSoftness");
            NormalCFM = info.GetFloat("NormalCFM", NormalCFM);
            StopERP = info.GetFloat("StopERP", StopERP);
            StopCFM = info.GetFloat("StopCFM", StopCFM);
            Bounce = info.GetFloat("Bounce");
            EnableMotor = info.GetBoolean("EnableMotor");
            CurrentLimitError = info.GetFloat("CurrentLimitError");
            CurrentLimit = info.GetInt32("CurrentLimit");
            AccumulatedImpulse = info.GetFloat("AccumulatedImpulse");
        }

        public virtual void getInfo(SaveInfo info)
        {
            info.AddValue("LoLimit", LoLimit);
            info.AddValue("HiLimit", HiLimit);
            info.AddValue("TargetVelocity", TargetVelocity);
            info.AddValue("MaxMotorForce", MaxMotorForce);
            info.AddValue("MaxLimitForce", MaxLimitForce);
            info.AddValue("Damping", Damping);
            info.AddValue("LimitSoftness", LimitSoftness);
            info.AddValue("NormalCFM", NormalCFM);
            info.AddValue("StopERP", StopERP);
            info.AddValue("StopCFM", StopCFM);
            info.AddValue("Bounce", Bounce);
            info.AddValue("EnableMotor", EnableMotor);
            info.AddValue("CurrentLimitError", CurrentLimitError);
            info.AddValue("CurrentLimit", CurrentLimit);
            info.AddValue("AccumulatedImpulse", AccumulatedImpulse);
        }

        #endregion Saving
    }

    
}
