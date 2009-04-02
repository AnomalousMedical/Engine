#include "StdAfx.h"
#include "..\include\PhysPrismaticJointDesc.h"
#include "NxPhysics.h"

namespace Engine
{

namespace Physics
{

PhysPrismaticJointDesc::PhysPrismaticJointDesc()
:joint(new NxPrismaticJointDesc()), PhysJointDesc(joint.Get())
{

}

}

}