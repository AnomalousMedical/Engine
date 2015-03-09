#include "btBulletDynamicsCommon.h"

struct ManagedRayResultCallback : public btCollisionWorld::RayResultCallback
{
	btVector3	m_rayFromWorld;//used to calculate hitPointWorld from hitFraction
	btVector3	m_rayToWorld;

	ManagedRayResultCallback(const btVector3&	rayFromWorld, const btVector3&	rayToWorld)
		:m_rayFromWorld(rayFromWorld),
		m_rayToWorld(rayToWorld)
	{
	}

	void reset()
	{
		m_collisionObject = 0;
	}
};

struct ClosestRayResultCallback : public ManagedRayResultCallback
{
	ClosestRayResultCallback(const btVector3&	rayFromWorld, const btVector3&	rayToWorld)
		:ManagedRayResultCallback(rayFromWorld, rayToWorld)
	{
	}

	virtual ~ClosestRayResultCallback() {}

	btVector3	m_hitNormalWorld;
	btVector3	m_hitPointWorld;

	virtual	btScalar addSingleResult(btCollisionWorld::LocalRayResult& rayResult, bool normalInWorldSpace);
};

struct	AllHitsRayResultCallback : public ManagedRayResultCallback
{
	AllHitsRayResultCallback(const btVector3&	rayFromWorld, const btVector3&	rayToWorld)
		:ManagedRayResultCallback(rayFromWorld, rayToWorld)
	{
	}

  virtual ~AllHitsRayResultCallback() {}

	btAlignedObjectArray<const btCollisionObject*>		m_collisionObjects;

	btAlignedObjectArray<btVector3>	m_hitNormalWorld;
	btAlignedObjectArray<btVector3>	m_hitPointWorld;
	btAlignedObjectArray<btScalar> m_hitFractions;

	virtual	btScalar addSingleResult(btCollisionWorld::LocalRayResult& rayResult, bool normalInWorldSpace);
};