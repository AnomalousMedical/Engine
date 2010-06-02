#include "Stdafx.h"

extern "C" __declspec(dllexport) bool Math_intersectsRayPoly(Ray3 ray, Vector3 a, Vector3 b, Vector3 c, bool positiveSide, bool negativeSide, float* dist)
{
	std::pair<bool, float> result = Ogre::Math::intersects(ray.toOgre(), a.toOgre(), b.toOgre(), c.toOgre(), positiveSide, negativeSide);
	*dist = result.second;
	return result.first;
}