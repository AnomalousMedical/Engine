#include "stdafx.h"
#include "PlaneBoundedVolume.h"
#include "OgrePlaneBoundedVolume.h"
#include "MathUtils.h"
#include "AxisAlignedBox.h"

namespace Rendering
{

Ogre::PlaneBoundedVolume* PlaneBoundedVolume::getVolume()
{
	return ogreVolume.Get();
}

PlaneBoundedVolume::PlaneBoundedVolume()
:ogreVolume(new Ogre::PlaneBoundedVolume())
{

}

PlaneBoundedVolume::PlaneBoundedVolume(Plane::Side theOutside)
:ogreVolume(new Ogre::PlaneBoundedVolume((Ogre::Plane::Side)theOutside))
{

}

bool PlaneBoundedVolume::intersects(AxisAlignedBox^ box)
{
	return ogreVolume->intersects(*(box->getOgreBox()));
}

bool PlaneBoundedVolume::intersects(EngineMath::Ray3 ray)
{
	return ogreVolume->intersects(MathUtils::copyRay(ray)).first;
}

bool PlaneBoundedVolume::intersects(EngineMath::Ray3% ray)
{
	return ogreVolume->intersects(MathUtils::copyRay(ray)).first;
}

void PlaneBoundedVolume::addPlane(Plane^ plane)
{
	ogreVolume->planes.push_back(*plane->getPlane());
}

void PlaneBoundedVolume::clear()
{
	ogreVolume->planes.clear();
}

Plane::Side PlaneBoundedVolume::Outside::get() 
{
	return outside;
}

}