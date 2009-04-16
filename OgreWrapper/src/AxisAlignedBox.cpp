#include "StdAfx.h"
#include "..\include\AxisAlignedBox.h"
#include "Ogre.h"
#include "MathUtils.h"
#include "Plane.h"

namespace OgreWrapper{

AxisAlignedBox::AxisAlignedBox()
:ogreBox(new Ogre::AxisAlignedBox())
{

}

AxisAlignedBox::AxisAlignedBox(const Ogre::AxisAlignedBox* ogreBox)
:ogreBox(new Ogre::AxisAlignedBox(*ogreBox))
{

}

AxisAlignedBox::AxisAlignedBox(Extent e)
:ogreBox(new Ogre::AxisAlignedBox((Ogre::AxisAlignedBox::Extent)e))
{

}

AxisAlignedBox::AxisAlignedBox(EngineMath::Vector3 minVal, EngineMath::Vector3 maxVal)
:ogreBox(new Ogre::AxisAlignedBox(MathUtils::copyVector3(minVal), MathUtils::copyVector3(maxVal)))
{

}

AxisAlignedBox::AxisAlignedBox(float mx, float my, float mz, float Mx, float My, float Mz)
:ogreBox(new Ogre::AxisAlignedBox(mx, my, mz, Mx, My, Mz))
{

}

AxisAlignedBox::~AxisAlignedBox(void)
{

}

Ogre::AxisAlignedBox* AxisAlignedBox::getOgreBox()
{
	return ogreBox.Get();
}

Vector3 AxisAlignedBox::getMinimum()
{
	return MathUtils::copyVector3(ogreBox->getMinimum());
}

Vector3 AxisAlignedBox::getMaximum()
{
	return MathUtils::copyVector3(ogreBox->getMaximum());
}

void AxisAlignedBox::setMinimum(Vector3 minimum)
{
	ogreBox->setMinimum(MathUtils::copyVector3(minimum));
}

void AxisAlignedBox::setMinimum(Vector3% minimum)
{
	ogreBox->setMinimum(MathUtils::copyVector3(minimum));
}

void AxisAlignedBox::setMaximum(Vector3 maximum)
{
	ogreBox->setMaximum(MathUtils::copyVector3(maximum));
}

void AxisAlignedBox::setMaximum(Vector3% maximum)
{
	ogreBox->setMaximum(MathUtils::copyVector3(maximum));
}

void AxisAlignedBox::setMinimum(float x, float y, float z)
{
	ogreBox->setMinimum(x, y, z);
}

void AxisAlignedBox::setMaximum(float x, float y, float z)
{
	ogreBox->setMaximum(x, y, z);
}

void AxisAlignedBox::setExtents(Vector3 minimum, Vector3 maximum)
{
	ogreBox->setExtents(MathUtils::copyVector3(minimum), MathUtils::copyVector3(maximum));
}

void AxisAlignedBox::setExtents(Vector3% minimum, Vector3% maximum)
{
	ogreBox->setExtents(MathUtils::copyVector3(minimum), MathUtils::copyVector3(maximum));
}

Vector3 AxisAlignedBox::getCorner(CornerEnum cornerToGet)
{
	return MathUtils::copyVector3(ogreBox->getCorner((Ogre::AxisAlignedBox::CornerEnum)cornerToGet));
}

void AxisAlignedBox::merge(AxisAlignedBox^ rhs)
{
	ogreBox->merge(*(rhs->ogreBox.Get()));
}

void AxisAlignedBox::merge(Vector3 point)
{
	ogreBox->merge(MathUtils::copyVector3(point));
}

void AxisAlignedBox::merge(Vector3% point)
{
	ogreBox->merge(MathUtils::copyVector3(point));
}

void AxisAlignedBox::setNull()
{
	ogreBox->setNull();
}

bool AxisAlignedBox::isNull()
{
	return ogreBox->isNull();
}

bool AxisAlignedBox::isFinite()
{
	return ogreBox->isFinite();
}

void AxisAlignedBox::setInfinite()
{
	ogreBox->setInfinite();
}

bool AxisAlignedBox::isInfinite()
{
	return ogreBox->isInfinite();
}

bool AxisAlignedBox::intersects(AxisAlignedBox^ box)
{
	return ogreBox->intersects(*(box->ogreBox.Get()));
}

AxisAlignedBox^ AxisAlignedBox::intersection(AxisAlignedBox^ box)
{
	return gcnew AxisAlignedBox(&ogreBox->intersection(*(box->ogreBox.Get())));
}

float AxisAlignedBox::volume()
{
	return ogreBox->volume();
}

void AxisAlignedBox::scale(Vector3 scale)
{
	ogreBox->scale(MathUtils::copyVector3(scale));
}

void AxisAlignedBox::scale(Vector3% scale)
{
	ogreBox->scale(MathUtils::copyVector3(scale));
}

bool AxisAlignedBox::intersects(Plane^ plane)
{
	return ogreBox->intersects(*(plane->getPlane()));
}

bool AxisAlignedBox::intersects(Vector3 point)
{
	return ogreBox->intersects(MathUtils::copyVector3(point));
}

bool AxisAlignedBox::intersects(Vector3% point)
{
	return ogreBox->intersects(MathUtils::copyVector3(point));
}

Vector3 AxisAlignedBox::getCenter()
{
	return MathUtils::copyVector3(ogreBox->getCenter());
}

Vector3 AxisAlignedBox::getSize()
{
	return MathUtils::copyVector3(ogreBox->getSize());
}

Vector3 AxisAlignedBox::getHalfSize()
{
	return MathUtils::copyVector3(ogreBox->getHalfSize());
}

bool AxisAlignedBox::contains(Vector3 point)
{
	return ogreBox->contains(MathUtils::copyVector3(point));
}

bool AxisAlignedBox::contains(Vector3% point)
{
	return ogreBox->contains(MathUtils::copyVector3(point));
}

}