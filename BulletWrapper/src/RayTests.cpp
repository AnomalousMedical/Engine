#include "Stdafx.h"
#include "RayTests.h"

btScalar ClosestRayResultCallback::addSingleResult(btCollisionWorld::LocalRayResult& rayResult, bool normalInWorldSpace)
{
	//caller already does the filter on the m_closestHitFraction
	btAssert(rayResult.m_hitFraction <= m_closestHitFraction);

	m_closestHitFraction = rayResult.m_hitFraction;
	m_collisionObject = rayResult.m_collisionObject;
	if (normalInWorldSpace)
	{
		m_hitNormalWorld = rayResult.m_hitNormalLocal;
	}
	else
	{
		///need to transform normal into worldspace
		m_hitNormalWorld = m_collisionObject->getWorldTransform().getBasis()*rayResult.m_hitNormalLocal;
	}
	m_hitPointWorld.setInterpolate3(m_rayFromWorld, m_rayToWorld, rayResult.m_hitFraction);
	return rayResult.m_hitFraction;
}

extern "C" _AnomalousExport const btCollisionObject* RayResultCallback_getCollisionObject(btCollisionWorld::RayResultCallback* cb)
{
	return cb->m_collisionObject;
}

extern "C" _AnomalousExport short RayResultCallback_getCollisionFilterGroup(btCollisionWorld::RayResultCallback* cb)
{
	return cb->m_collisionFilterGroup;
}

extern "C" _AnomalousExport void RayResultCallback_setCollisionFilterGroup(btCollisionWorld::RayResultCallback* cb, short collisionFilterGroup)
{
	cb->m_collisionFilterGroup = collisionFilterGroup;
}

extern "C" _AnomalousExport short RayResultCallback_getCollisionFilterMask(btCollisionWorld::RayResultCallback* cb)
{
	return cb->m_collisionFilterMask;
}

extern "C" _AnomalousExport void RayResultCallback_setCollisionFilterMask(btCollisionWorld::RayResultCallback* cb, short collisionFilterMask)
{
	cb->m_collisionFilterMask = collisionFilterMask;
}

extern "C" _AnomalousExport void RayResultCallback_Delete(btCollisionWorld::RayResultCallback* cb)
{
	delete cb;
}

extern "C" _AnomalousExport void ManagedRayResultCallback_reset(ManagedRayResultCallback* cb)
{
	cb->reset();
}

extern "C" _AnomalousExport bool ManagedRayResultCallback_hasHit(ManagedRayResultCallback* cb)
{
	return cb->hasHit();
}

extern "C" _AnomalousExport void ManagedRayResultCallback_setRayFromWorld(ClosestRayResultCallback* cb, Vector3 rayFromWorld)
{
	cb->m_rayFromWorld = rayFromWorld.toBullet();
}

extern "C" _AnomalousExport Vector3 ManagedRayResultCallback_getRayFromWorld(ClosestRayResultCallback* cb)
{
	return cb->m_rayFromWorld;
}

extern "C" _AnomalousExport void ManagedRayResultCallback_setRayToWorld(ClosestRayResultCallback* cb, Vector3 rayToWorld)
{
	cb->m_rayToWorld = rayToWorld.toBullet();
}

extern "C" _AnomalousExport Vector3 ManagedRayResultCallback_getRayToWorld(ClosestRayResultCallback* cb)
{
	return cb->m_rayToWorld;
}

extern "C" _AnomalousExport ClosestRayResultCallback* ClosestRayResultCallback_Create(Vector3 rayFromWorld, Vector3 rayToWorld)
{
	return new ClosestRayResultCallback(rayFromWorld.toBullet(), rayToWorld.toBullet());
}

extern "C" _AnomalousExport Vector3 ClosestRayResultCallback_getHitNormalWorld(ClosestRayResultCallback* cb)
{
	return cb->m_hitNormalWorld;
}

extern "C" _AnomalousExport Vector3 ClosestRayResultCallback_getHitPointWorld(ClosestRayResultCallback* cb)
{
	return cb->m_hitPointWorld;
}

//btScalar AllHitsRayResultCallback::addSingleResult(btCollisionWorld::LocalRayResult& rayResult, bool normalInWorldSpace)
//{
//	m_collisionObject = rayResult.m_collisionObject;
//	m_collisionObjects.push_back(rayResult.m_collisionObject);
//	btVector3 hitNormalWorld;
//	if (normalInWorldSpace)
//	{
//		hitNormalWorld = rayResult.m_hitNormalLocal;
//	}
//	else
//	{
//		///need to transform normal into worldspace
//		hitNormalWorld = m_collisionObject->getWorldTransform().getBasis()*rayResult.m_hitNormalLocal;
//	}
//	m_hitNormalWorld.push_back(hitNormalWorld);
//	btVector3 hitPointWorld;
//	hitPointWorld.setInterpolate3(m_rayFromWorld, m_rayToWorld, rayResult.m_hitFraction);
//	m_hitPointWorld.push_back(hitPointWorld);
//	m_hitFractions.push_back(rayResult.m_hitFraction);
//	return m_closestHitFraction;
//}