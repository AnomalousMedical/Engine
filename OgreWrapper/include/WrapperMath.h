#pragma once

namespace OgreWrapper
{

public ref class WrapperMath
{
private:
	WrapperMath(void);

public:
	static Engine::Pair<bool, float>^ intersects(Engine::Ray3 ray, Engine::Vector3 a, Engine::Vector3 b, Engine::Vector3 c, bool positiveSide, bool negativeSide);
};

}