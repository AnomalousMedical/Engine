#pragma once

//#include "btManifoldPoint.h"

namespace BulletPlugin
{

public ref class ManifoldPoint
{
private:
	float distance;
	int lifeTime;
	Engine::Vector3 positionWorldOnA;
	Engine::Vector3 positionWorldOnB;
	float appliedImpulse;

internal:
	void setInfo(const btManifoldPoint& point);

public:
	ManifoldPoint(void);

	float getDistance();

	int getLifeTime();

	Engine::Vector3 getPositionWorldOnA();

	Engine::Vector3 getPositionWorldOnB();

	float getAppliedImpulse();
};

}