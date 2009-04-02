#include "StdAfx.h"
#include "..\include\PhysBoxShape.h"

#include "NxBoxShape.h"
#include "MathUtil.h"
#include "PhysBoxShapeDesc.h"

namespace Physics
{

PhysBoxShape::PhysBoxShape(NxBoxShape* nxBox)
:PhysShape(nxBox), nxBox( nxBox )
{

}

PhysBoxShape::~PhysBoxShape()
{
	if(nxBox != 0)
	{
		nxBox = 0;
	}
}

NxBoxShape* PhysBoxShape::getNxBoxShape()
{
	return nxBox;
}

void PhysBoxShape::setDimensions(EngineMath::Vector3 dimen)
{
	nxBox->setDimensions(MathUtil::copyVector3(dimen));
}

void PhysBoxShape::setDimensions(EngineMath::Vector3% dimen)
{
	nxBox->setDimensions(MathUtil::copyVector3(dimen));
}

EngineMath::Vector3 PhysBoxShape::getDimensions()
{
	return MathUtil::copyVector3(nxBox->getDimensions());
}

void PhysBoxShape::saveToDesc(PhysBoxShapeDesc^ desc)
{
	nxBox->saveToDesc(*desc->boxShape.Get());
}

}