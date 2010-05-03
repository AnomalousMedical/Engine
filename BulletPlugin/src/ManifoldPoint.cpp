#include "StdAfx.h"
#include "..\include\ManifoldPoint.h"

namespace BulletPlugin
{

ManifoldPoint::ManifoldPoint(void)
{
}

void ManifoldPoint::setInfo(const btManifoldPoint& point)
{
	distance = point.getDistance();
	lifeTime = point.getLifeTime();
	const btVector3* posA = &point.getPositionWorldOnA();
	positionWorldOnA = Engine::Vector3(posA->x(), posA->y(), posA->z());
	const btVector3* posB = &point.getPositionWorldOnA();
	positionWorldOnB = Engine::Vector3(posB->x(), posB->y(), posB->z());
	appliedImpulse = point.getAppliedImpulse();
}

float ManifoldPoint::getDistance()
{
	return distance;
}

int ManifoldPoint::getLifeTime()
{
	return lifeTime;
}

Engine::Vector3 ManifoldPoint::getPositionWorldOnA()
{
	return positionWorldOnA;
}

Engine::Vector3 ManifoldPoint::getPositionWorldOnB()
{
	return positionWorldOnB;
}

float ManifoldPoint::getAppliedImpulse()
{
	return appliedImpulse;
}

}