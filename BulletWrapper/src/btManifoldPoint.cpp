#include "Stdafx.h"

extern "C" _AnomalousExport float btManifoldPoint_getDistance(btManifoldPoint* point)
{
	return point->getDistance();
}

extern "C" _AnomalousExport int btManifoldPoint_getLifeTime(btManifoldPoint* point)
{
	return point->getLifeTime();
}

extern "C" _AnomalousExport Vector3 btManifoldPoint_getPositionWorldOnA(btManifoldPoint* point)
{
	return point->getPositionWorldOnA();
}

extern "C" _AnomalousExport Vector3 btManifoldPoint_getPositionWorldOnB(btManifoldPoint* point)
{
	return point->getPositionWorldOnB();
}

extern "C" _AnomalousExport float btManifoldPoint_getAppliedImpulse(btManifoldPoint* point)
{
	return point->getAppliedImpulse();
}