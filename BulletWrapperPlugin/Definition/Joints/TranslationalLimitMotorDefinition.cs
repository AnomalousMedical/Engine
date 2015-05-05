using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using Engine.Editing;
using Engine.Saving;

namespace BulletPlugin
{
    [StructLayout(LayoutKind.Sequential)]
    public class TranslationalLimitMotorDefinition : Saveable
    {
        private Vector3 m_lowerLimit;//!< the constraint lower limits
        private Vector3 m_upperLimit;//!< the constraint upper limits
        private Vector3 m_accumulatedImpulse;
        //! Linear_Limit_parameters
        //!@{
        private float m_limitSoftness;//!< Softness for linear limit
        private float m_damping;//!< Damping for linear limit
        private float m_restitution;//! Bounce parameter for linear limit
        private Vector3 m_normalCFM;//!< Constraint force mixing factor
        private Vector3 m_stopERP;//!< Error tolerance factor when joint is at limit
        private Vector3 m_stopCFM;//!< Constraint force mixing factor when joint is at limit
        //!@}
        private bool m_enableMotorX; //must replace the bool array with 3 elements
        private bool m_enableMotorY;
        private bool m_enableMotorZ;
        private Vector3 m_targetVelocity;//!< target motor velocity
        private Vector3 m_maxMotorForce;//!< max force on motor
        private Vector3 m_currentLimitError;//!  How much is violated this limit
        private Vector3 m_currentLinearDiff;//!  Current relative offset of constraint frames
        private int m_currentLimitX;//!< 0=free, 1=at lower limit, 2=at upper limit
        private int m_currentLimitY;//!< 0=free, 1=at lower limit, 2=at upper limit
        private int m_currentLimitZ;//!< 0=free, 1=at lower limit, 2=at upper limit

        public TranslationalLimitMotorDefinition()
        {
            m_lowerLimit = new Vector3(0.0f, 0.0f, 0.0f);
            m_upperLimit = new Vector3(0.0f, 0.0f, 0.0f);
            m_accumulatedImpulse = new Vector3(0.0f, 0.0f, 0.0f);
            m_normalCFM = new Vector3(0.0f, 0.0f, 0.0f);
            m_stopERP = new Vector3(0.2f, 0.2f, 0.2f);
            m_stopCFM = new Vector3(0.0f, 0.0f, 0.0f);

            m_limitSoftness = 0.7f;
            m_damping = 1.0f;
            m_restitution = 0.5f;

            m_targetVelocity = new Vector3(0.0f, 0.0f, 0.0f);
            m_enableMotorX = m_enableMotorY = m_enableMotorZ = false;
            m_maxMotorForce = new Vector3(0.0f, 0.0f, 0.0f);

            m_currentLimitError = new Vector3(0.0f, 0.0f, 0.0f);
            m_currentLinearDiff = new Vector3(0.0f, 0.0f, 0.0f);
            m_currentLimitX = m_currentLimitY = m_currentLimitZ = 0;
        }

        [Editable]
        public Vector3 LowerLimit
        {
            get
            {
                return m_lowerLimit;
            }
            set
            {
                m_lowerLimit = value;
            }
        }

        [Editable]
        public Vector3 UpperLimit
        {
            get
            {
                return m_upperLimit;
            }
            set
            {
                m_upperLimit = value;
            }
        }

        [Editable]
        public Vector3 AccumulatedImpulse
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
        public float Restitution
        {
            get
            {
                return m_restitution;
            }
            set
            {
                m_restitution = value;
            }
        }

        [Editable]
        public Vector3 NormalCFM
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
        public Vector3 StopERP
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
        public Vector3 StopCFM
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
        public bool EnableMotorX
        {
            get
            {
                return m_enableMotorX;
            }
            set
            {
                m_enableMotorX = value;
            }
        }

        [Editable]
        public bool EnableMotorY
        {
            get
            {
                return m_enableMotorY;
            }
            set
            {
                m_enableMotorY = value;
            }
        }

        [Editable]
        public bool EnableMotorZ
        {
            get
            {
                return m_enableMotorZ;
            }
            set
            {
                m_enableMotorZ = value;
            }
        }

        [Editable]
        public Vector3 TargetVelocity
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
        public Vector3 MaxMotorForce
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
        public Vector3 CurrentLimitError
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
        public Vector3 CurrentLinearDiff
        {
            get
            {
                return m_currentLinearDiff;
            }
            set
            {
                m_currentLinearDiff = value;
            }
        }

        [Editable]
        public int CurrentLimitX
        {
            get
            {
                return m_currentLimitX;
            }
            set
            {
                m_currentLimitX = value;
            }
        }

        [Editable]
        public int CurrentLimitY
        {
            get
            {
                return m_currentLimitY;
            }
            set
            {
                m_currentLimitY = value;
            }
        }

        [Editable]
        public int CurrentLimitZ
        {
            get
            {
                return m_currentLimitZ;
            }
            set
            {
                m_currentLimitZ = value;
            }
        }

        #region Saving

        protected TranslationalLimitMotorDefinition(LoadInfo info)
        {
            LowerLimit = info.GetVector3("LowerLimit");
            UpperLimit = info.GetVector3("UpperLimit");
            AccumulatedImpulse = info.GetVector3("AccumulatedImpulse");
            LimitSoftness = info.GetFloat("LimitSoftness");
            Damping = info.GetFloat("Damping");
            Restitution = info.GetFloat("Restitution");
            EnableMotorX = info.GetBoolean("EnableMotorX");
            EnableMotorY = info.GetBoolean("EnableMotorY");
            EnableMotorZ = info.GetBoolean("EnableMotorZ");
            TargetVelocity = info.GetVector3("TargetVelocity");
            MaxMotorForce = info.GetVector3("MaxMotorForce");
            CurrentLimitError = info.GetVector3("CurrentLimitError");
            CurrentLimitX = info.GetInt32("CurrentLimitX");
            CurrentLimitY = info.GetInt32("CurrentLimitY");
            CurrentLimitZ = info.GetInt32("CurrentLimitZ");
            NormalCFM = info.GetVector3("NormalCFM", NormalCFM);
            StopERP = info.GetVector3("StopERP", StopERP);
            StopCFM = info.GetVector3("StopCFM", StopCFM);
        }

        public virtual void getInfo(SaveInfo info)
        {
            info.AddValue("LowerLimit", LowerLimit);
            info.AddValue("UpperLimit", UpperLimit);
            info.AddValue("AccumulatedImpulse", AccumulatedImpulse);
            info.AddValue("LimitSoftness", LimitSoftness);
            info.AddValue("Damping", Damping);
            info.AddValue("Restitution", Restitution);
            info.AddValue("EnableMotorX", EnableMotorX);
            info.AddValue("EnableMotorY", EnableMotorY);
            info.AddValue("EnableMotorZ", EnableMotorZ);
            info.AddValue("TargetVelocity", TargetVelocity);
            info.AddValue("MaxMotorForce", MaxMotorForce);
            info.AddValue("CurrentLimitError", CurrentLimitError);
            info.AddValue("CurrentLimitX", CurrentLimitX);
            info.AddValue("CurrentLimitY", CurrentLimitY);
            info.AddValue("CurrentLimitZ", CurrentLimitZ);
            info.AddValue("NormalCFM", NormalCFM);
            info.AddValue("StopERP", StopERP);
            info.AddValue("StopCFM", StopCFM);
        }

        #endregion Saving
    }
}
