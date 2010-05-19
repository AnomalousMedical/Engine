#include "Stdafx.h"

extern "C" _declspec(dllexport) float btManifoldPoint_getDistance(btManifoldPoint* point)
{
	return point->getDistance();
}

extern "C" _declspec(dllexport) int btManifoldPoint_getLifeTime(btManifoldPoint* point)
{
	return point->getLifeTime();
}

extern "C" _declspec(dllexport) Vector3 btManifoldPoint_getPositionWorldOnA(btManifoldPoint* point)
{
	return point->getPositionWorldOnA();
}

extern "C" _declspec(dllexport) Vector3 btManifoldPoint_getPositionWorldOnB(btManifoldPoint* point)
{
	return point->getPositionWorldOnB();
}

extern "C" _declspec(dllexport) float btManifoldPoint_getAppliedImpulse(btManifoldPoint* point)
{
	return point->getAppliedImpulse();
}