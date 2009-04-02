#include "StdAfx.h"
#include "..\include\PhysPrismaticJointDesc.h"
#include "NxPhysics.h"

namespace Physics
{

PhysPrismaticJointDesc::PhysPrismaticJointDesc()
:joint(new NxPrismaticJointDesc()), PhysJointDesc(joint.Get())
{

}

}