using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace BulletPlugin
{
    [StructLayout(LayoutKind.Sequential)]
    struct	RigidBodyConstructionInfo
	{
		public float			m_mass;

        public Vector3 worldTranslation;
        public Quaternion worldRotation;

        public Vector3 m_localInertia;
        public float m_linearDamping;
        public float m_angularDamping;

		///best simulation results when friction is non-zero
        public float m_friction;
		///best simulation results using zero restitution.
        public float m_restitution;

        public float m_linearSleepingThreshold;
        public float m_angularSleepingThreshold;

		//Additional damping can help avoiding lowpass jitter motion, help stability for ragdolls etc.
		//Such damping is undesirable, so once the overall simulation quality of the rigid body dynamics system has improved, this should become obsolete
        public bool m_additionalDamping;
        public float m_additionalDampingFactor;
        public float m_additionalLinearDampingThresholdSqr;
        public float m_additionalAngularDampingThresholdSqr;
        public float m_additionalAngularDampingFactor;

		public RigidBodyConstructionInfo(float mass)
		{
            m_mass = mass;
			m_localInertia = Vector3.Zero;
			m_linearDamping = 0.0f;
			m_angularDamping = 0.0f;
			m_friction = 0.5f;
			m_restitution = 0.0f;
			m_linearSleepingThreshold = 0.8f;
			m_angularSleepingThreshold = 1.0f;
			m_additionalDamping = false;
			m_additionalDampingFactor = 0.005f;
			m_additionalLinearDampingThresholdSqr = 0.01f;
			m_additionalAngularDampingThresholdSqr = 0.01f;
			m_additionalAngularDampingFactor = 0.01f;
            worldTranslation = Vector3.Zero;
            worldRotation = Quaternion.Identity;
		}
	};
}
