#include "StdAfx.h"
#include "..\include\MathUtils.h"
#include "Ogre.h"

namespace OgreWrapper
{

void MathUtils::copyVector3( const Ogre::Vector3& source, EngineMath::Vector3% dest )
{
	dest.x = source.x;
	dest.y = source.y;
	dest.z = source.z;
}

void MathUtils::copyVector3( const EngineMath::Vector3% source, Ogre::Vector3& dest )
{
	dest.x = source.x;
	dest.y = source.y;
	dest.z = source.z;
}

Ogre::Vector3 MathUtils::copyVector3(const EngineMath::Vector3% source)
{
	return Ogre::Vector3(source.x, source.y, source.z);
}

EngineMath::Vector3 MathUtils::copyVector3(const Ogre::Vector3& source)
{
	return EngineMath::Vector3(source.x, source.y, source.z);
}

void MathUtils::copyQuaternion( const Ogre::Quaternion& source, EngineMath::Quaternion% dest )
{
	dest.x = source.x;
	dest.y = source.y;
	dest.z = source.z;
	dest.w = source.w;
}

void MathUtils::copyQuaternion( const EngineMath::Quaternion% source, Ogre::Quaternion& dest )
{
	dest.x = source.x;
	dest.y = source.y;
	dest.z = source.z;
	dest.w = source.w;
}

Ogre::Quaternion MathUtils::copyQuaternion(const EngineMath::Quaternion% source)
{
	return Ogre::Quaternion(source.w, source.x, source.y, source.z);
}

EngineMath::Quaternion MathUtils::copyQuaternion(const Ogre::Quaternion& source)
{
	return EngineMath::Quaternion(source.x, source.y, source.z, source.w);
}

void MathUtils::copyRay(EngineMath::Ray3% source, Ogre::Ray& dest)
{
	Ogre::Vector3 vec3;
	copyVector3(source.Origin, vec3);
	dest.setOrigin(vec3);
	copyVector3(source.Direction, vec3);
	dest.setDirection(vec3);
}

void MathUtils::copyRay(Ogre::Ray& source, EngineMath::Ray3% dest)
{
	EngineMath::Vector3 vec = EngineMath::Vector3();
	copyVector3(source.getOrigin(), vec);
	dest.Origin = vec;
	copyVector3(source.getDirection(), vec);
	dest.Direction = vec;
}

Ogre::Ray MathUtils::copyRay(EngineMath::Ray3% source)
{
	return Ogre::Ray(copyVector3(source.Origin), copyVector3(source.Direction));
}

EngineMath::Ray3 MathUtils::copyRay(Ogre::Ray& source)
{
	return EngineMath::Ray3(copyVector3(source.getOrigin()), copyVector3(source.getDirection()));
}

EngineMath::Color MathUtils::copyColor(const Ogre::ColourValue& source)
{
	return EngineMath::Color(source.r, source.g, source.b, source.a);
}

Ogre::ColourValue MathUtils::copyColor(EngineMath::Color% source)
{
	return Ogre::ColourValue(source.r, source.g, source.b, source.a);
}

}