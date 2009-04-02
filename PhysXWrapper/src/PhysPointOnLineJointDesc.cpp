#include "StdAfx.h"
#include "..\include\PhysPointOnLineJointDesc.h"
#include "NxPhysics.h"

namespace Engine
{

namespace Physics
{

PhysPointOnLineJointDesc::PhysPointOnLineJointDesc(Engine::Identifier^ name)
:joint(new NxPointOnLineJointDesc()), PhysJointDesc(joint.Get(), name)
{

}

}

}