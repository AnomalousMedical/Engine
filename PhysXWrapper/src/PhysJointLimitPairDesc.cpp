#include "StdAfx.h"
#include "..\include\PhysJointLimitPairDesc.h"
#include "PhysJointLimitDesc.h"

namespace Physics
{

PhysJointLimitPairDesc::PhysJointLimitPairDesc(NxJointLimitPairDesc* limitDesc)
:limitDesc(limitDesc)
{
	low = gcnew PhysJointLimitDesc(&limitDesc->low);
	high = gcnew PhysJointLimitDesc(&limitDesc->high);
}

PhysJointLimitPairDesc::PhysJointLimitPairDesc()
:autoLimitDesc(new NxJointLimitPairDesc())
{
	limitDesc = autoLimitDesc.Get();
	low = gcnew PhysJointLimitDesc(&limitDesc->low);
	high = gcnew PhysJointLimitDesc(&limitDesc->high);
}

bool PhysJointLimitPairDesc::isValid()
{
	return limitDesc->isValid();
}

PhysJointLimitDesc^ PhysJointLimitPairDesc::Low::get() 
{
	return low;
}

PhysJointLimitDesc^ PhysJointLimitPairDesc::High::get() 
{
	return high;
}

}