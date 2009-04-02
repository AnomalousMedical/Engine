#include "StdAfx.h"
#include "..\include\PhysFixedJointDesc.h"
#include "NxPhysics.h"

namespace Engine
{

namespace Physics
{

PhysFixedJointDesc::PhysFixedJointDesc(Engine::Identifier^ name)
:joint(new NxFixedJointDesc()), PhysJointDesc(joint.Get(), name)
{

}

}

}