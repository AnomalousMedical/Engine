#pragma  once

_declspec(dllexport) class RigidBodyConstructionInfo
{
public:
	float			m_mass;

	Vector3         worldTranslation;
	Quaternion      worldRotation;

	Vector3			m_localInertia;
	float			m_linearDamping;
	float			m_angularDamping;

	///best simulation results when friction is non-zero
	float			m_friction;
	///best simulation results using zero restitution.
	float			m_restitution;

	float			m_linearSleepingThreshold;
	float			m_angularSleepingThreshold;

	//Additional damping can help avoiding lowpass jitter motion, help stability for ragdolls etc.
	//Such damping is undesirable, so once the overall simulation quality of the rigid body dynamics system has improved, this should become obsolete
	bool			m_additionalDamping;
	float			m_additionalDampingFactor;
	float			m_additionalLinearDampingThresholdSqr;
	float			m_additionalAngularDampingThresholdSqr;
	float			m_additionalAngularDampingFactor;

	void toBullet(btRigidBody::btRigidBodyConstructionInfo& bulletInfo, btMotionState* motionState, btCollisionShape* collisionShape)
	{
		bulletInfo.m_mass = m_mass;

		///When a motionState is provided, the rigid body will initialize its world transform from the motion state
		///In this case, m_startWorldTransform is ignored.
		bulletInfo.m_motionState = motionState;
		bulletInfo.m_startWorldTransform.setOrigin(worldTranslation.toBullet());
		bulletInfo.m_startWorldTransform.setRotation(worldRotation.toBullet());

		bulletInfo.m_collisionShape = collisionShape;
		bulletInfo.m_localInertia = m_localInertia.toBullet();
		bulletInfo.m_linearDamping = m_linearDamping;
		bulletInfo.m_angularDamping = m_angularDamping;

		///best simulation results when friction is non-zero
		bulletInfo.m_friction = m_friction;
		///best simulation results using zero restitution.
		bulletInfo.m_restitution = m_restitution;

		bulletInfo.m_linearSleepingThreshold = m_linearSleepingThreshold;
		bulletInfo.m_angularSleepingThreshold = m_angularSleepingThreshold;

		//Additional damping can help avoiding lowpass jitter motion, help stability for ragdolls etc.
		//Such damping is undesirable, so once the overall simulation quality of the rigid body dynamics system has improved, this should become obsolete
		bulletInfo.m_additionalDamping = m_additionalDamping;
		bulletInfo.m_additionalDampingFactor = m_additionalDampingFactor;
		bulletInfo.m_additionalLinearDampingThresholdSqr = m_additionalLinearDampingThresholdSqr;
		bulletInfo.m_additionalAngularDampingThresholdSqr = m_additionalAngularDampingThresholdSqr;
		bulletInfo.m_additionalAngularDampingFactor = m_additionalAngularDampingFactor;
	}
};