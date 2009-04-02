#include "StdAfx.h"
#include "..\include\PhysPrismaticJointDesc.h"
#include "NxPhysics.h"

namespace PhysXWrapper
{

PhysPrismaticJointDesc::PhysPrismaticJointDesc()
:joint(new NxPrismaticJointDesc()), PhysJointDesc(joint.Get())
{

}

}