#include "StdAfx.h"
#include "..\include\MathUtils.h"
#include "Ogre.h"

namespace OgreWrapper
{

void MathUtils::copyVector3( const Ogre::Vector3& source, Engine::Vector3% dest )
{
	dest.x = source.x;
	dest.y = source.y;
	dest.z = source.z;
}

void MathUtils::copyVector3( const Engine::Vector3% source, Ogre::Vector3& dest )
{
	dest.x = source.x;
	dest.y = source.y;
	dest.z = source.z;
}

Ogre::Vector3 MathUtils::copyVector3(const Engine::Vector3% source)
{
	return Ogre::Vector3(source.x, source.y, source.z);
}

Engine::Vector3 MathUtils::copyVector3(const Ogre::Vector3& source)
{
	return Engine::Vector3(source.x, source.y, source.z);
}

void MathUtils::copyQuaternion( const Ogre::Quaternion& source, Engine::Quaternion% dest )
{
	dest.x = source.x;
	dest.y = source.y;
	dest.z = source.z;
	dest.w = source.w;
}

void MathUtils::copyQuaternion( const Engine::Quaternion% source, Ogre::Quaternion& dest )
{
	dest.x = source.x;
	dest.y = source.y;
	dest.z = source.z;
	dest.w = source.w;
}

Ogre::Quaternion MathUtils::copyQuaternion(const Engine::Quaternion% source)
{
	return Ogre::Quaternion(source.w, source.x, source.y, source.z);
}

Engine::Quaternion MathUtils::copyQuaternion(const Ogre::Quaternion& source)
{
	return Engine::Quaternion(source.x, source.y, source.z, source.w);
}

void MathUtils::copyRay(Engine::Ray3% source, Ogre::Ray& dest)
{
	Ogre::Vector3 vec3;
	copyVector3(source.Origin, vec3);
	dest.setOrigin(vec3);
	copyVector3(source.Direction, vec3);
	dest.setDirection(vec3);
}

void MathUtils::copyRay(Ogre::Ray& source, Engine::Ray3% dest)
{
	Engine::Vector3 vec = Engine::Vector3();
	copyVector3(source.getOrigin(), vec);
	dest.Origin = vec;
	copyVector3(source.getDirection(), vec);
	dest.Direction = vec;
}

Ogre::Ray MathUtils::copyRay(Engine::Ray3% source)
{
	return Ogre::Ray(copyVector3(source.Origin), copyVector3(source.Direction));
}

Engine::Ray3 MathUtils::copyRay(Ogre::Ray& source)
{
	return Engine::Ray3(copyVector3(source.getOrigin()), copyVector3(source.getDirection()));
}

Engine::Color MathUtils::copyColor(const Ogre::ColourValue& source)
{
	return Engine::Color(source.r, source.g, source.b, source.a);
}

Ogre::ColourValue MathUtils::copyColor(Engine::Color% source)
{
	return Ogre::ColourValue(source.r, source.g, source.b, source.a);
}

Engine::Matrix4x4 MathUtils::copyMatrix4x4(const Ogre::Matrix4& source)
{
	return *(Engine::Matrix4x4*)&source;
}

}