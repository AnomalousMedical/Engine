#include "StdAfx.h"
#include "..\include\PhysPrismaticJointDesc.h"
#include "NxPhysics.h"

namespace Engine
{

namespace Physics
{

PhysPrismaticJointDesc::PhysPrismaticJointDesc(Engine::Identifier^ name)
:joint(new NxPrismaticJointDesc()), PhysJointDesc(joint.Get(), name)
{

}

}

}