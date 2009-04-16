#include "stdafx.h"
#include "Plane.h"
#include "MathUtils.h"
#include "AxisAlignedBox.h"

namespace Rendering
{

Plane::Plane()
:ogrePlane(new Ogre::Plane())
{

}

Plane::Plane(EngineMath::Vector3 normal, float constant)
:ogrePlane(new Ogre::Plane(MathUtils::copyVector3(normal), constant))
{

}

Plane::Plane(float a, float b, float c, float d)
:ogrePlane(new Ogre::Plane(a, b, c, d))
{

}

Plane::Plane(EngineMath::Vector3 normal, EngineMath::Vector3 point)
:ogrePlane(new Ogre::Plane(MathUtils::copyVector3(normal), MathUtils::copyVector3(point)))
{

}

Plane::Plane(EngineMath::Vector3 point0, EngineMath::Vector3 point1, EngineMath::Vector3 point2)
:ogrePlane(new Ogre::Plane(MathUtils::copyVector3(point0), MathUtils::copyVector3(point1), MathUtils::copyVector3(point2)))
{

}

Ogre::Plane* Plane::getPlane()
{
	return ogrePlane.Get();
}

Plane::Side Plane::getSide(EngineMath::Vector3 point)
{
	return (Side)ogrePlane->getSide(MathUtils::copyVector3(point));
}

Plane::Side Plane::getSide(AxisAlignedBox^ box)
{
	return (Side)ogrePlane->getSide(*(box->getOgreBox()));
}

Plane::Side Plane::getSide(EngineMath::Vector3 center, EngineMath::Vector3 halfSize)
{
	return (Side)ogrePlane->getSide(MathUtils::copyVector3(center), MathUtils::copyVector3(halfSize));
}

float Plane::getDistance(EngineMath::Vector3 point)
{
	return ogrePlane->getDistance(MathUtils::copyVector3(point));
}

void Plane::redefine(EngineMath::Vector3 point0, EngineMath::Vector3 point1, EngineMath::Vector3 point2)
{
	ogrePlane->redefine(MathUtils::copyVector3(point0), MathUtils::copyVector3(point1), MathUtils::copyVector3(point2));
}

void Plane::redefine(EngineMath::Vector3 normal, EngineMath::Vector3 point)
{
	ogrePlane->redefine(MathUtils::copyVector3(normal), MathUtils::copyVector3(point));
}

EngineMath::Vector3 Plane::projectVector(EngineMath::Vector3 v)
{
	return MathUtils::copyVector3(ogrePlane->projectVector(MathUtils::copyVector3(v)));
}

float Plane::normalize()
{
	return ogrePlane->normalise();
}

}