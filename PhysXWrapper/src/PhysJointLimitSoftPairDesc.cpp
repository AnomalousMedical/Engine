#include "StdAfx.h"
#include "..\include\PhysJointLimitSoftPairDesc.h"
#include "PhysJointLimitSoftDesc.h"

namespace PhysXWrapper
{

PhysJointLimitSoftPairDesc::PhysJointLimitSoftPairDesc(NxJointLimitSoftPairDesc* limitDesc)
:limitDesc(limitDesc),
low(gcnew PhysJointLimitSoftDesc(&limitDesc->low)),
high(gcnew PhysJointLimitSoftDesc(&limitDesc->high))
{

}

bool PhysJointLimitSoftPairDesc::isValid()
{
	return limitDesc->isValid();
}

PhysJointLimitSoftDesc^ PhysJointLimitSoftPairDesc::Low::get() 
{
	return low;
}

PhysJointLimitSoftDesc^ PhysJointLimitSoftPairDesc::High::get() 
{
	return high;
}

}