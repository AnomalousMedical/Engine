#include "StdAfx.h"
#include "ContactIterator.h"
#include "NxContactStreamIterator.h"
#include "MathUtil.h"

namespace Engine
{

namespace Physics
{

ContactIterator::ContactIterator(void)
{
}

void ContactIterator::setContactStreamIterator( NxContactStreamIterator* contactStreamIterator )
{
	this->contactStreamIterator = contactStreamIterator;
}

bool ContactIterator::goNextPair()
{
	return contactStreamIterator->goNextPair();
}

bool ContactIterator::goNextPatch()
{
	return contactStreamIterator->goNextPatch();
}
  
bool ContactIterator::goNextPoint()
{
	return contactStreamIterator->goNextPoint();
}
  
unsigned int ContactIterator::getNumPairs()
{
	return contactStreamIterator->getNumPairs();
}

//NOT CURRENTLY SUPPORTED NO MANAGED SHAPE INTERFACES
//NxShape * getShape(unsigned int shapeIndex);

unsigned short ContactIterator::getShapeFlags()
{
	return contactStreamIterator->getShapeFlags();
}
  
unsigned int ContactIterator::getNumPatches()
{
	return contactStreamIterator->getNumPatches();
}
  
unsigned int ContactIterator::getNumPatchesRemaining()
{
	return contactStreamIterator->getNumPatchesRemaining();
}
  
void ContactIterator::getPatchNormal( EngineMath::Vector3% normal )
{
	MathUtil::copyVector3( contactStreamIterator->getPatchNormal(), normal );
}
  
unsigned int ContactIterator::getNumPoints()
{
	return contactStreamIterator->getNumPoints();
}
   
unsigned int ContactIterator::getNumPointsRemaining()
{
	return contactStreamIterator->getNumPointsRemaining();
}
  
void ContactIterator::getPoint( EngineMath::Vector3% point )
{
	MathUtil::copyVector3( contactStreamIterator->getPoint(), point );
}
  
float ContactIterator::getSeparation()
{
	return contactStreamIterator->getSeparation();
}
  
unsigned int ContactIterator::getFeatureIndex0()
{
	return contactStreamIterator->getFeatureIndex0();
}
   
unsigned int ContactIterator::getFeatureIndex1()
{
	return contactStreamIterator->getFeatureIndex1();
}
  
float ContactIterator::getPointNormalForce()
{
	return contactStreamIterator->getPointNormalForce();
}

}

}
