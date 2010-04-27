#include "StdAfx.h"
#include "..\include\WrapperMath.h"
#include "OgreMath.h"
#include "MathUtils.h"

namespace OgreWrapper
{

WrapperMath::WrapperMath(void)
{
}

Engine::Pair<bool, float>^ WrapperMath::intersects(Engine::Ray3 ray, Engine::Vector3 a, Engine::Vector3 b, Engine::Vector3 c, bool positiveSide, bool negativeSide)
{
	std::pair<bool, float> result = Ogre::Math::intersects(MathUtils::copyRay(ray), MathUtils::copyVector3(a),
            MathUtils::copyVector3(b), MathUtils::copyVector3(c), positiveSide, negativeSide);
	return gcnew Engine::Pair<bool, float>(result.first, result.second);
}

}