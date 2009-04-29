#include "StdAfx.h"
#include "..\include\PhysBoxShapeDesc.h"

#include "NxBoxShapeDesc.h"
#include "MathUtil.h"

namespace PhysXWrapper
{

PhysBoxShapeDesc::PhysBoxShapeDesc()
:boxShape(new NxBoxShapeDesc()), PhysShapeDesc(boxShape.Get())
{

}

Engine::Vector3 PhysBoxShapeDesc::Dimensions::get() 
{ 
	return MathUtil::copyVector3(boxShape->dimensions);
}

void PhysBoxShapeDesc::Dimensions::set(Engine::Vector3 dimensions) 
{ 
	MathUtil::copyVector3(dimensions, boxShape->dimensions);
} 

}